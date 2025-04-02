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
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class SOShipmentEntryCreditHoldExt : PXGraphExtension<SOShipmentEntry>
    {
        public SelectFrom<ARBalances>.
            Where<ARBalances.customerID.IsEqual<SOShipment.customerID.FromCurrent>.
                And<ARBalances.customerLocationID.IsEqual<SOShipment.customerLocationID.FromCurrent>>>.View Balances;


        #region Workflow Actions
        public PXAction<SOShipment> RemoveCreditHold;
        [PXButton, PXUIField(DisplayName = "Remove Credit Hold",
            MapEnableRights = PXCacheRights.Delete,
            MapViewRights = PXCacheRights.Delete)]
        protected virtual IEnumerable removeCreditHold(PXAdapter adapter)
        {
            SOShipmentEntry entry = Base;

            SOShipment ship = entry.Document.Current;
            if (ship == null) return adapter.Get();

            SOShipmentCreditHold shipExt = ship.GetExtension<SOShipmentCreditHold>();

            shipExt.UsrCreditHold = false;

            return adapter.Get();
        }


        public PXAction<SOShipment> ProductionConfirmed;
        [PXButton, PXUIField(DisplayName = "Production Confirmed",
            MapEnableRights = PXCacheRights.Select,
            MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable productionConfirmed(PXAdapter adapter)
            => adapter.Get();
        #endregion

        #region Workflow Events
        public PXWorkflowEventHandler<SOShipment> OnCreditLimitViolated;
        public PXWorkflowEventHandler<SOShipment> OnCreditLimitSatisfied;
        #endregion

        #region Events
        protected virtual void _(Events.FieldUpdated<ARBalances.currentBal> e, PXFieldUpdated b)
        {
            b?.Invoke(e.Cache, e.Args);
            ARBalances row = (ARBalances)e.Row;
            if (row == null) return;

            SOShipment shipment = SelectFrom<SOShipment>.
                Where<SOShipment.customerID.IsEqual<P.AsInt>.
                And<SOShipment.customerLocationID.IsEqual<P.AsInt>>>.View.Select(Base, row.CustomerID, row.CustomerLocationID);


            Customer customer = SelectFrom<Customer>.
                Where<Customer.bAccountID.IsEqual<P.AsInt>>.View.Select(Base, row.CustomerID);
            if (customer == null) return;

            var creditLimit = customer.CreditLimit;

            var balance = creditLimit - ((row.CurrentBal ?? 0) + (row.TotalOpenOrders ?? 0) + (row.TotalShipped ?? 0) - (row.TotalPrepayments ?? 0));


            if (balance < decimal.Zero)
            {
                SOShipmentCreditHold shipmentExt = shipment.GetExtension<SOShipmentCreditHold>();
                e.Cache.SetValueExt<SOShipmentCreditHold.usrCreditHold>(shipmentExt.UsrCreditHold, true);
                SOShipmentCreditHold.MyEvents
                    .Select(events => events.CreditLimitViolated)
                    .FireOn(Base, shipment);
                // PXEntityEventBase<ARBalances>.Container<ARBalancesCreditHoldExt.MyEvents>.Select(ev => ev.CreditLimitViolated).FireOn(Base, row);
            }
            if (balance > decimal.Zero)
            {
                SOShipmentCreditHold shipmentExt = shipment.GetExtension<SOShipmentCreditHold>();
                e.Cache.SetValueExt<SOShipmentCreditHold.usrCreditHold>(shipmentExt.UsrCreditHold, false);
                SOShipmentCreditHold.MyEvents
                    .Select(events => events.CreditLimitSatisfied)
                    .FireOn(Base, shipment);
                //PXEntityEventBase<ARBalances>.Container<ARBalancesCreditHoldExt.MyEvents>.Select(ev => ev.CreditLimitViolated).FireOn(Base, row);
            }
            if (row.CurrentBal == 0) return;
        }

        protected virtual void _(Events.RowUpdated<SOShipment> e, PXRowUpdated b)
        {
            b?.Invoke(e.Cache, e.Args);
            SOShipment row = e.Row;
            if (row == null) return;

            ARBalances remBal = SelectFrom<ARBalances>.
            Where<ARBalances.customerID.IsEqual<P.AsInt>.
            And<ARBalances.customerLocationID.IsEqual<P.AsInt>>>.View.Select(Base, row.CustomerID, row.CustomerLocationID);

            if (remBal == null) return;

            Customer customer = SelectFrom<Customer>.
                Where<Customer.bAccountID.IsEqual<P.AsInt>>.View.Select(Base, row.CustomerID);
            if (customer == null) return;

            var creditLimit = customer.CreditLimit;
            var balance = creditLimit - ((remBal.CurrentBal ?? 0) + (remBal.TotalOpenOrders ?? 0) + (remBal.TotalShipped ?? 0) - (remBal.TotalPrepayments ?? 0));

            if (balance < 0)
            {
                SOShipmentCreditHold rowExt = row.GetExtension<SOShipmentCreditHold>();
                e.Cache.SetValueExt<SOShipmentCreditHold.usrCreditHold>(rowExt.UsrCreditHold, true);

                SOShipmentCreditHold.MyEvents
                    .Select(events => events.CreditLimitViolated)
                    .FireOn(Base, row);

            }



        }

        protected virtual void _(Events.FieldDefaulting<SOShipment, SOShipmentCreditHold.usrCreditHold> e)
        {
            SOShipment row = e.Row;
            if (row == null) return;

            ARBalances balances = SelectFrom<ARBalances>.
            Where<ARBalances.customerID.IsEqual<P.AsInt>.
            And<ARBalances.customerLocationID.IsEqual<P.AsInt>>>.View.Select(Base, row.CustomerID, row.CustomerLocationID);
            if (balances == null) return;

            Customer customer = SelectFrom<Customer>.
                Where<Customer.bAccountID.IsEqual<P.AsInt>>.View.Select(Base, row.CustomerID);
            if (customer == null) return;

            SOOrderShipment ordship = SelectFrom<SOOrderShipment>.
                Where<SOOrderShipment.shipmentNbr.IsEqual<P.AsString>>.View.Select(Base, row.ShipmentNbr);
            if (ordship == null) return;

            SOOrder order = SelectFrom<SOOrder>.
                Where<SOOrder.orderType.IsEqual<P.AsString>.
                And<SOOrder.orderNbr.IsEqual<P.AsString>>>.View.Select(Base, ordship.OrderType, ordship.OrderNbr);
            if (order == null) return;

            var creditLimit = customer.CreditLimit;
            var balance = creditLimit - ((balances.CurrentBal ?? 0) + (balances.TotalOpenOrders ?? 0) + (balances.TotalShipped ?? 0) - (balances.TotalPrepayments ?? 0));

            if (balance < decimal.Zero)
            {
                e.NewValue = true;

            }

        }

        protected virtual void _(Events.RowInserting<SOShipment> e, PXRowInserting b)
        {
            b?.Invoke(e.Cache, e.Args);

            SOShipment row = e.Row;
            if (row == null) return;

            ARBalances balances = SelectFrom<ARBalances>.
            Where<ARBalances.customerID.IsEqual<P.AsInt>.
            And<ARBalances.customerLocationID.IsEqual<P.AsInt>>>.View.Select(Base, row.CustomerID, row.CustomerLocationID);
            if (balances == null) return;

            Customer customer = SelectFrom<Customer>.
                Where<Customer.bAccountID.IsEqual<P.AsInt>>.View.Select(Base, row.CustomerID);
            if (customer == null) return;

            SOOrderShipment ordship = SelectFrom<SOOrderShipment>.
                Where<SOOrderShipment.shipmentNbr.IsEqual<P.AsString>>.View.Select(Base, row.ShipmentNbr);
            if (ordship == null) return;

            SOOrder order = SelectFrom<SOOrder>.
                Where<SOOrder.orderType.IsEqual<P.AsString>.
                And<SOOrder.orderNbr.IsEqual<P.AsString>>>.View.Select(Base, ordship.OrderType, ordship.OrderNbr);
            if (order == null) return;

            var creditLimit = customer.CreditLimit;
            var balance = creditLimit - ((balances.CurrentBal ?? 0) + (balances.TotalOpenOrders ?? 0) + (balances.TotalShipped ?? 0) - (balances.TotalPrepayments ?? 0));

            if (balance < decimal.Zero)
            {
                SOShipmentCreditHold rowExt = row.GetExtension<SOShipmentCreditHold>();
                rowExt.UsrCreditHold = true;
                SOShipmentCreditHold.MyEvents
                    .Select(events => events.CreditLimitViolated)
                    .FireOn(Base, row);
                //PXEntityEventBase<SOOrder>.Container<SOOrder.Events>.Select(ev => ev.CreditLimitViolated).FireOn(Base, order);
            }

        }

        protected virtual void _(Events.RowSelected<SOShipment> e, PXRowSelected b)
        {
            SOShipment row = e.Row;
            if (row == null) return;

            b?.Invoke(e.Cache, e.Args);

            PXUIFieldAttribute.SetEnabled<SOShipment.lineTotal>(e.Cache, row, false);

            e.Cache.SetDefaultExt<SOShipmentCreditHold.usrCreditHold>(row);
        }
        #endregion


    }
}
