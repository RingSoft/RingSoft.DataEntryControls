using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public enum SalesEntryDetailsLineTypes
    {
        Product = 0,
        NonInventoryCode = 1,
        SpecialOrder = 2,
        Comment = 3
    }

    public abstract class SalesEntryDetailsRow : DataEntryGridRow
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
            var lineTypeItem = _lineTypeSetup.GetItem((int) LineType);
            return new DataEntryGridComboBoxCellProps(this, columnId, _lineTypeSetup, lineTypeItem);
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

        public virtual void LoadFromOrderDetail(OrderDetails orderDetail)
        {
            IsNew = false;
        }

        public abstract bool ValidateRow();

        public abstract void SaveToOrderDetail(OrderDetails orderDetail);
    }
}
