using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace NCRLog
{
  public class QualityControlSetupMaint : PXGraph<QualityControlSetupMaint>
  {
        public PXSave<ISOSetup> Save;

        public SelectFrom<ISOSetup>.View Setup;

  }
}