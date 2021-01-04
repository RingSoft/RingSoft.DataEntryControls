using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using ComboBoxItem = RingSoft.DataEntryControls.Engine.ComboBoxItem;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public class DataEntryGridComboBoxHost : DataEntryGridEditingControlHost<ComboBoxControl>
    {
        public override bool IsDropDownOpen => Control.IsDropDownOpen;
        private ComboBoxControlSetup _comboBoxSetup;
        private ComboBoxItem _selectedItem;
        private ComboBoxValueChangedTypes _valueChangeType;
        private bool _proceessingValidationFail;

        public DataEntryGridComboBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridComboBoxCellProps(Row, ColumnId,
                _comboBoxSetup, Control.SelectedItem, _valueChangeType);
        }

        public override bool HasDataChanged()
        {
            return Control.SelectedItem != _selectedItem;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);
            _selectedItem = comboBoxCellProps.SelectedItem;
        }

        protected override void OnControlLoaded(ComboBoxControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);

            control.Setup = _comboBoxSetup = comboBoxCellProps.ComboBoxSetup;
            control.SelectedItem = _selectedItem = comboBoxCellProps.SelectedItem;
            _valueChangeType = comboBoxCellProps.ChangeType;

            Control.SelectionChanged += (sender, args) => OnSelectionChanged();
        }

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
        }

        private void OnSelectionChanged()
        {
            if (_proceessingValidationFail || _valueChangeType == ComboBoxValueChangedTypes.EndEdit)
                return;

            OnControlDirty();
            var controlValue = GetComboBoxCellProps(GetCellValue());
            OnUpdateSource(controlValue);

            if (!controlValue.OverrideCellMovement)
                _selectedItem = Control.SelectedItem;
            else
            {
                _proceessingValidationFail = true;
                Control.SelectedItem = _selectedItem;
                _proceessingValidationFail = false;
            }

            Grid.UpdateRow(Row);
        }

        private DataEntryGridComboBoxCellProps GetComboBoxCellProps(DataEntryGridCellProps cellProps)
        {
            var comboBoxProps = cellProps as DataEntryGridComboBoxCellProps;
            if (comboBoxProps == null)
            {
                var rowName = cellProps.ToString();
                throw new ArgumentException(
                    $"{nameof(DataEntryGridComboBoxCellProps)} not setup for Row: {rowName} Column Id={cellProps.ColumnId}");
            }

            if (comboBoxProps.SelectedItem == null)
                throw new ArgumentException($"ComboBox Selected Item cannot be null.");

            return comboBoxProps;
        }

        public override bool CanGridProcessKey(Key key)
        {
            switch (key)
            {
                case Key.Up:
                case Key.Down:
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                        return false;
                    if (Control.IsDropDownOpen)
                        return false;

                    break;
                case Key.Escape:
                case Key.Enter:
                    if (Control.IsDropDownOpen)
                        return false;
                    break;
            }
            return base.CanGridProcessKey(key);
        }
    }
}
