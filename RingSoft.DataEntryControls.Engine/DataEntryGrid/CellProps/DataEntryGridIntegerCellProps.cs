// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridIntegerCellProps : DataEntryGridCellProps
    {
        public override int EditingControlId => IntegerEditHostId;

        public IntegerEditControlSetup NumericEditSetup { get; }

        public int? Value { get; set; }

        public override string Text => NumericEditSetup.FormatValue(Value);

        public DataEntryGridIntegerCellProps(DataEntryGridRow row, int columnId, IntegerEditControlSetup setup, int? value) : base(row, columnId)
        {
            NumericEditSetup = setup;
            Value = value;
        }
    }
}
