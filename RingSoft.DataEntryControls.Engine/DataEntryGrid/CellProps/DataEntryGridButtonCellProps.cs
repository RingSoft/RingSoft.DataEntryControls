// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridButtonCellProps : DataEntryGridTextCellProps
    {
        public override int EditingControlId => DataEntryGridEditingCellProps.ButtonHostId;

        public string ButtonContent { get; }

        public DataEntryGridButtonCellProps(DataEntryGridRow row, int columnId, string buttonContent) : base(row, columnId)
        {
            ButtonContent = buttonContent;
        }
    }
}
