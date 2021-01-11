using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using System;
using System.Windows.Controls;

namespace TestDummyApp
{
    public class LineTypeControlHost : DataEntryGridComboBoxHost<LineTypeControl>
    {
        protected override ComboBox ComboBox => Control.ComboBox;

        private AppGridLineTypes _lineType;

        public LineTypeControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override bool HasDataChanged()
        {
            return _lineType != Control.LineType;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is LineTypeCellProps lineTypeCellProps)
                Control.LineType = lineTypeCellProps.LineType;
        }

        protected override DataEntryGridComboBoxCellProps GetComboBoxCellProps(ComboBoxValueChangedTypes valueChangeType)
        {
            return new LineTypeCellProps(Row, ColumnId, Control.LineType, valueChangeType);
        }

        protected override void ValidateComboBoxCellProps(DataEntryGridComboBoxCellProps comboBoxCellProps)
        {
            if (!(comboBoxCellProps is LineTypeCellProps))
                throw new Exception(
                    $"Row: {comboBoxCellProps.Row} ColumnId: {comboBoxCellProps.ColumnId} {nameof(DataEntryGridRow.GetCellProps)} must return a valid {nameof(LineTypeCellProps)} object.");
        }

        protected override void SetSelectedItem(bool overrideCellMovement)
        {
            if (overrideCellMovement)
            {
                Control.LineType = _lineType;
            }
            else
            {
                _lineType = Control.LineType;
            }
        }

        protected override void OnComboControlLoaded(LineTypeControl control, DataEntryGridComboBoxCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            if (cellProps is LineTypeCellProps lineTypeCellProps)
                _lineType = Control.LineType = lineTypeCellProps.LineType;
        }
    }
}
