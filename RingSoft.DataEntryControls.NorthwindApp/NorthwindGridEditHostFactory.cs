using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class NorthwindGridEditHostFactory : DataEntryGridHostFactory
    {
        public override DataEntryGridControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == DataEntryGridAutoFillCellProps.AutoFillControlHostId)
                return new DataEntryGridAutoFillHost(grid);
            
            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
