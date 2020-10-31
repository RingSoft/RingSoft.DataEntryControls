using System.Drawing;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public class PurchaseOrderDetailsDirectExpenseRow : PurchaseOrderDetailsRow
    {
        public override PurchaseOrderDetailsLineTypes LineType => PurchaseOrderDetailsLineTypes.DirectExpense;

        public string Description { get; set; }

        public decimal Price { get; set; }

        private DecimalEditControlSetup _priceSetup = AppGlobals.CreateNewDecimalEditControlSetup();

        public PurchaseOrderDetailsDirectExpenseRow(PurchaseOrderDetailsGridManager manager) : base(manager)
        {
            BackgroundColor = Color.LightPink;
            ForegroundColor = Color.Black;
            _priceSetup.FormatType = DecimalEditFormatTypes.Currency;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    break;
                case PurchaseOrderColumns.Item:
                    return new DataEntryGridTextCellProps(this, columnId){Text = Description};
                case PurchaseOrderColumns.Quantity:
                    return new DataEntryGridTextCellProps(this, columnId);
                case PurchaseOrderColumns.Price:
                    return new DataEntryGridDecimalCellProps(this, columnId, _priceSetup, Price);
                case PurchaseOrderColumns.ExtendedPrice:
                    return new DataEntryGridTextCellProps(this, columnId);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                case PurchaseOrderColumns.Price:
                    break;
                case PurchaseOrderColumns.Item:
                    return new DataEntryGridCellStyle{ColumnHeader = "Description"};
                    break;
                default:
                    return new DataEntryGridCellStyle{CellStyle = DataEntryGridCellStyles.Disabled};
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (PurchaseOrderColumns) value.ColumnId;
            switch (column)
            {
                case PurchaseOrderColumns.Item:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        Description = textCellProps.Text;
                    }
                    break;
                case PurchaseOrderColumns.Price:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        if (decimalCellProps.Value != null) 
                            Price = (decimal)decimalCellProps.Value;
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }
    }
}
