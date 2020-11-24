using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridButtonColumn : DataEntryGridColumn
    {
        protected override DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(Button));
            factory.SetBinding(ContentControl.ContentProperty, new Binding(DataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }
    }
}
