using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using System;
using PX.Objects.SO;
using GILCustomizations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.AM;
using PX.Data.BQL.Fluent;

namespace NCRLog
{
    [PXCacheName(Messages.QCDetails)]
    public class GCQCLine : PXBqlTable, IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<GCQCLine>.By<docNbr, batchNbr, lineNbr>
        {
            public static GCQCLine Find(PXGraph graph, string docNbr, string batchNbr, int? lineNbr, PKFindOptions options = PKFindOptions.None) => FindBy(graph, docNbr, batchNbr, lineNbr, options);
        }
        public static class FK
        {
            public class Quality : GCQCRecord.PK.ForeignKeyOf<GCQCLine>.By<docNbr, batchNbr> { }
            public class Move : AMMTran.PK.ForeignKeyOf<GCQCLine>.By<batchNbr, aMProdOrdID, lineNbr > { }
        }
        #endregion

        #region DocNbr
        public abstract class docNbr : BqlString.Field<docNbr> { }
        [PXDBDefault(typeof(GCQCRecord.docNbr))]
        [PXParent(typeof(FK.Quality))]
        [PXDBString(15, IsKey = true, IsUnicode = true)]
        [PXUIField(DisplayName = "Doc Nbr", Enabled = false, Visible = false)]
        public virtual string DocNbr
        {
            get;
            set;
        }
        #endregion


        #region BatchNbr
        public abstract class batchNbr : BqlString.Field<batchNbr> { }
        [PXDBString(15, IsKey = true, IsUnicode = true)]
        [PXDBDefault(typeof(GCQCRecord.batchNbr))]
        [PXUIField(DisplayName = "Batch Nbr")]
        public virtual string BatchNbr
        {
            get;
            set;
        }
        #endregion

        #region LineNbr
        public abstract class lineNbr : BqlInt.Field<lineNbr> { }

        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(GCQCRecord.qCLine))]
        [PXUIField(DisplayName = "Line Nbr", Enabled = false, Visible = false)]
        public virtual int? LineNbr
        {
            get;
            set;
        }
        #endregion

        #region TranDate
        public abstract class tranDate : BqlDateTime.Field<tranDate> { }
        [PXDBDate()]
        [PXDBDefault(typeof(GCQCRecord.date))]
        [PXUIField(DisplayName = "Tran Date")]
        public virtual DateTime? TranDate
        {
            get;
            set;
        }
        #endregion


        #region AMProdOrdID
        public abstract class aMProdOrdID : BqlString.Field<aMProdOrdID> { }
        [PXDBString(15, IsUnicode = true)]
        [PXSelector(typeof(AMProdItem.prodOrdID))]
        [PXUIField(DisplayName = "Production Order")]
        public virtual string AMProdOrdID
        {
            get;
            set;
        }
        #endregion


        #region Thickness
        public abstract class thickness : BqlString.Field<thickness> { }

        [PXDBString(25, IsUnicode = true)]
        [PXUIField(DisplayName = "Panel Thickness")]
        public virtual string Thickness
        {
            get;
            set;
        }
        #endregion

        #region Lengthmm
        public abstract class lengthmm : BqlString.Field<lengthmm> { }

        [PXDBString(25, IsUnicode = true)]
        [PXUIField(DisplayName = "Length (mm)")]
        public virtual string Lengthmm
        {
            get;
            set;
        }
        #endregion

        #region Widthmm
        public abstract class widthmm : BqlString.Field<widthmm> { }

        [PXDBString(25, IsUnicode = true)]
        [PXUIField(DisplayName = "Width (mm)")]
        public virtual string Widthmm
        {
            get;
            set;
        }
        #endregion

        #region DiagonalCheck
        public abstract class diagonalCheck : BqlString.Field<diagonalCheck> { }

        [PXDBString(25, IsUnicode = true)]
        [PXUIField(DisplayName = "Diagonal Check")]
        public virtual string DiagonalCheck
        {
            get;
            set;
        }
        #endregion

        #region Cleaning
        public abstract class cleaning : BqlString.Field<cleaning> { }

        [PXDBString(25, IsUnicode = true)]
        [PXUIField(DisplayName = "Cleaning")]
        public virtual string Cleaning
        {
            get;
            set;
        }
        #endregion

        #region CustomerSpecific
        public abstract class customerSpecific : BqlString.Field<customerSpecific> { }

        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Customer Specific")]
        public virtual string CustomerSpecific
        {
            get;
            set;
        }
        #endregion

        #region DailyBondTest
        public abstract class dailyBondTest : BqlString.Field<dailyBondTest> { }

        [PXDBString(128, IsUnicode = true)]
        [PXUIField(DisplayName = "Daily Bond Test")]
        public virtual string DailyBondTest
        {
            get;
            set;
        }
        #endregion

        #region PanelRef
        public abstract class panelRef : BqlString.Field<panelRef> { }
        [PXDBString(25, IsUnicode = true)]
        [PXUIField(DisplayName = "Panel Ref.")]
        public virtual string PanelRef
        {
            get;
            set;
        }
        #endregion

        #region QCPass
        public abstract class qCPass : BqlBool.Field<qCPass> { }

        [PXDBBool]
        [PXUIField(DisplayName = "QC Pass?")]
        public virtual bool? QCPass
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