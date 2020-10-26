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

        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public decimal Freight { get; set; }

        protected override Purchases PopulatePrimaryKeyControls(Purchases newEntity, PrimaryKeyValue primaryKeyValue)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
