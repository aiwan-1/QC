using PX.Data;
using PX.Objects.AM;
using PX.Objects.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    public class BOMMaintECNExt : PXGraphExtension<BOMMaint>
    {
        public static bool IsActive() => true;

        public PXAction<AMBomItem> CreateECNAction;
        [PXButton]
        [PXUIField(DisplayName = "Create ECN", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable createECNAction(PXAdapter adapter)
        {
            if (Base.Documents.Current == null)
            {
                return adapter.Get();
            }

            var gilMaint = PXGraph.CreateInstance<GilcrestMaint>();
            var bom = Base.Documents.Current;
            var header = gilMaint.ISORecords.Insert();
            header.DocType = "ECN";
            header.ECNType = "D";
            gilMaint.ISORecords.Update(header);

            gilMaint.Actions.PressSave();

            PXRedirectHelper.TryRedirect(gilMaint, PXRedirectHelper.WindowMode.New);

            return adapter.Get();

        }

    }
}
