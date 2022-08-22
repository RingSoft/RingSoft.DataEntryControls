using System;
using System.Globalization;
using System.Linq;

namespace RingSoft.DataEntryControls.Engine
{
    public static class ExtensionMethods
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

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

        public static decimal ToDecimal(this string text, CultureInfo culture = null)
        {
            text.TryParseDecimal(out var result, culture);
            return result;
        }

        public static bool TryParseDecimal(this string text, out decimal resultDecimal, CultureInfo culture = null)
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

            var result = decimal.TryParse(text,
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

        public static double ToDouble(this decimal value)
        {
            return value.ToDouble();
        }

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

        public static bool TryParseDateTime(this string text, out DateTime result, CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            var parseResult = DateTime.TryParse(text, culture,
                DateTimeStyles.AllowInnerWhite | DateTimeStyles.AllowWhiteSpaces, out result);

            return parseResult;
        }

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