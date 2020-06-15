using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridCheckBoxHost : DataEntryGridControlHost<CheckBox>
    {
        public DataEntryGridCheckBoxCellProps CheckBoxCellProps { get; private set; }
        public CheckBox CheckBox { get; private set; }

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
            bool checkBoxValue = CheckBox.IsChecked != null && (bool) CheckBox.IsChecked;

            return new DataEntryGridCheckBoxCellProps(CellProps.Row, CellProps.ColumnId, checkBoxValue);
        }

        public override bool HasDataChanged()
        {
            return CheckBoxCellProps.Value != CheckBox.IsChecked;
        }

        protected override void OnControlLoaded(CheckBox control, DataEntryGridCellProps cellProps)
        {
            CheckBox = control;
            CheckBoxCellProps = cellProps as DataEntryGridCheckBoxCellProps;
            control.IsChecked = CheckBoxCellProps != null && CheckBoxCellProps.Value;

            CheckBox.Checked += (sender, args) =>
            {
                OnControlDirty();
                OnUpdateSource(GetCellValue());
                CheckBoxCellProps.Value = (bool)control.IsChecked;
            };
            CheckBox.Unchecked += (sender, args) =>
            {
                OnControlDirty();
                OnUpdateSource(GetCellValue());
                CheckBoxCellProps.Value = (bool) control.IsChecked;
            };
        }
    }
}
