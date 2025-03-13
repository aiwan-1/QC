using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;
using GILCustomizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    /*public class SOShipmentEntryDefQtyExt : PXGraphExtension<SOShipmentEntry>
    {
        public static bool IsActive() => true;

        protected virtual void _(Events.RowUpdated<SOShipLineSplitPackage> e, PXRowUpdated b)
        {
            SOShipLineSplitPackage row = e.Row;
            if (row == null) return;

            b?.Invoke(e.Cache, e.Args);

            
            row.PackedQty = 1;
        }



        protected virtual void _(Events.RowSelected<SOShipLineSplitPackage> e, PXRowSelected b)
        {
            SOShipLineSplitPackage row = e.Row;
            if (row == null) return;

            SOShipLineSplitPackageExt rowExt = row.GetExtension<SOShipLineSplitPackageExt>();

            b?.Invoke(e.Cache, e.Args);

            PXUIFieldAttribute.SetEnabled<SOShipLineSplitPackage.packedQty>(e.Cache, row, false);
            PXUIFieldAttribute.SetEnabled<SOShipLineSplitPackageExt.usrPanelRef>(e.Cache, row, true);
            PXUIFieldAttribute.SetEnabled<SOShipLineSplitPackage.shipmentSplitLineNbr>(e.Cache, row, false);
            

        }

        /*[PXMergeAttributes(Method = MergeMethod.Append)]
        [PXSelector(typeof(Search<SOShipLineExt.usrPanelRef, Where<SOShipLine.shipmentNbr, Equal<Current<SOShipLineSplitPackage.shipmentNbr>>>>),
            new[]
            {
                typeof(SOShipLineExt.usrPanelRef),
                typeof(SOShipLineSplit.lineNbr),
                typeof(SOShipLineSplit.splitLineNbr),
                typeof(SOShipLineSplit.origOrderNbr),
                typeof(SOShipLineSplit.origOrderType),
                typeof(SOShipLineSplit.inventoryID),
                typeof(SOShipLineSplit.lotSerialNbr),
                typeof(SOShipLineSplit.qty),
                typeof(SOShipLineSplit.packedQty),
                typeof(SOShipLineSplit.uOM) 
            }, DirtyRead = true)]
        protected virtual void _(Events.CacheAttached<SOShipLineSplitPackageExt.usrPanelRef> e) { }
    }*/
}
