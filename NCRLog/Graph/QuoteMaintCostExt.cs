using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CM;
using PX.Objects.CM.Extensions;
using PX.Objects.CR;
using PX.Objects.IN;
using static PX.Objects.CR.QuoteMaint;

namespace QualityControl
{
    public class QuoteMaintCostExt : PXGraphExtension<PX.Objects.CR.QuoteMaint>
    {
        public static bool IsActive() => true;

        #region Actions
        public PXAction<CRQuote> UpdateCosts;
        [PXButton(DisplayOnMainToolbar = true)]
        [PXUIField(DisplayName = "Update Costs")]
        protected virtual void updateCosts()
        {
            var quote = Base.Products.Current;
            if (quote.QuoteID == null) return;
            var lines = Base.Products.Select();
            foreach(CROpportunityProducts product in lines)
            {
                if (product.POCreate == true) continue;
                if (product.POCreate == false || product.POCreate == null)
                {

                    InventoryItemCurySettings settings = SelectFrom<InventoryItemCurySettings>
                                     .Where<InventoryItemCurySettings.inventoryID.IsEqual<@P.AsInt>>
                                     .View
                                     .Select(Base, product.InventoryID);
                        
                    if (settings != null)
                    {
                            product.UnitCost = settings.StdCost;

                            Base.Products.Update(product);
                    
                    }
                }
            }

        }
        #endregion 

        protected virtual void _(Events.RowSelected<CRQuote> e, PXRowSelected b)
        {
            CRQuote row = e.Row;

            b?.Invoke(e.Cache, e.Args);

            UpdateCosts.SetEnabled(row.Status == "D");
        }

        protected virtual void _(Events.RowUpdating<CROpportunityProducts> e, PXRowUpdating b)
        {
            b?.Invoke(e.Cache, e.Args);

            CROpportunityProducts row = e.Row;
            if (row == null) return;

            InventoryItemCurySettings item = SelectFrom<InventoryItemCurySettings>.
                Where<InventoryItemCurySettings.inventoryID.IsEqual<P.AsInt>>.View.Select(Base, row.InventoryID);
            if (item == null) return;

            if (row.POCreate == false && row.InventoryID != null && row.UnitCost == decimal.Zero )
            {
                e.Cache.SetValueExt<CROpportunityProducts.unitCost>(row, item.StdCost);
            }
            else return;
        }

        protected virtual void _(Events.RowPersisting<CROpportunityProducts> e, PXRowPersisting b)
        {
            b?.Invoke(e.Cache, e.Args);

            CROpportunityProducts row = e.Row;
            if (row == null) return;

            InventoryItemCurySettings item = SelectFrom<InventoryItemCurySettings>.
                Where<InventoryItemCurySettings.inventoryID.IsEqual<P.AsInt>>.View.Select(Base, row.InventoryID);
            if (item == null) return;

            if (row.POCreate == false && row.UnitCost == decimal.Zero)
            {
                e.Cache.SetValueExt<CROpportunityProducts.unitCost>(row, item.StdCost);
            }
            else return;
        }

        /*protected virtual void _(Events.FieldUpdated<CRQuote, CRQuote.curyID> e, PXFieldUpdated b)
        {
            b?.Invoke(e.Cache, e.Args);

            CRQuote row = e.Row;
            if (row == null) return;

            if(e.OldValue != e.NewValue)
            {
                CurrencyRate2 curyInfo = SelectFrom<CurrencyRate2>.
                     Where<CurrencyRate2.toCuryID.IsEqual<P.AsString>.
                     And<CurrencyRate2.fromCuryID.IsEqual<P.AsString>>>.View.Select(Base, "GBP", row.CuryID);

                var newCurrency = e.NewValue;
                var oldCurrency = e.OldValue;

                CROpportunityProducts products = SelectFrom<CROpportunityProducts>.
                    Where<CROpportunityProducts.quoteID.IsEqual<P.AsGuid>>.View.Select(Base, row.QuoteID);

                var curyRate = curyInfo.CuryRate;
                var newCost = products.UnitCost * curyRate;

                e.Cache.SetValueExt<CROpportunityProducts.curyUnitCost>(products, newCost);

            }
        }*/
    }
}
