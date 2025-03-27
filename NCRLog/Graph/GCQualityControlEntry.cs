﻿using PX.Data.BQL.Fluent;
using PX.Data;
using PX.Objects.SO;
using PX.Data.BQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.AM;
using System.Collections;

namespace NCRLog
{
    public class GCQualityControlEntry : PXGraph<GCQualityControlEntry, GCQCRecord>
    {
        #region Views
        public SelectFrom<GCQCRecord>.View QCCheck;

        public SelectFrom<GCQCLine>.
            Where<GCQCLine.docNbr.IsEqual<GCQCRecord.docNbr.FromCurrent>.
            And<GCQCLine.batchNbr.IsEqual<GCQCRecord.batchNbr.FromCurrent>>>.View Details;

        public PXSetup<ISOSetup> AutoNumSetup;
        #endregion

        #region Graph Constructor

        public GCQualityControlEntry()
        {
            ISOSetup setup = AutoNumSetup.Current;
        }
        #endregion

        #region Actions
        public PXAction<GCQCRecord> ViewBatch;
        [PXButton, PXUIField(DisplayName = "View Manufacturing Batch")]
        protected virtual IEnumerable viewBatch(PXAdapter adapter)
        {
            var entry = PXGraph.CreateInstance<MoveEntry>();
            if (entry == null) return adapter.Get();


            var batch = entry.batch.Search<AMBatch.batNbr>(this.QCCheck.Current.BatchNbr);
            if (batch == null) return adapter.Get();

            entry.batch.Current = batch;

            throw new PXRedirectRequiredException(entry, true, nameof(viewBatch))
            {
                Mode = PXBaseRedirectException.WindowMode.NewWindow
            };
        }

        public PXAction<GCQCRecord> ViewOrder;
        [PXButton, PXUIField(DisplayName = "View Sales Order")]
        protected virtual IEnumerable viewOrder(PXAdapter adapter)
        {
            var entry = PXGraph.CreateInstance<SOOrderEntry>();
            if (entry == null) return adapter.Get();

            var order = entry.Document.Search<SOOrder.orderNbr>(this.QCCheck.Current.SOOrderNbr);
            if (order == null) return adapter.Get();

            throw new PXRedirectRequiredException(entry, true, nameof(viewOrder))
            {
                Mode = PXBaseRedirectException.WindowMode.NewWindow
            };
        }

        public PXAction<GCQCRecord> ReleaseFromHold;
        [PXButton, PXUIField(DisplayName = "Remove Hold")]
        protected virtual IEnumerable releaseFromHold(PXAdapter adapter) => adapter.Get();

        public PXAction<GCQCRecord> PutOnHold;
        [PXButton, PXUIField(DisplayName = "Put On Hold")]
        protected virtual IEnumerable putOnHold(PXAdapter adapter) => adapter.Get();

        public PXAction<GCQCRecord> Release;
        [PXButton, PXUIField(DisplayName = "Release")]
        protected virtual IEnumerable release(PXAdapter adapter) => adapter.Get();
        #endregion

        #region Events

        protected virtual void _(Events.FieldUpdated<GCQCRecord, GCQCRecord.sOOrderNbr> e)
        {
            GCQCRecord row = e.Row;

            if (row == null) return;

            SOOrder order = SelectFrom<SOOrder>.
                Where<SOOrder.orderNbr.IsEqual<@P.AsString>>.View.Select(this, row.SOOrderNbr);

            if (order == null) return;

            e.Cache.SetValueExt<GCQCRecord.customerID>(row, order.CustomerID);

        }


        protected virtual void _(Events.FieldUpdated<GCQCRecord, GCQCRecord.batchNbr> e)
        {
            GCQCRecord row = e.Row;

            if (row == null) return;

            AMBatch batch = SelectFrom<AMBatch>.
               Where<AMBatch.batNbr.IsEqual<@P.AsString>>.View.Select(this, row.BatchNbr);

            if (batch == null) return;

            e.Cache.SetValueExt<GCQCRecord.docType>(row, batch.DocType);
        }

        protected virtual void _(Events.RowSelected<GCQCRecord> e)
        {
            GCQCRecord row = e.Row;
            if (row == null) return;

            PXUIFieldAttribute.SetEnabled<GCQCRecord.docType>(e.Cache, e.Row, false);
        }
        #endregion
    }
}