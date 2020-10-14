using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
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
            SalesEntryViewModel.CheckDirtyMessageShown += (sender, args) =>
            {
                if (args.Result == MessageButtons.Cancel)
                    DetailsGrid.Refocus();
            };
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

        public bool ShowCommentEditor(GridMemoValue comment)
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
