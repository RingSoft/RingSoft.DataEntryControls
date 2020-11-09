namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    public enum CellLostFocusTypes
    {
        MouseClick = 0,
        TabLeft = 1,
        TabRight = 2,
        KeyboardNavigation = 3
    }
    public abstract class DataEntryGridCellProps
    {
        public const int TextBoxHostId = 0;
        public const int ComboBoxHostId = 1;
        public const int CheckBoxHostId = 2;
        public const int ButtonHostId = 3;
        public const int DecimalEditHostId = 4;
        public const int DateEditHostId = 5;
        public const int IntegerEditHostId = 6;

        public abstract int EditingControlId { get; }

        public int ColumnId { get; }

        public DataEntryGridRow Row { get; }

        public virtual string Text { get; set; }

        public bool OverrideCellMovement { get; set; }

        public CellLostFocusTypes CellLostFocusType { get; set; }

        public DataEntryGridCellProps(DataEntryGridRow row, int columnId)
        {
            Row = row;
            ColumnId = columnId;
        }
    }
}
