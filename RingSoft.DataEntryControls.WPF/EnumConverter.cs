using System;
using System.Windows.Data;
// ReSharper disable ConstantNullCoalescingCondition

namespace RingSoft.DataEntryControls.WPF
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            Enum enumValue = default(Enum);
            if (parameter is Type)
            {
                if (value != null) enumValue = (Enum) Enum.Parse((Type) parameter, value.ToString() ?? string.Empty);
            }
            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            int returnValue = 0;
            if (parameter is Type)
            {
                if (value != null) returnValue = (int) Enum.Parse((Type) parameter, value.ToString() ?? string.Empty);
            }
            return returnValue;
        }
    }
}
