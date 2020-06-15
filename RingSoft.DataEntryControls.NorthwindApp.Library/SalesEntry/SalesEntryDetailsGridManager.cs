using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public enum SalesEntryGridColumns
    {
        LineType = SalesEntryDetailsGridManager.LineTypeColumnId,
        Item = SalesEntryDetailsGridManager.ItemColumnId,
        Quantity = SalesEntryDetailsGridManager.QuantityColumnId,
        Price = SalesEntryDetailsGridManager.PriceColumnId,
        ExtendedPrice= SalesEntryDetailsGridManager.ExtendedPriceColumnId,
        Discount = SalesEntryDetailsGridManager.DiscountColumnId
    }

    public class SalesEntryDetailsGridManager : DataEntryGridManager
    {
        public SalesEntryViewModel ViewModel { get; }

        public const int LineTypeColumnId = 0;
        public const int ItemColumnId = 1;
        public const int QuantityColumnId = 2;
        public const int PriceColumnId = 3;
        public const int ExtendedPriceColumnId = 4;
        public const int DiscountColumnId = 5;

        public SalesEntryDetailsGridManager(SalesEntryViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new SalesEntryDetailsProductRow(this);
        }
    }
}
