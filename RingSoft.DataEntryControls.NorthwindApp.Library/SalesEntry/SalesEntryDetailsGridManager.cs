using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System;
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

    public class SalesEntryDetailsGridManager : DbMaintenanceDataEntryGridManager<OrderDetails>
    {
        public SalesEntryViewModel SalesEntryViewModel { get; }

        public const int LineTypeColumnId = 0;
        public const int ItemColumnId = 1;
        public const int QuantityColumnId = 2;
        public const int PriceColumnId = 3;
        public const int ExtendedPriceColumnId = 4;
        public const int DiscountColumnId = 5;

        public SalesEntryDetailsGridManager(SalesEntryViewModel viewModel) : base(viewModel)
        {
            SalesEntryViewModel = viewModel;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SalesEntry.SalesEntryViewModel.ScannerMode))
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
            return CreateRowFromLineType(SalesEntryDetailsLineTypes.NewRow);
        }

        public SalesEntryDetailsRow CreateRowFromLineType(SalesEntryDetailsLineTypes lineType)
        {
            switch (lineType)
            {
                case SalesEntryDetailsLineTypes.NewRow:
                    return new SalesEntryDetailsNewRow(this);
                case SalesEntryDetailsLineTypes.Product:
                    return new SalesEntryDetailsProductRow(this);
                //case SalesEntryDetailsLineTypes.NonInventoryCode:
                //    break;
                //case SalesEntryDetailsLineTypes.SpecialOrder:
                //    break;
                case SalesEntryDetailsLineTypes.Comment:
                    return new SalesEntryDetailsCommentRow(this);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override DbMaintenanceDataEntryGridRow<OrderDetails> ConstructNewRowFromEntity(OrderDetails entity)
        {
            return CreateRowFromLineType((SalesEntryDetailsLineTypes) entity.LineType);
        }

        protected override string GetParentRowIdFromEntity(OrderDetails entity)
        {
            return entity.ParentRowId;
        }
    }
}
