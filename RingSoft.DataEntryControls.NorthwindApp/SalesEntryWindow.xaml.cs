using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for SalesEntryWindow.xaml
    /// </summary>
    public partial class SalesEntryWindow : ISalesEntryMaintenanceView
    {
        public override DbMaintenanceViewModelBase ViewModel => SalesEntryViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public SalesEntryWindow()
        {
            InitializeComponent();
            Initialize();

            CustomerControl.LostFocus += (sender, args) => SalesEntryViewModel.OnCustomerIdLostFocus();
            CustomerControl.PreviewLostKeyboardFocus += (sender, args) =>
            {
                if (!this.IsWindowClosing(args.NewFocus))
                {
                    if (!SalesEntryViewModel.ValidateCustomer())
                        args.Handled = true;
                }
            };
            SalesEntryViewModel.CheckDirtyMessageShown += (sender, args) =>
            {
                if (args.Result == MessageButtons.Cancel)
                    DetailsGrid.Refocus();
            };

            var tableDefinition = SalesEntryViewModel.TableDefinition;
            ShipNameEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.ShipName).MaxLength;
            AddressEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.ShipAddress).MaxLength;
            CityEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.ShipCity).MaxLength;
            RegionEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.ShipRegion).MaxLength;
            PostalCodeEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.ShipPostalCode).MaxLength;
            CountryEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.ShipCountry).MaxLength;
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
            CustomerControl.Focus();
            base.ResetViewForNewRecord();
        }

        public InvalidProductResult CorrectInvalidProduct(AutoFillValue invalidProductValue)
        {
            var invalidProductWindow = new InvalidProductWindow(invalidProductValue);
            invalidProductWindow.Owner = this;
            return invalidProductWindow.ShowDialog();
        }

        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var memoEditor = new DataEntryGridMemoEditor(comment);
            memoEditor.Owner = this;
            memoEditor.Title = "Edit Comment";
            return memoEditor.ShowDialog();
        }

        public void GridValidationFail()
        {
            TabControl.SelectedIndex = 0;
            DetailsGrid.Focus();
        }

        public void SetInitGridFocus(SalesEntryDetailsRow row, int columnId)
        {
            TabControl.SelectedIndex = 0;
            DetailsGrid.Focus();
            DetailsGrid.GotoCell(row, columnId);
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = AppGlobals.LookupContext.Orders;
            var focusSuccess = true;

            if (fieldDefinition == table.GetFieldDefinition(p => p.CustomerId))
                focusSuccess = CustomerControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.EmployeeId))
                focusSuccess = EmployeeControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.ShipVia))
                focusSuccess = ShipViaControl.Focus();

            if (focusSuccess)
                base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
