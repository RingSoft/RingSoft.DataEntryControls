using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class AppDbMaintenanceProcessorFactory : DbMaintenanceProcessorFactory
    {
        public override DbMaintenanceWindowProcessor GetProcessor()
        {
            return new AppDbMaintenanceWindowProcessor();
        }
    }
}
