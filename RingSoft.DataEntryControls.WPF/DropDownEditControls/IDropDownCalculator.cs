using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DropDownEditControls
{
    public interface IDropDownCalculator
    {
        Control Control { get; }

        decimal? Value { get; set; }

        int Precision { get; set; }

        event RoutedPropertyChangedEventHandler<object> ValueChanged;
    }
}
