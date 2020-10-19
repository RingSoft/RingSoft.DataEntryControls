using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels
{
    public class NonInventoryCodeViewModel : DbMaintenanceViewModel<NonInventoryCodes>
    {
        public override TableDefinition<NonInventoryCodes> TableDefinition =>
            AppGlobals.LookupContext.NonInventoryCodes;

        private int _nonInventoryCodeId;

        public int NonInventoryCodeId
        {
            get => _nonInventoryCodeId;
            set
            {
                if (_nonInventoryCodeId == value)
                    return;

                _nonInventoryCodeId = value;
                OnPropertyChanged(nameof(NonInventoryCodeId));
            }
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price == value)
                    return;

                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        
        protected override NonInventoryCodes PopulatePrimaryKeyControls(NonInventoryCodes newEntity, PrimaryKeyValue primaryKeyValue)
        {
            NonInventoryCodeId = newEntity.NonInventoryCodeId;

            var nonInventoryCode = AppGlobals.DbContextProcessor.GetNonInventoryCode(newEntity.NonInventoryCodeId);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, nonInventoryCode.Description);
            return nonInventoryCode;
        }

        protected override void LoadFromEntity(NonInventoryCodes entity)
        {
            Price = entity.Price;
        }

        protected override NonInventoryCodes GetEntityData()
        {
            var nonInventoryCode = new NonInventoryCodes
            {
                NonInventoryCodeId = NonInventoryCodeId,
                Description = KeyAutoFillValue.Text,
                Price = Price
            };
            return nonInventoryCode;
        }

        protected override void ClearData()
        {
            NonInventoryCodeId = 0;
            Price = 0;
        }

        protected override bool SaveEntity(NonInventoryCodes entity)
        {
            return AppGlobals.DbContextProcessor.SaveNonInventoryCode(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DbContextProcessor.DeleteNonInventoryCode(NonInventoryCodeId);
        }
    }
}
