﻿// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum CellLostFocusTypes
    {
        LostFocus = 0,
        TabLeft = 1,
        TabRight = 2,
        KeyboardNavigation = 3,
        ValidatingGrid = 4
    }
    public abstract class DataEntryGridCellProps
    {
        public int ColumnId { get; }

        public DataEntryGridRow Row { get; }

        public abstract string DataValue { get; }

        public bool ControlMode { get; set; }

        public DataEntryGridCellProps(DataEntryGridRow row, int columnId)
        {
            Row = row;
            ColumnId = columnId;
        }
    }

    public abstract class DataEntryGridEditingCellProps : DataEntryGridCellProps
    {
        public const int TextBoxHostId = 0;
        public const int ComboBoxHostId = 1;
        public const int CheckBoxHostId = 2;
        public const int ButtonHostId = 3;
        public const int DecimalEditHostId = 4;
        public const int DateEditHostId = 5;
        public const int IntegerEditHostId = 6;

        public abstract int EditingControlId { get; }

        public bool OverrideCellMovement { get; set; }

        public CellLostFocusTypes CellLostFocusType { get; set; }

        protected DataEntryGridEditingCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }
    }
}
