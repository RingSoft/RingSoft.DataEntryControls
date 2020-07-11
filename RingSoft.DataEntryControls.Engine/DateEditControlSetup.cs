using System;

namespace RingSoft.DataEntryControls.Engine
{
    public enum DateFormatTypes
    {
        DateOnly = 0,
        DateTime = 1,
    }
    public class DateEditControlSetup
    {
        public string EntryFormat { get; set; }

        public string DisplayFormat { get; set; }

        public DateFormatTypes DateFormatType { get; set; }

        public DateEditControlSetup()
        {
            DateFormatType = DateFormatTypes.DateOnly;
        }

        public string GetEntryFormat()
        {
            var result = EntryFormat;
            if (result.IsNullOrEmpty())
                result = GetDefaultFormatForType(DateFormatType);

            return result;
        }

        public string GetDisplayFormat()
        {
            var result = DisplayFormat;
            if (result.IsNullOrEmpty())
                result = GetDefaultFormatForType(DateFormatType);

            return result;
        }

        public static void ValidateEntryFormat(string dateFormatString)
        {
            ValidateDateFormat(dateFormatString);
        }

        public static void ValidateDateFormat(string dateFormatString)
        {
            var date = new DateTime(2000, 01, 01);
            var format = $"{{0:{dateFormatString}}}";
            try
            {
                var dateCheckFormat = string.Format(format, date);
                var unused = DateTime.Parse(dateCheckFormat);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid date format string.");
            }
        }

        private string GetDefaultFormatForType(DateFormatTypes formatType)
        {
            switch (formatType)
            {
                case DateFormatTypes.DateOnly:
                    return "MM/dd/yyyy";
                case DateFormatTypes.DateTime:
                    return "MM/dd/yyyy hh:mm:ss tt";
                default:
                    throw new ArgumentOutOfRangeException(nameof(formatType), formatType, null);
            }
        }
    }
}
