// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum TextCasing
    {
        Normal = 0,
        Upper = 1,
        Lower = 2
    }

    public class DataEntryGridTextCellProps : DataEntryGridCellProps
    {
        public override int EditingControlId => TextBoxHostId;

        public int MaxLength { get; set; }

        public TextCasing CharacterCasing { get; set; }

        public DataEntryGridTextCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }
    }
}
