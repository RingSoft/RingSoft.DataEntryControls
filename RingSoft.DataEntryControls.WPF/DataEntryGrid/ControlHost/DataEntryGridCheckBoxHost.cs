using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridCheckBoxHost : DataEntryGridControlHost<CheckBox>
    {
        public DataEntryGridCheckBoxCellProps CheckBoxCellProps { get; private set; }

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

            return new DataEntryGridCheckBoxCellProps(CellProps.Row, CellProps.ColumnId, checkBoxValue);
        }

        public override bool HasDataChanged()
        {
            return CheckBoxCellProps.Value != Control.IsChecked;
        }

        protected override void OnControlLoaded(CheckBox control, DataEntryGridCellProps cellProps)
        {
            CheckBoxCellProps = cellProps as DataEntryGridCheckBoxCellProps;
            control.IsChecked = CheckBoxCellProps != null && CheckBoxCellProps.Value;

            Control.Checked += (sender, args) =>
            {
                OnControlDirty();
                OnUpdateSource(GetCellValue());
                CheckBoxCellProps.Value = (bool)control.IsChecked;
            };
            Control.Unchecked += (sender, args) =>
            {
                OnControlDirty();
                OnUpdateSource(GetCellValue());
                CheckBoxCellProps.Value = (bool) control.IsChecked;
            };
        }
    }
}
