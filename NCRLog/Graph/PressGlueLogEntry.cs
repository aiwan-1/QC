using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PX.Objects.AM;
using PX.Objects.Common.Exceptions;
using PX.Data.Update.ExchangeService;
using System.Collections;

namespace NCRLog
{
    public class GCPressGlueLogEntry : PXGraph<GCPressGlueLogEntry, PressGlueLogHeader>
    {
        #region Views
        public SelectFrom<PressGlueLogHeader>.View Header;

        public SelectFrom<PressGlueLogDetails>.
            Where<PressGlueLogDetails.pressNo.IsEqual<PressGlueLogHeader.pressNo.FromCurrent>>.View Details;

        public PXSetup<ISOSetup> Setup;
        #region Graph Constructors
        public GCPressGlueLogEntry()
        {
            ISOSetup setup = Setup.Current;

        }
        #endregion
        #endregion

        #region Actions
        public PXAction<PressGlueLogHeader> ViewBatch;
        [PXButton, PXUIField(DisplayName = "View Manufacturing Batch")]
        protected virtual IEnumerable viewBatch(PXAdapter adapter)
        {
            var entry = PXGraph.CreateInstance<MoveEntry>();
            if (entry == null) return adapter.Get();


            var batch = entry.batch.Search<AMBatch.batNbr>(this.Header.Current.BatchNbr);
            if (batch == null) return adapter.Get();

            entry.batch.Current = batch;

            throw new PXRedirectRequiredException(entry, true, nameof(viewBatch))
            {
                Mode = PXBaseRedirectException.WindowMode.NewWindow
            };
        }

        public PXAction<PressGlueLogHeader> ReleaseFromHold;
        [PXButton, PXUIField(DisplayName = "Remove Hold")]
        protected virtual IEnumerable releaseFromHold(PXAdapter adapter) => adapter.Get();

        public PXAction<PressGlueLogHeader> PutOnHold;
        [PXButton, PXUIField(DisplayName = "Put On Hold")]
        protected virtual IEnumerable putOnHold(PXAdapter adapter) => adapter.Get();

        public PXAction<PressGlueLogHeader> Release;
        [PXButton, PXUIField(DisplayName = "Release")]
        protected virtual IEnumerable release(PXAdapter adapter) => adapter.Get();
        #endregion

        #region Event Handlers
        /*  protected virtual void _(Events.FieldUpdated<PressGlueLogHeader, PressGlueLogHeader.siteID> e)
          {
              PressGlueLogHeader row = e.Row;

              PressGlueLogHeader header = SelectFrom<PressGlueLogHeader>.
                  Where<PressGlueLogHeader.date.IsEqual<@P.AsDateTime>>.View.Select(this, row.Date);

              e.Cache.SetValueExt<PressGlueLogHeader.glueManufacturer>(row, header.GlueManufacturer);
              e.Cache.SetValueExt<PressGlueLogHeader.glueSetting>(row, header.GlueSetting);
              e.Cache.SetValueExt<PressGlueLogHeader.diffPressure>(row, header.DiffPressure);
              e.Cache.SetValueExt<PressGlueLogHeader.resultg>(row, header.Resultg);

          }*/

        protected virtual void _(Events.FieldUpdated<PressGlueLogDetails, PressGlueLogDetails.pressTimeMins> e)
        {
            PressGlueLogDetails row = e.Row;

            if (row.OpenTimeMin != null && row.PressTimeMins != null)
            {

                var cureTime = row.PressTimeMins + row.OpenTimeMin;

                e.Cache.SetValueExt<PressGlueLogDetails.cureTimeMins>(row, cureTime);


            }
        }

        protected virtual void _(Events.FieldUpdated<PressGlueLogDetails, PressGlueLogDetails.exitTimePress> e)
        {
            PressGlueLogDetails row = e.Row;

            if (row.StartTimePress != null && row.ExitTimePress != null)
            {
                TimeSpan pressTime = row.ExitTimePress.Value - row.StartTimePress.Value;
                if ((int)pressTime.TotalMinutes > 0)
                {
                    e.Cache.SetValueExt<PressGlueLogDetails.pressTimeMins>(row, (int)pressTime.TotalMinutes);
                }
                if ((int)pressTime.TotalMinutes < 0)
                {
                    DateTime exit = (DateTime)row.ExitTimePress;
                    DateTime exitTime = exit.AddDays(1);
                    DateTime? time = exitTime;

                    TimeSpan totalTime = (row.StartTimePress.Value - time.Value);

                    int timeSpan = 24 - (int)totalTime.TotalMinutes;

                    e.Cache.SetValueExt<PressGlueLogDetails.pressTimeMins>(row, timeSpan);
                }
            }
        }

        protected virtual void _(Events.FieldUpdated<PressGlueLogDetails, PressGlueLogDetails.lastTime> e)
        {
            PressGlueLogDetails row = e.Row;

            if (row.FirstTime != null && row.LastTime != null)
            {
                TimeSpan openTime = row.LastTime.Value - row.FirstTime.Value;

                e.Cache.SetValueExt<PressGlueLogDetails.openTimeMin>(row, (int)openTime.TotalMinutes);
            }

            e.Cache.SetValueExt<PressGlueLogDetails.startTimePress>(row, row.LastTime);
        }

        protected virtual void _(Events.FieldUpdated<PressGlueLogHeader, PressGlueLogHeader.batchNbr> e)
        {
            PressGlueLogHeader row = e.Row;
            if (e.Row == null) return;

            AMBatch batch = SelectFrom<AMBatch>.
                Where<AMBatch.batNbr.IsEqual<P.AsString>>.View.Select(this, row.BatchNbr);

            e.Cache.SetValueExt<PressGlueLogHeader.docType>(row, batch.DocType);
        }

        protected virtual void _(Events.RowSelected<PressGlueLogHeader> e)
        {
            PressGlueLogHeader row = e.Row;
            if (row == null) return;

            PXUIFieldAttribute.SetEnabled<PressGlueLogHeader.docType>(e.Cache, e.Row, false);
        }

        protected virtual void _(Events.FieldUpdated<PressGlueLogHeader, PressGlueLogHeader.siteID> e)
        {
            PressGlueLogHeader row = e.Row;
            if (row == null) return;

            PressGlueLogHeader dateSame = SelectFrom<PressGlueLogHeader>.
                Where<PressGlueLogHeader.date.IsEqual<P.AsDateTime>.
                And<PressGlueLogHeader.siteID.IsEqual<P.AsInt>.
                And<PressGlueLogHeader.pressNo.IsNotEqual<P.AsString>>>>.View.Select(this, row.Date, row.SiteID, row.PressNo);

            if (dateSame == null) return;

            if (dateSame.Date == row.Date)
            {
                e.Cache.SetValueExt<PressGlueLogHeader.targetWeightg>(row, dateSame.TargetWeightg);
                e.Cache.SetValueExt<PressGlueLogHeader.diffPressure>(row, dateSame.DiffPressure);
                e.Cache.SetValueExt<PressGlueLogHeader.glueSetting>(row, dateSame.GlueSetting);
                e.Cache.SetValueExt<PressGlueLogHeader.resultg>(row, dateSame.Resultg);
                e.Cache.SetValueExt<PressGlueLogHeader.testedBy>(row, dateSame.TestedBy);
            }
        }

        /*protected virtual void _(Events.FieldUpdated<PressGlueLogDetails, PressGlueLogDetails.startTemp> e)
        {
            PressGlueLogDetails row = e.Row;
            if(row == null) return;

            //TODO: When temp = x, Total Cure time is y
        }*/


        #endregion
    }
}
