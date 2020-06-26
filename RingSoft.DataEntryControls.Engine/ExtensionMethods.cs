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
        /// <returns>Text without numeric symbols.</returns>
        public static string NumTextToString(this string text)
        {
            var stripText = NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator;
            stripText += NumberFormatInfo.CurrentInfo.CurrencySymbol;
            stripText += NumberFormatInfo.CurrentInfo.PercentSymbol;
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

        public static int CountTextForNumSymbols(this string text)
        {
            var searchString = NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator;
            searchString += NumberFormatInfo.CurrentInfo.CurrencySymbol;
            searchString += NumberFormatInfo.CurrentInfo.PercentSymbol;

            return text.CountTextForChars(searchString);
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
    }
}
