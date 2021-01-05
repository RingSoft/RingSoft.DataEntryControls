using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridCheckBoxColumn : DataEntryGridColumn<DataEntryGridCheckBox>
    {
        protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory,
            string dataColumnName)
        {
            factory.SetBinding(ToggleButton.IsCheckedProperty, new Binding(dataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        }
    }
}
