using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridCheckBoxHost : DataEntryGridControlHost<CheckBox>
    {
        public override bool IsDropDownOpen => false;
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

        protected override void OnControlLoaded(CheckBox control, DataEntryGridCellProps cellProps)
        {
            var checkBoxCellProps = cellProps as DataEntryGridCheckBoxCellProps;
            control.IsChecked = _value = checkBoxCellProps != null && checkBoxCellProps.Value;

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
