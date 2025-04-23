using System;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using static NCRLog.NPD;
using PX.Objects.AM;
using static NCRLog.NPDHeader;
using PX.Objects.CS;
using PX.SM;

namespace NCRLog
{
    [Serializable]
    [PXCacheName("NPDHeader")]
    [PXPrimaryGraph(typeof(NPDApprovalEntry))]
    public class NPDHeader : PXBqlTable, IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<NPDHeader>.By<projectNo, productTitle>
        {
            public static NPDHeader Find(PXGraph graph, string projectNo, string productTitle, PKFindOptions options = PKFindOptions.None) => FindBy(graph, projectNo, productTitle, options);
        }
        public static class FK
        {
            
        }
        #endregion

        #region ProjectNo
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Project No", Enabled = false)]
        [AutoNumber(typeof(ISOSetup.autoNumberingNPDApproval),
            typeof(NPDHeader.date))]
        [PXSelector(typeof(NPDHeader.projectNo),
            typeof(NPDHeader.productTitle), ValidateValue = false)]
        public virtual string ProjectNo { get; set; }
        public abstract class projectNo : PX.Data.BQL.BqlString.Field<projectNo> { }
        #endregion

        #region ProductTitle
        [PXDBString(128, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Product Title")]
        [PXDefault]
        public virtual string ProductTitle { get; set; }
        public abstract class productTitle : PX.Data.BQL.BqlString.Field<productTitle> { }
        #endregion

        #region ProjectOwner
        [PX.TM.Owner]
        [PXDefault]
        [PXUIField(DisplayName = "Project Owner")]
        public virtual int? ProjectOwner { get; set; }
        public abstract class projectOwner : PX.Data.BQL.BqlInt.Field<projectOwner> { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Email
        [PXDBString(64, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Email")]
        public virtual string Email { get; set; }
        public abstract class email : PX.Data.BQL.BqlString.Field<email> { }
        #endregion

        #region Version
        [PXDBInt()]
        [PXUIField(DisplayName = "Version", Enabled = false)]
        [PXDefault(0)]
        public virtual int? Version { get; set; }
        public abstract class version : PX.Data.BQL.BqlInt.Field<version> { }
        #endregion

        #region ProjectOverview
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Project Overview")]
        public virtual string ProjectOverview { get; set; }
        public abstract class projectOverview : PX.Data.BQL.BqlString.Field<projectOverview> { }
        #endregion

        #region MatlLineCount
        [PXDBInt()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Matl Line Count")]
        public virtual int? MatlLineCount { get; set; }
        public abstract class matlLineCount : PX.Data.BQL.BqlInt.Field<matlLineCount> { }
        #endregion

        #region FindingLineCount
        [PXDBInt()]
        [PXUIField(DisplayName = "Finding Line Count")]
        public virtual int? FindingLineCount { get; set; }
        public abstract class findingLineCount : PX.Data.BQL.BqlInt.Field<findingLineCount> { }
        #endregion

        #region ObjectiveCount
        [PXDBInt()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Objective Count")]
        public virtual int? ObjectiveCount { get; set; }
        public abstract class objectiveCount : PX.Data.BQL.BqlInt.Field<objectiveCount> { }
        #endregion

        #region FindingCount
        [PXDBInt()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Finding Count")]
        public virtual int? FindingCount { get; set; }
        public abstract class findingCount : PX.Data.BQL.BqlInt.Field<findingCount> { }
        #endregion

        #region DependencyCount
        [PXDBInt()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Dependency Count")]
        public virtual int? DependencyCount { get; set; }
        public abstract class dependencyCount : PX.Data.BQL.BqlInt.Field<dependencyCount> { }
        #endregion

        #region Status
        public abstract class status : BqlString.Field<status> { }
        [NPDApprovalStatus.List]
        [PXDBString(1, IsUnicode = true)]
        [PXUIField(DisplayName = "Status")]
        public virtual string Status
        {
            get;
            set;
        }
        #endregion

        #region ApprovedBy
        public abstract class approvedBy : BqlString.Field<approvedBy> { }

        [PXDBString(64, IsUnicode = true)]
        [PXUIField(DisplayName = "Approved By")]
        public virtual string ApprovedBy
        {
            get;
            set;
        }
        #endregion

        #region ApprovedByIntroductory
        public abstract class approvedByIntroductory : BqlString.Field<approvedByIntroductory> { }

        [PXDBString(64, IsUnicode = true)]
        [PXUIField(DisplayName = "Approved By (Introductory)")]
        public virtual string ApprovedByIntroductory
        {
            get;
            set;
        }
        #endregion

        #region ApprovedByResearch
        public abstract class approvedByResearch : BqlString.Field<approvedByResearch> { }

        [PXDBString(64, IsUnicode = true)]
        [PXUIField(DisplayName = "Approved By (Research)")]
        public virtual string ApprovedByResearch
        {
            get;
            set;
        }
        #endregion

        #region ApprovedByDesign
        public abstract class approvedByDesign : BqlString.Field<approvedByDesign> { }

        [PXDBString(64, IsUnicode = true)]
        [PXUIField(DisplayName = "Approved By (Design)")]
        public virtual string ApprovedByDesign
        {
            get;
            set;
        }
        #endregion

        #region Design
        public abstract class design : BqlBool.Field<design> { }

        [PXDBBool]
        [PXUIField(DisplayName = "Design", Enabled = false)]
        public virtual bool? Design
        {
            get;
            set;
        }
        #endregion

        #region Research
        public abstract class research : BqlBool.Field<research> { }

        [PXDBBool]
        [PXUIField(DisplayName = "Research", Enabled = false)]
        public virtual bool? Research
        {
            get;
            set;
        }
        #endregion

        #region Feasibility
        public abstract class feasibility : BqlBool.Field<feasibility> { }

        [PXDBBool]
        [PXUIField(DisplayName = "Feasibility", Enabled = false)]
        public virtual bool? Feasibility
        {
            get;
            set;
        }
        #endregion

        #region Introductory
        public abstract class introductory : BqlBool.Field<introductory> { }

        [PXDBBool]
        [PXUIField(DisplayName = "Introductory", Enabled = false)]
        public virtual bool? Introductory
        {
            get;
            set;
        }
        #endregion


        #region ApprovedByFeasibility
        public abstract class approvedByFeasibility : BqlString.Field<approvedByFeasibility> { }

        [PXDBString(64, IsUnicode = true)]
        [PXUIField(DisplayName = "Approved By (Feasibility)")]
        public virtual string ApprovedByFeasibility
        {
            get;
            set;
        }
        #endregion

        #region Reason
        public abstract class reason : BqlString.Field<reason> { }

        [PXDBText(IsUnicode = true)]
        [PXUIField(DisplayName = "Reason")]
        public virtual string Reason
        {
            get;
            set;
        }
        #endregion

        #region CustomerFeedback
        public abstract class customerFeedback : BqlString.Field<customerFeedback> { }

        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "Customer Feedback")]
        public virtual string CustomerFeedback
        {
            get;
            set;
        }
        #endregion

        #region TotalCost
        public abstract class totalCost : BqlDecimal.Field<totalCost> { }

        [PXDBDecimal(6)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Total Cost", Enabled = false)]
        public virtual decimal? TotalCost
        {
            get;
            set;
        }
        #endregion

        #region Expenditure
        public abstract class expenditure : BqlDecimal.Field<expenditure> { }

        [PXDBDecimal(6)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Expenditure", Visible = false)]
        public virtual decimal? Expenditure
        {
            get;
            set;
        }
        #endregion

        #region TotalOpex
        public abstract class totalOpex : BqlDecimal.Field<totalOpex> { }

        [PXDBDecimal(6)]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Total Opex", Enabled = false)]
        public virtual decimal? TotalOpex
        {
            get;
            set;
        }
        #endregion

        #region TotalCapex
        public abstract class totalCapex : BqlDecimal.Field<totalCapex> { }

        [PXDBDecimal(6)]
        [PXFormula(null,
            typeof(SumCalc<NPDDesignMatlCost.capex>))]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Total Capex", Enabled = false)]
        public virtual decimal? TotalCapex
        {
            get;
            set;
        }
        #endregion

        #region TotalMaterialCost
        public abstract class totalMaterialCost : BqlDecimal.Field<totalMaterialCost> { }

        [PXDBDecimal(6)]
        [PXFormula(null,
            typeof(SumCalc<NPDDesignMatlCost.extCost>))]
        [PXDefault(TypeCode.Decimal, "0.00", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Total Material Cost", Enabled = false)]
        public virtual decimal? TotalMaterialCost
        {
            get;
            set;
        }
        #endregion


        #region Non-Persisted
        #region AwaitingApproval
        public abstract class awaitingApproval : BqlBool.Field<awaitingApproval> { }
        [PXBool]
        //[PXFormula(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.awaiting>>, True,
        //    Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.awaiting>>, False>>>))]
        [PXUnboundDefault(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.awaiting>.
                Or<NPDHeader.status.IsEqual<NPDApprovalStatus.approved>>>, True,
                    Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.awaiting>.
                        Or<NPDHeader.status.IsNotEqual<NPDApprovalStatus.approved>>>, False>>>))]
        [PXUIField(DisplayName = "AwaitingApproval")]
        public virtual bool? AwaitingApproval
        {
            get;
            set;
        }
        #endregion

        #region IntroductoryNP
        public abstract class introductoryNP : BqlBool.Field<introductoryNP> { }
        [PXBool]
        //[PXFormula(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.introductory>>, True,
        //    Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.introductory>>, False>>>))]
        [PXUnboundDefault(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.introductory>>, True,
            Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.introductory>>, False>>>))]
        [PXUIField(DisplayName = "IntroductoryNP")]
        public virtual bool? IntroductoryNP
        {
            get;
            set;
        }
        #endregion

        #region DesignNP
        public abstract class designNP : BqlBool.Field<designNP> { }
        [PXBool]
        //[PXFormula(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.design>>, True,
        //    Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.design>>, False>>>))]
        [PXUnboundDefault(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.design>>, True,
            Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.design>>, False>>>))]
        [PXUIField(DisplayName = "DesignNP")]
        public virtual bool? DesignNP
        {
            get;
            set;
        }
        #endregion

        #region ResearchNP
        public abstract class researchNP : BqlBool.Field<researchNP> { }
        [PXBool]
        //[PXFormula(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.research>>, True,
        //    Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.research>>, False>>>))]
        [PXUnboundDefault(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.research>>, True,
            Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.research>>, False>>>))]
        [PXUIField(DisplayName = "ResearchNP")]
        public virtual bool? ResearchNP
        {
            get;
            set;
        }
        #endregion

        #region FeasibilityNP
        public abstract class feasibilityNP : BqlBool.Field<feasibilityNP> { }
        [PXBool]
        //[PXFormula(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.feasibility>>, True,
        //    Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.feasibility>>, False>>>))]
        [PXUnboundDefault(typeof(Switch<Case<Where<NPDHeader.status.IsEqual<NPDApprovalStatus.feasibility>>, True,
            Case<Where<NPDHeader.status.IsNotEqual<NPDApprovalStatus.feasibility>>, False>>>))]
        [PXUIField(DisplayName = "FeasibilityNP")]
        public virtual bool? FeasibilityNP
        {
            get;
            set;
        }
        #endregion



        #endregion


        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion
    }

    public class NPD
    {
        public class NPDApprovalStatus
        {
            public class ListAttribute : PXStringListAttribute
            {
                public static readonly (string, string)[] ValuesToLabels = new[]
                {
                    (Introductory, "Introductory"),
                    (Design, "Design"),
                    (Research, "Research"),
                    (Approved, "Approved for Plan"),
                    (Awaiting, "Awaiting Approval"),
                    (Feasibility, "Feasibility"),
                    (Rejected, "Rejected"),
                };
                public ListAttribute() : base(ValuesToLabels) { }
            }

            public const string Introductory = "I";
            public const string Design = "D";
            public const string Research = "R";
            public const string Approved = "A";
            public const string Awaiting = "W";
            public const string Feasibility = "F";
            public const string Rejected = "X";

            public class introductory : BqlString.Constant<introductory>
            {
                public introductory() : base(Introductory)
                {
                }
            }

            public class design : BqlString.Constant<design>
            {
                public design() : base(Design)
                {
                }
            }

            public class research : BqlString.Constant<research>
            {
                public research() : base(Research)
                {
                }
            }

            public class approved : BqlString.Constant<approved>
            {
                public approved() : base(Approved)
                {
                }
            }

            public class awaiting : BqlString.Constant<awaiting>
            {
                public awaiting() : base(Awaiting)
                {
                }
            }

            public class feasibility : BqlString.Constant<feasibility>
            {
                public feasibility() : base(Feasibility)
                {
                }
            }

            public class rejected : BqlString.Constant<rejected>
            {
                public rejected() : base(Rejected) { }
            }
        }
    }
}