using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.IN;

namespace NCRLog
{
    [Serializable]
    [PXCacheName("NPDDesignMatlCost")]
    public class NPDDesignMatlCost : IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<NPDDesignMatlCost>.By<projectNo, productTitle, matlLineNbr>
        {
            public static NPDDesignMatlCost Find(PXGraph graph, string projectNo, string productTitle, int matlLineNbr, PKFindOptions options = PKFindOptions.None) => FindBy(graph, projectNo, productTitle, matlLineNbr, options);
        }
        public static class FK
        {
            public class Approval : NPDHeader.PK.ForeignKeyOf<NPDDesignMatlCost>.By<projectNo, productTitle> { }
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

        #region MatlLineNbr
        [PXDBInt(IsKey = true)]
        [PXDefault]
        [PXLineNbr(typeof(NPDHeader.matlLineCount))]
        [PXUIField(DisplayName = "Matl Line Nbr")]
        public virtual int? MatlLineNbr { get; set; }
        public abstract class matlLineNbr : PX.Data.BQL.BqlInt.Field<matlLineNbr> { }
        #endregion

        #region InventoryID
        [Inventory]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region Non-InventoryID
        [PXDBString(32, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Non- Inventory ID")]
        public virtual string NonInventoryID { get; set; }
        public abstract class nonInventoryID : PX.Data.BQL.BqlString.Field<nonInventoryID> { }
        #endregion

        #region UnitCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Unit Cost")]
        public virtual Decimal? UnitCost { get; set; }
        public abstract class unitCost : PX.Data.BQL.BqlDecimal.Field<unitCost> { }
        #endregion

        #region ExtCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Ext Cost")]
        [PXFormula(typeof(Mult<unitCost,quantity>))]
        public virtual Decimal? ExtCost { get; set; }
        public abstract class extCost : PX.Data.BQL.BqlDecimal.Field<extCost> { }
        #endregion

        #region Quantity
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Quantity")]
        public virtual Decimal? Quantity { get; set; }
        public abstract class quantity : PX.Data.BQL.BqlDecimal.Field<quantity> { }
        #endregion

        #region Uom
        [PXDBString(6, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Unit of Measure")]
        public virtual string Uom { get; set; }
        public abstract class uom : PX.Data.BQL.BqlString.Field<uom> { }
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