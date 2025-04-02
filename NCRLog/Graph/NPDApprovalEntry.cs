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
using System.Runtime.CompilerServices;
using PX.Objects.CN.Common.Extensions;
using PX.Objects.AM.Attributes;
using System.Runtime.InteropServices.WindowsRuntime;
using PX.Objects.IN;
using PX.Objects.TX;
using System;
using static PX.Objects.EP.EPApprovalProcess;

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

        public PXAction<NPDDesignMatlCost> CreateInventoryItem;
        [PXButton]
        [PXUIField(DisplayName = "Create Inventory")]
        public virtual IEnumerable createInventoryItem(PXAdapter adapter)
        {
            if (this.DesignMatlCost.Current == null)
            {
                // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
                throw new PXSetPropertyException<NPDDesignMatlCost.nonInventoryID>("Current Records are empty, please fill in and try again", PXErrorLevel.Error);
                //PXTrace.WriteError("There is no current item to create from");
            }

            foreach (NPDDesignMatlCost line in this.DesignMatlCost.Select())
            {
                if (line.Selected == true)
                {
                    try
                    {
                        InventoryItemMaint itemMaint = PXGraph.CreateInstance<InventoryItemMaint>();
                        var header = new InventoryItem();
                        header.InventoryCD = line.NonInventoryID;
                        header.Descr = line.NonInventoryID + line.ProjectNo + line.ProductTitle;
                        itemMaint.Item.Insert(header);

                        var settings = header;
                        settings.ItemClassID = 103;
                        settings.TaxCategoryID = "STANDARD";
                        settings.PostClassID = "COMPONENTS";
                        settings.LotSerClassID = "NOTTRACKED";
                        settings.BaseUnit = line.Uom;
                        settings.SalesUnit = line.Uom;
                        settings.PurchaseUnit = line.Uom;
                        settings.InvtAcctID = 1001;
                        settings.SalesAcctID = 4000;
                        settings.COGSAcctID = 5020;
                        settings.StdCstVarAcctID = 5090;
                        settings.POAccrualAcctID = 2109;
                        itemMaint.ItemSettings.Insert(settings);

                        itemMaint.Actions.PressSave();

                        throw new PXRedirectRequiredException(itemMaint, "Redirect Successful");
                    }
                    catch (Exception ex) 
                    {
                        // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
                        // Acuminator disable once PX1051 NonLocalizableString [Justification]
                        throw new PXSetPropertyException<NPDDesignMatlCost.nonInventoryID>(ex.Message, PXErrorLevel.Error);
                    }
                    
                }
                else continue;
            }


            return adapter.Get();
        }
        #endregion

        #region Events
        protected virtual void _(Events.RowUpdated<NPDHeader> e)
        {
            NPDHeader row = e.Row;
            if (row == null) return;

            row.Version++;

            /*if (row.Status == NPDApprovalStatus.Awaiting || row.Status == NPDApprovalStatus.Approved)
            {
                row.AwaitingApproval = true;
            }
            else
            {
                row.AwaitingApproval = false;
            }

            if (row.Status == NPDApprovalStatus.Design)
            {
                row.DesignNP = true;
            }
            else
            {
                row.DesignNP = false;
            }

            if (row.Status == NPDApprovalStatus.Introductory)
            {
                row.IntroductoryNP = true;
            }
            else
            {
                row.IntroductoryNP = false;
            }

            if (row.Status == NPDApprovalStatus.Research)
            {
                row.ResearchNP = true;
            }
            else
            {
                row.ResearchNP = false;
            }

            if (row.Status == NPDApprovalStatus.Feasibility)
            {
                row.FeasibilityNP = true;
            }
            else
            {
                row.FeasibilityNP = false;
            }*/

        }

       /* protected virtual void _(Events.RowSelecting<NPDHeader> e)
        {
            NPDHeader row = e.Row;
            if (row == null) return;

            e.Cache.SetDefaultExt<NPDHeader.awaitingApproval>(row);
            e.Cache.SetDefaultExt<NPDHeader.introductoryNP>(row);
            e.Cache.SetDefaultExt<NPDHeader.designNP>(row);
            e.Cache.SetDefaultExt<NPDHeader.feasibilityNP>(row);
            e.Cache.SetDefaultExt<NPDHeader.researchNP>(row);
        }*/

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


            //ApproveIntroductory.SetEnabled(IsNPDApprover("NPDApprover", this));
            ApproveDesign.SetEnabled(IsNPDApprover("NPDApprover", this));
            ApproveFeasibility.SetEnabled(IsNPDApprover("NPDApprover", this));
            ApproveResearch.SetEnabled(IsNPDApprover("NPDApprover", this));
            Approve.SetEnabled(IsNPDApprover("NPDApprover", this));

            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedBy>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedByDesign>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedByFeasibility>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedByIntroductory>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<NPDHeader.approvedByResearch>(e.Cache, row, false);

            PXUIFieldAttribute.SetVisible<NPDHeader.awaitingApproval>(e.Cache, row, IsAdmin("Administrator", this));
            PXUIFieldAttribute.SetVisible<NPDHeader.designNP>(e.Cache, row, IsAdmin("Administrator", this)); 
            PXUIFieldAttribute.SetVisible<NPDHeader.feasibilityNP>(e.Cache, row, IsAdmin("Administrator", this));
            PXUIFieldAttribute.SetVisible<NPDHeader.introductoryNP>(e.Cache, row, IsAdmin("Administrator", this));
            PXUIFieldAttribute.SetVisible<NPDHeader.researchNP>(e.Cache, row, IsAdmin("Administrator", this));

            e.Cache.SetDefaultExt<NPDHeader.awaitingApproval>(row);
            e.Cache.SetDefaultExt<NPDHeader.introductoryNP>(row);
            e.Cache.SetDefaultExt<NPDHeader.designNP>(row);
            e.Cache.SetDefaultExt<NPDHeader.feasibilityNP>(row);
            e.Cache.SetDefaultExt<NPDHeader.researchNP>(row);
        }

        protected virtual void _(Events.FieldUpdated<NPDHeader, NPDHeader.status> e)
        {
            NPDHeader row = e.Row;
            if (row == null) return;

            if((string)e.NewValue == NPDApprovalStatus.Research && row.Introductory == true)
            {
                e.Cache.SetValueExt<NPDHeader.approvedByIntroductory>(row, Accessinfo.DisplayName);
            }

            if ((string)e.NewValue == NPDApprovalStatus.Design && row.Research == true)
            {
                e.Cache.SetValueExt<NPDHeader.approvedByResearch>(row, Accessinfo.DisplayName);
            }

            if ((string)e.NewValue == NPDApprovalStatus.Feasibility && row.Design == true)
            {
                e.Cache.SetValueExt<NPDHeader.approvedByDesign>(row, Accessinfo.DisplayName);
            }

            if ((string)e.NewValue == NPDApprovalStatus.Awaiting && row.Feasibility == true)
            {
                e.Cache.SetValueExt<NPDHeader.approvedByFeasibility>(row, Accessinfo.DisplayName);
            }

            if ((string)e.NewValue == NPDApprovalStatus.Approved)
            {
                e.Cache.SetValueExt<NPDHeader.approvedBy>(row, Accessinfo.DisplayName);
            }

        }

        protected virtual void _(Events.FieldUpdated<NPDDesignMatlCost, NPDDesignMatlCost.inventoryID> e)
        {
            NPDDesignMatlCost row = e.Row;
            if (row == null) return;

            InventoryItemCurySettings item = SelectFrom<InventoryItemCurySettings>.Where<InventoryItemCurySettings.inventoryID.IsEqual<P.AsInt>>.View.Select(this, row.InventoryID);
            if (item == null) return;

            e.Cache.SetValueExt<NPDDesignMatlCost.unitCost>(row, item.StdCost);
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

        public static bool IsAdmin(string roleName, PXGraph graph)
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
