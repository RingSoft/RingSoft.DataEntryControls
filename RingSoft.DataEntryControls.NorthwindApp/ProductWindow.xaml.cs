using RingSoft.DataEntryControls.NorthwindApp.Library;
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

        public ProductWindow()
        {
            InitializeComponent();

            Initialize();
            RegisterFormKeyControl(ProductNameControl);

            QtyPerUnitEdit.MaxLength =
                AppGlobals.LookupContext.Products.GetFieldDefinition(p => p.QuantityPerUnit).MaxLength;
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
            ProductNameControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = AppGlobals.LookupContext.Products;

            if (fieldDefinition == table.GetFieldDefinition(p => p.ProductName))
                ProductNameControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.SupplierId))
                SupplierControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.CategoryId))
                CategoryControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.NonInventoryCodeId))
                NonInventoryCodeControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
