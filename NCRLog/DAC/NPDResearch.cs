using System;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using static NCRLog.NPDRisks;

namespace NCRLog
{
    [Serializable]
    [PXCacheName("NPDResearch")]
    public class NPDResearch : PXBqlTable, IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<NPDResearch>.By<projectNo, productTitle, findingID>
        {
            public static NPDResearch Find(PXGraph graph, string projectNo, string productTitle, int findingID, PKFindOptions options = PKFindOptions.None) => FindBy(graph, projectNo, productTitle, findingID, options);
        }
        public static class FK
        {
            public class Approval : NPDHeader.PK.ForeignKeyOf<NPDResearch>.By<projectNo, productTitle> { }
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

        #region RiskLineCntr
        public abstract class riskLineCntr : BqlInt.Field<riskLineCntr> { }

        [PXDBInt]
        [PXDefault(0)]
        public virtual int? RiskLineCntr
        {
            get;
            set;
        }
        #endregion


        #region FindingID
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXLineNbr(typeof(NPDHeader.findingCount))]
        [PXUIField(DisplayName = "Finding ID")]
        public virtual int? FindingID { get; set; }
        public abstract class findingID : PX.Data.BQL.BqlInt.Field<findingID> { }
        #endregion

        #region FindingDescription
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Finding Description")]
        public virtual string FindingDescription { get; set; }
        public abstract class findingDescription : PX.Data.BQL.BqlString.Field<findingDescription> { }
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