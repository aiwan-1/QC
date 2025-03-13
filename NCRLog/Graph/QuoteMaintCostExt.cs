using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CR;
using PX.Objects.IN;
using static PX.Objects.CR.QuoteMaint;

namespace QualityControl
{
    public class QuoteMaintCostExt : PXGraphExtension<PX.Objects.CR.QuoteMaint>
    {
        public static bool IsActive() => true;

        
       
        public PXOrderedSelect<CRQuote, CROpportunityProducts,
                Where<CROpportunityProducts.quoteID, Equal<Current<CRQuote.quoteID>>>,
                OrderBy<Asc<CROpportunityProducts.sortOrder>>>
            Products;

        
        public PXSelect<CRQuote,
                Where<CRQuote.opportunityID, Equal<Optional<CRQuote.opportunityID>>, And<CRQuote.quoteType, Equal<CRQuoteTypeAttribute.distribution>>>> Quote;


        #region Actions
        public PXAction<CRQuote> UpdateCosts;
        [PXButton(DisplayOnMainToolbar = true)]
        [PXUIField(DisplayName = "Update Costs")]
        protected virtual void updateCosts()
        {
            var quote = Products.Current;
            if (quote.QuoteID == null) return;
            var lines = Products.Select();
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
                            product.CuryUnitCost = settings.StdCost;

                            Products.Update(product);
                    
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

    }
}
