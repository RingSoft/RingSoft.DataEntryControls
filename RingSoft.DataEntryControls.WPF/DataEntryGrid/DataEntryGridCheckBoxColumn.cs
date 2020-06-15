using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridCheckBoxColumn : DataEntryGridColumn
    {
        protected override DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(CheckBox));
            factory.SetBinding(ToggleButton.IsCheckedProperty, new Binding(DataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }
    }
}
