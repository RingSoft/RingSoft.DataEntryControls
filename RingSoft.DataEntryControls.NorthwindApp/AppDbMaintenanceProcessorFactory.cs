using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class AppDbMaintenanceProcessorFactory : DbMaintenanceProcessorFactory
    {
        public override DbMaintenanceWindowProcessor GetProcessor()
        {
            return new AppDbMaintenanceWindowProcessor();
        }

        public override DbMaintenanceUserControlProcessor GetUserControlProcessor(DbMaintenanceViewModelBase viewModel, Control buttonsControl,
            DbMaintenanceUserControl userControl, DbMaintenanceStatusBar statusBar, IUserControlHost host)
        {
            return new AppDbMaintenanceUserControlProcessor(viewModel, buttonsControl, userControl, statusBar, host);
        }
    }
}
