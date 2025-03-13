using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    [Serializable]
    [PXCacheName(Messages.Calibration)]
    public class CalibrationRecord : IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<CalibrationRecord>.By<refNbr>
        {
            public static CalibrationRecord Find(PXGraph graph, int refNbr, PKFindOptions options = PKFindOptions.None) => FindBy(graph, refNbr, options);
        }
		public static class FK
		{
			
		}
        #endregion

        #region RefNbr
        public abstract class refNbr : BqlInt.Field<refNbr> { }

		[PXDBInt(IsKey = true)]
		[PXIntList(
			new int[]
			{
				1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19,20, 21, 22
			},
			new string[]
			{
				"AVO01", "AVO02", "AVO03", "AVO04", "AVO05", "AVO06", "AVO07", "AVO08", "AVO09", "AVO10", "AVO11", "AVO12", "AVO13", "AVO14", "AVO15", "NPT01", "NPT02", "NPT03", "NPT04", "NPT05", "NPT06", "NPT07"
            })]
		[PXUIField(DisplayName = "Ref Nbr.")]
		public virtual int? RefNbr
		{
			get;
			set;
		}
		#endregion

		#region ItemType
		public abstract class itemType : BqlString.Field<itemType> { }

		[PXDBString(2, IsUnicode = true)]
		[PXStringList(
            new string[]
            {
                "WS", "VC", "5W", "SB", "SE","GS", "GP", "FG"
            },
            new string[]
            {
                "Weighing Scales", "Vernier Callipers", "500g Weight", "40mm Steel Gauge Block", "Precision Straight Edge", "Granite Surface Plate - 450mm x 450mm", "Granite Surface Plate - 1000mm x 1000mm", "Feeler Gauge"
            })]
        [PXUIField(DisplayName = "Item Type")]
		public virtual string ItemType
		{
			get;
			set;
		}
		#endregion

		#region Model
		public abstract class model : BqlString.Field<model> { }

		[PXDBString(16, IsUnicode = true)]
		[PXUIField(DisplayName = "Model")]
		public virtual string Model
		{
			get;
			set;
		}
		#endregion

		#region SerialNbr
		public abstract class serialNbr : BqlString.Field<serialNbr> { }

		[PXDBString(16, IsUnicode = true)]
		[PXUIField(DisplayName = "Serial Nbr.")]
		public virtual string SerialNbr
		{
			get;
			set;
		}
		#endregion

		#region InternalSchedule
		public abstract class internalSchedule : BqlString.Field<internalSchedule> { }

		[PXDBString(1, IsUnicode = true)]
		[PXStringList(
			new string[]
			{
				"M", "4", "6"
			},
			new string[]
			{
				"Month", "4 Months", "6 Months"
			})]
		[PXUIField(DisplayName = "Internal Schedule")]
		public virtual string InternalSchedule
		{
			get;
			set;
		}
		#endregion

		#region Comments
		public abstract class comments : BqlString.Field<comments> { }

		[PXDBString(256, IsUnicode = true)]
		[PXUIField(DisplayName = "Comments")]
		public virtual string Comments
		{
			get;
			set;
		}
		#endregion

		#region ExternalCert
		public abstract class externalCert : BqlString.Field<externalCert> { }

		[PXDBString(1, IsUnicode = true)]
		[PXStringList(
			new string[]
			{
				"1", "5"
			},
			new string[]
			{
				"1 year", "5 years"
			})]
		[PXUIField(DisplayName = "External Certification")]
		public virtual string ExternalCert
		{
			get;
			set;
		}
		#endregion

		#region CertNbr
		public abstract class certNbr : BqlString.Field<certNbr> { }

		[PXDBString(32, IsUnicode = true)]
		[PXUIField(DisplayName = "CertNbr")]
		public virtual string CertNbr
		{
			get;
			set;
		}
		#endregion

		#region LastChecked
		public abstract class lastChecked : BqlDateTime.Field<lastChecked> { }

		[PXDBDate()]
		[PXDefault(typeof(AccessInfo.businessDate), PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "Last Checked")]
		public virtual DateTime? LastChecked
		{
			get;
			set;
		}
		#endregion


		#region NextDue
		public abstract class nextDue : BqlDateTime.Field<nextDue> { }

		[PXDBDate()]
		[PXUIField(DisplayName = "NextDue")]
		public virtual DateTime? NextDue
		{
			get;
			set;
		}
        #endregion

        #region SiteID
        public abstract class siteID : BqlInt.Field<siteID> { }

        [Site]
        [PXUIField(DisplayName = "Site")]
        public virtual int? SiteID
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
