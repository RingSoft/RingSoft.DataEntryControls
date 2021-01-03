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

        public string ColumnHeader { get; set; }
    }
}
