using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup.AutoFill;
using System;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public abstract class SalesEntryDetailsValueRow : SalesEntryDetailsRow
    
    {
        public decimal Quantity { get; protected set; }

        public decimal Price { get; protected set; }

        public decimal ExtendedPrice => Math.Round(Quantity * Price, 2);

        private DecimalEditControlSetup _quantitySetup;
        private DecimalEditControlSetup _priceSetup;
        private DecimalEditControlSetup _extendedPriceSetup;

        protected SalesEntryDetailsValueRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            _quantitySetup = AppGlobals.CreateNewDecimalEditControlSetup();
            _priceSetup = AppGlobals.CreateNewDecimalEditControlSetup();
            _extendedPriceSetup = AppGlobals.CreateNewDecimalEditControlSetup();

            _extendedPriceSetup.FormatType = _priceSetup.FormatType = DecimalEditFormatTypes.Currency;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Quantity:
                    return new DataEntryGridDecimalCellProps(this, columnId, _quantitySetup, Quantity);
                case SalesEntryGridColumns.Price:
                    return new DataEntryGridDecimalCellProps(this, columnId, _priceSetup, Price);
                case SalesEntryGridColumns.ExtendedPrice:
                    return new DataEntryGridDecimalCellProps(this, columnId, _extendedPriceSetup, ExtendedPrice);
            }

            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (SalesEntryGridColumns)columnId;
            switch (column)
            {
                case SalesEntryGridColumns.ExtendedPrice:
                    return new DataEntryGridCellStyle {CellStyle = DataEntryGridCellStyles.Disabled};
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (SalesEntryGridColumns)value.ColumnId;
            switch (column)
            {
                case SalesEntryGridColumns.Quantity:
                    if (value is DataEntryGridDecimalCellProps quantityDecimalCellProps)
                    {
                        if (quantityDecimalCellProps.Value != null)
                        {
                            Quantity = (decimal)quantityDecimalCellProps.Value;
                            SalesEntryDetailsManager.SalesEntryViewModel.RefreshTotalControls();
                        }
                    }
                    break;
                case SalesEntryGridColumns.Price:
                    if (value is DataEntryGridDecimalCellProps priceDecimalCellProps)
                    {
                        if (priceDecimalCellProps.Value != null)
                        {
                            Price = (decimal)priceDecimalCellProps.Value;
                            SalesEntryDetailsManager.SalesEntryViewModel.RefreshTotalControls();
                        }
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(OrderDetails entity)
        {
            if (entity.Quantity != null)
                Quantity = (decimal)entity.Quantity;

            if (entity.UnitPrice != null)
                Price = (decimal)entity.UnitPrice;
        }

        public override void SaveToEntity(OrderDetails entity, int rowIndex)
        {
            entity.Quantity = Quantity;
            entity.UnitPrice = Price;
            base.SaveToEntity(entity, rowIndex);
        }

        protected void CorrectInvalidItem(DataEntryGridAutoFillCellProps autoFillCellProps)
        {
            if (CheckConvertItem(autoFillCellProps)) 
                return;

            var correctedValue =
                SalesEntryDetailsManager.SalesEntryViewModel.SalesEntryView.CorrectInvalidProduct(
                    autoFillCellProps.AutoFillValue);

            switch (correctedValue.ReturnCode)
            {
                case InvalidProductResultReturnCodes.Cancel:
                    autoFillCellProps.ValidationResult = false;
                    break;
                case InvalidProductResultReturnCodes.NewProduct:
                    CorrectInvalidProduct(correctedValue);
                    break;
                case InvalidProductResultReturnCodes.NewNonInventory:
                    CorrectInvalidNiCode(correctedValue);
                    break;
                case InvalidProductResultReturnCodes.NewSpecialOrder:
                    var soRow = new SalesEntryDetailsSpecialOrderRow(SalesEntryDetailsManager);
                    SalesEntryDetailsManager.ReplaceRow(this, soRow);
                    soRow.SpecialOrderText = autoFillCellProps.Text;
                    soRow.Quantity = 1;
                    Manager.Grid.UpdateRow(soRow);
                    autoFillCellProps.NextTabFocusColumnId = (int) SalesEntryGridColumns.Price;
                    autoFillCellProps.NextTabFocusRow = soRow;
                    break;
                case InvalidProductResultReturnCodes.NewComment:
                    var commentRow = new SalesEntryDetailsCommentRow(SalesEntryDetailsManager);
                    SalesEntryDetailsManager.ReplaceRow(this, commentRow);
                    commentRow.SetValue(correctedValue.Comment);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CheckConvertItem(DataEntryGridAutoFillCellProps autoFillCellProps)
        {
            if (this is SalesEntryDetailsProductRow)
            {
                if (CheckInvalidProductIsValidNiCode(autoFillCellProps))
                    return true;
            }
            else if (this is SalesEntryDetailsNonInventoryRow)
            {
                if (CheckInvalidNiCodeIsValidProduct(autoFillCellProps))
                    return true;
            }

            return false;
        }

        private bool CheckInvalidNiCodeIsValidProduct(DataEntryGridAutoFillCellProps autoFillCellProps)
        {
            var product = AppGlobals.DbContextProcessor.GetProduct(autoFillCellProps.Text);
            if (product != null)
            {
                var productRow = new SalesEntryDetailsProductRow(SalesEntryDetailsManager);
                SalesEntryDetailsManager.ReplaceRow(this, productRow);
                var productAutoFillValue =
                    new AutoFillValue(AppGlobals.LookupContext.Products.GetPrimaryKeyValueFromEntity(product),
                        autoFillCellProps.AutoFillValue.Text);
                productRow.LoadFromItemAutoFillValue(productAutoFillValue);
                Manager.Grid.UpdateRow(productRow);
                return true;
            }

            return false;
        }

        private bool CheckInvalidProductIsValidNiCode(DataEntryGridAutoFillCellProps autoFillCellProps)
        {
            var niCode = AppGlobals.DbContextProcessor.GetNonInventoryCode(autoFillCellProps.Text);
            if (niCode != null)
            {
                var niCodeRow = new SalesEntryDetailsNonInventoryRow(SalesEntryDetailsManager);
                SalesEntryDetailsManager.ReplaceRow(this, niCodeRow);
                var niAutoFillValue =
                    new AutoFillValue(
                        AppGlobals.LookupContext.NonInventoryCodes.GetPrimaryKeyValueFromEntity(niCode),
                        autoFillCellProps.Text);
                niCodeRow.LoadFromNiCodeAutoFillValue(niAutoFillValue);
                Manager.Grid.UpdateRow(niCodeRow);
                return true;
            }

            return false;
        }

        private void CorrectInvalidNiCode(InvalidProductResult correctedValue)
        {
            SalesEntryDetailsNonInventoryRow nIRow;
            if (LineType == SalesEntryDetailsLineTypes.NonInventoryCode &&
                this is SalesEntryDetailsNonInventoryRow)
            {
                nIRow = (SalesEntryDetailsNonInventoryRow) this;
            }
            else
            {
                nIRow = new SalesEntryDetailsNonInventoryRow(SalesEntryDetailsManager);
                SalesEntryDetailsManager.ReplaceRow(this, nIRow);
            }

            nIRow.LoadFromNiCodeAutoFillValue(correctedValue.NewItemValue);
            Manager.Grid.UpdateRow(nIRow);
        }

        private void CorrectInvalidProduct(InvalidProductResult correctedValue)
        {
            SalesEntryDetailsProductRow productRow;
            if (LineType == SalesEntryDetailsLineTypes.Product && this is SalesEntryDetailsProductRow)
            {
                productRow = (SalesEntryDetailsProductRow) this;
            }
            else
            {
                productRow = new SalesEntryDetailsProductRow(SalesEntryDetailsManager);
                SalesEntryDetailsManager.ReplaceRow(this, productRow);
            }

            productRow.LoadFromItemAutoFillValue(correctedValue.NewItemValue);
            Manager.Grid.UpdateRow(productRow);
        }
    }
}
