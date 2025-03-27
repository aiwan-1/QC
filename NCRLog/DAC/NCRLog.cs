using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Objects.AP;
using PX.Objects.CR;
using PX.Objects.AM;
using PX.Objects.PO;
using PX.Data.BQL.Fluent;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.EP;

namespace NCRLog
{
    [Serializable]
    [PXCacheName(Messages.NCRLog)]
    public class NCRLog : PXBqlTable, IBqlTable
    { 
      #region Keys
       public class PK : PrimaryKeyOf<NCRLog>.By<nCRNumber, docType> 
       {
            public static NCRLog Find(PXGraph graph, string nCRNumber, string docType, PKFindOptions options = PKFindOptions.None) => FindBy(graph, nCRNumber, docType, options);
       }
       public static class FK
       {
            public class ISO : ISORecord.PK.ForeignKeyOf<NCRLog>.By<nCRNumber, docType> { }
       }
       #endregion

       #region NCRNumber
       public abstract class nCRNumber : BqlString.Field<nCRNumber> { }
       [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
       [PXDBDefault(typeof(ISORecord.docNumber))]
       [PXParent(typeof(FK.ISO))]
       public virtual string NCRNumber
       { get; set; }
       #endregion

        #region DocType
        public abstract class docType : BqlString.Field<docType> { }
        [PXDBString(3, IsKey = true, IsUnicode = true)]
        [PXDBDefault(typeof(ISORecord.docType))]
        [PXParent(typeof(FK.ISO))]
        public virtual string DocType
        { get; set; }
        #endregion

        #region CustomerID
        public abstract class customerID : BqlInt.Field<customerID> { }
        [PXDBInt()]
        [PXSelector(
        typeof(Customer.bAccountID),
        typeof(Customer.acctCD),
        typeof(Customer.acctName),
        SubstituteKey = typeof(Customer.acctName))]
        [PXUIField(DisplayName = "Customer ID")]
        public virtual int? CustomerID
        { get; set; }
        #endregion

      #region VendorID
      public abstract class vendorID : BqlInt.Field<vendorID> { }

      [PXDBInt()]
      [PXUIField(DisplayName = "VendorID")]
      [PXSelector(
        typeof(Vendor.bAccountID),
        typeof(Vendor.acctCD),
        typeof(Vendor.acctName),
        SubstituteKey = typeof(Vendor.acctName))]
      public virtual int? VendorID
      {
        get;
        set;
      }
      #endregion


      #region SOOrderNbr
      public abstract class sOOrderNbr : BqlString.Field<sOOrderNbr> { }
      [PXDBString(30, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
      [PXSelector(
        typeof(SOOrder.orderNbr),
        typeof(SOOrder.orderType),
        typeof(SOOrder.customerID),
        typeof(SOOrder.orderDesc))]
      [PXUIField(DisplayName = "SO Order Nbr.")]
      public virtual string SOOrderNbr
      { get; set; }
        #endregion

        #region SOOrderType
        public abstract class sOOrderType : BqlString.Field<sOOrderType> { }
        [PXDBString(15, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXSelector(
          typeof(SOOrder.orderType),
          typeof(SOOrder.orderNbr),
          typeof(SOOrder.customerID),
          typeof(SOOrder.orderDesc))]
        [PXUIField(DisplayName = "SO Order Nbr.", Visible = false)]
        public virtual string SOOrderType
        { get; set; }
        #endregion

        #region POOrderNbr
        public abstract class pOOrderNbr : BqlString.Field<pOOrderNbr> { }
        [PXDBString(15, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXUIField(DisplayName = "PO Order Nbr.")]
        [PXSelector(
            typeof(POOrder.orderNbr),
            typeof(POOrder.orderType),
            typeof(POOrder.vendorID),
            typeof(POOrder.orderDesc),
            SubstituteKey = typeof(POOrder.orderNbr))]
        public virtual string POOrderNbr { get; set; }
        #endregion

      #region CustomerPONbr
        public abstract class customerPONbr : BqlString.Field<customerPONbr> { }
        [PXDBString(75, IsUnicode = true)]
        [PXUIField(DisplayName = "Customer PO Nbr.")]
        public virtual string CustomerPONbr
        {
          get;
          set;
        }
        #endregion


      #region AMProdOrdID
      public abstract class aMProdOrdID : BqlString.Field<aMProdOrdID> { }
      [PXSelector(
        typeof(AMProdItem.prodOrdID),
        typeof(AMProdItem.ordNbr),
        typeof(AMProdItem.orderType))]
      [PXDBString(15, IsUnicode = true, InputMask =">CCCCCCCCCCCCCCC")]
      [PXUIField(DisplayName = "Production Order ID")]
      public virtual string AMProdOrdID
        { get; set; }
      #endregion

      


      #region Description
        public abstract class description : BqlString.Field<description> { }
      [PXDBText(IsUnicode = true)]
      [PXUIField(DisplayName = "Description")]
      public virtual string Description
        { get; set; }
      #endregion

      #region CrctveAction
      public abstract class crctveAction : BqlString.Field<crctveAction> { }
      [PXDBText(IsUnicode = true)]
        [PXUIField(DisplayName = "Corrective Action")]
      public virtual string CrctveAction
        { get; set; }
      #endregion

      #region ContactName
      public abstract class contactName : BqlString.Field<contactName> { }
      [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Contact")]
      public virtual string ContactName
        { get; set; }
        #endregion

      #region ContactEmail
        public abstract class contactEmail : BqlString.Field<contactEmail> { }
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Contact Email")]
        public virtual string ContactEmail
        { get; set; }
        #endregion

      #region ContactPNbr
        public abstract class contactPNbr : BqlString.Field<contactPNbr> { }
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Contact Phone Number")]
        public virtual string ContactPNbr
        { get; set; }
        #endregion

      #region Owner
        public abstract class owner : BqlString.Field<owner> { }
        [PXDBString(50, IsUnicode = true)]
      [PXSelector(typeof(CREmployee.acctCD),
        typeof(CREmployee.acctName),
        SubstituteKey = typeof(CREmployee.acctName))]
        [PXUIField(DisplayName = "NCR Owner")]
        public virtual string Owner
        { get; set; }
        #endregion

      #region Code
        public abstract class code : BqlString.Field<code> { }
      [PXDBString(1, IsUnicode = true)]
            [PXStringList(
               new string[]
               {
                 "A",
                 "B",
                 "C",
                 "D",
                 "E",
                 "F",
                 "G",
                 "H",
                 "I",
                 "J",
                 "K",
                 "L"
               },
               new string[]
               {
                       "Defective Goods",
                       "Damaged on Delivery",
                       "Missing Items",
                       "Late Delivery",
                       "Incorrect Items",
                       "Fault with Equipment",
                       "Drawing Interpretation",
                       "Admin/Drawing Issue",
                       "Scratches/Damage",
                       "Panel Not Aligned",
                       "Out of Tolerance",
                       "Delamination"
                   })]
            [PXUIField(DisplayName = "Code", Visible = false)]
      public virtual string Code
        { get; set; }
      #endregion

      #region NCRType
      public abstract class nCRType : BqlString.Field<nCRType> { }
      [PXDBString(1, IsUnicode = true)]
      [PXStringList(
        new string[]
        {
          "a",
          "b",
          "c"
        },
        new string[]
        {
          "Customer Complaint",
          "Internal Failure",
          "Supplier Issue"
        })]
      [PXUIField(DisplayName = "NCR Type", Visible = false)]
      public virtual string NCRType
        { get; set; }
        #endregion

        #region Why1
        public abstract class why1 : BqlString.Field<why1> { }

        [PXDBString(128, IsUnicode = true)]
        [PXUIField(DisplayName = "Why 1")]
        public virtual string Why1
        {
            get;
            set;
        }
        #endregion

        #region Why2
        public abstract class why2 : BqlString.Field<why2> { }

        [PXDBString(128, IsUnicode = true)]
        [PXUIField(DisplayName = "Why 2")]
        public virtual string Why2
        {
            get;
            set;
        }
        #endregion

        #region Why3
        public abstract class why3 : BqlString.Field<why3> { }

        [PXDBString(128, IsUnicode = true)]
        [PXUIField(DisplayName = "Why 3")]
        public virtual string Why3
        {
            get;
            set;
        }
        #endregion

        #region Why4
        public abstract class why4 : BqlString.Field<why4> { }

        [PXDBString(128, IsUnicode = true)]
        [PXUIField(DisplayName = "Why 4")]
        public virtual string Why4
        {
            get;
            set;
        }
        #endregion

        #region Why5
        public abstract class why5 : BqlString.Field<why5> { }

        [PXDBString(128, IsUnicode = true)]
        [PXUIField(DisplayName = "Why 5")]
        public virtual string Why5
        {
            get;
            set;
        }
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