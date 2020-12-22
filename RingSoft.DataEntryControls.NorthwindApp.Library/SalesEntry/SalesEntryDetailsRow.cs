using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public enum SalesEntryDetailsLineTypes
    {
        Product = 0,
        [Description("Non Inventory")]
        NonInventoryCode = 1,
        [Description("Special Order")]
        SpecialOrder = 2,
        Comment = 3,
        NewRow = 4
    }

    public abstract class SalesEntryDetailsRow : DbMaintenanceDataEntryGridRow<OrderDetails>
    {
        public SalesEntryDetailsGridManager SalesEntryDetailsManager { get; }

        public int DbOrderDetailId { get; private set; }

        public abstract SalesEntryDetailsLineTypes LineType { get; }

        private ComboBoxControlSetup _lineTypeSetup = new ComboBoxControlSetup();

        protected SalesEntryDetailsRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            SalesEntryDetailsManager = manager;
            _lineTypeSetup.LoadFromEnum<SalesEntryDetailsLineTypes>();
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            if (column == SalesEntryGridColumns.LineType)
            {
                var lineTypeItem = _lineTypeSetup.GetItem((int) LineType);
                return new DataEntryGridComboBoxCellProps(this, columnId, _lineTypeSetup, lineTypeItem);
            }
            return  new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.LineType:
                    return new DataEntryGridCellStyle(){CellStyle = DataEntryGridCellStyles.Disabled};
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
