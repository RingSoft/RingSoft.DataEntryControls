using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;


namespace TestDummyApp
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
            PriceSetup.FormatType = DecimalEditFormatTypes.Currency;
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
                case AppGridColumns.Date:
                case AppGridColumns.Integer:
                case AppGridColumns.Button:
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
                    if (value is DataEntryGridTextCellProps textCellProps)
                        StockNumber = textCellProps.Text;

                    DataEntryGridRow nextTabFocusRow = null;
                    if (IsNew)
                    {
                        var lastRowIndex = Manager.Rows.Count - 1;
                        var rowIndex = Manager.Rows.IndexOf(this);
                        if (rowIndex == lastRowIndex - 1)
                        {
                            nextTabFocusRow = Manager.Rows[lastRowIndex];
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

                    if (nextTabFocusRow != null)
                    {
                        value.OverrideCellMovement = true;
                        Manager.Grid.GotoCell(nextTabFocusRow, (int)AppGridColumns.StockNumber);
                    }
                    break;
                case AppGridColumns.Location:
                    if (value is DataEntryGridTextCellProps locationTextCellProps)
                        Location = locationTextCellProps.Text;
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
                case AppGridColumns.LineType:
                case AppGridColumns.StockNumber:
                case AppGridColumns.CheckBox:
                    break;
                default:
                    if (IsNew)
                        return new DataEntryGridCellStyle{State = DataEntryGridCellStates.Disabled};
                    break;
            }
            switch (column)
            {
                case AppGridColumns.Price:
                    if (CheckBoxValue)
                        return new DataEntryGridCellStyle {DisplayStyleId = AppGridManager.RedDisplayId};
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
