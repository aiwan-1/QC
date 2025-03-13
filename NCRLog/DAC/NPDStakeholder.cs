using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.EP;
using static NCRLog.NPDDesignMatlCost;

namespace NCRLog
{
    [Serializable]
    [PXCacheName("NPDStakeholder")]
    public class NPDStakeholder : IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<NPDStakeholder>.By<projectNo, productTitle, stakeholderID>
        {
            public static NPDStakeholder Find(PXGraph graph, string projectNo, string productTitle, int stakeholderID, PKFindOptions options = PKFindOptions.None) => FindBy(graph, projectNo, productTitle, stakeholderID, options);
        }
        public static class FK
        {
            public class Approval : NPDHeader.PK.ForeignKeyOf<NPDStakeholder>.By<projectNo, productTitle> { }
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

        #region StakeholderID
        [PXDBString(64, IsKey = true)]
        [PXDefault]
        [PXSelector(
            typeof(EPEmployee.acctName),
            typeof(EPEmployee.departmentID))]
        [PXUIField(DisplayName = "Stakeholder ID")]
        public virtual string StakeholderID { get; set; }
        public abstract class stakeholderID : PX.Data.BQL.BqlString.Field<stakeholderID> { }
        #endregion

        #region FirstName
        [PXDBString(64, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "First Name")]
        public virtual string FirstName { get; set; }
        public abstract class firstName : PX.Data.BQL.BqlString.Field<firstName> { }
        #endregion

        #region LastName
        [PXDBString(64, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Last Name")]
        public virtual string LastName { get; set; }
        public abstract class lastName : PX.Data.BQL.BqlString.Field<lastName> { }
        #endregion

        #region Role
        [PXDBString(64, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Role")]
        public virtual string Role { get; set; }
        public abstract class role : PX.Data.BQL.BqlString.Field<role> { }
        #endregion

        #region Responsibility
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Responsibility")]
        public virtual string Responsibility { get; set; }
        public abstract class responsibility : PX.Data.BQL.BqlString.Field<responsibility> { }
        #endregion

        #region Interests
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Interests")]
        public virtual string Interests { get; set; }
        public abstract class interests : PX.Data.BQL.BqlString.Field<interests> { }
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