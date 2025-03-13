using PX.Data;
using PX.Data.BQL;
using PX.Objects.CS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NCRLog.ISOSetup;

namespace NCRLog
{
	[PXCacheName(Messages.ISOSetup)]
	[PXPrimaryGraph(typeof(QualityControlSetupMaint))]
	public class ISOSetup : IBqlTable
    {
        #region AutoNumberingType
		public abstract class autoNumberingType : BqlString.Field<autoNumberingType> { }
		[PXDBString(25, IsUnicode = true)]
		[PXSelector(typeof(Numbering.numberingID),
			DescriptionField = typeof(Numbering.descr))]
        [PXDefault("ISO")]
		[PXUIField(DisplayName = "Auto Numbering Type (ISO Records)")]
		public virtual string AutoNumberingType
        {
			get;
			set;
		}
        #endregion

        #region AutoNumberingCalib
        public abstract class autoNumberingCalib : BqlString.Field<autoNumberingCalib> { }
        [PXDBString(25, IsUnicode = true)]
        [PXSelector(typeof(Numbering.numberingID),
            DescriptionField = typeof(Numbering.descr))]
        [PXDefault("Calib")]
        [PXUIField(DisplayName = "Auto Numbering Type (Calibration)")]
        public virtual string AutoNumberingCalib
        {
            get;
            set;
        }
        #endregion

        #region AutoNumberingPGLog
        public abstract class autoNumberingPGLog : BqlString.Field<autoNumberingPGLog> { }
        [PXDBString(25, IsUnicode = true)]
        [PXSelector(typeof(Numbering.numberingID),
            DescriptionField = typeof(Numbering.descr))]
        [PXDefault("PGLog")]
        [PXUIField(DisplayName = "Auto Numbering Type (Press & Glue Log)")]
        public virtual string AutoNumberingPGLog
        {
            get;
            set;
        }
        #endregion

        #region AutoNumberingQC
        public abstract class autoNumberingQC : BqlString.Field<autoNumberingQC> { }
        [PXDBString(25, IsUnicode = true)]
        [PXSelector(typeof(Numbering.numberingID),
            DescriptionField = typeof(Numbering.descr))]
        [PXDefault("QC")]
        [PXUIField(DisplayName = "Auto Numbering Type (End of Line Checks)")]
        public virtual string AutoNumberingQC
        {
            get;
            set;
        }
        #endregion

        #region AutoNumberingNPDApproval
        public abstract class autoNumberingNPDApproval : BqlString.Field<autoNumberingNPDApproval> { }
        [PXDBString(25, IsUnicode = true)]
        [PXSelector(typeof(Numbering.numberingID),
            DescriptionField = typeof(Numbering.descr))]
        [PXDefault("NPDAPPROVE")]
        [PXUIField(DisplayName = "Auto Numbering Type (NPD Approval)")]
        public virtual string AutoNumberingNPDApproval
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
