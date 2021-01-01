using System.Drawing;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum DataEntryGridCellStyleTypes
    {
        Enabled = 0,
        ReadOnly = 1,
        Disabled = 2
    }

    public class DataEntryGridCellStyle
    {
        public int DisplayStyleId { get; set; }

        public DataEntryGridCellStyleTypes CellStyleType { get; set; }

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public Color SelectionColor { get; set; }

        public string ColumnHeader { get; set; }
    }
}
