using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DbLookup.AutoFill;
using System.Drawing;

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
            BackgroundColor = Color.Blue;
            ForegroundColor = Color.White;
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
                    return new DataEntryGridCellStyle{ColumnHeader = "Non Inventory Code"};
                case SalesEntryGridColumns.Quantity:
                case SalesEntryGridColumns.ExtendedPrice:
                case SalesEntryGridColumns.Discount:
                    return new DataEntryGridCellStyle{CellStyle = DataEntryGridCellStyles.ReadOnly};
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (SalesEntryGridColumns) value.ColumnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        var validNiCode = autoFillCellProps.AutoFillValue.PrimaryKeyValue.ContainsValidData();
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
            if (NonInventoryValue.PrimaryKeyValue.ContainsValidData())
            {
                var niCode =
                    AppGlobals.LookupContext.NonInventoryCodes.GetEntityFromPrimaryKeyValue(NonInventoryValue
                        .PrimaryKeyValue);
                niCode = AppGlobals.DbContextProcessor.GetNonInventoryCode(niCode.NonInventoryCodeId);
                Price = niCode.Price;
                SalesEntryDetailsManager.SalesEntryViewModel.RefreshTotalControls();
            }
        }

        public override bool ValidateRow()
        {
            return true;
        }
    }
}
