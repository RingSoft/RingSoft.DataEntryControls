using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public abstract class SalesEntryDetailsValueRow : SalesEntryDetailsRow
    
    {
        public decimal Quantity { get; private set; }

        public decimal Price { get; private set; }

        public decimal ExtendedPrice { get; private set; }

        protected SalesEntryDetailsValueRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Quantity:
                case SalesEntryGridColumns.Price:
                case SalesEntryGridColumns.ExtendedPrice:
                    return new DataEntryGridTextCellProps(this, columnId);
            }

            return base.GetCellProps(columnId);
        }
    }
}
