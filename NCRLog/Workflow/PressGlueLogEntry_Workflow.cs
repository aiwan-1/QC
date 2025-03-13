using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using static PX.Data.WorkflowAPI.BoundedTo<NCRLog.GCPressGlueLogEntry,
    NCRLog.PressGlueLogHeader>;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.WorkflowAPI;

namespace NCRLog
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class PressGlueLogEntry_Workflow : PXGraphExtension<GCPressGlueLogEntry>
    {
        #region Constants
        public static class States
        {
            public const string Hold = Constants.Hold;
            public const string Released = Constants.Released;
            public const string Open = Constants.Open;
            public const string Closed = Constants.Closed;

            public class hold : BqlString.Constant<hold>
            {
                public hold() : base(Hold) { }
            }
            public class released : BqlString.Constant<released>
            {
                public released() : base(Released) { }
            }
            public class open : BqlString.Constant<open>
            {
                public open() : base(Open) { }
            }
            public class closed : BqlString.Constant<closed>
            {
                public closed() : base(Closed) { }
            }
            
        }
        #endregion

        protected static void Configure(WorkflowContext<GCPressGlueLogEntry,
            PressGlueLogHeader> context)
        {

        }

        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<GCPressGlueLogEntry,
                PressGlueLogHeader>());
        }
    }
}
