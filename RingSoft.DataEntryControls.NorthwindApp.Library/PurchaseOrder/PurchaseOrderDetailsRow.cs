using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System.ComponentModel;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public enum PurchaseOrderDetailsLineTypes
    {
        Product = 0,
        [Description("Direct Expense")]
        DirectExpense = 1,
        Comment = 2
    }
    public abstract class PurchaseOrderDetailsRow : DbMaintenanceDataEntryGridRow<PurchaseDetails>
    {
        public PurchaseOrderDetailsGridManager PurchaseOrderDetailsManager { get; }

        public abstract PurchaseOrderDetailsLineTypes LineType { get; }

        private DataEntryComboBoxSetup _lineTypeSetup = new DataEntryComboBoxSetup();

        protected PurchaseOrderDetailsRow(PurchaseOrderDetailsGridManager manager) : base(manager)
        {
            PurchaseOrderDetailsManager = manager;
            _lineTypeSetup.LoadFromEnum<PurchaseOrderDetailsLineTypes>();
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (PurchaseOrderColumns)columnId;
            if (column == PurchaseOrderColumns.LineType)
            {
                var lineTypeItem = _lineTypeSetup.GetItem((int)LineType);
                return new DataEntryGridComboBoxCellProps(this, columnId, _lineTypeSetup, lineTypeItem,
                    ComboBoxValueChangedTypes.SelectedItemChanged);
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

    }
}
