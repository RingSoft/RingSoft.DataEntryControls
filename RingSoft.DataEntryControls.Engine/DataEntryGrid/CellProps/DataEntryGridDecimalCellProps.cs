// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridDecimalCellProps : DataEntryGridEditingCellProps
    {
        public override int EditingControlId => DataEntryGridEditingCellProps.DecimalEditHostId;

        public DecimalEditControlSetup NumericEditSetup { get; }

        public decimal? Value { get; set; }

        public DataEntryGridDecimalCellProps(DataEntryGridRow row, int columnId, DecimalEditControlSetup setup, decimal? value) : base(row, columnId)
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
