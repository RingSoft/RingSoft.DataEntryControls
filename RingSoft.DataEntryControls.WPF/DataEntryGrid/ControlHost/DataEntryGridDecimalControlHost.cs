﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridDecimalControlHost : DataEntryGridDropDownControlHost<DecimalEditControl>
    {
        public DataEntryGridDecimalCellProps DecimalCellProps { get; private set; }
        public DataEntryGridDecimalControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridDecimalCellProps(DecimalCellProps.Row, DecimalCellProps.ColumnId,
                DecimalCellProps.NumericEditSetup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != DecimalCellProps.Value;
        }

        protected override void OnControlLoaded(DecimalEditControl control, DataEntryGridCellProps cellProps)
        {
            DecimalCellProps = (DataEntryGridDecimalCellProps) cellProps;

            control.Setup = DecimalCellProps.NumericEditSetup;
            control.Value = DecimalCellProps.Value;

            control.CalculatorValueChanged += (sender, args) => OnUpdateSource(GetCellValue());
            
            base.OnControlLoaded(control, cellProps);
        }
    }
}
