using PX.Data;
using PX.Objects.SO;
using PX.Data.BQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.WorkflowAPI;
using PX.Objects.AR;
using System.Collections;
using PX.Data.BQL.Fluent;
using PX.SM;
using PX.EP;

namespace NCRLog
{
    public class SOShipmentEntryCreditHoldExt : PXGraphExtension<SOShipmentEntry>
    {
        public static bool IsActive() => true;

        public SelectFrom<ARBalances>.
            Where<ARBalances.customerID.IsEqual<SOShipment.customerID.FromCurrent>.
                And<ARBalances.customerLocationID.IsEqual<SOShipment.customerLocationID.FromCurrent>>>.View Balances;

        public PXAction<SOShipment> RemoveCreditHold;
        [PXButton, PXUIField(DisplayName = "Remove Credit Hold",
            MapEnableRights = PXCacheRights.Delete,
            MapViewRights = PXCacheRights.Delete)]
        protected virtual IEnumerable removeCreditHold(PXAdapter adapter)
        { 
            return adapter.Get();
        }

        public PXAction<SOShipment> ProductionConfirmed;
        [PXButton, PXUIField(DisplayName = "Production Confirmed",
            MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable productionConfirmed(PXAdapter adapter)
        {
            return adapter.Get();
        }

        protected virtual void _(Events.RowUpdated<SOShipment> e, PXRowUpdated b)
        {
            b?.Invoke(e.Cache, e.Args); 
            SOShipment row = e.Row;
            if(row == null) return;

            ARBalances remBal = SelectFrom<ARBalances>.
            Where<ARBalances.customerID.IsEqual<P.AsInt>.
                And<ARBalances.customerLocationID.IsEqual<P.AsInt>>>.View.Select(Base, row.CustomerID, row.CustomerLocationID);
            if (remBal == null) return;

            Customer customer = SelectFrom<Customer>.
                Where<Customer.bAccountID.IsEqual<P.AsInt>>.View.Select(Base, row.CustomerID);
            if(customer == null) return;    

            var creditLimit = customer.CreditLimit;
            var balance = creditLimit - ((remBal.CurrentBal ?? 0 ) + (remBal.TotalOpenOrders ?? 0) + (remBal.TotalShipped ?? 0) - (remBal.TotalPrepayments ?? 0));


            if (balance < 0)
            {
                SOShipmentCreditHold rowExt = row.GetExtension<SOShipmentCreditHold>();
                e.Cache.SetValueExt<SOShipmentCreditHold.usrCreditHold>(rowExt.UsrCreditHold, true);
            }

            UpdateBalances(row, customer);

        }

        protected virtual void _(Events.FieldDefaulting<SOShipment, SOShipmentCreditHold.usrCreditHold> e)
        {
            SOShipment row = e.Row;
            if(row == null) return;

            ARBalances balances = SelectFrom<ARBalances>.
            Where<ARBalances.customerID.IsEqual<P.AsInt>.
                And<ARBalances.customerLocationID.IsEqual<P.AsInt>>>.View.Select(Base, row.CustomerID, row.CustomerLocationID);
            if (balances == null) return;

            Customer customer = SelectFrom<Customer>.
                Where<Customer.bAccountID.IsEqual<P.AsInt>>.View.Select(Base, row.CustomerID);
            if (customer == null) return;

            var creditLimit = customer.CreditLimit;
            var balance = creditLimit - ((balances.CurrentBal ?? 0) + (balances.TotalOpenOrders ?? 0) + (balances.TotalShipped ?? 0) - (balances.TotalPrepayments ?? 0));

            if(balance < 0)
            {
                e.NewValue = true;
            }
            
        }

        public PXWorkflowEventHandler<SOShipment> OnCreditLimitViolated;
        public PXWorkflowEventHandler<SOShipment> OnCreditLimitSatisfied;

        public virtual void UpdateBalances(SOShipment shipment, Customer customer)
        {
            SOShipmentEntry graph = Base;

            SOShipment ship = shipment = graph.Document.Current;
            if (ship == null) return;

            graph.Document.Current = graph.Document.Search<SOShipment.shipmentNbr>(ship.ShipmentNbr);

            ARBalances balance = SelectFrom<ARBalances>.
                Where<ARBalances.customerID.IsEqual<P.AsInt>.
                And<ARBalances.customerLocationID.IsEqual<P.AsInt>>>.View.Select(graph, ship.CustomerID, ship.CustomerLocationID);
               
            if (balance == null) return;

            Customer cust = customer = SelectFrom<Customer>.
                Where<Customer.bAccountID.IsEqual<P.AsInt>>.View.Select(graph, ship.CustomerID);
            if (cust == null) return;

            var creditLimit = cust.CreditLimit;
            var remBal = creditLimit - ((balance.CurrentBal ?? 0) + (balance.TotalOpenOrders ?? 0) + (balance.TotalShipped ?? 0) - (balance.TotalPrepayments ?? 0));

            if( remBal < decimal.Zero)
            {
                SOShipmentCreditHold.MyEvents
                    .Select(e => e.CreditLimitViolated)
                    .FireOn(graph, ship);
            }
            if(remBal > decimal.Zero)
            {
                SOShipmentCreditHold.MyEvents
                    .Select(e=> e.CreditLimitSatisfied)
                    .FireOn(graph, ship);
            }
        }
        /*protected virtual void _(Events.RowUpdated<SOShipment> e, PXRowUpdated b)
        {
            b?.Invoke(e.Cache, e.Args);

            SOShipment shipment = e.Row;
            if (shipment == null) return;

            Customer cust = SelectFrom<Customer>.
                Where<Customer.bAccountID.IsEqual<P.AsInt>>.View.Select(Base, shipment.CustomerID);

            UpdateBalances(shipment, cust);
        }*/
    }
}
