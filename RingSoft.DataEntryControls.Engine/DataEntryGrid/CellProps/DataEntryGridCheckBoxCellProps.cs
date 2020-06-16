﻿namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    public class DataEntryGridCheckBoxCellProps : DataEntryGridCellProps
    {
        public bool Value { get; set; }

        public DataEntryGridCheckBoxCellProps(DataEntryGridRow row, int columnId, bool value) : base(row, columnId)
        {
            Value = value;
        }

        public override int EditingControlId => CheckBoxHostId;
    }
}