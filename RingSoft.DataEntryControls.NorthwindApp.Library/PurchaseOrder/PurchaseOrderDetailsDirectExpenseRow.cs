﻿using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public class PurchaseOrderDetailsDirectExpenseRow : PurchaseOrderDetailsRow
    {
        public override PurchaseOrderDetailsLineTypes LineType => PurchaseOrderDetailsLineTypes.DirectExpense;

        public string Description { get; set; }

        public double Price { get; set; }

        private DecimalEditControlSetup _priceSetup = AppGlobals.CreateNewDecimalEditControlSetup();

        public PurchaseOrderDetailsDirectExpenseRow(PurchaseOrderDetailsGridManager manager) : base(manager)
        {
            DisplayStyleId = PurchaseOrderDetailsGridManager.DirectExpenseDisplayId;
            _priceSetup.FormatType = DecimalEditFormatTypes.Currency;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    break;
                case PurchaseOrderColumns.Item:
                    return new DataEntryGridTextCellProps(this, columnId){Text = Description};
                case PurchaseOrderColumns.Quantity:
                    return new DataEntryGridTextCellProps(this, columnId);
                case PurchaseOrderColumns.Price:
                    return new DataEntryGridDecimalCellProps(this, columnId, _priceSetup, Price);
                case PurchaseOrderColumns.ExtendedPrice:
                    return new DataEntryGridTextCellProps(this, columnId);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    break;
                case PurchaseOrderColumns.Price:
                    return new DataEntryGridCellStyle { ColumnHeader = "Amount" };
                case PurchaseOrderColumns.Item:
                    return new DataEntryGridCellStyle{ColumnHeader = "Description"};
                case PurchaseOrderColumns.Received:
                    return new DataEntryGridControlCellStyle {State = DataEntryGridCellStates.Disabled};
                default:
                    return new DataEntryGridCellStyle{State = DataEntryGridCellStates.Disabled};
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (PurchaseOrderColumns) value.ColumnId;
            switch (column)
            {
                case PurchaseOrderColumns.Item:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        Description = textCellProps.Text;
                    }
                    break;
                case PurchaseOrderColumns.Price:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        if (decimalCellProps.Value != null)
                        {
                            Price = (double) decimalCellProps.Value;
                            PurchaseOrderDetailsManager.PurchaseOrderViewModel.RefreshTotalControls();
                        }
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override bool ValidateRow()
        {
            if (string.IsNullOrEmpty(Description))
            {
                PurchaseOrderDetailsManager.PurchaseOrderViewModel.PurchaseOrderView.GridValidationFail();
                Manager.Grid.GotoCell(this, (int)PurchaseOrderColumns.Item);
                ControlsGlobals.UserInterface.ShowMessageBox("Description must have a value",
                    "Invalid Direct Expense Description", RsMessageBoxIcons.Exclamation);
                return false;
            }

            return true;
        }

        public override void SaveToEntity(PurchaseDetails entity, int rowIndex)
        {
            entity.DirectExpenseText = Description;
            entity.Price = Price;
            base.SaveToEntity(entity, rowIndex);
        }

        public override void LoadFromEntity(PurchaseDetails entity)
        {
            Description = entity.DirectExpenseText;
            if (entity.Price != null)
                Price = (double) entity.Price;

            base.LoadFromEntity(entity);
        }
    }
}
