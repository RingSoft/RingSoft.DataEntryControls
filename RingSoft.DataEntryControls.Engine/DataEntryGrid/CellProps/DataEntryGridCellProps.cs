namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{

    public abstract class DataEntryGridCellProps
    {
        public const int TextBoxHostId = 0;
        public const int ComboBoxHostId = 1;
        public const int CheckBoxHostId = 2;
        public const int ButtonHostId = 3;

        public abstract int EditingControlId { get; }

        public int ColumnId { get; }

        public DataEntryGridRow Row { get; }

        public virtual string Text { get; set; }

        public bool ValidationResult { get; set; } = true;

        public DataEntryGridRow NextTabFocusRow { get; set; }

        public int NextTabFocusColumnId { get; set; } = -1;

        public DataEntryGridCellProps(DataEntryGridRow row, int columnId)
        {
            Row = row;
            ColumnId = columnId;
        }
    }
}
