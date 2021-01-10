using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;

namespace TestDummyApp
{
    public class AppGridEditHostFactory : DataEntryGridHostFactory
    {
        public override DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == Globals.LineTypeControlId)
                return new LineTypeControlHost(grid);

            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
