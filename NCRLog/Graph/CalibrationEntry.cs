using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.Extensions.Discount;

namespace NCRLog
{
    public class CalibrationEntry : PXGraph<CalibrationEntry, CalibrationHeader>
    {
        #region Views
        public SelectFrom<CalibrationHeader>.View Header;

        public SelectFrom<CalibrationDetail>.
            Where<CalibrationDetail.docNbr.IsEqual<CalibrationHeader.docNbr.FromCurrent>>.View CalDetails;

        public SelectFrom<InspectionDetail>.
            Where<InspectionDetail.docNbr.IsEqual<CalibrationHeader.docNbr.FromCurrent>>.View InsDetails;

        public PXSetup<ISOSetup> Setup;
        #region Graph Constructors
        public CalibrationEntry()
        {
            ISOSetup setup = Setup.Current;
        }
        #endregion
        #endregion

        #region Events
        protected virtual void _(Events.FieldUpdated<CalibrationDetail, CalibrationDetail.refNbr> e)
        {
            CalibrationDetail row = e.Row;
            if (row == null) return;

            CalibrationRecord record = SelectFrom<CalibrationRecord>.
                Where<CalibrationRecord.refNbr.IsEqual<P.AsInt>>.View.Select(this, row.RefNbr);
            if(record == null) return;

            if(row.RefNbr == record.RefNbr)
            {
                e.Cache.SetValueExt<CalibrationDetail.itemType>(row, record.ItemType);
            }

        }
        protected virtual void _(Events.RowInserted<CalibrationDetail> e)
        {
            CalibrationDetail row = e.Row;
            if (row == null) return;

            CalibrationHeader header = SelectFrom<CalibrationHeader>.
                Where<CalibrationHeader.docNbr.IsEqual<P.AsString>>.View.Select(this, row.DocNbr);
            if (header == null) return;

            if (header.Date != null)
            {
                e.Cache.SetValueExt<CalibrationDetail.date>(row, header.Date);
            }
        }

        protected virtual void _(Events.RowInserted<InspectionDetail> e)
        {
            InspectionDetail row = e.Row;
            if (row == null) return;

            CalibrationHeader header = SelectFrom<CalibrationHeader>.
                Where<CalibrationHeader.docNbr.IsEqual<P.AsString>>.View.Select(this, row.DocNbr);
            if (header == null) return;

            if (header.Date != null)
            {
                e.Cache.SetValueExt<InspectionDetail.date>(row, header.Date);
            }

           
        }

        protected virtual void _(Events.FieldUpdated<InspectionDetail, InspectionDetail.refNbr> e)
        {
            InspectionDetail row = e.Row;
            if (row == null) return;

            CalibrationRecord record = SelectFrom<CalibrationRecord>.
                Where<CalibrationRecord.refNbr.IsEqual<P.AsInt>>.View.Select(this, row.RefNbr);
            if (record == null) return;

            e.Cache.SetValueExt<InspectionDetail.itemType>(row, record.ItemType);
        }

       /* protected virtual void _(Events.FieldSelecting<CalibrationDetail, CalibrationDetail.refNbr> e)
        {
            CalibrationDetail row = e.Row;
            if (row == null) return;

            if(row.SiteID == 10192)
            {
                int values = 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12;

                PXIntListAttribute.SetList<CalibrationDetail.refNbr>(e.Cache, row, values, label)
            }
        }*/
        #endregion
    }
}
