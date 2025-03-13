using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects;
using PX.Objects.SO;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Objects.IN;
using PX.Objects.PO;
using PX.Objects.CN.Common.Extensions;
using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.CM.Extensions;
using PX.Common;
using PX.Objects.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
  public class GilcrestMaint : PXGraph<GilcrestMaint, ISORecord>
  { 

      #region Views
      public SelectFrom<ISORecord>.View ISORecords;

      public SelectFrom<NCRLog>.
            Where<NCRLog.nCRNumber.IsEqual<ISORecord.docNumber.FromCurrent>.
                And<NCRLog.docType.IsEqual<ISORecord.docType.FromCurrent>>>.View NCRs;

      public SelectFrom<ECNLog>.
            Where<ECNLog.nCRNumber.IsEqual<ISORecord.docNumber.FromCurrent>.
                And<ECNLog.docType.IsEqual<ISORecord.docType.FromCurrent>>>.View ECNs;

        public PXSetup<ISOSetup> AutoNumSetup;
        #region Graph Constructor
        public GilcrestMaint()
        {
            ISOSetup setup = AutoNumSetup.Current;
        }
        #endregion

        #endregion

        #region Events


        protected virtual void _(Events.RowSelected<ISORecord> e)
        {
          ISORecord row = e.Row;
          if (row.DocType == "ECN")
          {
                PXUIFieldAttribute.SetEnabled<ISORecord.eCNType>(e.Cache, e.Row, true);
                PXUIFieldAttribute.SetVisible<ISORecord.nCRType>(e.Cache, e.Row, false);
                PXUIFieldAttribute.SetVisible<ISORecord.code>(e.Cache, e.Row, false);
          }

          if (row.DocType == "NCR")
          {
              PXUIFieldAttribute.SetEnabled<ISORecord.nCRType>(e.Cache, e.Row, true);
              PXUIFieldAttribute.SetEnabled<ISORecord.code>(e.Cache, e.Row, true);
              PXUIFieldAttribute.SetVisible<ISORecord.eCNType>(e.Cache, e.Row, false);

          }

            NCRs.Cache.AllowSelect =
                  row.DocType != "ECN";

            ECNs.Cache.AllowSelect =
                  row.DocType !=  "NCR";

          
        }

      protected virtual void _(Events.FieldUpdated<ISORecord, ISORecord.sOOrderNbr> e)
      {
            ISORecord row = e.Row;
            if (row.SOOrderNbr == null) return;

            if(row.SOOrderNbr != null)
            {
                SOOrder order = SelectFrom<SOOrder>.
                    Where<SOOrder.orderNbr.IsEqual<@P.AsString>>.View.Select(this, row.SOOrderNbr);

                var customer = order.CustomerID;
                var owner = order.OwnerID;

                e.Cache.SetValueExt<ISORecord.customerID>(row, customer);
                e.Cache.SetValueExt<ISORecord.ownerID>(row, owner);

            }
      }

      protected virtual void _(Events.FieldUpdated<NCRLog, NCRLog.sOOrderNbr> e)
      {
            NCRLog row = e.Row;
            if(row.SOOrderNbr == null) return;

            if (row.SOOrderNbr != null)
            {
                SOOrder order = SelectFrom<SOOrder>.
                    Where<SOOrder.orderNbr.IsEqual<@P.AsString>>.View.Select(this, row.SOOrderNbr);

                var customerPO = order.CustomerRefNbr;

                e.Cache.SetValueExt<NCRLog.customerPONbr>(row, customerPO);
            }
      }

      protected virtual void _(Events.FieldUpdated<NCRLog, NCRLog.pOOrderNbr> e)
      {
            NCRLog row = e.Row;
            if (row.POOrderNbr == null) return;

            if(row.POOrderNbr != null)
            {
                POOrder order = SelectFrom<POOrder>.
                    Where<POOrder.orderNbr.IsEqual<@P.AsString>>.View.Select(this, row.POOrderNbr);

                var vendor = order.VendorID;

                e.Cache.SetValueExt<NCRLog.vendorID>(row, vendor);
            }
      }



      protected virtual void _(Events.FieldUpdated<ECNLog, ECNLog.inventoryID> e)
      {
            ECNLog row = e.Row;
            if (row.InventoryID == null) return;

            if(row.InventoryID != null)
            {
                InventoryItem item = SelectFrom<InventoryItem>.
                    Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>.View.Select(this, row.InventoryID);

                var descr = item.Descr;

                e.Cache.SetValueExt<ECNLog.effectiveProd>(row, descr);

            }
      }

        #endregion


  }

  /*  using static BoundedTo<GilcrestMaint, ISORecord>;
    using static ISORecord;
    using State = ISORecordsStatus;
    using This = GilcrestMaint_ApprovalWorkflow;
    

    public class GilcrestMaint_ApprovalWorkflow : PXGraphExtension<GilcrestMaint>
    {
       private class ISOApprovalSettings : IPrefetchable
        {

        }
    }*/
}