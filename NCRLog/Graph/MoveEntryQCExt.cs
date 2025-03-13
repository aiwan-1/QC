using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    public class MoveEntryQCExt : PXGraphExtension<MoveEntry>
    {
        public static bool IsActive() => true;

        #region Actions
        private static void checkQCCheckNoteID()
        {
            var moveEntry = PXGraph.CreateInstance<MoveEntry>();
            var qcEntry = PXGraph.CreateInstance<GCQualityControlEntry>();

            var header = qcEntry.QCCheck.Current;
            GCQCRecord qcHeader = SelectFrom<GCQCRecord>.
                Where<GCQCRecord.noteID.IsEqual<P.AsGuid>>.View.Select(moveEntry, header.NoteID);

            if (qcHeader != null)
            {
                header.NoteID = Guid.NewGuid();
            }
        }

        public PXAction<AMBatch> StartQCChecks;
        [PXButton]
        [PXUIField(DisplayName = "Start QC Checks", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable startQCChecks(PXAdapter adapter)
        {
            var moveEntry = PXGraph.CreateInstance<MoveEntry>();
            if(moveEntry == null)
            {
                return adapter.Get();
            }

            var qcEntry = PXGraph.CreateInstance<GCQualityControlEntry>();
            var move = Base.batch.Current;
            var record = new GCQCRecord();

            record.BatchNbr = move.BatNbr;
            record.DocType = move.DocType;
            record.Date = move.TranDate;
            
            qcEntry.QCCheck.Update(record);

            
            foreach (AMMTran tran in Base.transactions.Select())
            {
                var details = new GCQCLine();
                details.AMProdOrdID = tran.ProdOrdID;
                details.TranDate = tran.TranDate;
                
                qcEntry.Details.Update(details);
                
            }


            qcEntry.Actions.PressSave();

            AMBatchQCExt moveExt = PXCache<AMBatch>.GetExtension<AMBatchQCExt>(move);
            moveExt.UsrQCCheckNo = record.DocNbr;

            
            moveEntry.Actions.PressSave();

            PXRedirectHelper.TryRedirect(qcEntry, PXRedirectHelper.WindowMode.New);

            return adapter.Get();
        }

        public PXAction<AMBatch> RecordPressGlueLogs;
        [PXButton]
        [PXUIField(DisplayName = "Record Press Glue Logs", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable recordPressGlueLogs(PXAdapter adapter)
        {
            var moveEntry = PXGraph.CreateInstance<MoveEntry>();
            if(moveEntry == null)
            {
                 return adapter.Get();
            }

            var logEntry = PXGraph.CreateInstance<GCPressGlueLogEntry>();
            var move = Base.batch.Current;
            var tran = Base.transactions.Current;
            var header = new PressGlueLogHeader();
            header.BatchNbr = move.BatNbr;
            header.DocType = move.DocType;
            header.Date = move.TranDate;
            header.SiteID = tran.SiteID;
            logEntry.Header.Update(header);

            logEntry.Actions.PressSave();
            AMBatchQCExt moveExt = PXCache<AMBatch>.GetExtension<AMBatchQCExt>(move);
            moveExt.UsrQCCheckNo = header.PressNo;

            
            moveEntry.Actions.PressSave();

            PXRedirectHelper.TryRedirect(logEntry, PXRedirectHelper.WindowMode.New);

            return adapter.Get();
        }

        
        #endregion

        #region Events
        protected virtual void _(Events.RowSelected<AMBatch> e, PXRowSelected b)
        {
            AMBatch row = e.Row;
            if (row == null) return;
            b?.Invoke(e.Cache, e.Args);

            if (row.Status == "R")
            {
                StartQCChecks.SetEnabled(true);
                RecordPressGlueLogs.SetEnabled(true);
            }
        }
        #endregion
    }
}
