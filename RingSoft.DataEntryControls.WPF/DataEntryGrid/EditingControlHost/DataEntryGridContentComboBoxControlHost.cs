using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridContentComboBoxControlHost : DataEntryGridComboBoxHost<ContentComboBoxControl>
    {
        protected override ComboBox ComboBox => Control;

        private int _selectedItemId;

        public DataEntryGridContentComboBoxControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override bool HasDataChanged()
        {
            return Control.SelectedItemId != _selectedItemId;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is DataEntryGridCustomControlCellProps customControlCellProps)
                Control.SelectedItemId = customControlCellProps.SelectedItemId;
        }

        protected override DataEntryGridComboBoxCellProps GetComboBoxCellProps(ComboBoxValueChangedTypes valueChangeType)
        {
            return new DataEntryGridCustomControlCellProps(Row, ColumnId, Control.SelectedItemId, valueChangeType);
        }

        protected override void ValidateComboBoxCellProps(DataEntryGridComboBoxCellProps comboBoxCellProps)
        {
            if (!(comboBoxCellProps is DataEntryGridCustomControlCellProps))
                throw new Exception(
                    $"Row: {comboBoxCellProps.Row} ColumnId: {comboBoxCellProps.ColumnId} {nameof(DataEntryGridRow.GetCellProps)} must return a valid {nameof(DataEntryGridCustomControlCellProps)} object.");
        }

        protected override void SetSelectedItem(bool overrideCellMovement)
        {
            if (overrideCellMovement)
            {
                Control.SelectedItemId = _selectedItemId;
            }
            else
            {
                _selectedItemId = Control.SelectedItemId;
            }
        }

        protected override void OnComboControlLoaded(ContentComboBoxControl control, DataEntryGridComboBoxCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            if (cellProps is DataEntryGridCustomControlCellProps customControlCellProps)
                _selectedItemId = Control.SelectedItemId = customControlCellProps.SelectedItemId;
        }

        protected override void SetupFrameworkElementFactory(FrameworkElementFactory factory, DataEntryGridColumn column)
        {
            if (column is DataEntryGridCustomControlColumn customControlColumn)
            {
                factory.SetValue(ContentComboBoxControl.ContentTemplateProperty, customControlColumn.ContentTemplate);
            }
            else
            {
                throw new Exception(
                    $"DataEntryGridRow: {Row} GetCellProps for ColumnId: {column.ColumnId} cannot return a {nameof(DataEntryGridCustomControlCellProps)} object.  It is only compatible with {nameof(DataEntryGridCustomControlColumn)} column types.");
            }
            base.SetupFrameworkElementFactory(factory, column);
        }
    }
}
