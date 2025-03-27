using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Objects.CR.Extensions;
using PX.Objects.SO;
using PX.Objects.CR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.SO.GraphExtensions.SOOrderEntryExt;
using PX.Data.WorkflowAPI;
//using GILCustomizations;
using PX.Api;
using static PX.Objects.SO.SOShipmentStatus;

namespace NCRLog
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class SOOrderEntryQCExt : PXGraphExtension<SOOrderEntry>
    {
       

        public SelectFrom<GCQCLine>.
            RightJoin<GCQCRecord>.
            On<GCQCRecord.docNbr.IsEqual<GCQCLine.docNbr>>.
            Where<GCQCRecord.sOOrderNbr.IsEqual<SOOrder.orderNbr.FromCurrent>.
                And<GCQCLine.panelRef.IsNotNull>>.View.ReadOnly QCDetails;

        public SelectFrom<ISORecord>.
             Where<ISORecord.sOOrderNbr.IsEqual<SOOrder.orderNbr.FromCurrent>>.View.ReadOnly ISO;




        #region Actions
        public PXAction<SOOrder> CreateNCRAction;
        [PXButton]
        [PXUIField(DisplayName = "Create NCR"/*, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select*/)]
        protected virtual IEnumerable createNCRAction(PXAdapter adapter)
        {
            List<SOOrder> list = new List<SOOrder>();  
            foreach (SOOrder order in adapter.Get<SOOrder>())
            {
                list.Add(order);
            }

            Base.Actions.PressSave();

            var document = Base.Document.Current;
            PXLongOperation.StartOperation(Base, delegate ()
            {
                CreateNCR(document);
            });

            return adapter.Get();
            
        }

        private static void CreateNCR(SOOrder order)
        {
            using (var ts = new PXTransactionScope())
            {
                var gilMaint = PXGraph.CreateInstance<GilcrestMaint>();
                var doc = new ISORecord()
                {
                    DocType = "NCR"
                };

                var orderEntry = PXGraph.CreateInstance<SOOrderEntry>();
                orderEntry.Document.Current = order;

                doc = gilMaint.ISORecords.Insert(doc);
                doc.SOOrderNbr = order.OrderNbr;
                doc.CustomerID = order.CustomerID;
                gilMaint.ISORecords.Update(doc);
                gilMaint.Actions.PressSave();

                SOOrderISOExt orderExt = PXCache<SOOrder>.GetExtension<SOOrderISOExt>(order);
                orderExt.UsrNCRNumber = doc.DocNumber;
                orderEntry.Document.Update(order);
                orderEntry.Actions.PressSave();

                ts.Complete();

                throw new PXRedirectRequiredException(gilMaint, true, nameof(CreateNCR))
                {
                    Mode = PXBaseRedirectException.WindowMode.NewWindow
                };
            }
        }

        public PXAction<SOOrder> CreateECNAction;
        [PXButton]
        [PXUIField(DisplayName = "Create ECN")]
        protected virtual IEnumerable createECNAction(PXAdapter adapter)
        {
            /*var orderEntry = Base;
            if (orderEntry == null)
            {
                return adapter.Get();
            }

            var gilMaint = PXGraph.CreateInstance<GilcrestMaint>();
            var order = Base.Document.Current;
            var header = gilMaint.ISORecords.Insert();
            header.DocType = "ECN";
            header.SOOrderNbr = order.OrderNbr;
            header.CustomerID = order.CustomerID;
            gilMaint.ISORecords.Update(header);
            gilMaint.Actions.PressSave();

            PXRedirectHelper.TryRedirect(gilMaint, PXRedirectHelper.WindowMode.New);

            SOOrderISOExt orderExt = PXCache<SOOrder>.GetExtension<SOOrderISOExt>(order);
            header.DocNumber = orderExt.UsrECNNumber;
            orderEntry.Document.Update(order);
            orderEntry.Actions.PressSave();

            return adapter.Get();*/
            List<SOOrder> list = new List<SOOrder>();
            foreach (SOOrder order in adapter.Get<SOOrder>())
            {
                list.Add(order);
            }

            Base.Actions.PressSave();

            var document = Base.Document.Current;
            PXLongOperation.StartOperation(Base, delegate ()
            {
                CreateECN(document);
            });

            return adapter.Get();
        }
        private static void CreateECN(SOOrder order)
        {
            using (var ts = new PXTransactionScope())
            {
                var gilMaint = PXGraph.CreateInstance<GilcrestMaint>();
                var doc = new ISORecord()
                {
                    DocType = "ECN"
                };

                var orderEntry = PXGraph.CreateInstance<SOOrderEntry>();
                orderEntry.Document.Current = order;

                doc = gilMaint.ISORecords.Insert(doc);
                doc.SOOrderNbr = order.OrderNbr;
                doc.CustomerID = order.CustomerID;
                gilMaint.ISORecords.Update(doc);
                gilMaint.Actions.PressSave();

                SOOrderISOExt orderExt = PXCache<SOOrder>.GetExtension<SOOrderISOExt>(order);
                orderExt.UsrECNNumber = doc.DocNumber;
                orderEntry.Document.Update(order);
                orderEntry.Actions.PressSave();

                ts.Complete();

                throw new PXRedirectRequiredException(gilMaint, true, nameof(CreateECN))
                {
                    Mode = PXBaseRedirectException.WindowMode.NewWindow
                };

            }
        }
        #endregion

        #region Events
        protected virtual void _(Events.RowSelected<SOOrder> e, PXRowSelected b)
        {
            SOOrder row = e.Row;
            if (row == null) return;

            b?.Invoke(e.Cache, e.Args);

            if (row.Status == "S" || row.Status == "C")
            {
                CreateNCRAction.SetVisible(false);
                CreateNCRAction.SetEnabled(false);
                CreateECNAction.SetVisible(false);
                CreateECNAction.SetEnabled(false);
                
            }
            if(row.Status == "N" || row.Status == "H" || row.Status == "Y" || row.Status == "Z" || row.Status == "X" )
            {
                PXUIFieldAttribute.SetEnabled<SOOrder.freightCost>(e.Cache, row, true);
            }
            if(row.Status == "S")
            {
                PXUIFieldAttribute.SetEnabled(e.Cache, row, false);
            }
        }

        protected virtual void _(Events.RowSelected<SOLine> e, PXRowSelected b)
        {
            b?.Invoke(e.Cache, e.Args);

            SOLine row = e.Row;
            if (row == null) return;

            SOOrder order = SelectFrom<SOOrder>.
                Where<SOOrder.orderType.IsEqual<P.AsString>.
                And<SOOrder.orderNbr.IsEqual<P.AsString>>>.View.Select(Base, row.OrderType, row.OrderNbr);
            if(order == null) return;

            if(row.ShippedQty > 0)
            {
                PXUIFieldAttribute.SetEnabled(e.Cache, e.Row, false);
            }
        }

        #endregion

       

        public class sO : BqlString.Constant<sO>
        {
            public sO() : base("Sales Order") { }
        }
    }
}
