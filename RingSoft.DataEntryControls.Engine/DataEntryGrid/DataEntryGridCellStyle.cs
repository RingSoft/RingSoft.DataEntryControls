using System.Drawing;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum DataEntryGridCellStyles
    {
        Enabled = 0,
        ReadOnly = 1,
        Disabled = 2
    }

    public class DataEntryGridCellStyle
    {
        public DataEntryGridCellStyles CellStyle { get; set; }

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public string ColumnHeader { get; set; }
    }
}
