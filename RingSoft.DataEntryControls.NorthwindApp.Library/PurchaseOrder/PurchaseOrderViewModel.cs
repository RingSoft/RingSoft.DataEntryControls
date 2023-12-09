using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.LookupModel;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using System;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public interface IPurchaseOrderView : IDbMaintenanceView
    {
        bool ShowCommentEditor(DataEntryGridMemoValue comment);

        void GridValidationFail();
    }

    public class PurchaseOrderViewModel : DbMaintenanceViewModel<Purchases>
    {
        public IPurchaseOrderView PurchaseOrderView { get; private set; }

        #region Properties

        private int _purchaseOrderId;

        public int PurchaseOrderId
        {
            get => _purchaseOrderId;
            set
            {
                if (_purchaseOrderId == value)
                    return;

                _purchaseOrderId = value;
                OnPropertyChanged(nameof(PurchaseOrderId));
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
                _supplierDirty = true;
                OnPropertyChanged(nameof(SupplierAutoFillValue));
            }
        }

        private DateTime _orderDate;

        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                if (_orderDate == value)
                    return;

                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }

        private DateTime? _requiredDate;

        public DateTime? RequiredDate
        {
            get => _requiredDate;
            set
            {
                if (_requiredDate == value)
                    return;

                _requiredDate = value;
                OnPropertyChanged(nameof(RequiredDate));
            }
        }

        private string _address;

        public string Address
        {
            get => _address;
            set
            {
                if (_address == value)
                    return;

                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private string _city;

        public string City
        {
            get => _city;
            set
            {
                if (_city == value)
                    return;

                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string _region;

        public string Region
        {
            get => _region;
            set
            {
                if (_region == value)
                    return;

                _region = value;
                OnPropertyChanged(nameof(Region));
            }
        }

        private string _postalCode;

        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode == value)
                    return;

                _postalCode = value;
                OnPropertyChanged(nameof(PostalCode));
            }
        }

        private string _country;

        public string Country
        {
            get => _country;
            set
            {
                if (_country == value)
                    return;

                _country = value;
                OnPropertyChanged(nameof(Country));
            }
        }

        private PurchaseOrderDetailsGridManager _detailsGridManager;

        public PurchaseOrderDetailsGridManager DetailsGridManager
        {
            get => _detailsGridManager;
            set
            {
                if (_detailsGridManager == value)
                    return;

                _detailsGridManager = value;
                OnPropertyChanged(nameof(DetailsGridManager), false);
            }
        }

        private double _subTotal;

        public double SubTotal
        {
            get => _subTotal;
            set
            {
                if (_subTotal == value)
                    return;

                _subTotal = value;
                OnPropertyChanged(nameof(SubTotal));
            }
        }

        private double _freight;

        public double Freight
        {
            get => _freight;
            set
            {
                if (_freight == value)
                    return;

                _freight = value;
                OnPropertyChanged(nameof(Freight));
            }
        }

        private double _total;

        public double Total
        {
            get => _total;
            set
            {
                if (_total == value)
                    return;

                _total = value;
                OnPropertyChanged(nameof(Total));
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

        #endregion

        public LookupDefinition<ProductLookup, Products> ProductsLookup { get; private set; }
        public UiCommand SupplierUiCommand { get; } = new UiCommand();

        private bool _supplierDirty;

        public PurchaseOrderViewModel()
        {
            SupplierUiCommand.LostFocus += (sender, args) =>
            {
                OnSupplierLostFocus();
            };
        }

        protected override void Initialize()
        {
            ProductsLookup = AppGlobals.LookupContext.ProductsLookup.Clone();

            PurchaseOrderView = View as IPurchaseOrderView ??
                             throw new ArgumentException(
                                 $"ViewModel requires an {nameof(IPurchaseOrderView)} interface.");

            SupplierAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.Purchases.GetFieldDefinition(p => p.SupplierId))
                {
                    AllowLookupAdd = false,
                    AllowLookupView = false
                };

            DetailsGridManager = new PurchaseOrderDetailsGridManager(this);
            RegisterGrid(DetailsGridManager);

            base.Initialize();
        }

        private void OnSupplierLostFocus()
        {
            if (_supplierDirty)
            {
                if (SupplierAutoFillValue?.PrimaryKeyValue != null &&
                    SupplierAutoFillValue.PrimaryKeyValue.IsValid())
                {
                    var supplier =
                        AppGlobals.LookupContext.Suppliers.GetEntityFromPrimaryKeyValue(SupplierAutoFillValue
                            .PrimaryKeyValue);

                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
                    supplier = AppGlobals.DbContextProcessor.GetSupplier(supplier.SupplierId);
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

                    if (supplier != null)
                    {
                        Address = supplier.Address;
                        City = supplier.City;
                        Region = supplier.Region;
                        PostalCode = supplier.PostalCode;
                        Country = supplier.Country;
                        UpdateProductLookup(supplier);
                    }
                }
                CheckSupplier();
                _supplierDirty = false;
            }
        }

        protected override void PopulatePrimaryKeyControls(Purchases newEntity, PrimaryKeyValue primaryKeyValue)
        {
            PurchaseOrderId = newEntity.PurchaseOrderId;
        }

        protected override void LoadFromEntity(Purchases entity)
        {
            _supplierDirty = false;
            SupplierAutoFillValue = entity.Supplier.GetAutoFillValue();
            OrderDate = entity.OrderDate;
            RequiredDate = entity.RequiredDate;
            Address = entity.Address;
            City = entity.City;
            Region = entity.Region;
            PostalCode = entity.PostalCode;
            Country = entity.Country;
            Freight = entity.Freight;
            Notes = entity.Notes;

            UpdateProductLookup(entity.Supplier);
            RefreshTotalControls();
        }

        protected override Purchases GetEntityData()
        {
            var purchase = new Purchases
            {
                PurchaseOrderId = PurchaseOrderId, 
                PoNumber = KeyAutoFillValue.Text,
                SupplierId = SupplierAutoFillValue.GetEntity<Suppliers>().SupplierId,
                OrderDate = OrderDate,
                RequiredDate = RequiredDate,
                Address = Address,
                City = City,
                Country = Country,
                Freight = Freight,
                PostalCode = PostalCode,
                Region = Region,
                Notes = Notes
            };

            return purchase;
        }

        protected override void ClearData()
        {
            PurchaseOrderId = 0;
            Address = City = Region = PostalCode = Country = string.Empty;
            SupplierAutoFillValue = null;
            OrderDate = DateTime.Today;
            RequiredDate = null;
            SubTotal = Total = 0;
            Freight = 0;
            Notes = string.Empty;

            _supplierDirty = false;
            SupplierUiCommand.IsEnabled = true;

            RefreshTotalControls();
        }

        public bool ValidSupplier()
        {
            return SupplierAutoFillValue != null && SupplierAutoFillValue.PrimaryKeyValue.IsValid();
        }

        public void CheckSupplier()
        {
            DetailsGridManager.Grid.RefreshDataSource();
        }

        public void UpdateSupplierEnabled()
        {
            SupplierUiCommand.IsEnabled = !DetailsGridManager.ValidProductInGrid();
        }

        private void UpdateProductLookup(Suppliers supplier)
        {
            ProductsLookup.FilterDefinition.ClearFixedFilters();
            ProductsLookup.FilterDefinition.AddFixedFilter(p => p.SupplierId, Conditions.Equals, supplier.SupplierId);
        }

        public void RefreshTotalControls()
        {
            double subTotal = 0;
            foreach (var gridRow in DetailsGridManager.Rows)
            {
                if (gridRow is PurchaseOrderDetailsProductRow productRow)
                    subTotal += productRow.ExtendedPrice;
                else if (gridRow is PurchaseOrderDetailsDirectExpenseRow directExpenseRow)
                    subTotal += directExpenseRow.Price;
            }

            SubTotal = subTotal;
            Total = subTotal + Freight;
        }
    }
}
