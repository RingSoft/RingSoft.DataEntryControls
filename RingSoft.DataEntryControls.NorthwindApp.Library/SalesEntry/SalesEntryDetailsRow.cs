﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

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

        public abstract SalesEntryDetailsLineTypes LineType { get; }

        private DataEntryComboBoxSetup _lineTypeSetup = new DataEntryComboBoxSetup();

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

        protected IEnumerable<OrderDetails> GetDetailChildren(OrderDetails parent)
        {
            return parent.Order.OrderDetails.Where(w => w.ParentRowId == parent.RowId);
        }
    }
}
