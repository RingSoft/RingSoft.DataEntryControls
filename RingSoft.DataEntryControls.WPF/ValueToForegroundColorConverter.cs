using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    public class ValueToForegroundColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var doubleValue = 0.0;
            if (value != null) 
                double.TryParse(value.ToString().NumTextToString(culture), out doubleValue);

            if (doubleValue < 0)
                return new SolidColorBrush(Colors.Red);

            return parameter ?? DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
