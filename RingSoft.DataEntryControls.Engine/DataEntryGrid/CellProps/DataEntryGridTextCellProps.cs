namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    public class DataEntryGridTextCellProps : DataEntryGridCellProps
    {
        public override int EditingControlId => TextBoxHostId;

        public int MaxLength { get; set; }

        public DataEntryGridTextCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }
    }
}
