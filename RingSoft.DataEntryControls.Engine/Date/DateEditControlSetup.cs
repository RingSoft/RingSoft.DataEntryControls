using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
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

        public DateTime? MaximumDate { get; set; }

        public DateTime? MinimumDate { get; set; }

        public CultureInfo Culture { get; private set; } = CultureInfo.CurrentCulture;

        public string CultureId
        {
            get => Culture.Name;
            set
            {
                if (value.IsNullOrEmpty())
                    Culture = CultureInfo.CurrentCulture;
                else
                    Culture = new CultureInfo(value);
            }
        }

        public bool AllowNullValue { get; set; }

        public DateEditControlSetup()
        {
            DateFormatType = DateFormatTypes.DateOnly;
            if (CultureId.IsNullOrEmpty())
                CultureId = CultureInfo.CurrentCulture.Name;
        }

        public string GetEntryFormat()
        {
            var result = EntryFormat;
            if (result.IsNullOrEmpty())
                result = GetDefaultFormatForType(DateFormatType);

            result = ScrubDateFormatForCulture(result, Culture);
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

            if (dateFormatString == "D" || dateFormatString == "f"
                                             || dateFormatString == "F"
                                             || dateFormatString == "M"
                                             || dateFormatString == "m"
                                             || dateFormatString == "o"
                                             || dateFormatString == "O"
                                             || dateFormatString == "r"
                                             || dateFormatString == "R"
                                             || dateFormatString == "s"
                                             || dateFormatString == "u"
                                             || dateFormatString == "U"
                                             || dateFormatString == "y"
                                             || dateFormatString == "Y")
                throw new Exception($"Entry DateTime format '{dateFormatString}' is not supported.  Entry formats 'd', 'g', 'G', 't', or 'T' are supported.");

            if (!dateFormatString.IsNullOrEmpty() && dateFormatString.Length > 1)
            {
                dateFormatString = ScrubDateFormat(dateFormatString);
                ValidateDateFormat(dateFormatString);
            }
        }

        public static string ScrubDateFormatForCulture(string dateFormatString, CultureInfo culture)
        {
            if (dateFormatString.IsNullOrEmpty())
                dateFormatString = string.Empty;

            dateFormatString = dateFormatString.Trim();
            if (dateFormatString == "d")
                dateFormatString = culture.DateTimeFormat.ShortDatePattern;
            else if (dateFormatString == "g")
                dateFormatString = culture.DateTimeFormat.ShortDatePattern + ' ' + culture.DateTimeFormat.ShortTimePattern;
            else if (dateFormatString == "G")
                dateFormatString = culture.DateTimeFormat.ShortDatePattern + ' ' + culture.DateTimeFormat.LongTimePattern;
            else if (dateFormatString == "t")
                dateFormatString = culture.DateTimeFormat.ShortTimePattern;
            else if (dateFormatString == "T")
                dateFormatString = culture.DateTimeFormat.LongTimePattern;

            return ScrubDateFormat(dateFormatString);
        }

        private static string ScrubDateFormat(string dateFormatString)
        {
            var result = dateFormatString;

            result = ScrubFormatSegment(result, "MM");
            result = ScrubFormatSegment(result, "dd");
            result = ScrubFormatSegment(result, "yyyy");

            result = ScrubFormatSegment(result, "hh");
            result = ScrubFormatSegment(result, "HH");
            result = ScrubFormatSegment(result, "mm");
            result = ScrubFormatSegment(result, "ss");
            result = ScrubFormatSegment(result, "tt");

            return result;
        }

        private static string ScrubFormatSegment(string format, string formatSegment)
        {
            GetSegmentFirstLastPosition(format, formatSegment[0], out var firstSegIndex, out var lastSegIndex);

            if (firstSegIndex < 0)
                //Format segment doesn't exist in Format--we're done.
                return format;

            if ((lastSegIndex - firstSegIndex) + 1 == formatSegment.Length)
                //Format's segment is the right length--we're done.
                return format;

            //Format has too many or too few chars in segment.  Cut out the old segment and replace it with szFormatSegment.
            format = format.LeftStr(firstSegIndex)
                       + formatSegment
                       + format.RightStr((format.Length - lastSegIndex) - 1);

            return format;
        }
        public static void GetSegmentFirstLastPosition(string format, char segmentChar, out int firstSegmentIndex, out int lastSegmentIndex)
        {
            firstSegmentIndex = format.IndexOf(segmentChar);
            lastSegmentIndex = format.LastIndexOf(segmentChar, format.Length - 1);
        }

        public static void ValidateDateFormat(string dateFormatString)
        {
            var date = new DateTime(2000, 01, 01);
            //var format = $"{{0:{dateFormatString}}}";
            try
            {
                var dateCheckFormat = date.ToString(dateFormatString);
                var unused = DateTime.Parse(dateCheckFormat);
            }
            catch (Exception)
            {
                throw new Exception("Invalid date format string.");
            }
        }

        private string GetDefaultFormatForType(DateFormatTypes formatType)
        {
            switch (formatType)
            {
                case DateFormatTypes.DateOnly:
                    return "d";
                case DateFormatTypes.DateTime:
                    return "G";
                default:
                    throw new ArgumentOutOfRangeException(nameof(formatType), formatType, null);
            }
        }

        public string FormatValueForDisplay(DateTime? value)
        {
            var displayFormat = GetDisplayFormat();
            ValidateDateFormat(displayFormat);

            if (value == null)
                return string.Empty;

            var formatValue = (DateTime) value;
            return formatValue.ToString(displayFormat, Culture);
        }
    }
}
