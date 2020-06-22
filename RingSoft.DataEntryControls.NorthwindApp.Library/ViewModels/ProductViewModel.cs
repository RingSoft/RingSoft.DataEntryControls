using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels
{
    public class ProductViewModel : DbMaintenanceViewModel<Products>
    {
        public override TableDefinition<Products> TableDefinition => AppGlobals.LookupContext.Products;

        private int _productId;

        public int ProductId
        {
            get => _productId;
            set
            {
                if (_productId == value)
                    return;

                _productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }

        private string _productName;

        public string ProductName
        {
            get => _productName;
            set
            {
                if (_productName == value)
                    return;

                _productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        private AutoFillSetup _supplierAutoFillSetup;

        public AutoFillSetup SupplierAutoFillSetup
        {
            get => _supplierAutoFillSetup;
            set
            {
                if (_supplierAutoFillSetup == value)
                    return;

                _supplierAutoFillSetup = value;
                OnPropertyChanged(nameof(SupplierAutoFillSetup));
            }
        }

        private AutoFillValue _supplierAutoFillValue;

        public AutoFillValue SupplierAutoFillValue
        {
            get => _supplierAutoFillValue;
            set
            {
                if (_supplierAutoFillValue == value)
                    return;

                _supplierAutoFillValue = value;
                OnPropertyChanged(nameof(SupplierAutoFillValue));
            }
        }

        private AutoFillSetup _categoryAutoFillSetup;

        public AutoFillSetup CategoryAutoFillSetup
        {
            get => _categoryAutoFillSetup;
            set
            {
                if (_categoryAutoFillSetup == value)
                    return;

                _categoryAutoFillSetup = value;
                OnPropertyChanged(nameof(CategoryAutoFillSetup));
            }
        }

        private AutoFillValue _categoryAutoFillValue;

        public AutoFillValue CategoryAutoFillValue
        {
            get => _categoryAutoFillValue;
            set
            {
                if (_supplierAutoFillValue == value)
                    return;

                _categoryAutoFillValue = value;
                OnPropertyChanged(nameof(CategoryAutoFillValue));
            }
        }

        protected override void Initialize()
        {
            SupplierAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.SupplierId))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            CategoryAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CategoryId))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            base.Initialize();
        }

        protected override void LoadFromEntity(Products newEntity)
        {
            throw new System.NotImplementedException();
        }

        protected override Products GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            throw new System.NotImplementedException();
        }

        protected override bool SaveEntity(Products entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
