using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public abstract class SalesEntryDetailsValueRow : SalesEntryDetailsRow
    
    {
        public decimal Quantity { get; private set; }

        public decimal Price { get; private set; }

        public decimal ExtendedPrice
        {
            get
            {
                if (!Quantity.Equals(0))
                {
                    return Math.Round(Quantity * Price, 2);
                }

                return Price;
            }
        }

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

        public override void LoadFromOrderDetail(OrderDetails orderDetail)
        {
            if (orderDetail.Quantity != null)
                Quantity = (decimal) orderDetail.Quantity;

            if (orderDetail.UnitPrice != null)
                Price = (decimal) orderDetail.UnitPrice;
        }
    }
}
