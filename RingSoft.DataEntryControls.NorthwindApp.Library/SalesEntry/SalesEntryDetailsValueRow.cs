using System;
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

        protected SalesEntryDetailsValueRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Quantity:
                    return new DataEntryGridTextCellProps(this, columnId){Text = Quantity.ToString("N")};
                case SalesEntryGridColumns.Price:
                    return new DataEntryGridTextCellProps(this, columnId) { Text = Price.ToString("C") };
                case SalesEntryGridColumns.ExtendedPrice:
                    return new DataEntryGridTextCellProps(this, columnId) { Text = ExtendedPrice.ToString("C") };
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
