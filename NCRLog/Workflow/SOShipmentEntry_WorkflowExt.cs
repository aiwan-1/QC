using PX.Api;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;
using PX.Objects.PO;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Data.WorkflowAPI.BoundedTo<PX.Objects.SO.SOShipmentEntry,
    PX.Objects.SO.SOShipment>;
using PX.Objects.SO.GraphExtensions.SOShipmentEntryExt;
using static PX.Objects.SO.SOShipment;

namespace NCRLog
{
    public class SOShipmentEntry_WorkflowExt : PXGraphExtension<SOShipmentEntry_Workflow,
         SOShipmentEntryCreditHoldExt, SOShipmentEntry>
    {

        public class SOShipmentStatus_CreditHold : SOShipmentStatus
        {
            public const string CreditHold = "Q";
            public class creditHold : BqlType<IBqlString, string>.Constant<creditHold>
            {
                public creditHold() : base("Q") { }
            }
            public const string ProdConfirmed = "M";
            public class prodConfirmed : BqlType<IBqlString, string>.Constant<prodConfirmed>
            {
                public prodConfirmed() : base(ProdConfirmed) { }
            }
        }

        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<SOShipmentEntry,
                SOShipment>());
        }

        protected static void DisableWholeScreen(FieldState.IContainerFillerFields states)
        {
            //states.AddAllFields<SOShipment>(state => state.IsDisabled());
            states.AddTable<SOShipLine>(state => state.IsDisabled());
            states.AddTable<SOShipLineSplit>(state => state.IsDisabled());
            states.AddTable<SOShipmentAddress>(state => state.IsDisabled());
            states.AddTable<SOShipmentContact>(state => state.IsDisabled());
            states.AddTable<SOOrderShipment>(state => state.IsDisabled());
            //states.AddTable<*>(state => state.IsDisabled());
        }

        protected static void Configure(WorkflowContext<SOShipmentEntry,
            SOShipment> context)
        {
            var financials = context.Categories.CreateNew(
                ActionCategories.FinancialCategoryID,
                category => category.DisplayName(ActionCategories.DisplayName.Financial));

            var confirmation = context.Categories.CreateNew(
                ActionCategories.ConfirmationID,
                category => category.DisplayName(ActionCategories.DisplayName.Confirmation));

            var removeCreditHold = context.ActionDefinitions
                .CreateExisting<SOShipmentEntryCreditHoldExt>(g => g.RemoveCreditHold,
                a => a.WithCategory(financials));

            var conditions = context.Conditions.GetPack<Conditions>();

            var productionConfirm = context.ActionDefinitions
                .CreateExisting<SOShipmentEntryCreditHoldExt>(g => g.ProductionConfirmed,
                a => a.WithCategory(confirmation));


            const string initialState = "_";
            context.UpdateScreenConfigurationFor(screen => screen
                .UpdateDefaultFlow(flow =>
                {
                    return flow
                        .WithFlowStates(flowStates =>
                        {
                            flowStates.Update(initialState, flowState =>
                            {
                                return flowState
                                .WithEventHandlers(handlers =>
                                {
                                    handlers.Add<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitViolated);
                                });
                            });
                            flowStates.Update<SOShipmentStatus.open>(flowState =>
                            {
                                return flowState
                                .WithActions(actions =>
                                actions.Add(productionConfirm, c =>
                                c.WithConnotation(ActionConnotation.Secondary)))
                                .WithEventHandlers(handlers =>
                                {
                                    handlers.Add<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitSatisfied);
                                   // handlers.Add<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitViolated);
                                });
                            });
                            flowStates.Add<SOShipmentStatus_CreditHold.creditHold>(flowState =>
                            {
                                return flowState
                                .WithActions(actions =>
                                actions.Add(removeCreditHold, c =>
                                c.WithConnotation(ActionConnotation.Success).IsDuplicatedInToolbar()));
                            });
                            flowStates.Update<SOShipmentStatus.hold>(flowstate =>
                            {
                                return flowstate
                                .WithEventHandlers(handlers =>
                                {
                                    handlers.Add<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitSatisfied);
                                    handlers.Add<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitViolated);
                                });
                            });
                            flowStates.Add<SOShipmentStatus_CreditHold.prodConfirmed>(flowState =>
                            {
                                return flowState
                                .WithActions(actions =>
                                actions.Add(g => g.confirmShipmentAction, c =>
                                c.WithConnotation(ActionConnotation.Secondary)))
                                .WithActions(actions =>
                                actions.Add(g => g.correctShipmentAction, c =>
                                c.WithConnotation(ActionConnotation.Secondary)));


                            });
                            flowStates.Remove<SOShipmentStatus.confirmed>();
                            flowStates.Add<SOShipmentStatus.confirmed>(flowState =>
                            {
                                return flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.createInvoice, a => a.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
                                        actions.Add(g => g.UpdateIN, a => a.IsDuplicatedInToolbar());
                                        actions.Add(g => g.printShipmentConfirmation);
                                        actions.Add(g => g.correctShipmentAction);
                                        actions.Add(g => g.validateAddresses);
                                        actions.Add(g => g.emailShipment);
                                        actions.Add<Intercompany>(e => e.generatePOReceipt);
                                    })
                                    .WithEventHandlers(handlers =>
                                    {
                                        handlers.Add(g => g.OnInvoiceLinked);
                                        handlers.Add(g => g.OnShipmentCorrected);
                                    })
                                    .WithFieldStates(DisableWholeScreen);
                            });
                        })
                        .WithTransitions(transitions =>
                        {
                            transitions.UpdateGroupFrom(initialState, ts =>
                            {
                                ts.Add(t =>
                                    t.To<SOShipmentStatus_CreditHold.creditHold>()
                                    .IsTriggeredOn(g => g.initializeState)
                                    .When(conditions.CreditViolated)

                                );
                                ts.Add(t =>
                                    t.To<SOShipmentStatus_CreditHold.creditHold>()
                                    .IsTriggeredOn<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitViolated)
                                );
                            });
                            transitions.AddGroupFrom<SOShipmentStatus_CreditHold.creditHold>(ts =>
                            {
                                ts.Add(t =>
                                    t.To<SOShipmentStatus.open>()
                                    .IsTriggeredOn<SOShipmentEntryCreditHoldExt>(g => g.RemoveCreditHold)
                                );

                                ts.Add(t =>
                                    t.To<SOShipmentStatus.open>()
                                    .IsTriggeredOn<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitSatisfied)
                                );

                            });
                            transitions.UpdateGroupFrom<SOShipmentStatus.open>(ts =>
                            {

                                ts.Add(t =>
                                    t.To<SOShipmentStatus_CreditHold.creditHold>()
                                    .IsTriggeredOn<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitViolated)
                                );

                                ts.Add(t =>
                                    t.To<SOShipmentStatus_CreditHold.prodConfirmed>()
                                    .IsTriggeredOn(productionConfirm)
                                );
                            });
                            transitions.UpdateGroupFrom<SOShipmentStatus.hold>(ts =>
                            {
                                ts.Add(t =>
                                    t.To<SOShipmentStatus_CreditHold.creditHold>()
                                    .IsTriggeredOn(g => g.releaseFromHold)
                                    .When(conditions.CreditViolated)
                                    .WithFieldAssignments(fs =>
                                    {
                                        fs.Add<SOShipment.hold>(false);
                                    })
                                );

                                ts.Add(t =>
                                    t.To<SOShipmentStatus_CreditHold.creditHold>()
                                    .IsTriggeredOn<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitViolated)
                                    .WithFieldAssignments(fs =>
                                    {
                                        fs.Add<SOShipment.hold>(false);
                                    })
                                );
                            });
                            transitions.AddGroupFrom<SOShipmentStatus_CreditHold.prodConfirmed>(ts =>
                            {
                                ts.Add(t =>
                                    t.To<SOShipmentStatus.confirmed>()
                                    .IsTriggeredOn(g => g.confirmShipmentAction)
                                );

                            });
                        });

                })
                .WithHandlers(handlers =>
                {
                    handlers.Add(handler => handler
                        .WithTargetOf<SOShipment>()
                        .OfEntityEvent<SOShipmentCreditHold.MyEvents>(e => e.CreditLimitViolated)
                        .Is<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitViolated)
                        .UsesPrimaryEntityGetter<
                            SelectFrom<SOShipment>>()
                    );
                    handlers.Add(handler => handler
                        .WithTargetOf<SOShipment>()
                        .OfEntityEvent<SOShipmentCreditHold.MyEvents>(e => e.CreditLimitSatisfied)
                        .Is<SOShipmentEntryCreditHoldExt>(g => g.OnCreditLimitSatisfied)
                        .UsesPrimaryEntityGetter<
                            SelectFrom<SOShipment>>()
                    );
                })
                .WithCategories(categories =>
                {
                    categories.Add(financials);
                })
                .WithActions(actions =>
                {
                    actions.Add(removeCreditHold);
                    actions.Add(productionConfirm);
                })
                .WithFieldStates(fs =>
                {
                    fs.Add<SOShipment.status>(state =>
                        state.SetComboValue(SOShipmentStatus_CreditHold
                        .CreditHold, "Credit Hold")
                        .SetComboValue(SOShipmentStatus_CreditHold.ProdConfirmed, "Confirmed by Production")
                    );
                })

            );
        }

        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition CreditSatisfied => GetOrCreate(b =>
            b.FromBql<Where<SOShipmentCreditHold.usrCreditHold.IsEqual<False>>>());

            public Condition CreditViolated => GetOrCreate(b =>
            b.FromBql<Where<SOShipmentCreditHold.usrCreditHold.IsEqual<True>>>());
        }
        #endregion

        public static class ActionCategories
        {
            public const string FinancialCategoryID = "Financial Category";
            public const string ConfirmationID = "Confirmation Category";

            [PXLocalizable]
            public static class DisplayName
            {
                public const string Financial = "Financials";
                public const string Confirmation = "Confirmation";
            }
        }


    }
}
