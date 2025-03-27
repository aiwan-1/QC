using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
	[PXCacheName(Messages.CalibrationHeader)]
	public class CalibrationHeader : PXBqlTable, IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<CalibrationHeader>.By<docNbr>
        {
            public static CalibrationHeader Find(PXGraph graph, string docNbr, PKFindOptions options = PKFindOptions.None) => FindBy(graph, docNbr, options);
        }
        #endregion

        #region DocNbr
        public abstract class docNbr : BqlString.Field<docNbr> { }

        [PXDBString(15, IsKey = true, IsUnicode = true)]
        [AutoNumber(typeof(ISOSetup.autoNumberingCalib),
            typeof(CalibrationHeader.date))]
        [PXSelector(typeof(CalibrationHeader.docNbr),
            typeof(CalibrationHeader.date),
            typeof(CalibrationHeader.siteID))]
        [PXUIField(DisplayName = "Doc Nbr")]
        public virtual string DocNbr
        {
            get;
            set;
        }
        #endregion

        #region Date
        public abstract class date : BqlDateTime.Field<date> { }

        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Date Calibrated")]
        public virtual DateTime? Date
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

        #region LineCntr
        public abstract class lineCntr : BqlInt.Field<lineCntr> { }

        [PXDBInt]
        [PXDefault(0)]
        [PXUIField(DisplayName = "LineCntr")]
        public virtual int? LineCntr
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
