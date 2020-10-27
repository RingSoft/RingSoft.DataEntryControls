using System;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public class PurchaseOrderViewModel : DbMaintenanceViewModel<Purchases>
    {
        public override TableDefinition<Purchases> TableDefinition => AppGlobals.LookupContext.Purchases;

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

        private string _poNumber;

        public string PoNumber
        {
            get => _poNumber;
            set
            {
                if (_poNumber == value)
                    return;

                _poNumber = value;
                OnPropertyChanged(nameof(PoNumber));
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
                OnPropertyChanged(nameof(_supplierAutoFillSetup));
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

        private decimal _subTotal;

        public decimal SubTotal
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

        private decimal _freight;

        public decimal Freight
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

        private decimal _total;

        public decimal Total
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

        protected override void Initialize()
        {
            SupplierAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.Purchases.GetFieldDefinition(p => p.SupplierId));

            base.Initialize();
        }

        protected override Purchases PopulatePrimaryKeyControls(Purchases newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var purchase = AppGlobals.DbContextProcessor.GetPurchase(newEntity.PurchaseOrderId);
            PurchaseOrderId = purchase.PurchaseOrderId;
            return purchase;
        }

        protected override void LoadFromEntity(Purchases entity)
        {
            throw new NotImplementedException();
        }

        protected override Purchases GetEntityData()
        {
            throw new NotImplementedException();
        }

        protected override void ClearData()
        {
            PurchaseOrderId = 0;
            PoNumber = Address = City = Region = PostalCode = Country = string.Empty;
            SupplierAutoFillValue = null;
            OrderDate = DateTime.Today;
            RequiredDate = null;
            SubTotal = Freight = Total = 0;
        }

        protected override bool SaveEntity(Purchases entity)
        {
            throw new NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new NotImplementedException();
        }
    }
}
