using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public enum DataEntryModes
    {
        FormatOnEntry = 0,
        ValidateOnly = 1,
        RawEntry = 2
    }


    public enum NumericEditFormatTypes
    {
        Number = 0,
        Currency = 1,
        Percent = 2
    }

    public enum CurrencySymbolLocations
    {
        Prefix = 0,
        Suffix = 1
    }

    public class DataEntryNumericEditSetup
    {
        /// <summary>
        /// Gets or sets the number of digits to the right of the decimal point.
        /// </summary>
        /// <value>
        /// The decimal count.
        /// </value>
        public int Precision { get; set; } = 2;

        /// <summary>
        /// Gets or sets the format type of the numeric edit control.
        /// </summary>
        /// <value>
        /// The format type of the numeric edit control.
        /// </value>
        public NumericEditFormatTypes EditFormatType { get; set; }

        /// <summary>
        /// Gets the number format string.  Default value is empty.
        /// </summary>
        /// <value>
        /// The number format string.
        /// </value>
        public string NumberFormatString { get; set; }

        public decimal? MaximumValue { get; set; }

        public decimal? MinimumValue { get; set; }

        public DataEntryModes DataEntryMode { get; set; }

        public CurrencySymbolLocations CurrencySymbolLocation { get; set; }

        public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

        public void InitializeFromType(Type type)
        {
            if (type == typeof(decimal)
                || type == typeof(decimal?)
                || type == typeof(double)
                || type == typeof(double?)
                || type == typeof(float)
                || type == typeof(float?))
            {
                Precision = 2;
                MaximumValue = decimal.MaxValue;
                MinimumValue = decimal.MinValue;
            }
            else if (type == typeof(int)
                     || type == typeof(int?))
            {
                Precision = 0;
                MaximumValue = int.MaxValue;
                MinimumValue = int.MinValue;
            }
            else if (type == typeof(long)
                     || type == typeof(long?))

            {
                Precision = 0;
                MaximumValue = long.MaxValue;
                MinimumValue = long.MinValue;
            }
            else if (type == typeof(byte)
                     || type == typeof(byte?))
            {
                Precision = 0;
                MaximumValue = byte.MaxValue;
                MinimumValue = byte.MinValue;
            }
            else if (type == typeof(short)
                     || type == typeof(short?))
            {
                Precision = 0;
                MaximumValue = short.MaxValue;
                MinimumValue = short.MinValue;
            }
        }

        public string GetNumberFormatString()
        {
            var result = NumberFormatString;
            if (result.IsNullOrEmpty())
            {
                switch (EditFormatType)
                {
                    case NumericEditFormatTypes.Number:
                        result = $"N{Precision}";
                        break;
                    case NumericEditFormatTypes.Currency:
                        result = $"C{Precision}";
                        break;
                    case NumericEditFormatTypes.Percent:
                        result = $"P{Precision}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }

        public string FormatValue(decimal value)
        {
            return value.ToString(GetNumberFormatString(), Culture.NumberFormat);
        }
    }
}