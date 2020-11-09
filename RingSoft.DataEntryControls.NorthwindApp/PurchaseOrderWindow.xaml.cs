using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrderWindow.xaml
    /// </summary>
    public partial class PurchaseOrderWindow : IPurchaseOrderView
    {
        public override DbMaintenanceViewModelBase ViewModel => PurchaseOrderViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public PurchaseOrderWindow()
        {
            InitializeComponent();
            Initialize();
            RegisterFormKeyControl(PoNumberControl);

            SupplierControl.LostFocus += (sender, args) => PurchaseOrderViewModel.OnSupplierLostFocus();
            PurchaseOrderViewModel.CheckDirtyMessageShown += (sender, args) =>
            {
                if (args.Result == MessageButtons.Cancel)
                    DetailsGrid.Refocus();
            };

            var tableDefinition = PurchaseOrderViewModel.TableDefinition;
            AddressEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.Address).MaxLength;
            CityEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.City).MaxLength;
            RegionEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.Region).MaxLength;
            PostalCodeEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.PostalCode).MaxLength;
            CountryEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.Country).MaxLength;
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
            PoNumberControl.Focus();
            base.ResetViewForNewRecord();
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
            var table = AppGlobals.LookupContext.Purchases;
            var focusSuccess = true;

            if (fieldDefinition == table.GetFieldDefinition(p => p.PoNumber))
                focusSuccess = PoNumberControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.SupplierId))
                focusSuccess = SupplierControl.Focus();

            if (focusSuccess)
                base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
