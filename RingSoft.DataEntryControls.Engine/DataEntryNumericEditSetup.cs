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