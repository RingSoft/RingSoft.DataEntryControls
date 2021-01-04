using System;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridDateCellProps : DataEntryGridEditingCellProps
    {
        public override string DataValue => Setup.FormatValueForDisplay(Value);

        public override int EditingControlId => DataEntryGridEditingCellProps.DateEditHostId;

        public DateEditControlSetup Setup { get; }

        public DateTime? Value { get; set; }

        public DataEntryGridDateCellProps(DataEntryGridRow row, int columnId, DateEditControlSetup setup,
            DateTime? value) : base(row, columnId)
        {
            Setup = setup;
            Value = value;
        }
    }
}
