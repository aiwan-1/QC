﻿using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using static PX.Data.WorkflowAPI.BoundedTo<NCRLog.GCPressGlueLogEntry,
    NCRLog.PressGlueLogHeader>;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.WorkflowAPI;
using PX.Objects.Common;

namespace NCRLog
{
    using State = PressGlueLogHeader.PGLog.PGLogStatus.ListAttribute;
    using Constant = PressGlueLogHeader.PGLog;
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class PressGlueLogEntry_Workflow : PXGraphExtension<GCPressGlueLogEntry>
    {
        #region Constants
        public static class States
        {
            public const string Hold = Constant.Hold;
            public const string Released = Constant.Released;
            public const string Open = Constant.Open;


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

        protected static void Configure(WorkflowContext<GCPressGlueLogEntry,
            PressGlueLogHeader> context)
        {
            #region Categories
            var commonCategories = CommonActionCategories.Get(context);
            var processingCategory = commonCategories.Processing;
            #endregion

            var conditions = context.Conditions.GetPack<Conditions>();

            context.AddScreenConfigurationFor(screen => screen
                .StateIdentifierIs<PressGlueLogHeader.status>()
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
                                fs.AddTable<PressGlueLogDetails>(a => a.IsDisabled());
                            });
                        });
                        flowStates.Add<States.released>(flowState =>
                        {
                            return flowState
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.PutOnHold);
                                actions.Add(g => g.ViewBatch);
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<PressGlueLogDetails>(a => a.IsDisabled());
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
                                fas.Add<PressGlueLogHeader.hold>(false);
                            })
                        );
                        transitions.Add(t => t.From<States.open>()
                            .To<States.hold>()
                            .IsTriggeredOn(g => g.PutOnHold)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<PressGlueLogHeader.hold>(true);
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
                                fas.Add<PressGlueLogHeader.hold>(true);
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
                    actions.Add(g => g.ViewBatch, c => c
                        .WithCategory(processingCategory)
                    );
                })
            #endregion
            /*.WithForms(forms =>
            {
                forms.Add(form =>
                {
                    form
                })
            })*/
            );
        }
        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition NotOpen => GetOrCreate(b => b.
                FromBql<Where<PressGlueLogHeader.status.IsNotEqual<States.open>>>()
            );

            public Condition IsNotOnHold => GetOrCreate(b => b.
                FromBql<Where<PressGlueLogHeader.status.IsNotEqual<States.hold>>>()
            );
            public Condition IsOnHold => GetOrCreate(b => b.
                FromBql<Where<PressGlueLogHeader.status.IsEqual<States.hold>>>()
            );
        }
        #endregion

        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<GCPressGlueLogEntry,
                PressGlueLogHeader>());
        }
    }
}
