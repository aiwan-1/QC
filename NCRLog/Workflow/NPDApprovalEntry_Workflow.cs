using static PX.Data.WorkflowAPI.BoundedTo<NCRLog.NPDApprovalEntry,
    NCRLog.NPDHeader>;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.WorkflowAPI;
using PX.Objects.Common;
using PX.Data;
using PX.Common;
using PX.SM;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.PO;

namespace NCRLog
{
    using State = NPD.NPDApprovalStatus;

    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class NPDApprovalEntry_Workflow : PXGraphExtension<NPDApprovalEntry>
    {
        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<NPDApprovalEntry,
                NPDHeader>());
        }
        

        protected static void Configure(WorkflowContext<NPDApprovalEntry,
            NPDHeader> context)
        {
            #region Categories
            var commonCategories = CommonActionCategories.Get(context);
            var processingCategory = commonCategories.Processing;
            var approvalCategory = commonCategories.Approval;
            var otherCategory = commonCategories.Other;
            var printingCategory = commonCategories.PrintingAndEmailing;
            #endregion

            #region Dialogs
            var formReject = context.Forms.Create("FormReason", form =>
                form.Prompt("Reason").WithFields(fields =>
                {
                    fields.Add("Reason", field => field
                        .WithRichTextEditorField()
                        .Prompt("Reason for Rejection")
                    );
                })
            );

            var formCustomerFeedback = context.Forms.Create("FormCustomerFeedback", form =>
                form.Prompt("Customer Feedback").WithFields(fields =>
                {
                    fields.Add("CustomerFeedback", field => field
                        .WithRichTextEditorField()
                        .Prompt("Customer Feedback")
                    );
                })
            );
            #endregion

            var conditions = context.Conditions.GetPack<Conditions>();

            context.AddScreenConfigurationFor(screen => screen
                .StateIdentifierIs<NPDHeader.status>()
                .AddDefaultFlow(flow =>
                {
                    return flow
                    #region FlowStates
                    .WithFlowStates(flowStates =>
                    {
                        
                        flowStates.Add<State.introductory>(flowstate =>
                        {

                            return flowstate
                            .IsInitial()
                            .WithActions(actions =>
                            {
                               /* actions.Add(g => g.ApproveIntroductory, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success)
                                    
                                );*/

                                actions.Add(g => g.Reject, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Danger)
                                );

                                actions.Add(g => g.IntroToResearch, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Info)
                                );
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<NPDDependency>(a => a.IsDisabled());
                                fs.AddTable<NPDDesignMatlCost>(a => a.IsDisabled());
                                fs.AddTable<NPDFeasibility>(a => a.IsDisabled());
                                fs.AddTable<NPDResearch>(a => a.IsDisabled());
                                fs.AddTable<NPDRisks>(a => a.IsDisabled());
                            });

                        });

                        flowStates.Add<State.research>(flowstate =>
                        {
                            return flowstate
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.ApproveResearch, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success)
                                );

                                actions.Add(g => g.Reject, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Danger)
                                );

                                actions.Add(g => g.ToDesign, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Info)
                                );
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<NPDDependency>(a => a.IsDisabled());
                                fs.AddTable<NPDDesignMatlCost>(a => a.IsDisabled());
                                fs.AddTable<NPDFeasibility>(a => a.IsDisabled());
                                fs.AddTable<NPDProductObjective>(a => a.IsDisabled());
                                fs.AddTable<NPDStakeholder>(a => a.IsDisabled());
                            });
                        });

                        flowStates.Add<State.design>(flowstate =>
                        {
                            return flowstate
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.ApproveDesign, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success)
                                );

                                actions.Add(g => g.Reject, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Danger)
                                );

                                actions.Add(g => g.ToResearch, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Info)
                                );
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<NPDRisks>(a => a.IsDisabled());
                                fs.AddTable<NPDResearch>(a => a.IsDisabled());
                                fs.AddTable<NPDFeasibility>(a => a.IsDisabled());
                                fs.AddTable<NPDProductObjective>(a => a.IsDisabled());
                                fs.AddTable<NPDStakeholder>(a => a.IsDisabled());
                            });
                        });

                        flowStates.Add<State.feasibility>(flowstate =>
                        {
                            return flowstate
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.ApproveFeasibility, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success)
                                );

                                actions.Add(g => g.Reject, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Danger)
                                );
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<NPDDependency>(a => a.IsDisabled());
                                fs.AddTable<NPDDesignMatlCost>(a => a.IsDisabled());
                                fs.AddTable<NPDResearch>(a => a.IsDisabled());
                                fs.AddTable<NPDProductObjective>(a => a.IsDisabled());
                                fs.AddTable<NPDStakeholder>(a => a.IsDisabled());
                                fs.AddTable<NPDRisks>(a => a.IsDisabled());
                            });
                        });

                        flowStates.Add<State.awaiting>(flowstate =>
                        {
                            return flowstate
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.Approve, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success)
                                );

                                actions.Add(g => g.Reject, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Danger)
                                );
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<NPDDependency>(a => a.IsDisabled());
                                fs.AddTable<NPDDesignMatlCost>(a => a.IsDisabled());
                                fs.AddTable<NPDFeasibility>(a => a.IsDisabled());
                                fs.AddTable<NPDProductObjective>(a => a.IsDisabled());
                                fs.AddTable<NPDStakeholder>(a => a.IsDisabled());
                                fs.AddTable<NPDResearch>(a => a.IsDisabled());
                                fs.AddTable<NPDRisks>(a => a.IsDisabled());
                            });
                        });

                        flowStates.Add<State.approved>(flowstate =>
                        {
                            return flowstate
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.Reject, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Danger)
                                );
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<NPDDependency>(a => a.IsDisabled());
                                fs.AddTable<NPDDesignMatlCost>(a => a.IsDisabled());
                                fs.AddTable<NPDFeasibility>(a => a.IsDisabled());
                                fs.AddTable<NPDProductObjective>(a => a.IsDisabled());
                                fs.AddTable<NPDStakeholder>(a => a.IsDisabled());
                                fs.AddTable<NPDResearch>(a => a.IsDisabled());
                                fs.AddTable<NPDRisks>(a => a.IsDisabled());
                                fs.AddTable<NPDHeader>(a => a.IsDisabled());
                            });

                        });

                        flowStates.Add<State.rejected>(flowstate =>
                        {
                            return flowstate
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.ToDesign, a => a
                                    .WithConnotation(ActionConnotation.Light)
                                );

                                actions.Add(g => g.ToIntro, a => a
                                    .WithConnotation(ActionConnotation.Light)
                                );

                                actions.Add(g => g.ToFeasibility, a => a
                                    .WithConnotation(ActionConnotation.Light)
                                );

                                actions.Add(g => g.ToResearch, a => a
                                    .WithConnotation(ActionConnotation.Light)
                                );
                            })
                            .WithFieldStates(fs =>
                            {
                                fs.AddTable<NPDDependency>(a => a.IsDisabled());
                                fs.AddTable<NPDDesignMatlCost>(a => a.IsDisabled());
                                fs.AddTable<NPDFeasibility>(a => a.IsDisabled());
                                fs.AddTable<NPDProductObjective>(a => a.IsDisabled());
                                fs.AddTable<NPDStakeholder>(a => a.IsDisabled());
                                fs.AddTable<NPDResearch>(a => a.IsDisabled());
                                fs.AddTable<NPDRisks>(a => a.IsDisabled());
                                fs.AddTable<NPDHeader>(a => a.IsDisabled());
                            });

                        })
                        ;
                    })
                    #endregion

                    #region Transitions
                    .WithTransitions(transitons =>
                    {


                        transitons.Add(t => t.From<State.introductory>()
                            .To<State.research>()
                            .IsTriggeredOn(g => g.IntroToResearch)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<NPDHeader.introductory>(true);
                            })
                        );

                        transitons.Add(t => t.From<State.research>()
                            .To<State.design>()
                            .IsTriggeredOn(g => g.ApproveResearch)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<NPDHeader.research>(true);
                            })
                        );

                        transitons.Add(t => t.From<State.research>()
                            .To<State.rejected>()
                            .IsTriggeredOn(g => g.Reject)
                        );

                        transitons.Add(t => t.From<State.design>()
                            .To<State.feasibility>()
                            .IsTriggeredOn(g => g.ApproveDesign)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<NPDHeader.design>(true);
                            })
                        );

                        transitons.Add(t => t.From<State.design>()
                            .To<State.rejected>()
                            .IsTriggeredOn(g => g.Reject)
                        );

                        transitons.Add(t => t.From<State.feasibility>()
                            .To<State.awaiting>()
                            .IsTriggeredOn(g => g.ApproveFeasibility)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<NPDHeader.feasibility>(true);
                            })
                        );

                        transitons.Add(t => t.From<State.feasibility>()
                            .To<State.rejected>()
                            .IsTriggeredOn(g => g.Reject)
                        );

                        transitons.Add(t => t.From<State.awaiting>()
                            .To<State.approved>()
                            .IsTriggeredOn(g => g.Approve)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<NPDHeader.approvedBy>(c => c.SetFromValue(typeof(AccessInfo.userName)));
                            })
                        );

                        transitons.Add(t => t.From<State.awaiting>()
                            .To<State.rejected>()
                            .IsTriggeredOn(g => g.Reject)
                        );

                        transitons.Add(t => t.From<State.design>()
                            .To<State.research>()
                            .IsTriggeredOn(g => g.ToResearch)
                            
                        );

                        transitons.Add(t => t.From<State.research>()
                            .To<State.design>()
                            .IsTriggeredOn(g => g.ToDesign)
                        );


                    });
                    #endregion
                   
                   
                })

                #region Categories
                .WithCategories(cat =>
                {
                    cat.Add(processingCategory);
                    cat.Add(approvalCategory);
                    cat.Add(otherCategory);
                })
                #endregion
                #region Actions
                .WithActions(actions =>
                {
                    actions.Add(g => g.Approve, c => c
                        .WithCategory(approvalCategory)
                        
                    );

                    actions.Add(g => g.ApproveDesign, c => c
                        .WithCategory(processingCategory)
                        .IsDisabledWhen(conditions.ResearchNotApproved)
                        
                    );

                    actions.Add(g => g.ApproveFeasibility, c => c
                        .WithCategory(processingCategory)
                        
                    );
                    actions.Add(g => g.ApproveIntroductory, c => c
                        .WithCategory(processingCategory)
                       
                    );

                    actions.Add(g => g.ApproveResearch, c => c
                        .WithCategory(processingCategory)
                        
                    );

                    actions.Add(g => g.Reject, c => c
                        .WithCategory(approvalCategory)
                        .WithForm(formReject)
                        .WithFieldAssignments( fields =>
                        {
                            fields.Add<NPDHeader.reason>(f => f
                                .SetFromFormField(formReject, "Reason")
                            );
                        })
                        
                    );

                    actions.Add(g => g.ToResearch, c => c
                        .WithCategory(processingCategory)
                        .WithForm(formCustomerFeedback)
                        .WithFieldAssignments(fields =>
                        {
                            fields.Add<NPDHeader.customerFeedback>(f => f
                                .SetFromFormField(formCustomerFeedback, "Customer Feedback")
                            );
                        })
                    );

                    actions.Add(g => g.ToDesign, c => c
                        .WithCategory(processingCategory)
                    );

                    actions.Add(g => g.IntroToResearch, c => c
                        .WithCategory(processingCategory)
                        .IsDisabledWhen(conditions.NotIntro)
                    );
                    
                })

                .WithForms( forms => forms
                    .Add(formReject)
                )
                .WithForms( forms => forms
                    .Add(formCustomerFeedback)
                )
                #endregion
            );
        }

        #region Conditions
        public class Conditions : Condition.Pack
        {
            public Condition ResearchNotApproved => GetOrCreate(b => b.
                FromBql<Where<NPDHeader.research.IsNotEqual<True>>>()
            );

            public Condition NotIntro => GetOrCreate(b => b.
                FromBql<Where<NPDHeader.status.IsNotEqual<NPD.NPDApprovalStatus.introductory>>>()
            );
        }
        #endregion

        

        
    }
}
