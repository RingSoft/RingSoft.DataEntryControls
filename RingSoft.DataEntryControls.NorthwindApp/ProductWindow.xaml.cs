using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
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
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public ProductWindow()
        {
            InitializeComponent();

            RegisterFormKeyControl(ProductNameControl);

            QtyPerUnitEdit.MaxLength =
                AppGlobals.LookupContext.Products.GetFieldDefinition(p => p.QuantityPerUnit).MaxLength;
        }
    }
}
