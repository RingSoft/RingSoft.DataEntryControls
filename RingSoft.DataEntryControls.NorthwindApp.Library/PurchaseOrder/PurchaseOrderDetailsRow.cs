﻿using System;
using System.Collections.Generic;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System.ComponentModel;
using System.Linq;

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

        public DateTime? PickDate { get; private set; }

        public bool Received { get; private set; }

        public int? DelayDays { get; private set; }

        private DataEntryComboBoxSetup _lineTypeSetup = new DataEntryComboBoxSetup();
        private DateEditControlSetup _pickDateSetup = AppGlobals.CreateNewDateEditControlSetup();
        private IntegerEditControlSetup _delayDaysSetup = AppGlobals.CreateNewIntegerEditControlSetup();

        protected PurchaseOrderDetailsRow(PurchaseOrderDetailsGridManager manager) : base(manager)
        {
            PurchaseOrderDetailsManager = manager;

            _lineTypeSetup.LoadFromEnum<PurchaseOrderDetailsLineTypes>();
            _pickDateSetup.AllowNullValue = true;
            _pickDateSetup.DateFormatType = DateFormatTypes.DateTime;

            _delayDaysSetup.AllowNullValue = true;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (PurchaseOrderColumns)columnId;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    var lineTypeItem = _lineTypeSetup.GetItem((int)LineType);
                    return new DataEntryGridComboBoxCellProps(this, columnId, _lineTypeSetup, lineTypeItem,
                        ComboBoxValueChangedTypes.SelectedItemChanged);
                case PurchaseOrderColumns.PickDate:
                    return new DataEntryGridDateCellProps(this, columnId, _pickDateSetup, PickDate);
                case PurchaseOrderColumns.Received:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, Received);
                case PurchaseOrderColumns.DelayDays:
                    return new DataEntryGridIntegerCellProps(this, columnId, _delayDaysSetup, DelayDays);
            }

            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (PurchaseOrderColumns) value.ColumnId;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    if (!IsNew)
                    {
                        var message = "Changing the line type will erase all row data.  Do you wish to continue?";
                        if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Erase Row?") ==
                            MessageBoxButtonsResult.No)
                        {
                            value.OverrideCellMovement = true;
                            return;
                        }
                    }

                    if (value is DataEntryGridComboBoxCellProps comboBoxCellProps)
                    {
                        var newLineType = (PurchaseOrderDetailsLineTypes) comboBoxCellProps.SelectedItem.NumericValue;
                        var newRow = PurchaseOrderDetailsManager.CreateRowFromLineType(newLineType);
                        PurchaseOrderDetailsManager.ReplaceRow(this, newRow);
                        PurchaseOrderDetailsManager.Grid.UpdateRow(newRow);
                        newRow.IsNew = true;
                    }
                    break;
                case PurchaseOrderColumns.PickDate:
                    if (value is DataEntryGridDateCellProps dateCellProps)
                    {
                        PickDate = dateCellProps.Value;
                    }
                    break;
                case PurchaseOrderColumns.Received:
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                    {
                        Received = checkBoxCellProps.Value;
                    }
                    break;
                case PurchaseOrderColumns.DelayDays:
                    if (value is DataEntryGridIntegerCellProps integerCellProps)
                    {
                        DelayDays = integerCellProps.Value;
                    }
                    break;
            }

            base.SetCellValue(value);
        }

        public override void SaveToEntity(PurchaseDetails entity, int rowIndex)
        {
            entity.PurchaseDetailId = rowIndex;
            entity.LineType = (byte) LineType;
            entity.RowId = RowId;
            entity.ParentRowId = ParentRowId;
            entity.PickDate = PickDate;
            entity.Received = Received;
            entity.DelayDays = DelayDays;
        }

        public override void LoadFromEntity(PurchaseDetails entity)
        {
            PickDate = entity.PickDate;
            Received = entity.Received;
            DelayDays = entity.DelayDays;
        }

        protected IEnumerable<PurchaseDetails> GetDetailChildren(PurchaseDetails parent)
        {
            var result = parent.PurchaseOrder.PurchaseDetails.Where(w =>
                w.ParentRowId != null && w.ParentRowId == parent.RowId);
            return result;
        }
    }
}
