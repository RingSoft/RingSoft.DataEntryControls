// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DateEditControlSetup.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Enum DateFormatTypes
    /// </summary>
    public enum DateFormatTypes
    {
        /// <summary>
        /// The date only
        /// </summary>
        DateOnly = 0,
        /// <summary>
        /// The date time
        /// </summary>
        DateTime = 1,
    }
    /// <summary>
    /// All the properties necessary to setup a date edit control.
    /// </summary>
    public class DateEditControlSetup
    {
        /// <summary>
        /// Gets or sets the entry format.  Use to override DateFormatType.
        /// </summary>
        /// <value>The entry format.</value>
        public string EntryFormat { get; set; }

        /// <summary>
        /// Gets or sets the display format.  Use to override DateFormatType.
        /// </summary>
        /// <value>The display format.</value>
        public string DisplayFormat { get; set; }

        /// <summary>
        /// Gets or sets the type of the default date format.
        /// </summary>
        /// <value>The type of the date format.</value>
        public DateFormatTypes DateFormatType { get; set; }

        /// <summary>
        /// Gets or sets the maximum date.
        /// </summary>
        /// <value>The maximum date.</value>
        public DateTime? MaximumDate { get; set; }

        /// <summary>
        /// Gets or sets the minimum date.
        /// </summary>
        /// <value>The minimum date.</value>
        public DateTime? MinimumDate { get; set; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; private set; } = CultureInfo.CurrentCulture;

        /// <summary>
        /// Gets or sets the culture ID.
        /// </summary>
        /// <value>The culture ID.</value>
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

        /// <summary>
        /// Gets or sets a value indicating whether null values are allowed.
        /// </summary>
        /// <value><c>true</c> if [allow null value]; otherwise, <c>false</c>.</value>
        public bool AllowNullValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateEditControlSetup" /> class.
        /// </summary>
        public DateEditControlSetup()
        {
            DateFormatType = DateFormatTypes.DateOnly;
            if (CultureId.IsNullOrEmpty())
                CultureId = CultureInfo.CurrentCulture.Name;
        }

        /// <summary>
        /// Gets the entry format.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetEntryFormat()
        {
            var result = EntryFormat;
            if (result.IsNullOrEmpty())
                result = GetDefaultFormatForType(DateFormatType);

            result = ScrubDateFormatForCulture(result, Culture);
            return result;
        }

        /// <summary>
        /// Gets the display format.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetDisplayFormat()
        {
            var result = DisplayFormat;
            if (result.IsNullOrEmpty())
                result = GetDefaultFormatForType(DateFormatType);

            return result;
        }

        /// <summary>
        /// Validates the entry format.
        /// </summary>
        /// <param name="dateFormatString">The date format string.</param>
        /// <exception cref="System.Exception">Entry DateTime format '{dateFormatString}' is not supported.  Entry formats 'd', 'g', 'G', 't', or 'T' are supported.</exception>
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

        /// <summary>
        /// Scrubs the date format for culture.
        /// </summary>
        /// <param name="dateFormatString">The date format string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Scrubs the date format.
        /// </summary>
        /// <param name="dateFormatString">The date format string.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Scrubs the format segment.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatSegment">The format segment.</param>
        /// <returns>System.String.</returns>
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
        /// <summary>
        /// Gets the segment first last position.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="segmentChar">The segment character.</param>
        /// <param name="firstSegmentIndex">First index of the segment.</param>
        /// <param name="lastSegmentIndex">Last index of the segment.</param>
        public static void GetSegmentFirstLastPosition(string format, char segmentChar, out int firstSegmentIndex, out int lastSegmentIndex)
        {
            firstSegmentIndex = format.IndexOf(segmentChar);
            lastSegmentIndex = format.LastIndexOf(segmentChar, format.Length - 1);
        }

        /// <summary>
        /// Validates the date format.
        /// </summary>
        /// <param name="dateFormatString">The date format string.</param>
        /// <exception cref="System.Exception">Invalid date format string.</exception>
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

        /// <summary>
        /// Gets the default type of the format for.
        /// </summary>
        /// <param name="formatType">Type of the format.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">formatType - null</exception>
        public static string GetDefaultFormatForType(DateFormatTypes formatType)
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

        /// <summary>
        /// Formats the value for display.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
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
