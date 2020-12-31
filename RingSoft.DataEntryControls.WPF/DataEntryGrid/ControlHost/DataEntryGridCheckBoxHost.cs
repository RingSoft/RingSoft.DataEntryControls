﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridCheckBoxHost : DataEntryGridControlHost<CheckBox>
    {
        public override bool IsDropDownOpen => false;
        public override bool SetSelection => true;

        private bool _value;

        public DataEntryGridCheckBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        protected override void SetupFrameworkElementFactory(FrameworkElementFactory factory)
        {
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            base.SetupFrameworkElementFactory(factory);
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            bool checkBoxValue = Control.IsChecked != null && (bool) Control.IsChecked;

            return new DataEntryGridCheckBoxCellProps(Row, ColumnId, checkBoxValue);
        }

        public override bool HasDataChanged()
        {
            return _value != Control.IsChecked;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            _value = cellProps is DataEntryGridCheckBoxCellProps checkBoxCellProps && checkBoxCellProps.Value;
        }

        protected override void OnControlLoaded(CheckBox control, DataEntryGridCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var checkBoxCellProps = cellProps as DataEntryGridCheckBoxCellProps;
            control.IsChecked = _value = checkBoxCellProps != null && checkBoxCellProps.Value;
            switch (cellStyle.CellStyle)
            {
                case DataEntryGridCellStyles.Enabled:
                    break;
                case DataEntryGridCellStyles.ReadOnly:
                case DataEntryGridCellStyles.Disabled:
                    control.IsEnabled = false;
                    break;
            }

            Control.Checked += (sender, args) =>
            {
                OnControlDirty();
                OnUpdateSource(GetCellValue());
                _value = (bool)control.IsChecked;
            };
            Control.Unchecked += (sender, args) =>
            {
                OnControlDirty();
                OnUpdateSource(GetCellValue());
                _value = (bool) control.IsChecked;
            };

            if (Mouse.LeftButton == MouseButtonState.Pressed)
                control.IsChecked = !control.IsChecked;

            Control.HorizontalAlignment = HorizontalAlignment.Center;
        }

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
        }
    }
}
