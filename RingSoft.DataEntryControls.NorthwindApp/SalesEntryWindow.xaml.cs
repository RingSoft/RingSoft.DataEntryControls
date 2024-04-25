using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for SalesEntryWindow.xaml
    /// </summary>
    public partial class SalesEntryWindow : ISalesEntryMaintenanceView
    {
        public override DbMaintenanceViewModelBase ViewModel => SalesEntryViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public SalesEntryWindow()
        {
            InitializeComponent();
            
            SalesEntryViewModel.CheckDirtyMessageShown += (_, args) =>
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
    }
}
