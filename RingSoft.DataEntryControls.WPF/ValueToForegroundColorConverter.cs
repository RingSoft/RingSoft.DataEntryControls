using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    public class ValueToForegroundParameter
    {
        public bool ShowNegativeValuesInRed { get; set; }
        public bool ShowPositiveValuesInGreen { get; set; }
        public object Parameter { get; set; }
    }
    public class ValueToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var showNegativeValuesInRed = false;
            var showPositiveValuesInGreen = false;
            object inputParameter = null;

            if (parameter is ValueToForegroundParameter valueToForegroundParameter)
            {
                showNegativeValuesInRed = valueToForegroundParameter.ShowNegativeValuesInRed;
                showPositiveValuesInGreen = valueToForegroundParameter.ShowPositiveValuesInGreen;
                inputParameter = valueToForegroundParameter.Parameter;
            }
            var doubleValue = 0.0;
            if (value != null) 
                double.TryParse(value.ToString().NumTextToString(culture), out doubleValue);

            if (doubleValue < 0 && showNegativeValuesInRed)
                return new SolidColorBrush(Colors.Red);
            else if (doubleValue > 0 && showPositiveValuesInGreen)
                return new SolidColorBrush(Colors.Green);


            return inputParameter ?? DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ValueToForegroundColorConverterGreen : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var doubleValue = 0.0;
            if (value != null)
                double.TryParse(value.ToString().NumTextToString(culture), out doubleValue);

            if (doubleValue > 0)
                return new SolidColorBrush(Colors.Green);

            return parameter ?? DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
