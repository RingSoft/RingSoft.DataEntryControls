// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridButtonCellProps : DataEntryGridTextCellProps
    {
        public override int EditingControlId => DataEntryGridEditingCellProps.ButtonHostId;

        public DataEntryGridButtonCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }
    }
}
