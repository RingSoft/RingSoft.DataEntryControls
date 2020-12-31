using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridButtonColumn : DataEntryGridControlColumn<Button>
    { protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory)
        {
            factory.SetBinding(ContentControl.ContentProperty, new Binding(DataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        }
    }

    public class DataEntryGridCheckBoxColumn : DataEntryGridControlColumn<CheckBox>
    {
        protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory)
        {
            factory.SetBinding(ToggleButton.IsCheckedProperty, new Binding(DataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        }
    }

    public abstract class DataEntryGridControlColumn<TControl> : DataEntryGridColumn where TControl : Control
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
                        switch (cellStyle.CellStyle)
                        {
                            case DataEntryGridCellStyles.Enabled:
                                break;
                            case DataEntryGridCellStyles.ReadOnly:
                            case DataEntryGridCellStyles.Disabled:
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
