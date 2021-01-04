using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public abstract class DataEntryGridColumn<TControl> : DataEntryGridTextColumn where TControl : Control
    {
        protected abstract void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory);

        protected override DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TControl));
            factory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(Loaded));
            ProcessCellFrameworkElementFactory(factory);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }

        protected virtual void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TControl control)
            {
                var grid = control.GetParentOfType<DataEntryGrid>();
                if (grid != null)
                {
                    var row = control.GetParentOfType<DataGridRow>();
                    if (row != null)
                    {
                        var cellStyle = grid.GetCellStyle(row, this);
                        switch (cellStyle.State)
                        {
                            case DataEntryGridCellStates.Enabled:
                                break;
                            case DataEntryGridCellStates.ReadOnly:
                            case DataEntryGridCellStates.Disabled:
                                control.IsEnabled = false;
                                break;
                        }

                        if (cellStyle is DataEntryGridControlCellStyle controlCellStyle)
                        {
                            control.Visibility = controlCellStyle.ControlVisible ? Visibility.Visible : Visibility.Collapsed;
                        }
                    }
                }
            }
        }
    }
}
