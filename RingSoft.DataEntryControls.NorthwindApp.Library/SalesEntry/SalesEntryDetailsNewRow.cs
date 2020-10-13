using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsNewRow : SalesEntryDetailsProductRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.NewRow;

        public SalesEntryDetailsNewRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
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
            var isLastRow = Manager.Rows.IndexOf(this) >= Manager.Rows.Count - 2;
            if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
            {
                var validProduct = autoFillCellProps.AutoFillValue.PrimaryKeyValue.ContainsValidData();
                if (validProduct)
                {
                    var productRow = new SalesEntryDetailsProductRow(SalesEntryDetailsManager);
                    SalesEntryDetailsManager.ReplaceRow(this, productRow);
                    productRow.LoadFromItemAutoFillValue(autoFillCellProps.AutoFillValue);
                    Manager.Grid.UpdateRow(productRow);
                    DoScannerMode(value, isLastRow);
                    return;
                }
            }

            base.SetCellValue(value);
            DoScannerMode(value, isLastRow);
        }

        private void DoScannerMode(DataEntryGridCellProps value, bool isLastRow)
        {
            if (isLastRow && RowReplacedBy != null && RowReplacedBy is SalesEntryDetailsProductRow)
            {
                value.NextTabFocusRow = Manager.Rows[^1];
                value.NextTabFocusColumnId = (int) SalesEntryGridColumns.Item;
            }
        }
    }
}
