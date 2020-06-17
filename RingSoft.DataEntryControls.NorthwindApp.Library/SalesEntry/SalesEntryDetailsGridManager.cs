using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public enum SalesEntryGridColumns
    {
        LineType = SalesEntryDetailsGridManager.LineTypeColumnId,
        Item = SalesEntryDetailsGridManager.ItemColumnId,
        Quantity = SalesEntryDetailsGridManager.QuantityColumnId,
        Price = SalesEntryDetailsGridManager.PriceColumnId,
        ExtendedPrice= SalesEntryDetailsGridManager.ExtendedPriceColumnId,
        Discount = SalesEntryDetailsGridManager.DiscountColumnId
    }

    public class SalesEntryDetailsGridManager : DataEntryGridManager
    {
        public SalesEntryViewModel ViewModel { get; }

        public const int LineTypeColumnId = 0;
        public const int ItemColumnId = 1;
        public const int QuantityColumnId = 2;
        public const int PriceColumnId = 3;
        public const int ExtendedPriceColumnId = 4;
        public const int DiscountColumnId = 5;

        public SalesEntryDetailsGridManager(SalesEntryViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new SalesEntryDetailsProductRow(this);
        }

        public void LoadFromEntity(Orders order)
        {
            PreLoadGridFromEntity();

            foreach (var orderDetail in order.OrderDetails)
            {
                SalesEntryDetailsRow newRow = null;
                var lineType = (SalesEntryDetailsLineTypes) orderDetail.LineType;
                switch (lineType)
                {
                    case SalesEntryDetailsLineTypes.Product:
                        newRow = new SalesEntryDetailsProductRow(this);
                        break;
                    case SalesEntryDetailsLineTypes.NonInventoryCode:
                        break;
                    case SalesEntryDetailsLineTypes.SpecialOrder:
                        break;
                    case SalesEntryDetailsLineTypes.Comment:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (newRow != null)
                {
                    newRow.LoadFromOrderDetail(orderDetail);
                    AddRow(newRow);
                }
            }

            PostLoadGridFromEntity();
        }
    }
}
