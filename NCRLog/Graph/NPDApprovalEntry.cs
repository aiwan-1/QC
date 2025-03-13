using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Data.Update.ExchangeService;
using PX.Objects.EP;
using System.Collections;
using static NCRLog.NPD;
using PX.Objects.PO;
using PX.TM;
using PX.SM;

namespace NCRLog
{
    public class NPDApprovalEntry : PXGraph<NPDApprovalEntry, NPDHeader>
    {
        #region Views
        public SelectFrom<NPDHeader>.View Header;

        public SelectFrom<NPDDependency>.
            Where<NPDDependency.projectNo.IsEqual<NPDHeader.projectNo.FromCurrent>.
                And<NPDDependency.productTitle.IsEqual<NPDHeader.productTitle.FromCurrent>>>.View Dependencies;

        public SelectFrom<NPDDesignMatlCost>.
            Where<NPDDesignMatlCost.projectNo.IsEqual<NPDHeader.projectNo.FromCurrent>.
                And<NPDDesignMatlCost.productTitle.IsEqual<NPDHeader.productTitle.FromCurrent>>>.View DesignMatlCost;

        public SelectFrom<NPDFeasibility>.
            Where<NPDFeasibility.projectNo.IsEqual<NPDHeader.projectNo.FromCurrent>.
                And<NPDFeasibility.productTitle.IsEqual<NPDHeader.productTitle.FromCurrent>>>.View Feasibility;

        public SelectFrom<NPDProductObjective>.
            Where<NPDProductObjective.projectNo.IsEqual<NPDHeader.projectNo.FromCurrent>.
                And<NPDProductObjective.productTitle.IsEqual<NPDHeader.productTitle.FromCurrent>>>.View Objectives;

        public SelectFrom<NPDResearch>.
            Where<NPDResearch.projectNo.IsEqual<NPDHeader.projectNo.FromCurrent>.
                And<NPDResearch.productTitle.IsEqual<NPDHeader.productTitle.FromCurrent>>>.View Research;

        public SelectFrom<NPDRisks>.
            Where<NPDRisks.projectNo.IsEqual<NPDResearch.projectNo.FromCurrent>.
                And<NPDRisks.productTitle.IsEqual<NPDResearch.productTitle.FromCurrent>.
                    And<NPDRisks.findingID.IsEqual<NPDResearch.findingID.FromCurrent>>>>.View Risks;

        public SelectFrom<NPDStakeholder>.
            Where<NPDStakeholder.projectNo.IsEqual<NPDHeader.projectNo.FromCurrent>.
                And<NPDStakeholder.productTitle.IsEqual<NPDHeader.productTitle.FromCurrent>>>.View Stakeholders;

        public PXSetup<ISOSetup> Setup;

        #region Graph Constructor
        public NPDApprovalEntry()
        {
            ISOSetup setup = Setup.Current;
        }
        #endregion

        #endregion

        #region Actions
        #region Workflow Actions
        public PXAction<NPDHeader> ApproveIntroductory;
        [PXButton]
        [PXUIField(DisplayName = "Approve Introductory")]
        protected virtual IEnumerable approveIntroductory(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> ApproveDesign;
        [PXButton]
        [PXUIField(DisplayName = "Approve Design")]
        protected virtual IEnumerable approveDesign(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> ApproveFeasibility;
        [PXButton]
        [PXUIField(DisplayName = "Approve Feasibility")]
        protected virtual IEnumerable approveFeasibility(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> ApproveResearch;
        [PXButton]
        [PXUIField(DisplayName = "Approve Research")]
        protected virtual IEnumerable approveResearch(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> Approve;
        [PXButton]
        [PXUIField(DisplayName = "Approve")]
        protected virtual IEnumerable approve(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> Reject;
        [PXButton]
        [PXUIField(DisplayName = "Reject")]
        protected virtual IEnumerable reject(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> ToResearch;
        [PXButton]
        [PXUIField(DisplayName = "Go To Research")]
        protected virtual IEnumerable toResearch(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> ToDesign;
        [PXButton]
        [PXUIField(DisplayName = "Go To Design")]
        protected virtual IEnumerable toDesign(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> ToFeasibility;
        [PXButton]
        [PXUIField(DisplayName = "Go To Feasibility")]
        protected virtual IEnumerable toFeasibility(PXAdapter adapter)
            => adapter.Get();

        public PXAction<NPDHeader> ToIntro;
        [PXButton]
        [PXUIField(DisplayName = "Go To Introductory")]
        protected virtual IEnumerable toIntroductory(PXAdapter adapter)
            => adapter.Get();
        #endregion
        #endregion

        #region Events
        protected virtual void _(Events.RowUpdated<NPDHeader> e)
        {
            NPDHeader row = e.Row;
            if (row == null) return;

            row.Version++;
        }

        protected virtual void _(Events.FieldUpdated<NPDStakeholder, NPDStakeholder.stakeholderID> e)
        {
            NPDStakeholder row = e.Row;
            if (row == null) return;

            EPEmployee emp = SelectFrom<EPEmployee>.
                Where<EPEmployee.acctName.IsEqual<P.AsString>>.View.Select(this, row.StakeholderID);
            if(emp == null) return;

            string[] names = emp.AcctName.Split(' ');
            string forename = names[0];
            string surname = names[1];

            e.Cache.SetValueExt<NPDStakeholder.firstName>(row, forename);
            e.Cache.SetValueExt<NPDStakeholder.lastName>(row, surname);
        }

        protected virtual void _(Events.RowSelected<NPDHeader> e)
        {
            NPDHeader row = e.Row; 
            if (row == null) return;

           
            ApproveIntroductory.SetEnabled(IsNPDApprover("NPDApprover", this));
            ApproveDesign.SetEnabled(IsNPDApprover("NPDApprover", this));
            ApproveFeasibility.SetEnabled(IsNPDApprover("NPDApprover", this));
            ApproveResearch.SetEnabled(IsNPDApprover("NPDApprover", this));
            Approve.SetEnabled(IsNPDApprover("NPDApprover", this));

            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedBy>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedByDesign>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedByFeasibility>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedByIntroductory>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedByResearch>(e.Cache, row, false);

            if(row.Status == NPDApprovalStatus.Awaiting || row.Status == NPDApprovalStatus.Approved)
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.AwaitingApproval = true;
            }
            else 
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.AwaitingApproval = false; 
            }

            if (row.Status == NPDApprovalStatus.Design)
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.DesignNP = true;
            }
            else
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.DesignNP = false;
            }

            if (row.Status == NPDApprovalStatus.Introductory)
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.IntroductoryNP = true;
            }
            else
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.IntroductoryNP = false;
            }

            if (row.Status == NPDApprovalStatus.Research)
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.ResearchNP = true;
            }
            else
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.ResearchNP = false;
            }

            if (row.Status == NPDApprovalStatus.Feasibility)
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.FeasibilityNP = true;
            }
            else
            {
                // Acuminator disable once PX1047 RowChangesInEventHandlersForbiddenForArgs [Justification]
                row.FeasibilityNP = false;
            }
        }

        protected virtual void _(Events.FieldUpdated<NPDHeader, NPDHeader.status> e)
        {
            NPDHeader row = e.Row;
            if (row == null) return;

            if((string)e.NewValue == "R")
            {
                e.Cache.SetValueExt<NPDHeader.approvedByIntroductory>(row, Accessinfo.DisplayName);
            }

            if ((string)e.NewValue == "D")
            {
                e.Cache.SetValueExt<NPDHeader.approvedByResearch>(row, Accessinfo.DisplayName);
            }

            if ((string)e.NewValue == "F")
            {
                e.Cache.SetValueExt<NPDHeader.approvedByDesign>(row, Accessinfo.DisplayName);
            }

            if ((string)e.NewValue == "W")
            {
                e.Cache.SetValueExt<NPDHeader.approvedByFeasibility>(row, Accessinfo.DisplayName);
            }

            if ((string)e.NewValue == "A")
            {
                e.Cache.SetValueExt<NPDHeader.approvedBy>(row, Accessinfo.DisplayName);
            }
        }
        #endregion

        #region Methods
        public static bool IsNPDApprover(string roleName, PXGraph graph)
        {
            string usrName = graph.Accessinfo.UserName;

            UsersInRoles assignedRoles = SelectFrom<UsersInRoles>.
                Where<UsersInRoles.username.IsEqual<P.AsString>.
                And<UsersInRoles.rolename.IsEqual<P.AsString>>>.View.Select(graph, usrName, roleName);
            if (assignedRoles == null)
                return false;

            else return true;
        }
        #endregion
    }
}
