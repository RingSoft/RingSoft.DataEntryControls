// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DecimalEditControlSetup.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Enum DecimalEditFormatTypes
    /// </summary>
    public enum DecimalEditFormatTypes
    {
        /// <summary>
        /// The number
        /// </summary>
        Number = 0,
        /// <summary>
        /// The currency
        /// </summary>
        Currency = 1,
        /// <summary>
        /// The percent
        /// </summary>
        Percent = 2
    }

    /// <summary>
    /// Enum NumberSymbolLocations
    /// </summary>
    public enum NumberSymbolLocations
    {
        /// <summary>
        /// The prefix
        /// </summary>
        Prefix = 0,
        /// <summary>
        /// The suffix
        /// </summary>
        Suffix = 1
    }

    /// <summary>
    /// All the properties necessary to set up a DecimalEditControl.
    /// </summary>
    public class DecimalEditControlSetup : NumericEditControlSetup<double?>
    {
        /// <summary>
        /// Gets or sets the number of digits to the right of the double point.
        /// </summary>
        /// <value>The double count.</value>
        public int Precision { get; set; } = 2;

        /// <summary>
        /// Gets or sets the format type of the decimal edit control.
        /// </summary>
        /// <value>The format type of the numeric edit control.</value>
        public DecimalEditFormatTypes FormatType { get; set; }

        /// <summary>
        /// Gets the currency symbol location.
        /// </summary>
        /// <value>The currency symbol location.</value>
        public NumberSymbolLocations CurrencySymbolLocation { get; private set; }

        /// <summary>
        /// Gets the currency text.
        /// </summary>
        /// <value>The currency text.</value>
        public string CurrencyText { get; private set; }

        /// <summary>
        /// Gets the percent symbol location.
        /// </summary>
        /// <value>The percent symbol location.</value>
        public NumberSymbolLocations PercentSymbolLocation { get; private set; }

        /// <summary>
        /// Gets the percent text.
        /// </summary>
        /// <value>The percent text.</value>
        public string PercentText { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalEditControlSetup" /> class.
        /// </summary>
        public DecimalEditControlSetup()
        {
            SetupNumericInfo(CultureInfo.CurrentCulture.Name);
        }

        /// <summary>
        /// Formats the culture.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public static void FormatCulture(CultureInfo culture)
        {
            culture.NumberFormat.CurrencyNegativePattern =
                culture.NumberFormat.NumberNegativePattern =
                    culture.NumberFormat.PercentNegativePattern = 1;

        }


        /// <summary>
        /// Setups the numeric information.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
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

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetNumberFormatString()
        {
            return GetDecimalFormatString(FormatType, Precision, base.GetNumberFormatString());
        }

        /// <summary>
        /// Gets the decimal format string.
        /// </summary>
        /// <param name="formatType">Type of the format.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="customFormatString">The custom format string.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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