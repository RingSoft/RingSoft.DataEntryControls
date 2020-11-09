using System;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridDateCellProps : DataEntryGridCellProps
    {
        public override int EditingControlId => DateEditHostId;

        public DateEditControlSetup Setup { get; }

        public DateTime? Value { get; set; }

        public override string Text => Setup.FormatValueForDisplay(Value);

        public DataEntryGridDateCellProps(DataEntryGridRow row, int columnId, DateEditControlSetup setup,
            DateTime? value) : base(row, columnId)
        {
            Setup = setup;
            Value = value;
        }
    }
}
