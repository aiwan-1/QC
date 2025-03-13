using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.SQLTree;
using PX.Objects.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    public class CalibrationMaint : PXGraph<CalibrationMaint, CalibrationRecord>
    {
        #region Views
        public SelectFrom<CalibrationRecord>.View Calibration;
        #endregion

        #region Events
        protected virtual void _(Events.RowSelected<CalibrationRecord> e)
        {
            CalibrationRecord row = e.Row;
            if (row == null) return;

            if(row.InternalSchedule != null && (row.RefNbr != 10 ||row.RefNbr != 11 ||row.RefNbr != 12 ||row.RefNbr != 13 ||row.RefNbr != 22))
            {
                PXUIFieldAttribute.SetVisible<CalibrationRecord.externalCert>(e.Cache, row, false);
                PXUIFieldAttribute.SetVisible<CalibrationRecord.certNbr>(e.Cache, row, false);
            }
            if(row.ExternalCert != null  && (row.RefNbr != 10 ||row.RefNbr != 11 ||row.RefNbr != 12 ||row.RefNbr != 13 ||row.RefNbr != 22))
            {
                PXUIFieldAttribute.SetVisible<CalibrationRecord.internalSchedule>(e.Cache, row, false);
            }
        }

        protected virtual void _(Events.FieldUpdated<CalibrationRecord, CalibrationRecord.externalCert> e)
        {
            CalibrationRecord row = e.Row;
            if (row == null) return;

            if (row.ExternalCert == "1")
            {
                var due = (DateTime)row.LastChecked;
                row.NextDue = due.AddYears(1);
            }
            if (row.ExternalCert == "5")
            {
                var due = (DateTime)row.LastChecked;
                row.NextDue = due.AddYears(5);
            }
        }

        protected virtual void _(Events.FieldUpdated<CalibrationRecord, CalibrationRecord.internalSchedule> e)
        {
            CalibrationRecord row = e.Row;
            if (row == null) return;

                if (row.InternalSchedule == "M")
                {
                    var due = (DateTime)row.LastChecked;
                    row.NextDue = due.AddMonths(1);
                }
                if (row.InternalSchedule == "4")
                {
                    var due = (DateTime)row.LastChecked;
                    row.NextDue = due.AddMonths(4);
                }
                if (row.InternalSchedule == "6")
                {
                var due = (DateTime)row.LastChecked;
                row.NextDue = due.AddMonths(6);
                }

        }

        protected virtual void _(Events.FieldUpdated<CalibrationRecord, CalibrationRecord.refNbr> e)
        {
            CalibrationRecord row = e.Row;

            if (row == null) return;

            if (row.RefNbr == 3)
            {
                string serialNbr = "1170MR";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 4)
            {
                string serialNbr = "170604543";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if(row.RefNbr == 5)
            {
                string serialNbr = "201306280018";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 6)
            {
                string serialNbr = "178984";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            /*if (row.RefNbr == 7)
            {
                string serialNbr = "201306280164";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }*/
            if (row.RefNbr == 8)
            {
                string serialNbr = "GL13248";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 9)
            {
                string serialNbr = "GL13197";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 10)
            {
                string serialNbr = "201880027";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 11)
            {
                string serialNbr = "151871";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 12)
            {
                string serialNbr = "1136878";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 13)
            {
                string serialNbr = "1979112/291";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 14)
            {
                string serialNbr = "21033004";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 19)
            {
                string serialNbr = "GD05446";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 20)
            {
                string serialNbr = "179685";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 21)
            {
                string serialNbr = "183552";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
            if (row.RefNbr == 22)
            {
                string serialNbr = "1510524";

                e.Cache.SetValueExt<CalibrationRecord.serialNbr>(row, serialNbr);
            }
        }
        #endregion
    }
}
