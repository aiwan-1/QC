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