using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Objects.AP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.EP;
using PX.TM;

namespace NCRLog
{
	[PXCacheName(Messages.ECNLog)]
	public class ECNLog : PXBqlTable, IBqlTable
    { 
        #region Keys
        public class PK : PrimaryKeyOf<ECNLog>.By<nCRNumber, docType> 
        {
            public static ECNLog Find(PXGraph graph, string nCRNumber, string docType, PKFindOptions options = PKFindOptions.None) => FindBy(graph, nCRNumber, docType, options);
        }
        public static class FK
        {
            public class ISO : ISORecord.PK.ForeignKeyOf<ISORecord>.By<nCRNumber, docType> { }
        }
        #endregion


        #region NCRNumber
        public abstract class nCRNumber : BqlString.Field<nCRNumber> { }
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXDBDefault(typeof(ISORecord.docNumber))]
        [PXParent(typeof(FK.ISO))]
        public string NCRNumber { get; set; }
        #endregion

        #region DocType
        public abstract class docType : BqlString.Field<docType> { }
        [PXDBString(3, IsKey = true, IsUnicode = true)]
        [PXDBDefault(typeof(ISORecord.docType))]
        [PXParent(typeof(FK.ISO))]
        public virtual string DocType { get; set; }
        #endregion

        #region EffectiveProd
        public abstract class effectiveProd : BqlString.Field<effectiveProd> { }
        [PXDBString(1024, IsUnicode = true)]
        [PXUIField(DisplayName = "Effective Product/s")]
        public virtual string EffectiveProd { get; set; }
        #endregion

        #region ECNType
        public abstract class eCNType : BqlString.Field<eCNType> { }
        [PXDBString(1, IsUnicode = true)]
        [PXStringList(
            new string[]
            {
                "F",
                "C",
                "D"
            },
            new string[]
            {
                Messages.F,
                Messages.C,
                Messages.D
            })]
        [PXUIField(DisplayName = "ECN Type")]
        public virtual string ECNType { get; set; }
        #endregion

        #region VendorID
        public abstract class vendorID : BqlInt.Field<vendorID> { }
        [PXDBInt()]
        [PXSelector(
            typeof(Vendor.bAccountID),
            typeof(Vendor.acctCD),
            typeof(Vendor.acctName),
            SubstituteKey = typeof(Vendor.acctName))]
        [PXUIField(DisplayName = "VendorID")]
        public virtual int? VendorID { get; set; }
        #endregion

        #region VPartNo
        public abstract class vPartNo : BqlString.Field<vPartNo> { }
        [PXDBString(128, IsUnicode = true)]
        [PXUIField(DisplayName = "Vendor Part Number")]
        public virtual string VPartNo { get; set; }
        #endregion

        #region SRfC
        public abstract class sRfC : BqlString.Field<sRfC> { }
        [PXDBText(IsUnicode = true)]
        [PXUIField(DisplayName = "Supplier Reason for Change")]
        public virtual string SRfC { get; set; }
        #endregion

        #region SRisks
        public abstract class sRisks : BqlString.Field<sRisks> { }
        [PXDBText(IsUnicode = true)]
        [PXUIField(DisplayName = "Risks Associated")]
        public virtual string SRisks { get; set; }
        #endregion

        #region InventoryID
        public abstract class inventoryID : BqlInt.Field<inventoryID> { }
        [Inventory]
        [PXUIField(DisplayName = "Existing or Modified Part")]
        public virtual int? InventoryID { get; set; }
        #endregion

        #region IRfC
        public abstract class iRfC : BqlString.Field<iRfC> { }
        [PXDBText(IsUnicode = true)]
        [PXUIField(DisplayName = "Internal Reason for Change")]
        public virtual string IRfC { get; set; }
        #endregion

        #region Cost
        public abstract class cost : BqlString.Field<cost> { }
        [PXDBString(128, IsUnicode = true)]
        [PXUIField(DisplayName = "Cost of Implementation")]
        public virtual string Cost { get; set; }
        #endregion

        #region SignOff
        public abstract class signOff : BqlString.Field<signOff> { }
        [PXDBString(64, IsUnicode = true)]
        [PXUIField(DisplayName = "Signed Off By")]
        public virtual string SignOff { get; set; }
        #endregion

        #region System Fields

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime :
        PX.Data.BQL.BqlDateTime.Field<createdDateTime>
        { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID :
        PX.Data.BQL.BqlGuid.Field<createdByID>
        { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID :
        PX.Data.BQL.BqlString.Field<createdByScreenID>
        { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime :
        PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime>
        { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID :
        PX.Data.BQL.BqlGuid.Field<lastModifiedByID>
        { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID :
        PX.Data.BQL.BqlString.Field<lastModifiedByScreenID>
        { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp :
        PX.Data.BQL.BqlByteArray.Field<tstamp>
        { }
        #endregion

        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion

        #endregion


    }
}
