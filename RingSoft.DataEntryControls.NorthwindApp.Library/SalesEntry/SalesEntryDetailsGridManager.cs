using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

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
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SalesEntryViewModel.ScannerMode))
                    UpdateNewRows();
            };
        }

        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnRowsChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var addedRows = e.NewItems.OfType<SalesEntryDetailsNewRow>().ToList();
                    if (addedRows.Any())
                        UpdateNewRows();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UpdateNewRows();
                    break;
            }
        }

        private void UpdateNewRows()
        {
            var newRows = Rows.OfType<SalesEntryDetailsNewRow>().ToList();
            foreach (var newRow in newRows)
            {
                Grid.UpdateRow(newRow);
            }
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new SalesEntryDetailsNewRow(this);
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

        public override void RaiseDirtyFlag()
        {
            ViewModel.RecordDirty = true;
            base.RaiseDirtyFlag();
        }

        public bool ValidateGrid()
        {
            if (!Grid.CommitEdit())
                return false;

            foreach (var dataEntryGridRow in Rows)
            {
                if (dataEntryGridRow is SalesEntryDetailsRow row && !row.IsNew)
                    if (!row.ValidateRow())
                        return false;
            }
            return true;
        }

        public List<OrderDetails> GetDetailsData()
        {
            var result = new List<OrderDetails>();
            var detailId = 0;
            foreach (var dataEntryGridRow in Rows)
            {
                if (dataEntryGridRow is SalesEntryDetailsRow row && !row.IsNew)
                {
                    var orderDetail = new OrderDetails();
                    orderDetail.OrderDetailId = detailId;
                    orderDetail.LineType = (byte) row.LineType;
                    orderDetail.RowId = row.RowId;
                    orderDetail.ParentRowId = row.ParentRowId;

                    row.SaveToOrderDetail(orderDetail);
                    result.Add(orderDetail);
                    detailId++;
                }
            }

            return result;
        }
    }
}
