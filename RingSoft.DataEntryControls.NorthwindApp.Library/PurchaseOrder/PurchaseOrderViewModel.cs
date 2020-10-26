using System;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
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
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string Address { get; set; }
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
