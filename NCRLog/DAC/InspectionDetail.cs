using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
	[PXCacheName(Messages.Inspection)]
	public class InspectionDetail : IBqlTable
	{
        #region Keys
        public class PK : PrimaryKeyOf<InspectionDetail>.By<docNbr, lineNbr>
        {
            public static InspectionDetail Find(PXGraph graph, string docNbr, int lineNbr, PKFindOptions options =  PKFindOptions.None) => FindBy(graph, docNbr, lineNbr, options); 
        }
        public static class FK
        {
            public class Header : CalibrationHeader.PK.ForeignKeyOf<InspectionDetail>.By<docNbr> { }
            public class Record : CalibrationRecord.PK.ForeignKeyOf<InspectionDetail>.By<refNbr> { }
        }

        #endregion

        #region DocNbr
        public abstract class docNbr : BqlString.Field<docNbr> { }

		[PXDBString(15, IsKey = true, IsUnicode = true)]
        [PXDBDefault(typeof(CalibrationHeader.docNbr))]
        [PXParent(typeof(FK.Header))]
        [PXUIField(DisplayName = "Doc Nbr")]
		public virtual string DocNbr
		{
			get;
			set;
		}
        #endregion

        #region RefNbr
        public abstract class refNbr : BqlInt.Field<refNbr> { }

        [PXDBInt]
        [PXIntList(
            new int[]
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19,20, 21, 22
            },
            new string[]
            {
                "AVO01", "AVO02", "AVO03", "AVO04", "AVO05", "AVO06", "AVO07", "AVO08", "AVO09", "AVO10", "AVO11", "AVO12", "AVO13", "AVO14", "AVO15", "NPT01", "NPT02", "NPT03", "NPT04", "NPT05", "NPT06", "NPT07"
            })]
        [PXUIField(DisplayName = "RefNbr")]
        public virtual int? RefNbr
        {
            get;
            set;
        }
        #endregion


        #region LineNbr
        public abstract class lineNbr : BqlInt.Field<lineNbr> { }
        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(CalibrationHeader.lineCntr))]
        [PXUIField(DisplayName = "LineNbr")]
        public virtual int? LineNbr
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

        #region Date
        public abstract class date : BqlDateTime.Field<date> { }

        [PXDBDate()]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date
        {
            get;
            set;
        }
        #endregion

        #region Who
        public abstract class who : BqlInt.Field<who> { }

        [PX.TM.Owner]
        [PXUIField(DisplayName = "Who")]
        public virtual int? Who
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
