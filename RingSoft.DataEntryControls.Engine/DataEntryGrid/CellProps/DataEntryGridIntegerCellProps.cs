// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridIntegerCellProps : DataEntryGridEditingCellProps
    {
        public override int EditingControlId => DataEntryGridEditingCellProps.IntegerEditHostId;

        public IntegerEditControlSetup NumericEditSetup { get; }

        public int? Value { get; set; }

        public DataEntryGridIntegerCellProps(DataEntryGridRow row, int columnId, IntegerEditControlSetup setup, int? value) : base(row, columnId)
        {
            NumericEditSetup = setup;
            Value = value;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return NumericEditSetup.FormatValue(Value);
        }
    }
}
