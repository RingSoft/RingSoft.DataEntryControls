using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridTextColumn : DataEntryGridColumn
    {
        public override DataEntryGridColumnTypes ColumnType => DataEntryGridColumnTypes.Text;

        protected internal override DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));

            ProcessCellFrameworkElementFactory(factory, DataColumnName);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }

        protected void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory,
            string dataColumnName)
        {
            factory.SetBinding(TextBlock.TextProperty, new Binding(dataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(TextBlock.TextAlignmentProperty, Alignment);
        }
    }
}
