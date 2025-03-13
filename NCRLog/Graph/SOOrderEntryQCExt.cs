using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    public class SOOrderEntryQCExt : PXGraphExtension<SOOrderEntry>
    {
        public static bool IsActive() => true;

        public SelectFrom<GCQCLine>.
            InnerJoin<GCQCRecord>.
            On<GCQCRecord.docNbr.IsEqual<GCQCLine.docNbr>>.
            Where<GCQCRecord.sOOrderNbr.IsEqual<SOOrder.orderNbr.FromCurrent>.
                And<GCQCLine.panelRef.IsNotNull>>.View.ReadOnly QCDetails;

        SelectFrom<ISORecord>.
            Where<ISORecord.sOOrderNbr.IsEqual<SOOrder.orderNbr.FromCurrent>>.View.ReadOnly ISO;


        #region Actions
        public PXAction<SOOrder> CreateNCRAction;
        [PXButton]
        [PXUIField(DisplayName = "Create NCR", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable createNCRAction(PXAdapter adapter)
        {
            var orderEntry = PXGraph.CreateInstance<SOOrderEntry>();
            if( orderEntry == null)
            {
                return adapter.Get();
            }

            var gilMaint = PXGraph.CreateInstance<GilcrestMaint>();
            var order = Base.CurrentDocument.Current;
            var header = gilMaint.ISORecords.Insert();
            header.DocType = "NCR";
            header.SOOrderNbr = order.OrderNbr;
            header.CustomerID = order.CustomerID;
            gilMaint.ISORecords.Update(header);

            var ncrDetail = gilMaint.NCRs.Insert();
            ncrDetail.CustomerPONbr = order.CustomerOrderNbr;
            gilMaint.NCRs.Update(ncrDetail);
            gilMaint.Actions.PressSave();

            SOOrderISOExt orderExt = PXCache<SOOrder>.GetExtension<SOOrderISOExt>(order);
            header.DocNumber = orderExt.UsrNCRNumber;
            orderEntry.CurrentDocument.Update(order);

            orderEntry.Actions.PressSave();
            

            PXRedirectHelper.TryRedirect(gilMaint, PXRedirectHelper.WindowMode.New);

            return adapter.Get();

        }

        public PXAction<SOOrder> CreateECNAction;
        [PXButton]
        [PXUIField(DisplayName = "Create ECN", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable createECNAction(PXAdapter adapter)
        {
            var orderEntry = Base;
            if (orderEntry == null)
            {
                return adapter.Get();
            }

            var gilMaint = PXGraph.CreateInstance<GilcrestMaint>();
            var order = Base.CurrentDocument.Current;
            var header = gilMaint.ISORecords.Insert();
            header.DocType = "ECN";
            header.SOOrderNbr = order.OrderNbr;
            header.CustomerID = order.CustomerID;
            gilMaint.ISORecords.Update(header);
            gilMaint.Actions.PressSave();

            SOOrderISOExt orderExt = PXCache<SOOrder>.GetExtension<SOOrderISOExt>(order);
            header.DocNumber = orderExt.UsrECNNumber;
            Base.CurrentDocument.Update(order);

            Base.Actions.PressSave();
            

            PXRedirectHelper.TryRedirect(gilMaint, PXRedirectHelper.WindowMode.New);

            return adapter.Get();

        }
        #endregion

        #region Events
        protected virtual void _(Events.RowSelected<SOOrder> e, PXRowSelected b)
        {
            SOOrder row = e.Row;
            if (row == null) return;

            b?.Invoke(e.Cache, e.Args);

            if (row.Status != "S")
            {
                CreateNCRAction.SetVisible(true);
                CreateNCRAction.SetEnabled(true);
                CreateECNAction.SetVisible(true);
                CreateECNAction.SetEnabled(true);
                
            }
        }

        #endregion

    }
}
