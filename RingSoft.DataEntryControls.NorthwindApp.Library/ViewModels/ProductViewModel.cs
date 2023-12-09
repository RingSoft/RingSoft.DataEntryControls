using System.ComponentModel;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.NorthwindApp.Library.LookupModel;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
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

        private bool _enableSupplier;

        public bool EnableSupplier
        {
            get => _enableSupplier;
            set
            {
                if (_enableSupplier == value)
                    return;

                _enableSupplier = value;
                OnPropertyChanged(nameof(EnableSupplier));
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

        private double? _unitPrice;

        public double? UnitPrice
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

        private double? _unitsInStock;

        public double? UnitsInStock
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

        private double? _unitsOnOrder;

        public double? UnitsOnOrder
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

        private double? _reorderLevel;

        public double? ReorderLevel
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

        private LookupDefinition<OrderDetailLookup, OrderDetails> _orderDetailsLookup;

        public LookupDefinition<OrderDetailLookup, OrderDetails> OrderDetailsLookupDefinition
        {
            get => _orderDetailsLookup;
            set
            {
                if (_orderDetailsLookup == value)
                    return;

                _orderDetailsLookup = value;
                OnPropertyChanged(nameof(OrderDetailsLookupDefinition), false);
            }
        }

        private LookupCommand _orderDetailsLookupCommand;

        public LookupCommand OrderDetailsLookupCommand
        {
            get => _orderDetailsLookupCommand;
            set
            {
                if (_orderDetailsLookupCommand == value)
                    return;

                _orderDetailsLookupCommand = value;
                OnPropertyChanged(nameof(OrderDetailsLookupCommand), false);
            }
        }

        private string _notes;

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        private bool _lockSupplier;
        private NorthwindViewModelInput _viewModelInput;
        
        public ProductViewModel()
        {
            EnableSupplier = true;
        }

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is NorthwindViewModelInput viewModelInput)
            {
                _viewModelInput = viewModelInput;
            }
            else
            {
                _viewModelInput = new NorthwindViewModelInput();
            }
            _viewModelInput.ProductViewModels.Add(this);

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
            NonInventoryCodeAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.NonInventoryCodeId));

            if (_viewModelInput.ProductInput != null)
            {
                _lockSupplier = true;
                SupplierAutoFillValue = _viewModelInput.ProductInput.LockSupplier;
                EnableSupplier = false;
                _viewModelInput.ProductInput = null;
            }

            var orderDetailsLookupDefinition =
                new LookupDefinition<OrderDetailLookup, OrderDetails>(AppGlobals.LookupContext.OrderDetails);
            orderDetailsLookupDefinition.Include(p => p.Order)
                .AddVisibleColumnDefinition(p => p.OrderDate, p => p.OrderDate);
            orderDetailsLookupDefinition.Include(p => p.Order)
                .Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer, p => p.CompanyName);
            orderDetailsLookupDefinition.AddVisibleColumnDefinition(p => p.Quantity, p => p.Quantity);
            orderDetailsLookupDefinition.AddVisibleColumnDefinition(p => p.UnitPrice, p => p.UnitPrice);
            OrderDetailsLookupDefinition = orderDetailsLookupDefinition;
            RegisterLookup(OrderDetailsLookupDefinition);

            base.Initialize();
        }

        protected override void PopulatePrimaryKeyControls(Products newEntity, PrimaryKeyValue primaryKeyValue)
        {
            ProductId = newEntity.ProductId;

            ReadOnlyMode = _viewModelInput.ProductViewModels.Any(a => a != this && a.ProductId == ProductId);
        }

        protected override Products GetEntityFromDb(Products newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var product = AppGlobals.DbContextProcessor.GetProduct(newEntity.ProductId);

            if (product.PurchaseDetails.Any())
            {
                EnableSupplier = false;
            }
            else
            {
                EnableSupplier = !ReadOnlyMode;
            }

            return product;
        }

        protected override void LoadFromEntity(Products entity)
        {
            PrimaryKeyValue primaryKey;
            if (entity.Supplier != null)
            {
                primaryKey = AppGlobals.LookupContext.Suppliers.GetPrimaryKeyValueFromEntity(entity.Supplier);
                SupplierAutoFillValue = new AutoFillValue(primaryKey, entity.Supplier.CompanyName);
            }
            else
            {
                SupplierAutoFillValue = null;
            }

            if (entity.Category != null)
            {
                primaryKey = AppGlobals.LookupContext.Categories.GetPrimaryKeyValueFromEntity(entity.Category);
                CategoryAutoFillValue = new AutoFillValue(primaryKey, entity.Category.CategoryName);
            }
            else
            {
                CategoryAutoFillValue = null;
            }

            if (entity.NonInventoryCode != null)
            {
                primaryKey =
                    AppGlobals.LookupContext.NonInventoryCodes.GetPrimaryKeyValueFromEntity(entity.NonInventoryCode);
                NonInventoryCodeAutoFillValue = new AutoFillValue(primaryKey, entity.NonInventoryCode.Description);
            }
            else
            {
                NonInventoryCodeAutoFillValue = null;
            }

            QuantityPerUnit = entity.QuantityPerUnit;
            UnitPrice = entity.UnitPrice;
            UnitsInStock = entity.UnitsInStock;
            UnitsOnOrder = entity.UnitsOnOrder;
            ReorderLevel = entity.ReorderLevel;
            Discontinued = entity.Discontinued;
            OrderComment = entity.OrderComment;
            PurchaseComment = entity.PurchaseComment;
            UnitDecimals = entity.UnitDecimals;
            Notes = entity.Notes;

            if (ReadOnlyMode)
            {
                ControlsGlobals.UserInterface.ShowMessageBox("This Product is being modified in another window.", "Editing not allowed", RsMessageBoxIcons.Exclamation);
            }
        }

        protected override Products GetEntityData()
        {
            var product = new Products
            {
                ProductId = ProductId,
                ProductName = KeyAutoFillValue.Text,
                QuantityPerUnit = QuantityPerUnit,
                UnitPrice = UnitPrice,
                UnitsInStock = UnitsInStock,
                UnitsOnOrder = UnitsOnOrder,
                ReorderLevel = ReorderLevel,
                Discontinued = Discontinued,
                OrderComment = OrderComment,
                PurchaseComment = PurchaseComment,
                UnitDecimals = UnitDecimals,
                Notes = Notes
            };

            if (CategoryAutoFillValue != null)
            {
                product.CategoryId = AppGlobals.LookupContext.Categories
                    .GetEntityFromPrimaryKeyValue(CategoryAutoFillValue.PrimaryKeyValue).CategoryId;
            }

            if (SupplierAutoFillValue != null)
            {
                product.SupplierId = AppGlobals.LookupContext.Suppliers
                    .GetEntityFromPrimaryKeyValue(SupplierAutoFillValue.PrimaryKeyValue).SupplierId;
            }

            if (NonInventoryCodeAutoFillValue != null)
            {
                product.NonInventoryCodeId = AppGlobals.LookupContext.NonInventoryCodes
                    .GetEntityFromPrimaryKeyValue(NonInventoryCodeAutoFillValue.PrimaryKeyValue).NonInventoryCodeId;
            }
            return product;
        }

        protected override void ClearData()
        {
            ProductId = 0;
            QuantityPerUnit = OrderComment = PurchaseComment = Notes = string.Empty;
            if (!_lockSupplier)
                SupplierAutoFillValue = null;

            CategoryAutoFillValue = NonInventoryCodeAutoFillValue = null;
            UnitPrice = UnitsInStock = UnitsOnOrder = ReorderLevel = null;
            Discontinued = false;
            UnitDecimals = 2;
            NonInventoryCodeAutoFillValue = null;

            OrderDetailsLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.CategoryId))
                return CategoryAutoFillValue;

            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.SupplierId))
                return SupplierAutoFillValue;

            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.NonInventoryCodeId))
                return NonInventoryCodeAutoFillValue;

            return base.GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        protected override bool SaveEntity(Products entity)
        {
            return AppGlobals.DbContextProcessor.SaveProduct(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DbContextProcessor.DeleteProduct(ProductId);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                _viewModelInput.ProductViewModels.Remove(this);
        }
    }
}
