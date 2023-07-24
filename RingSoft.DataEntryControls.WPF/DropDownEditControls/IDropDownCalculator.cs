using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DropDownEditControls
{
    public interface IDropDownCalculator
    {
        Control Control { get; }

        double? Value { get; set; }

        int Precision { get; set; }

        event RoutedPropertyChangedEventHandler<object> ValueChanged;
    }
}
