using System;

namespace RingSoft.DataEntryControls.Engine
{
    public enum NumericTypes
    {
        Number = 0,
        Currency = 1,
        Percent = 2
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
        /// Gets or sets the type of the number.
        /// </summary>
        /// <value>
        /// The type of the number.
        /// </value>
        public NumericTypes NumericType { get; set; }

        /// <summary>
        /// Gets the number format string.  Default value is empty.
        /// </summary>
        /// <value>
        /// The number format string.
        /// </value>
        public string NumberFormatString { get; set; }

        public decimal MaximumValue { get; set; }

        public decimal MinimumValue { get; set; }

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
                switch (NumericType)
                {
                    case NumericTypes.Number:
                        result = $"N{Precision}";
                        break;
                    case NumericTypes.Currency:
                        result = $"C{Precision}";
                        break;
                    case NumericTypes.Percent:
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
            return value.ToString(GetNumberFormatString());
        }
    }
}