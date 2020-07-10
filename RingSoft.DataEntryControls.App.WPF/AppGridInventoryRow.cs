using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using System;
using System.Drawing;
using System.Globalization;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.App.WPF
{
    public class AppGridInventoryRow : AppGridRow
    {
        public override AppGridLineTypes LineType => AppGridLineTypes.Inventory;

        public string StockNumber { get; private set; }

        public string Location { get; private set; }

        public double Price { get; private set; }

        public DecimalEditControlSetup PriceSetup { get; } = Globals.GetNumericEditSetup();

        private bool _childrenAdded;
        public AppGridInventoryRow(AppGridManager manager) : base(manager)
        {
            PriceSetup.EditFormatType = DecimalEditFormatTypes.Currency;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            AppGridColumns column = (AppGridColumns) columnId;
            DataEntryGridCellProps result = null;
            switch (column)
            {
                case AppGridColumns.LineType:
                case AppGridColumns.Disabled:
                case AppGridColumns.CheckBox:
                    break;
                case AppGridColumns.StockNumber:
                    result = new DataEntryGridTextCellProps(this, columnId) { Text = StockNumber };
                    break;
                case AppGridColumns.Location:
                    result = new DataEntryGridTextCellProps(this, columnId) { Text = Location };
                    break;
                case AppGridColumns.Price:
                    result = new DataEntryGridDecimalCellProps(this, columnId, PriceSetup, (decimal)Price);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (result != null)
            {
                //FormatCellProps(result);
                return result;
            }
            return base.GetCellProps(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            AppGridColumns column = (AppGridColumns)value.ColumnId;
            switch (column)
            {
                case AppGridColumns.LineType:
                case AppGridColumns.CheckBox:
                    break;
                case AppGridColumns.StockNumber:
                    StockNumber = value.Text;
                    if (IsNew)
                    {
                        var lastRowIndex = Manager.Rows.Count - 1;
                        var rowIndex = Manager.Rows.IndexOf(this);
                        if (rowIndex == lastRowIndex - 1)
                        {
                            value.NextTabFocusRow = Manager.Rows[lastRowIndex];
                            value.NextTabFocusColumnId = (int)AppGridColumns.StockNumber;
                        }
                    }

                    if (!_childrenAdded)
                    {
                        var nonInventoryRow = new AppGridNonInventoryRow(AppGridManager)
                        {
                            NonInventoryCode = "Deposit",
                            Price = 5
                        };
                        AddChildRow(nonInventoryRow);

                        var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
                        var commentRow = new AppGridCommentRow(AppGridManager);
                        AddChildRow(commentRow);
                        commentRow.SetValue(text);

                        nonInventoryRow = new AppGridNonInventoryRow(AppGridManager)
                        {
                            NonInventoryCode = "Non Inventory Code #2",
                            Price = 5
                        };
                        AddChildRow(nonInventoryRow);
                        _childrenAdded = true;
                    }

                    break;
                case AppGridColumns.Location:
                    Location = value.Text;
                    break;
                case AppGridColumns.Price:
                    var decimalCellProps = (DataEntryGridDecimalCellProps) value;
                    if (decimalCellProps.Value != null) 
                        Price = (double) decimalCellProps.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (AppGridColumns) columnId;
            switch (column)
            {
                case AppGridColumns.Price:
                    if (CheckBoxValue)
                        return new DataEntryGridCellStyle(){ForegroundColor = Color.Red};
                    break;
            }
            return base.GetCellStyle(columnId);
        }

        public override void LoadSale_DetailRow(SaleDetail saleDetail)
        {
            StockNumber = saleDetail.StockNumber;
            Location = saleDetail.Location;
            Price = saleDetail.Price;
            base.LoadSale_DetailRow(saleDetail);
        }
    }
}
