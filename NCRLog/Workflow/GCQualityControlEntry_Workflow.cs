using PX.Data;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    using State = GCQCRecord.QC.QCStatus;
    using static BoundedTo<GCQualityControlEntry, GCQCRecord>;
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class GCQualityControlEntry_Workflow : PXGraphExtension<GCQualityControlEntry>
    {
        #region Constants
        public static class States
        {
            public const string Hold = Constants.Hold;
            public const string Released = Constants.Released;
            public const string Open = Constants.Open;


            public class hold : BqlString.Constant<hold>
            {
                public hold() : base(Hold) { }
            }
            public class released : BqlString.Constant<released>
            {
                public released() : base(Released) { }
            }
            public class open : BqlString.Constant<open>
            {
                public open() : base(Open) { }
            }

        }
        #endregion

        protected static void Configure(WorkflowContext<GCQualityControlEntry,
            GCQCRecord> context)
        {
            #region Categories
            var commonCategories = CommonActionCategories.Get(context);
            var processingCategory = commonCategories.Processing;
            #endregion

            var conditions = context.Conditions.GetPack<Conditions>();

            context.AddScreenConfigurationFor(screen => screen
                .StateIdentifierIs<GCQCRecord.status>()
                .AddDefaultFlow(flow =>
                {
                    return flow
                    #region States
                    .WithFlowStates(flowStates =>
                    {
                        flowStates.Add<States.hold>(flowState =>
                        {
                            return flowState
                            .IsInitial()
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.ReleaseFromHold, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success)
                                );
                            });
                        });
                        flowStates.Add<States.open>(flowState =>
                        {
                            return flowState
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.PutOnHold, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success)
                                );
                                actions.Add(g => g.Release, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success)
                                );
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<GCQCLine>(a => a.IsDisabled());
                            });
                        });
                        flowStates.Add<States.released>(flowState =>
                        {
                            return flowState
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.PutOnHold);
                                actions.Add(g => g.ViewBatch);
                                actions.Add(g => g.ViewOrder);
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<GCQCLine>(a => a.IsDisabled());
                            });

                        });
                    })
                    #endregion
                    #region Transitions
                    .WithTransitions(transitions =>
                    {
                        transitions.Add(t => t.From<States.hold>()
                            .To<States.open>()
                            .IsTriggeredOn(g => g.ReleaseFromHold)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<GCQCRecord.hold>(false);
                            })
                        );
                        transitions.Add(t => t.From<States.open>()
                            .To<States.hold>()
                            .IsTriggeredOn(g => g.PutOnHold)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<GCQCRecord.hold>(true);
                            })
                        );
                        transitions.Add(t => t.From<States.open>()
                            .To<States.released>()
                            .IsTriggeredOn(g => g.Release)

                        );
                        transitions.Add(t => t.From<States.released>()
                            .To<States.hold>()
                            .IsTriggeredOn(g => g.PutOnHold)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<GCQCRecord.hold>(true);
                            })
                        );
                    });
                    #endregion
                })

            #region Categories
                .WithCategories(categories =>
                {
                    categories.Add(processingCategory);
                })
            #endregion
            #region Actions
                .WithActions(actions =>
                {
                    actions.Add(g => g.ReleaseFromHold, c => c
                        .WithCategory(processingCategory)

                    );

                    actions.Add(g => g.PutOnHold, c => c
                        .WithCategory(processingCategory)

                    );
                    actions.Add(g => g.Release, c => c
                        .WithCategory(processingCategory)

                    );
                    actions.Add(g => g.ViewBatch

                    );
                    actions.Add(g => g.ViewOrder

                    );
                })
            #endregion
            );
        }

        public override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<GCQualityControlEntry,
                GCQCRecord>());
        }

        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition NotOpen => GetOrCreate(b => b.
                FromBql<Where<GCQCRecord.status.IsNotEqual<States.open>>>()
            );

            public Condition IsNotOnHold => GetOrCreate(b => b.
                FromBql<Where<GCQCRecord.status.IsNotEqual<States.hold>>>()
            );
            public Condition IsOnHold => GetOrCreate(b => b.
                FromBql<Where<GCQCRecord.status.IsEqual<States.hold>>>()
            );

            public Condition NoOwner => GetOrCreate(b => b.
                FromBql<Where<GCQCRecord.owner.IsNull>>()
            );
        }
        #endregion
    }
}
