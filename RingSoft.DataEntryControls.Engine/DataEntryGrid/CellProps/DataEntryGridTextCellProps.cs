using System.Drawing;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
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

        public Color? SelectionColor { get; set; }

        public DataEntryGridTextCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }
    }
}
