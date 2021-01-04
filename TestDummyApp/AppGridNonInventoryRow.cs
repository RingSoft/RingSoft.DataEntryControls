using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;


namespace TestDummyApp
{
    public class AppGridNonInventoryRow : AppGridRow
    {
        public override AppGridLineTypes LineType => AppGridLineTypes.NonInventory;

        public string NonInventoryCode { get; set; }

        public double Price { get; set; }

        public DecimalEditControlSetup PriceSetup { get; } = Globals.GetNumericEditSetup();

        public AppGridNonInventoryRow(AppGridManager manager) : base(manager)
        {
            PriceSetup.FormatType = DecimalEditFormatTypes.Currency;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            AppGridColumns column = (AppGridColumns)columnId;
            DataEntryGridCellProps result = null;

            switch (column)
            {
                case AppGridColumns.StockNumber:
                    result = new DataEntryGridTextCellProps(this, columnId) {Text = NonInventoryCode};
                    break;
                case AppGridColumns.Location:
                    result = new DataEntryGridTextCellProps(this, columnId)
                    {
                        Text = "Disabled",
                    };
                    break;
                case AppGridColumns.Price:
                    result = new DataEntryGridDecimalCellProps(this, columnId, PriceSetup, (decimal)Price);
                    break;
            }

            if (result != null)
                return result;

            return base.GetCellProps(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            AppGridColumns column = (AppGridColumns)value.ColumnId;
            switch (column)
            {
                case AppGridColumns.LineType:
                case AppGridColumns.CheckBox:
                case AppGridColumns.Date:
                case AppGridColumns.Integer:
                case AppGridColumns.Button:
                    break;
                case AppGridColumns.StockNumber:
                    NonInventoryCode = ((DataEntryGridTextCellProps)value).Text;
                    break;
                case AppGridColumns.Location:
                    break;
                case AppGridColumns.Price:
                    var decimalCellProps = (DataEntryGridDecimalCellProps)value;
                    if (decimalCellProps.Value != null)
                        Price = (double)decimalCellProps.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            if (!string.IsNullOrEmpty(ParentRowId))
                DisplayStyleId = Globals.NonInventoryDisplayStyleId;

            AppGridColumns column = (AppGridColumns)columnId;
            DataEntryGridCellStyle result = null;

            var setSelectionColor = true;
            switch (column)
            {
                case AppGridColumns.Disabled:
                    setSelectionColor = false;
                    break;
                case AppGridColumns.LineType:
                    if (!ParentRowId.IsNullOrEmpty())
                        result = new DataEntryGridCellStyle(){CellStyleType = DataEntryGridCellStyleTypes.ReadOnly};
                    break;
                case AppGridColumns.StockNumber:
                    result = new DataEntryGridCellStyle(){ColumnHeader = "Non Inventory Code" };
                    break;
                case AppGridColumns.Location:
                    result = new DataEntryGridCellStyle();
                    if (ParentRowId.IsNullOrEmpty())
                    {
                        result.CellStyleType = DataEntryGridCellStyleTypes.Disabled;
                        result.DisplayStyleId = AppGridManager.RedDisplayId;
                    }
                    else
                    {
                        result.CellStyleType = DataEntryGridCellStyleTypes.ReadOnly;
                    }
                    break;
                case AppGridColumns.Price:
                    if (CheckBoxValue)
                        result = new DataEntryGridCellStyle {DisplayStyleId = AppGridManager.RedDisplayId};
                    break;
            }

            if (setSelectionColor && result == null)
                result = new DataEntryGridCellStyle();

            if (result != null)
            {
                if (result.DisplayStyleId == 0)
                    result.DisplayStyleId = DisplayStyleId;

                return result;
            }

            return base.GetCellStyle(columnId);
        }

        public override void LoadSale_DetailRow(SaleDetail saleDetail)
        {
            NonInventoryCode = saleDetail.StockNumber;
            Price = saleDetail.Price;
            base.LoadSale_DetailRow(saleDetail);
        }
    }
}
