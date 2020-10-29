using System.Drawing;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    public abstract class DataEntryGridDropDownCellProps : DataEntryGridCellProps
    {
        public Color? SelectionColor { get; set; }

        protected DataEntryGridDropDownCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }
    }
}
