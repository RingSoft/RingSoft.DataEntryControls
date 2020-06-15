using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for SalesEntryWindow.xaml
    /// </summary>
    public partial class SalesEntryWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => SalesEntryViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public SalesEntryWindow()
        {
            InitializeComponent();
            Initialize();

            CustomerControl.LostFocus += (sender, args) => SalesEntryViewModel.OnCustomerIdLostFocus();
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
            CustomerControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = AppGlobals.LookupContext.Orders;

            if (fieldDefinition == table.GetFieldDefinition(p => p.CustomerId))
                CustomerControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.EmployeeId))
                EmployeeControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.ShipVia))
                ShipViaControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
