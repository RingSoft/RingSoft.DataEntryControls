﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridIntegerControlHost : DataEntryGridDropDownControlHost<IntegerEditControl>
    {
        public DataEntryGridIntegerCellProps IntegerCellProps { get; private set; }
        public DataEntryGridIntegerControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridIntegerCellProps(Row, ColumnId,
                IntegerCellProps.NumericEditSetup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != IntegerCellProps.Value;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            IntegerCellProps = (DataEntryGridIntegerCellProps)cellProps;
        }

        protected override void OnControlLoaded(IntegerEditControl control, DataEntryGridCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            IntegerCellProps = (DataEntryGridIntegerCellProps) cellProps;

            control.Setup = IntegerCellProps.NumericEditSetup;
            control.Value = IntegerCellProps.Value;
            
            base.OnControlLoaded(control, cellProps, cellStyle);
        }
    }
}
