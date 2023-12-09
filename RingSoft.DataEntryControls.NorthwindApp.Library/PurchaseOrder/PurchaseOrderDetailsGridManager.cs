using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbMaintenance;

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

        public const int DirectExpenseDisplayId = 200;

        public PurchaseOrderViewModel PurchaseOrderViewModel { get; }
        public PurchaseOrderDetailsGridManager(PurchaseOrderViewModel viewModel) : base(viewModel)
        {
            PurchaseOrderViewModel = viewModel;

            RowsChanged += PurchaseOrderDetailsGridManager_RowsChanged;
        }

        private void PurchaseOrderDetailsGridManager_RowsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    PurchaseOrderViewModel.UpdateSupplierEnabled();
                    break;
            }
        }

        public PurchaseOrderDetailsRow CreateRowFromLineType(PurchaseOrderDetailsLineTypes lineType)
        {
            switch (lineType)
            {
                case PurchaseOrderDetailsLineTypes.Product:
                    return new PurchaseOrderDetailsProductRow(this);
                case PurchaseOrderDetailsLineTypes.DirectExpense:
                    return new PurchaseOrderDetailsDirectExpenseRow(this);
                case PurchaseOrderDetailsLineTypes.Comment:
                    return new PurchaseOrderDetailsCommentRow(this);
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

        protected override string GetParentRowIdFromEntity(PurchaseDetails entity)
        {
            return entity.ParentRowId;
        }

        public bool ValidProductInGrid()
        {
            var productRows = Rows.OfType<PurchaseOrderDetailsProductRow>();
            return productRows.Any(a => a.ValidProduct);
        }

        public override void LoadGrid(IEnumerable<PurchaseDetails> entityList)
        {
            base.LoadGrid(entityList);
            PurchaseOrderViewModel.UpdateSupplierEnabled();
        }
    }
}
