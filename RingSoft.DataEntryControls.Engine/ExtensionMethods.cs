using System;

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
    }
}
