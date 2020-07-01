namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    public class DataEntryGridDecimalCellProps : DataEntryGridCellProps
    {
        public override int EditingControlId => DecimalEditHostId;

        public DataEntryNumericEditSetup NumericEditSetup { get; }

        public decimal? Value { get; set; }

        public override string Text => NumericEditSetup.FormatValue(Value);

        public DataEntryGridDecimalCellProps(DataEntryGridRow row, int columnId, DataEntryNumericEditSetup setup, decimal? value) : base(row, columnId)
        {
            NumericEditSetup = setup;
            Value = value;
        }
    }
}
