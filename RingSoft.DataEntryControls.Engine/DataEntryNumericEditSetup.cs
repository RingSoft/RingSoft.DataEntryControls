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
        public int DecimalCount { get; set; } = 2;

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

        public int MaxDigits { get; set; } = 18;

        public decimal MaxValue { get; set; }

        public void InitializeFromType(Type type)
        {
            if (type == typeof(decimal)
                     || type == typeof(decimal?)
                     || type == typeof(double)
                     || type == typeof(double?)
                     || type == typeof(float)
                     || type == typeof(float?))
            {
                DecimalCount = 2;
                MaxDigits = 18;
                MaxValue = decimal.MaxValue;
            }
            else if (type == typeof(int)
                     || type == typeof(int?))
            {
                DecimalCount = 0;
                MaxDigits = int.MaxValue.ToString().Length;
                MaxValue = int.MaxValue;
            }
            else if(type == typeof(long)
                     || type == typeof(long?))

            {
                DecimalCount = 0;
                MaxDigits = long.MaxValue.ToString().Length;
                MaxValue = long.MaxValue;
            }
            else if (type == typeof(byte)
                     || type == typeof(byte?))
            {
                DecimalCount = 0;
                MaxDigits = 3;
                MaxValue = byte.MaxValue;
            }
            else if (type == typeof(short)
                     || type == typeof(short?))
            {
                DecimalCount = 0;
                MaxDigits = short.MaxValue.ToString().Length;
                MaxValue = short.MaxValue;
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
                        result = $"N{DecimalCount}";
                        break;
                    case NumericTypes.Currency:
                        result = $"C{DecimalCount}";
                        break;
                    case NumericTypes.Percent:
                        result = $"P{DecimalCount}";
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