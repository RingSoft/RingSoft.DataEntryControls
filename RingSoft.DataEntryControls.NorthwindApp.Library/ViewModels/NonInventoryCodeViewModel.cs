using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels
{
    public class NonInventoryCodeViewModel : DbMaintenanceViewModel<NonInventoryCodes>
    {
        #region Properties

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

        private double _price;
        public double Price
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

        #endregion

        protected override void PopulatePrimaryKeyControls(NonInventoryCodes newEntity, PrimaryKeyValue primaryKeyValue)
        {
            NonInventoryCodeId = newEntity.NonInventoryCodeId;
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
    }
}
