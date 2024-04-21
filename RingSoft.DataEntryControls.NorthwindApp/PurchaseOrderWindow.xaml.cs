using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrderWindow.xaml
    /// </summary>
    public partial class PurchaseOrderWindow : IPurchaseOrderView
    {
        public override DbMaintenanceViewModelBase ViewModel => PurchaseOrderViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public PurchaseOrderWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(PoNumberControl);

            PurchaseOrderViewModel.CheckDirtyMessageShown += (_, args) =>
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

            PoNumberControl.LostFocus += PoNumberControl_LostFocus;
        }

        private void PoNumberControl_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Tab) && !PurchaseOrderViewModel.SupplierUiCommand.IsEnabled)
            {
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                {
                    OrderDateControl.Focus();
                }
            }
        }

        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var memoEditor = new DataEntryGridMemoEditor(comment);
            memoEditor.Owner = this;
            memoEditor.Title = "Edit Comment";
            return memoEditor.ShowDialog();
        }
    }
}
