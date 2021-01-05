﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public class DataEntryGridDecimalControlHost : DataEntryGridDropDownControlHost<DecimalEditControl>
    {
        public DataEntryGridDecimalCellProps DecimalCellProps { get; private set; }
        public DataEntryGridDecimalControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridDecimalCellProps(Row, ColumnId,
                DecimalCellProps.NumericEditSetup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != DecimalCellProps.Value;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            DecimalCellProps = (DataEntryGridDecimalCellProps)cellProps;
        }

        protected override void OnControlLoaded(DecimalEditControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            DecimalCellProps = (DataEntryGridDecimalCellProps) cellProps;

            control.Setup = DecimalCellProps.NumericEditSetup;
            control.Value = DecimalCellProps.Value;

            control.CalculatorValueChanged += (sender, args) => OnUpdateSource(GetCellValue());
            
            base.OnControlLoaded(control, cellProps, cellStyle);
        }
    }
}