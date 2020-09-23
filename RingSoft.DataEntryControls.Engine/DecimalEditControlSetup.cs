using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public enum DecimalEditFormatTypes
    {
        Number = 0,
        Currency = 1,
        Percent = 2
    }

    public enum NumberSymbolLocations
    {
        Prefix = 0,
        Suffix = 1
    }

    public class DecimalEditControlSetup : NumericEditControlSetup<decimal?>
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
        public DecimalEditFormatTypes EditFormatType { get; set; }

        public NumberSymbolLocations CurrencySymbolLocation { get; private set; }

        public string CurrencyText { get; private set; }

        public NumberSymbolLocations PercentSymbolLocation { get; private set; }

        public string PercentText { get; private set; }

        public DecimalEditControlSetup()
        {
            SetupNumericInfo(CultureInfo.CurrentCulture.Name);
        }

        protected sealed override void SetupNumericInfo(string cultureId)
        {
            base.SetupNumericInfo(cultureId);

            var value = 0;
            var text = value.ToString("C0", Culture.NumberFormat);

            var symbolLocation = text.IndexOf(Culture.NumberFormat.CurrencySymbol, StringComparison.Ordinal);
            var digitLocation = text.IndexOf("0", StringComparison.Ordinal);

            if (symbolLocation == 0)
            {
                CurrencySymbolLocation = NumberSymbolLocations.Prefix;
                CurrencyText = text.LeftStr(digitLocation);
            }
            else
            {
                CurrencySymbolLocation = NumberSymbolLocations.Suffix;
                CurrencyText = text.GetRightText(digitLocation, 1);
            }

            text = value.ToString("P0", Culture.NumberFormat);
            symbolLocation = text.IndexOf(Culture.NumberFormat.PercentSymbol, StringComparison.Ordinal);
            digitLocation = text.IndexOf("0", StringComparison.Ordinal);

            if (symbolLocation == 0)
            {
                PercentSymbolLocation = NumberSymbolLocations.Prefix;
                PercentText = text.LeftStr(digitLocation);
            }
            else
            {
                PercentSymbolLocation = NumberSymbolLocations.Suffix;
                PercentText = text.GetRightText(digitLocation, 1);
            }
        }

        public override string GetNumberFormatString()
        {
            return GetDecimalFormatString(EditFormatType, Precision, base.GetNumberFormatString());
        }

        public static string GetDecimalFormatString(DecimalEditFormatTypes formatType, int precision,
            string customFormatString)
        {
            var result = customFormatString;
            if (result.IsNullOrEmpty())
            {
                switch (formatType)
                {
                    case DecimalEditFormatTypes.Number:
                        result = $"N{precision}";
                        break;
                    case DecimalEditFormatTypes.Currency:
                        result = $"C{precision}";
                        break;
                    case DecimalEditFormatTypes.Percent:
                        result = $"P{precision}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }
    }
}