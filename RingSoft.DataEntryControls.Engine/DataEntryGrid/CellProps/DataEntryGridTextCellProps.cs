// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum TextCasing
    {
        Normal = 0,
        Upper = 1,
        Lower = 2
    }

    public class DataEntryGridTextCellProps : DataEntryGridEditingCellProps
    {
        public override int EditingControlId => DataEntryGridEditingCellProps.TextBoxHostId;

        public string Text { get; set; }

        public int MaxLength { get; set; }

        public TextCasing CharacterCasing { get; set; }

        public DataEntryGridTextCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }

        public DataEntryGridTextCellProps(DataEntryGridRow row, int columnId, string text) : base(row, columnId)
        {
            Text = text;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return Text;
        }
    }
}
