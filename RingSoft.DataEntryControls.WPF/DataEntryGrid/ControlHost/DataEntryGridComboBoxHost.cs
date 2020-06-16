using System;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using ComboBoxItem = RingSoft.DataEntryControls.Engine.ComboBoxItem;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridComboBoxHost : DataEntryGridControlHost<ComboBox>
    {
        private bool _selectingItem;
        public DataEntryGridComboBoxCellProps ComboBoxCellProps { get; private set; }

        public DataEntryGridComboBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            ComboBoxItem selectedItem = Control.SelectedItem as ComboBoxItem;

            return new DataEntryGridComboBoxCellProps(ComboBoxCellProps.Row, ComboBoxCellProps.ColumnId,
                ComboBoxCellProps.ComboBoxSetup, selectedItem);
        }

        public override bool HasDataChanged()
        {
            return Control.SelectedItem != ComboBoxCellProps.SelectedItem;
        }

        protected override void OnControlLoaded(ComboBox control, DataEntryGridCellProps cellProps)
        {
            ComboBoxCellProps = GetComboBoxCellProps(cellProps);
            
            control.ItemsSource = ComboBoxCellProps.ComboBoxSetup.Items;
            control.SelectedItem = ComboBoxCellProps.SelectedItem;

            Control.SelectionChanged += (sender, args) =>
            {
                if (!_selectingItem)
                {
                    OnControlDirty();
                    var controlValue = GetComboBoxCellProps(GetCellValue());
                    controlValue.ChangeType = ComboBoxValueChangedTypes.SelectedItemChanged;
                    OnUpdateSource(controlValue);
                }
            };
        }

        private void SelectItem(ComboBoxItem item)
        {
            _selectingItem = true;
            Control.SelectedItem = item;
            _selectingItem = false;
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
                    if (Control.IsDropDownOpen)
                        return false;
                    break;
            }
            return base.CanGridProcessKey(key);
        }

        public override void ProcessValidationFail(DataEntryGridCellProps cellProps)
        {
            var comboBoxProps = GetComboBoxCellProps(cellProps);
            SelectItem(comboBoxProps.SelectedItem);
            base.ProcessValidationFail(cellProps);
        }
    }
}
