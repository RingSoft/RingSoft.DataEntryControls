using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrderWindow.xaml
    /// </summary>
    public partial class PurchaseOrderWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => PurchaseOrderViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public PurchaseOrderWindow()
        {
            InitializeComponent();
            Initialize();
        }
    }
}
