using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsNewRow : SalesEntryDetailsProductRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.NewRow;

        public bool IsScannerRow
        {
            get
            {
                if (SalesEntryDetailsManager.SalesEntryViewModel.ScannerMode)
                {
                    var currentRowIndex = Manager.Rows.IndexOf(this);
                    return currentRowIndex >= Manager.Rows.Count - 2;
                }

                return false;
            }
        }

        public SalesEntryDetailsNewRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.LineType:
                    var text = "New";
                    if (IsScannerRow)
                        text = "Scanner";
                    return new DataEntryGridTextCellProps(this, columnId){Text = text};
                case SalesEntryGridColumns.Item:
                    break;
                default:
                    return new DataEntryGridTextCellProps(this, columnId);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (SalesEntryGridColumns)columnId;
            switch (column)
            {
                case SalesEntryGridColumns.LineType:

                case SalesEntryGridColumns.Item:
                    break;
                default:
                    return new DataEntryGridCellStyle {CellStyle = DataEntryGridCellStyles.Disabled};
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var isScannerRow = IsScannerRow;
            if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
            {
                var validProduct = autoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid;
                if (validProduct)
                {
                    var productRow = new SalesEntryDetailsProductRow(SalesEntryDetailsManager);
                    SalesEntryDetailsManager.ReplaceRow(this, productRow);
                    productRow.LoadFromItemAutoFillValue(autoFillCellProps.AutoFillValue);
                    Manager.Grid.UpdateRow(productRow);
                    DoScannerMode(value, isScannerRow);
                    return;
                }
            }

            base.SetCellValue(value);
            DoScannerMode(value, isScannerRow);
        }

        private void DoScannerMode(DataEntryGridCellProps value, bool isScannerRow)
        {
            if (isScannerRow && RowReplacedBy != null && RowReplacedBy is SalesEntryDetailsProductRow &&
                value.CellLostFocusType == CellLostFocusTypes.TabRight)
            {
                Manager.Grid.GotoCell(Manager.Rows[^1], (int)SalesEntryGridColumns.Item);
            }
        }
    }
}
