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

        private string _quantityPerUnit;

        public string QuantityPerUnit
        {
            get => _quantityPerUnit;
            set
            {
                if (_quantityPerUnit == value)
                    return;

                _quantityPerUnit = value;
                OnPropertyChanged(nameof(QuantityPerUnit));
            }
        }

        private decimal? _unitPrice;

        public decimal? UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPrice == value)
                    return;

                _unitPrice = value;
                OnPropertyChanged(nameof(UnitPrice));
            }
        }

        private decimal? _unitsInStock;

        public decimal? UnitsInStock
        {
            get => _unitsInStock;
            set
            {
                if (_unitsInStock == value)
                    return;

                _unitsInStock = value;
                OnPropertyChanged(nameof(UnitsInStock));
            }
        }

        private decimal? _unitsOnOrder;

        public decimal? UnitsOnOrder
        {
            get => _unitsOnOrder;
            set
            {
                if (_unitsOnOrder == value)
                    return;

                _unitsOnOrder = value;
                OnPropertyChanged(nameof(UnitsOnOrder));
            }
        }

        private decimal? _reorderLevel;

        public decimal? ReorderLevel
        {
            get => _reorderLevel;
            set
            {
                if (_reorderLevel == value)
                    return;

                _reorderLevel = value;
                OnPropertyChanged(nameof(ReorderLevel));
            }
        }

        private bool _discontinued;

        public bool Discontinued
        {
            get => _discontinued;
            set
            {
                if (_discontinued == value)
                    return;

                _discontinued = value;

                OnPropertyChanged(nameof(Discontinued));
            }
        }

        private string _orderComment;

        public string OrderComment
        {
            get => _orderComment;
            set
            {
                if (_orderComment == value)
                    return;

                _orderComment = value;
                OnPropertyChanged(nameof(OrderComment));
            }
        }

        private string _purchaseComment;

        public string PurchaseComment
        {
            get => _purchaseComment;
            set
            {
                if (_purchaseComment == value)
                    return;

                _purchaseComment = value;
                OnPropertyChanged(nameof(PurchaseComment));
            }
        }

        private AutoFillSetup _nonInventoryCodeAutoFillSetup;

        public AutoFillSetup NonInventoryCodeAutoFillSetup
        {
            get => _nonInventoryCodeAutoFillSetup;
            set
            {
                if (_nonInventoryCodeAutoFillSetup == value)
                    return;

                _nonInventoryCodeAutoFillSetup = value;
                OnPropertyChanged(nameof(NonInventoryCodeAutoFillSetup));
            }
        }

        private AutoFillValue _nonInventoryCodeAutoFillValue;

        public AutoFillValue NonInventoryCodeAutoFillValue
        {
            get => _nonInventoryCodeAutoFillValue;
            set
            {
                if (_nonInventoryCodeAutoFillValue == value)
                    return;

                _nonInventoryCodeAutoFillValue = value;
                OnPropertyChanged(nameof(NonInventoryCodeAutoFillValue));
            }
        }

        private byte _unitDecimals;

        public byte UnitDecimals
        {
            get => _unitDecimals;
            set
            {
                if (_unitDecimals == value)
                    return;

                _unitDecimals = value;
                OnPropertyChanged(nameof(UnitDecimals));
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
            ProductId = newEntity.ProductId;
        }

        protected override Products GetEntityData()
        {
            return null;
        }

        protected override void ClearData()
        {
            ProductId = 0;
            ProductName = QuantityPerUnit = OrderComment = PurchaseComment = string.Empty;
            SupplierAutoFillValue = CategoryAutoFillValue = null;
            UnitPrice = UnitsInStock = UnitsOnOrder = ReorderLevel = null;
            Discontinued = false;
            UnitDecimals = 2;
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
