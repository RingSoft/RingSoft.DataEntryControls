using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using ComboBoxItem = RingSoft.DataEntryControls.Engine.ComboBoxItem;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridComboBoxHost : DataEntryGridControlHost<ComboBoxControl>
    {
        public override bool IsDropDownOpen => Control.IsDropDownOpen;
        private DataEntryComboBoxSetup _comboBoxSetup;
        private ComboBoxItem _selectedItem;
        private ComboBoxValueChangedTypes _valueChangeType;
        private bool _proceessingValidationFail;

        public DataEntryGridComboBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            ComboBoxItem selectedItem = Control.SelectedItem as ComboBoxItem;

            return new DataEntryGridComboBoxCellProps(Row, ColumnId,
                _comboBoxSetup, selectedItem, _valueChangeType);
        }

        public override bool HasDataChanged()
        {
            return Control.SelectedItem != _selectedItem;
        }

        protected override void OnControlLoaded(ComboBoxControl control, DataEntryGridCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);
            _comboBoxSetup = comboBoxCellProps.ComboBoxSetup;
            
            control.ItemsSource = comboBoxCellProps.ComboBoxSetup.Items;
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

            if (controlValue.ValidationResult)
                _selectedItem = Control.SelectedItem as ComboBoxItem;
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
