using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public enum PurchaseOrderColumns
    {
        LineType = PurchaseOrderDetailsGridManager.LineTypeColumnId,
        Item = PurchaseOrderDetailsGridManager.ItemColumnId,
        Quantity = PurchaseOrderDetailsGridManager.QuantityColumnId,
        Price = PurchaseOrderDetailsGridManager.PriceColumnId,
        ExtendedPrice = PurchaseOrderDetailsGridManager.ExtendedPriceColumnId,
        PickDate = PurchaseOrderDetailsGridManager.PickDateColumnId,
        Received = PurchaseOrderDetailsGridManager.ReceivedColumnId,
        DelayDays = PurchaseOrderDetailsGridManager.DelayDaysId

    }

    public class PurchaseOrderDetailsGridManager : DbMaintenanceDataEntryGridManager<PurchaseDetails>
    {
        public const int LineTypeColumnId = 0;
        public const int ItemColumnId = 1;
        public const int QuantityColumnId = 2;
        public const int PriceColumnId = 3;
        public const int ExtendedPriceColumnId = 4;
        public const int PickDateColumnId = 5;
        public const int ReceivedColumnId = 6;
        public const int DelayDaysId = 7;

        public PurchaseOrderViewModel PurchaseOrderViewModel { get; }
        public PurchaseOrderDetailsGridManager(PurchaseOrderViewModel viewModel) : base(viewModel)
        {
            PurchaseOrderViewModel = viewModel;
        }

        public PurchaseOrderDetailsRow CreateRowFromLineType(PurchaseOrderDetailsLineTypes lineType)
        {
            switch (lineType)
            {
                case PurchaseOrderDetailsLineTypes.Product:
                    return new PurchaseOrderDetailsProductRow(this);
                //case PurchaseOrderDetailsLineTypes.DirectExpense:
                //case PurchaseOrderDetailsLineTypes.Comment:
                default:
                    throw new ArgumentOutOfRangeException(nameof(lineType), lineType, null);
            }
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return CreateRowFromLineType(PurchaseOrderDetailsLineTypes.Product);
        }

        protected override DbMaintenanceDataEntryGridRow<PurchaseDetails> ConstructNewRowFromEntity(PurchaseDetails entity)
        {
            return CreateRowFromLineType((PurchaseOrderDetailsLineTypes) entity.LineType);
        }
    }
}
