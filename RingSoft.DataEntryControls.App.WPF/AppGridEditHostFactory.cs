using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;

namespace RingSoft.DataEntryControls.App.WPF
{
    public class AppGridEditHostFactory : DataEntryGridHostFactory
    {
        public override DataEntryGridControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            //if (editingControlHostId == DataEntryGridCellProps.DecimalEditHostId)
            //    return new XceedCalcHost(grid);

            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
