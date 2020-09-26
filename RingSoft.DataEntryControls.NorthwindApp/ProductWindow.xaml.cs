using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => ProductViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public ProductWindow()
        {
            InitializeComponent();

            Initialize();
            RegisterFormKeyControl(ProductNameControl);
        }

    }
}
