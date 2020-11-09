﻿// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridDecimalCellProps : DataEntryGridCellProps
    {
        public override int EditingControlId => DecimalEditHostId;

        public DecimalEditControlSetup NumericEditSetup { get; }

        public decimal? Value { get; set; }

        public override string Text => NumericEditSetup.FormatValue(Value);

        public DataEntryGridDecimalCellProps(DataEntryGridRow row, int columnId, DecimalEditControlSetup setup, decimal? value) : base(row, columnId)
        {
            NumericEditSetup = setup;
            Value = value;
        }
    }
}
