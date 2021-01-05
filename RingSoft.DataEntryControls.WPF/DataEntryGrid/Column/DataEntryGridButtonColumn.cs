using System.Windows;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridButtonColumn : DataEntryGridColumn<DataEntryGridButton>
    {
        protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory,
            string dataColumnName)
        {
            factory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(Loaded));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DataEntryGridButton button)
            {
                button.Content = DesignText;
            }
        }
    }
}
