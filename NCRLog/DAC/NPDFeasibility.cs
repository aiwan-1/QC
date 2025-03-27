using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;

namespace NCRLog
{
    [Serializable]
    [PXCacheName("NPDFeasibility")]
    public class NPDFeasibility : PXBqlTable, IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<NPDFeasibility>.By<projectNo, productTitle, feasibilityStudyType>
        {
            public static NPDFeasibility Find(PXGraph graph, string projectNo, string productTitle, int feasibilityStudyType, PKFindOptions options = PKFindOptions.None) => FindBy(graph, projectNo, productTitle, feasibilityStudyType, options);
        }
        public static class FK
        {
            public class Approval : NPDHeader.PK.ForeignKeyOf<NPDFeasibility>.By<projectNo, productTitle> { }
        }
        #endregion

        #region ProjectNo
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Project No")]
        [PXParent(typeof(FK.Approval))]
        [PXDBDefault(typeof(NPDHeader.projectNo))]
        public virtual string ProjectNo { get; set; }
        public abstract class projectNo : PX.Data.BQL.BqlString.Field<projectNo> { }
        #endregion

        #region ProductTitle
        [PXDBString(128, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Product Title")]
        [PXDBDefault(typeof(NPDHeader.productTitle))]
        public virtual string ProductTitle { get; set; }
        public abstract class productTitle : PX.Data.BQL.BqlString.Field<productTitle> { }
        #endregion

        #region FeasibilityStudyType
        [PXDBString(1, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Feasibility Study Type")]
        [PXDefault("F")]
        [PXStringList(new string[]
        {
            "F",
            "O",
            "T"
        }, 
        new string[]
        {
            "Financial",
            "Operational",
            "Technical"
        })]
        public virtual string FeasibilityStudyType { get; set; }
        public abstract class feasibilityStudyType : PX.Data.BQL.BqlString.Field<feasibilityStudyType> { }
        #endregion

        #region FeasibilityStudyFinding
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Feasibility Study Finding")]
        public virtual string FeasibilityStudyFinding { get; set; }
        public abstract class feasibilityStudyFinding : PX.Data.BQL.BqlString.Field<feasibilityStudyFinding> { }
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
}