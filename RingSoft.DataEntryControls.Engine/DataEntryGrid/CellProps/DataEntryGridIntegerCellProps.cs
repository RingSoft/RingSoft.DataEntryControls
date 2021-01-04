// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridIntegerCellProps : DataEntryGridEditingCellProps
    {
        public override string DataValue => NumericEditSetup.FormatValue(Value);

        public override int EditingControlId => DataEntryGridEditingCellProps.IntegerEditHostId;

        public IntegerEditControlSetup NumericEditSetup { get; }

        public int? Value { get; set; }

        public DataEntryGridIntegerCellProps(DataEntryGridRow row, int columnId, IntegerEditControlSetup setup, int? value) : base(row, columnId)
        {
            NumericEditSetup = setup;
            Value = value;
        }
    }
}
