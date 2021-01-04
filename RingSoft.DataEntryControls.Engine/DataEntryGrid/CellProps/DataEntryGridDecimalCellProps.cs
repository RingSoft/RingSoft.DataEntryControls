// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridDecimalCellProps : DataEntryGridEditingCellProps
    {
        public override string DataValue => NumericEditSetup.FormatValue(Value);

        public override int EditingControlId => DataEntryGridEditingCellProps.DecimalEditHostId;

        public DecimalEditControlSetup NumericEditSetup { get; }

        public decimal? Value { get; set; }

        public DataEntryGridDecimalCellProps(DataEntryGridRow row, int columnId, DecimalEditControlSetup setup, decimal? value) : base(row, columnId)
        {
            NumericEditSetup = setup;
            Value = value;
        }
    }
}
