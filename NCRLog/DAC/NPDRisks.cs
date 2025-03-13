using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.BQL.Fluent;
using static NCRLog.NPDStakeholder;

namespace NCRLog
{
    [Serializable]
    [PXCacheName("NPDRisks")]
    public class NPDRisks : IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<NPDRisks>.By<projectNo, productTitle, findingID, riskid>
        {
            public static NPDRisks Find(PXGraph graph, string projectNo, string productTitle, int findingID, int riskid, PKFindOptions options = PKFindOptions.None) => FindBy(graph, projectNo, productTitle, findingID, riskid, options);
        }
        public static class FK
        {
            public class Finding : NPDResearch.PK.ForeignKeyOf<NPDRisks>.By<projectNo, productTitle, findingID> { }
        }
        #endregion

        #region ProjectNo
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Project No")]
        [PXDBDefault(typeof(NPDResearch.projectNo))]
        [PXParent(typeof(FK.Finding))]
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

        #region FindingID
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXSelector(typeof(SearchFor<NPDResearch.findingID>.
            Where<NPDResearch.projectNo.IsEqual<NPDRisks.projectNo.FromCurrent>.
                And<NPDResearch.productTitle.IsEqual<NPDRisks.productTitle.FromCurrent>>>),
            typeof(NPDResearch.findingID),
            typeof(NPDResearch.findingDescription))]
        [PXUIField(DisplayName = "Finding ID")]
        public virtual int? FindingID { get; set; }
        public abstract class findingID : PX.Data.BQL.BqlInt.Field<findingID> { }
        #endregion

        #region Riskid
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXLineNbr(typeof(NPDResearch.riskLineCntr))]
        [PXUIField(DisplayName = "Risk ID", Enabled = false)]
        public virtual int? Riskid { get; set; }
        public abstract class riskid : PX.Data.BQL.BqlInt.Field<riskid> { }
        #endregion

        #region RiskDescription
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Risk Description")]
        public virtual string RiskDescription { get; set; }
        public abstract class riskDescription : PX.Data.BQL.BqlString.Field<riskDescription> { }
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