using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public class DataEntryGridTextComboBoxHost : DataEntryGridComboBoxHost<TextComboBoxControl>
    {
        protected override ComboBox ComboBox => Control;
        private TextComboBoxControlSetup _comboBoxSetup;
        private TextComboBoxItem _selectedItem;

        public DataEntryGridTextComboBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        protected override DataEntryGridComboBoxCellProps GetComboBoxCellProps(
            ComboBoxValueChangedTypes valueChangeType)
        {
            return new DataEntryGridTextComboBoxCellProps(Row, ColumnId,
                _comboBoxSetup, Control.SelectedItem, valueChangeType);
        }

        protected override void ValidateComboBoxCellProps(DataEntryGridComboBoxCellProps comboBoxCellProps)
        {
            if (comboBoxCellProps is DataEntryGridTextComboBoxCellProps textComboBoxCellProps)
                if (textComboBoxCellProps.SelectedItem == null)
                    throw new ArgumentException($"ComboBox Selected Item cannot be null.");
        }

        public override bool HasDataChanged()
        {
            return Control.SelectedItem != _selectedItem;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);
            if (comboBoxCellProps is DataEntryGridTextComboBoxCellProps textComboBoxCellProps)
                _selectedItem = textComboBoxCellProps.SelectedItem;
        }

        protected override void SetSelectedItem(bool overrideCellMovement)
        {
            if (overrideCellMovement)
                Control.SelectedItem = _selectedItem;
            else
            {
                _selectedItem = Control.SelectedItem;
            }
        }

        protected override void OnComboControlLoaded(TextComboBoxControl control, DataEntryGridComboBoxCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);

            if (comboBoxCellProps is DataEntryGridTextComboBoxCellProps textComboBoxCellProps)
            {
                control.Setup = _comboBoxSetup = textComboBoxCellProps.ComboBoxSetup;
                control.SelectedItem = _selectedItem = textComboBoxCellProps.SelectedItem;
            }
        }
    }

    public abstract class DataEntryGridComboBoxHost<TControl> : DataEntryGridEditingControlHost<TControl>
        where TControl : Control
    {
        protected abstract ComboBox ComboBox { get; }

        public sealed override bool IsDropDownOpen => ComboBox.IsDropDownOpen;

        private ComboBoxValueChangedTypes _valueChangeType;
        private bool _proceessingValidationFail;

        protected DataEntryGridComboBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        public sealed override DataEntryGridEditingCellProps GetCellValue()
        {
            return GetComboBoxCellProps(_valueChangeType);
        }

        protected abstract DataEntryGridComboBoxCellProps GetComboBoxCellProps(ComboBoxValueChangedTypes valueChangeType);

        protected abstract void ValidateComboBoxCellProps(DataEntryGridComboBoxCellProps comboBoxCellProps);

        protected DataEntryGridComboBoxCellProps GetComboBoxCellProps(DataEntryGridCellProps cellProps)
        {
            var comboBoxProps = cellProps as DataEntryGridComboBoxCellProps;
            if (comboBoxProps == null)
            {
                var rowName = cellProps.Row.ToString();
                throw new ArgumentException(
                    $"{nameof(DataEntryGridTextComboBoxCellProps)} not setup for Row: {rowName} Column Id={cellProps.ColumnId}");
            }

            ValidateComboBoxCellProps(comboBoxProps);

            return comboBoxProps;
        }

        protected abstract void SetSelectedItem(bool overrideCellMovement);

        protected sealed override void OnControlLoaded(TControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);

            _valueChangeType = comboBoxCellProps.ChangeType;

            OnComboControlLoaded(control, comboBoxCellProps, cellStyle);

            ComboBox.SelectionChanged += (sender, args) => OnSelectionChanged();
        }

        protected abstract void OnComboControlLoaded(TControl control, DataEntryGridComboBoxCellProps cellProps,
            DataEntryGridCellStyle cellStyle);

        private void OnSelectionChanged()
        {
            if (_proceessingValidationFail || _valueChangeType == ComboBoxValueChangedTypes.EndEdit)
                return;

            OnControlDirty();
            var controlValue = GetComboBoxCellProps(GetCellValue());
            OnUpdateSource(controlValue);

            if (controlValue.OverrideCellMovement)
                SetSelectedItem(true);
            else
            {
                _proceessingValidationFail = true;
                SetSelectedItem(false);
                _proceessingValidationFail = false;
            }

            Grid.UpdateRow(Row);
        }

        public sealed override bool CanGridProcessKey(Key key)
        {
            switch (key)
            {
                case Key.Up:
                case Key.Down:
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                        return false;
                    if (ComboBox.IsDropDownOpen)
                        return false;

                    break;
                case Key.Escape:
                case Key.Enter:
                    if (ComboBox.IsDropDownOpen)
                        return false;
                    break;
            }
            return base.CanGridProcessKey(key);
        }

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            Control.MinHeight = dataGridCell.MinHeight;
            Control.MinWidth = dataGridCell.MinWidth;

            //base.ImportDataGridCellProperties(dataGridCell);
        }
    }
}
