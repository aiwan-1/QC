using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Data.WorkflowAPI;
using PX.Objects.CR;
using PX.Objects.SO;
using PX.Objects.SO.Workflow;
using static PX.Data.WorkflowAPI.BoundedTo<PX.Objects.SO.SOOrderEntry,
    PX.Objects.SO.SOOrder>;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.PO;
using PX.Objects.CN.Common.Extensions;

namespace NCRLog
{
    using State = SOOrderStatus;
    using static SOOrder;
    using CreatePaymentExt = PX.Objects.SO.GraphExtensions.SOOrderEntryExt.CreatePaymentExt;

    public class SOOrderEntryCreditHoldExt : PXGraphExtension<PX.Objects.SO.Workflow.SalesOrder.WorkflowSO, SOOrderEntry>
    {

        public static bool IsActive() => true;


        /*public static void DisableWholeScreen(FieldState.IContainerFillerFields states)
        {
            states.AddTable<SOOrder>(state => state.IsDisabled());
            states.AddTable<SOLine>(state => state.IsDisabled());
            states.AddTable<SOTaxTran>(state => state.IsDisabled());
            states.AddTable<SOBillingAddress>(state => state.IsDisabled());
            states.AddTable<SOBillingContact>(state => state.IsDisabled());
            states.AddTable<SOShippingAddress>(state => state.IsDisabled());
            states.AddTable<SOShippingContact>(state => state.IsDisabled());
            states.AddTable<SOLineSplit>(state => state.IsDisabled());
            states.AddTable<CRRelation>(state => state.IsDisabled());
        }*/

        public static void EnableSOLine(FieldState.IContainerFillerFields states)
        {
            states.AddTable<SOOrder>(state => state.IsDisabled());
            states.AddTable<SOTaxTran>(state => state.IsDisabled());
            states.AddTable<SOBillingAddress>(state => state.IsDisabled());
            states.AddTable<SOBillingContact>(state => state.IsDisabled());
            states.AddTable<SOShippingAddress>(state => state.IsDisabled());
            states.AddTable<SOShippingContact>(state => state.IsDisabled());
            states.AddTable<CRRelation>(state => state.IsDisabled());
        }




        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<SOOrderEntry,
            SOOrder>());
        }

        protected static void Configure(WorkflowContext<SOOrderEntry,
            SOOrder> context)
        {
            context.UpdateScreenConfigurationFor(screen => screen
                .WithFlows(flows =>
                {
                    flows.Update<SOBehavior.sO>(flow => flow
                    .WithFlowStates(flowStates =>
                    {
                        flowStates.Remove<State.shipping>();
                        flowStates.Add<State.shipping>(flowState =>
                        {
                            return flowState
                                .WithActions(actions =>
                                {
                                    actions.Add(g => g.createShipmentIssue);
                                    actions.Add(g => g.emailSalesOrder);
                                    actions.Add(g => g.createPurchaseOrder);
                                    actions.Add<CreatePaymentExt>(e => e.createAndCapturePayment);
                                    actions.Add<CreatePaymentExt>(e => e.createAndAuthorizePayment);
                                })
                                .WithEventHandlers(handlers =>
                                {
                                    handlers.Add(g => g.OnShipmentUnlinked);
                                    handlers.Add(g => g.OnShipmentConfirmed);
                                })
                                .WithFieldStates(EnableSOLine);
                        });
                    }));
                })); 
        }
      
    }
}
