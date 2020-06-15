namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    public class DataEntryGridTextCellProps : DataEntryGridCellProps
    {
        public override int EditingControlId => TextBoxHostId;

        public DataEntryGridTextCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }
    }
}
