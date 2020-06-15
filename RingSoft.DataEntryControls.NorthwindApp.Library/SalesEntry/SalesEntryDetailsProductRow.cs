using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsProductRow : SalesEntryDetailsValueRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.Product;

        public AutoFillValue ProductValue { get; private set; }

        private AutoFillSetup _productAutoFillSetup;

        public SalesEntryDetailsProductRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            _productAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.ProductId));
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    return new DataEntryGridAutoFillCellProps(this, columnId, _productAutoFillSetup, ProductValue);
            }

            return base.GetCellProps(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (SalesEntryGridColumns) value.ColumnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                        ProductValue = autoFillCellProps.AutoFillValue;
                    break;
            }
            base.SetCellValue(value);
        }
    }
}
