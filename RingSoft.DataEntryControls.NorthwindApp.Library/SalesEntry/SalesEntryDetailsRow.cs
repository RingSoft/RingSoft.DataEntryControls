using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbMaintenance;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public enum SalesEntryDetailsLineTypes
    {
        Product = AppGlobals.SalesProductId,
        [Description("Non Inventory")]
        NonInventoryCode = AppGlobals.SalesNonInventoryId,
        [Description("Special Order")]
        SpecialOrder = AppGlobals.SalesSpecialOrderId,
        Comment = AppGlobals.SalesCommentId,
        NewRow = AppGlobals.SalesNewRowId
    }

    public abstract class SalesEntryDetailsRow : DbMaintenanceDataEntryGridRow<OrderDetails>
    {
        public SalesEntryDetailsGridManager SalesEntryDetailsManager { get; }

        public int DbOrderDetailId { get; private set; }

        public abstract SalesEntryDetailsLineTypes LineType { get; }

        protected SalesEntryDetailsRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            SalesEntryDetailsManager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            if (column == SalesEntryGridColumns.LineType)
            {
                return new DataEntryGridCustomControlCellProps(this, columnId, (int)LineType);
            }
            return  new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.LineType:
                    return new DataEntryGridControlCellStyle{State = DataEntryGridCellStates.Disabled};
            }

            return base.GetCellStyle(columnId);
        }

        public override void SaveToEntity(OrderDetails entity, int rowIndex)
        {
            entity.OrderDetailId = rowIndex;
            entity.LineType = (byte)LineType;
            entity.RowId = RowId;
            entity.ParentRowId = ParentRowId;
        }

        public override void LoadFromEntity(OrderDetails entity)
        {
            DbOrderDetailId = entity.OrderDetailId;
        }

        protected IEnumerable<OrderDetails> GetDetailChildren(OrderDetails parent)
        {
            var result = parent.Order.OrderDetails.Where(w =>
                w.ParentRowId != null && w.ParentRowId == parent.RowId);
            return result;
        }
    }
}
