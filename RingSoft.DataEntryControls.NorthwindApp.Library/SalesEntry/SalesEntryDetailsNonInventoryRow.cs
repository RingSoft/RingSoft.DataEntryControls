using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsNonInventoryRow : SalesEntryDetailsValueRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.NonInventoryCode;

        public AutoFillValue NonInventoryValue { get; private set; }

        private AutoFillSetup _nonInventoryAutoFillSetup;

        public SalesEntryDetailsNonInventoryRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            _nonInventoryAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.NonInventoryCodeId));
            Quantity = 1;

            DisplayStyleId = AppGlobals.NonInventoryDisplayStyleId;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    return new DataEntryGridAutoFillCellProps(this, columnId, _nonInventoryAutoFillSetup,
                        NonInventoryValue);
                case SalesEntryGridColumns.Quantity:
                    return new DataEntryGridTextCellProps(this, columnId);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    return new DataEntryGridCellStyle
                    {
                        ColumnHeader = "Non Inventory Code"
                    };
                case SalesEntryGridColumns.Price:
                    if (Price < 0)
                        return new DataEntryGridCellStyle { DisplayStyleId = AppGlobals.NegativeDisplayStyleId };

                    return new DataEntryGridCellStyle{DisplayStyleId = AppGlobals.NonInventoryDisplayStyleId};
                case SalesEntryGridColumns.Quantity:
                case SalesEntryGridColumns.ExtendedPrice:
                case SalesEntryGridColumns.Discount:
                    return new DataEntryGridCellStyle{State = DataEntryGridCellStates.ReadOnly};
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (SalesEntryGridColumns) value.ColumnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        var validNiCode = autoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid();
                        if (validNiCode)
                        {
                            LoadFromNiCodeAutoFillValue(autoFillCellProps.AutoFillValue);
                        }
                        else if (string.IsNullOrEmpty(autoFillCellProps.AutoFillValue.Text))
                        {
                            NonInventoryValue = autoFillCellProps.AutoFillValue;
                        }
                        else
                        {
                            CorrectInvalidItem(autoFillCellProps);
                        }
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public void LoadFromNiCodeAutoFillValue(AutoFillValue value)
        {
            NonInventoryValue = value;
            if (NonInventoryValue.PrimaryKeyValue.IsValid())
            {
                var niCode =
                    AppGlobals.LookupContext.NonInventoryCodes.GetEntityFromPrimaryKeyValue(NonInventoryValue
                        .PrimaryKeyValue);
                niCode = AppGlobals.DbContextProcessor.GetNonInventoryCode(niCode.NonInventoryCodeId);
                Price = niCode.Price;
                SalesEntryDetailsManager.SalesEntryViewModel.RefreshTotalControls();
            }
        }

        public void LoadFromNiCode(NonInventoryCodes niCode)
        {
            NonInventoryValue = new AutoFillValue(AppGlobals.LookupContext.NonInventoryCodes.GetPrimaryKeyValueFromEntity(niCode), niCode.Description);
            Price = niCode.Price;
        }

        public override bool ValidateRow()
        {
            if (NonInventoryValue == null || !NonInventoryValue.PrimaryKeyValue.IsValid())
            {
                SalesEntryDetailsManager.GotoCell(this, (int)SalesEntryGridColumns.Item);

                var message = "Non Inventory Code must contain a valid value.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Validation Failure!",
                    RsMessageBoxIcons.Exclamation);

                return false;
            }
            return true;
        }

        public override void SaveToEntity(OrderDetails entity, int rowIndex)
        {
            entity.NonInventoryCodeId = AppGlobals.LookupContext.NonInventoryCodes
                .GetEntityFromPrimaryKeyValue(NonInventoryValue.PrimaryKeyValue).NonInventoryCodeId;
            base.SaveToEntity(entity, rowIndex);
            entity.Quantity = null;
        }

        public override void LoadFromEntity(OrderDetails entity)
        {
            LoadFromNiCode(entity.NonInventoryCode);
            base.LoadFromEntity(entity);
        }
    }
}
