using PX.Data;
using PX.Objects.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    public class POReceiptEntryExt : PXGraphExtension<POReceiptEntry>
    {
        public static bool IsActive() => true;

        [PXRemoveBaseAttribute(typeof(PXDefaultAttribute))]
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXDefault(typeof(ReturnCostMode.originalCost), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void _(Events.CacheAttached<POReceipt.returnInventoryCostMode> e) { }

        
    }
}
