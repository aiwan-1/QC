using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.GL;
using static NCRLog.ISO;
using PX.Objects.CS;
using PX.Objects.AR;
using PX.Data.EP;
using PX.TM;


namespace NCRLog
{
	[PXCacheName(Messages.ISO)]
    [PXPrimaryGraph(typeof(GilcrestMaint))]
	public class ISORecord : IBqlTable, IAssign
	{
        #region Keys
        public class PK : PrimaryKeyOf<ISORecord>.By<docNumber, docType> 
        {
            public static ISORecord Find(PXGraph graph, string docNumber, string docType, PKFindOptions options = PKFindOptions.None) => FindBy(graph, docNumber, docType, options);
        }
        public static class FK
        {
            public class Workgroup : EPCompanyTree.PK.ForeignKeyOf<ISORecord>.By<workGroupID> { }
            public class Order : SOOrder.PK.ForeignKeyOf<ISORecord>.By<sOOrderNbr, customerID> { }
        }
        #endregion 

        #region DocNumber
        public abstract class docNumber : BqlString.Field<docNumber> { }
		[PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [AutoNumber(typeof(ISOSetup.autoNumberingType),
            typeof(ISORecord.date))]
        /*[PXSelector(typeof(ISORecord.docNumber),
            typeof(ISORecord.docType))]*/
		[PXUIField(DisplayName = "Doc Number")]
		public virtual string DocNumber
        { get; set; }
        #endregion

        #region Status
        public abstract class status : BqlString.Field<status> { }
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Status", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        [ISORecordsStatus.List]
        public virtual string Status { get; set; }
        #endregion

        #region DocType
        public abstract class docType : BqlString.Field<docType> { }
		[PXDBString(3, IsKey = true, IsUnicode = true)]
        [PXStringList(
			 new string[]
			  {
			   "ECN",
			   "NCR"
			  },
		     new string[]
			  {
			    Messages.ECN,
				Messages.NCR
			  })]
        [PXUIField(DisplayName = "Doc Type")]
		public virtual string DocType
        { get; set; }
        #endregion

        #region Date
        public abstract class date : BqlDateTime.Field<date> { }
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXDBDate()]
        [PXUIField(DisplayName = "Date of NCR")]
        public virtual DateTime? Date
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
        [PXUIField(DisplayName = "Code", Enabled = false)]
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
        [PXUIField(DisplayName = "NCR Type", Enabled = false)]
        public virtual string NCRType
        { get; set; }
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
        [PXUIField(DisplayName = "ECN Type", Enabled = false)]
        public virtual string ECNType { get; set; }
        #endregion

        #region OwnerID
        public abstract class ownerID : BqlInt.Field<ownerID> { }
        [PX.TM.Owner]
        [PXUIField(DisplayName = "Owner")]
        
        public virtual int? OwnerID
        { get; set; }
        #endregion

        #region SOOrderNbr
        public abstract class sOOrderNbr : BqlString.Field<sOOrderNbr> { }
        [PXDBString(15, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXSelector(typeof(SOOrder.orderNbr),
          typeof(SOOrder.customerID),
          typeof(SOOrder.orderDesc))]
        [PXUIField(DisplayName = "SO Order Nbr.")]
        public virtual string SOOrderNbr
        { get; set; }
        #endregion

        #region CustomerID
        public abstract class customerID : BqlInt.Field<customerID> { }
        [Customer]
        [PXUIField(DisplayName = "Customer")]
        public virtual int? CustomerID
        { get; set; }
        #endregion

        #region WorkGroupID
        public abstract class workGroupID : BqlInt.Field<workGroupID> { }
        [PXDBInt()]
        [PXUIField(DisplayName = "Work Group ID")]
        [PXSelector(typeof(Search<EPCompanyTree.workGroupID>), DescriptionField = typeof(EPCompanyTree.description))]
        public virtual int? WorkGroupID { get; set; }
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

        #region IAssign Members
        int? PX.Data.EP.IAssign.WorkgroupID
        {
            get { return WorkGroupID;  }
            set { WorkGroupID = value; }
        }
        #endregion

    }

    public class ISO
    {


        public class ISORecordsStatus
        {
            public class ListAttribute : PXStringListAttribute
            {
                public static readonly (string, string)[] ValuesToLabels = new[]
                {
                    (Hold, "On Hold"),
                    (Awaiting, "Awaiting Review"),
                    (Open, "Open"),
                    (Close, "Closed")
                };
                public ListAttribute() : base(ValuesToLabels) { }
            }
        }

        public const string Hold = "H";
        public const string Awaiting = "A";
        public const string Open = "O";
        public const string Close = "C";    

        public class AutoNumberingSequence
        {
            public class AutoNbrAttribute : PXSelectorAttribute
            {
                public AutoNbrAttribute(Type SearchType) : base(SearchType,
                    typeof(ISORecord.docType),
                    typeof(ISORecord.docNumber),
                    typeof(ISORecord.status))
                { }
            }
            public class NumberingAttribute : AutoNumberAttribute
            {
                public NumberingAttribute() : base(typeof(Search<ISORecord.docNumber>), typeof(AccessInfo.businessDate)) { }
            }
        }

       

    }
}

