// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="ExtensionMethods.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;
using System.Linq;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Class ExtensionMethods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Determines whether a string is null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is null or empty] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Gets the left string value starting at length.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string LeftStr(this string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            if (string.IsNullOrEmpty(param))
                return "";

            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }
        /// <summary>
        /// Gets the right string value starting at length.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string RightStr(this string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            if (string.IsNullOrEmpty(param))
                return "";

            int nStart = param.Length - length;
            string result = param.Substring(nStart, length);
            //return the result of the operation
            return result;
        }

        /// <summary>
        /// Gets the left string value starting at index and ending at length.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string MidStr(this string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            if (string.IsNullOrEmpty(param))
                return "";

            string result = param.Substring(startIndex, length);
            //return the result of the operation
            return result;
        }

        /// <summary>
        /// Converts to bool.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ToBool(this string value)
        {
            value = value.ToUpper();
            if (value == "TRUE")
                return true;
            if (value == "FALSE")
                return false;
            if (value.Trim().Length == 0)
                return false;
            int intVal;
            int.TryParse(value, out intVal);
            return (intVal != 0);
        }

        /// <summary>
        /// Gets the right text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="selectionStart">The selection start.</param>
        /// <param name="selectionLength">Length of the selection.</param>
        /// <returns>System.String.</returns>
        public static string GetRightText(this string text, int selectionStart, int selectionLength)
        {
            if (text.IsNullOrEmpty())
                return string.Empty;

            return text.RightStr(text.Length - (selectionLength + selectionStart));
        }

        /// <summary>
        /// Removes all currency, percent, and thousands separator text from the text.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="culture">The culture that contains the format characters.</param>
        /// <returns>Text without numeric symbols.</returns>
        public static string NumTextToString(this string text, CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            var stripText = culture.NumberFormat.CurrencyGroupSeparator;
            stripText += culture.NumberFormat.NumberGroupSeparator;
            stripText += culture.NumberFormat.PercentSymbol;
            stripText += culture.NumberFormat.CurrencySymbol;
            return StripText(text, stripText);
        }

        /// <summary>
        /// Strips the text of all the characters in the stripString.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="stripString">The characters to strip.</param>
        /// <returns>The text without the characters in stripString.</returns>
        public static string StripText(this string text, string stripString)
        {
            if (text.IsNullOrEmpty())
                return text;

            string returnString = text;
            foreach (char cChar in stripString)
                returnString = returnString.Replace(cChar.ToString(), "");

            return returnString;
        }

        /// <summary>
        /// Counts the text for chars.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns>System.Int32.</returns>
        public static int CountTextForChars(this string text, string searchString)
        {
            var result = 0;
            if (text.IsNullOrEmpty())
                return result;

            if (searchString.IsNullOrEmpty())
                return result;

            foreach (var cChar in searchString)
            {
                result += text.Count(c => c.Equals(cChar));
            }

            return result;
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System.Double.</returns>
        public static double ToDecimal(this string text, CultureInfo culture = null)
        {
            text.TryParseDecimal(out var result, culture);
            return result;
        }

        /// <summary>
        /// Tries the parse decimal.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="resultDecimal">The result decimal.</param>
        /// <param name="culture">The culture.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryParseDecimal(this string text, out double resultDecimal, CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            var percent = false;
            if (!text.IsNullOrEmpty())
            {
                percent = text.Contains(culture.NumberFormat.PercentSymbol);
                if (percent)
                    text = text.StripText(culture.NumberFormat.PercentSymbol);
            }

            var result = double.TryParse(text,
                NumberStyles.AllowParentheses |
                NumberStyles.AllowLeadingWhite |
                NumberStyles.AllowTrailingWhite |
                NumberStyles.AllowThousands |
                NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowCurrencySymbol |
                NumberStyles.AllowLeadingSign,
                culture, out resultDecimal);

            if (percent)
                resultDecimal /= 100;

            return result;
        }


        /// <summary>
        /// Converts the string representation of a number to an integer.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System.Int32.</returns>
        public static int ToInt(this string text, CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            int.TryParse(text, NumberStyles.AllowParentheses |
                               NumberStyles.AllowLeadingWhite |
                               NumberStyles.AllowTrailingWhite |
                               NumberStyles.AllowThousands |
                               NumberStyles.AllowDecimalPoint |
                               NumberStyles.AllowCurrencySymbol |
                               NumberStyles.AllowLeadingSign, culture.NumberFormat, out var result);

            return result;
        }

        /// <summary>
        /// Tries the parse date time.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="result">The result.</param>
        /// <param name="culture">The culture.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryParseDateTime(this string text, out DateTime result, CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            var parseResult = DateTime.TryParse(text, culture,
                DateTimeStyles.AllowInnerWhite | DateTimeStyles.AllowWhiteSpaces, out result);

            return parseResult;
        }

        /// <summary>
        /// Determines whether the specified object is nullable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the specified object is nullable; otherwise, <c>false</c>.</returns>
        public static bool IsNullable<T>(this T obj)
        {
            if (obj == null)
                return true; // obvious
            Type type = typeof(T);
            if (!type.IsValueType)
                return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null)
                return true; // Nullable<T>
            return false; // value-type
        }
    }
}