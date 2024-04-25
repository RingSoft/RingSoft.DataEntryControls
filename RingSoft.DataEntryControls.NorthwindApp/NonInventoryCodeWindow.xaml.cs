using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for NonInventoryCodeWindow.xaml
    /// </summary>
    public partial class NonInventoryCodeWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => NonInventoryCodeViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public NonInventoryCodeWindow()
        {
            InitializeComponent();

            RegisterFormKeyControl(DescriptionControl);
        }
    }
}
