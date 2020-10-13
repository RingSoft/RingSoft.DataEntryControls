using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

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
                            SalesEntryDetailsManager.ViewModel.RefreshTotalControls();
                        }
                    }
                    break;
                case SalesEntryGridColumns.Price:
                    if (value is DataEntryGridDecimalCellProps priceDecimalCellProps)
                    {
                        if (priceDecimalCellProps.Value != null)
                        {
                            Price = (decimal)priceDecimalCellProps.Value;
                            SalesEntryDetailsManager.ViewModel.RefreshTotalControls();
                        }
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromOrderDetail(OrderDetails orderDetail)
        {
            if (orderDetail.Quantity != null)
                Quantity = (decimal) orderDetail.Quantity;

            if (orderDetail.UnitPrice != null)
                Price = (decimal) orderDetail.UnitPrice;

            base.LoadFromOrderDetail(orderDetail);
        }

        public override void SaveToOrderDetail(OrderDetails orderDetail)
        {
            orderDetail.Quantity = Quantity;
            orderDetail.UnitPrice = Price;
        }

        protected void CorrectInvalidItem(DataEntryGridAutoFillCellProps autoFillCellProps)
        {
            var correctedValue =
                SalesEntryDetailsManager.ViewModel.SalesEntryView.CorrectInvalidProduct(
                    autoFillCellProps.AutoFillValue);

            switch (correctedValue.ReturnCode)
            {
                case InvalidProductResultReturnCodes.Cancel:
                    autoFillCellProps.ValidationResult = false;
                    break;
                case InvalidProductResultReturnCodes.NewProduct:
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
                    break;
                case InvalidProductResultReturnCodes.NewNonInventory:
                    break;
                case InvalidProductResultReturnCodes.NewSpecialOrder:
                    break;
                case InvalidProductResultReturnCodes.NewComment:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
