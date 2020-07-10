﻿using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public enum DataEntryModes
    {
        FormatOnEntry = 0,
        ValidateOnly = 1,
        RawEntry = 2
    }

    public class NumericEditControlSetup
    {
        /// <summary>
        /// Gets the number format string.  Default value is empty.
        /// </summary>
        /// <value>
        /// The number format string.
        /// </value>
        public string NumberFormatString { get; set; }

        public DataEntryModes DataEntryMode { get; set; }

        public string CultureId
        {
            set => SetupNumericInfo(value);
        }

        public CultureInfo Culture { get; private set; }

        protected virtual void SetupNumericInfo(string cultureId)
        {
            Culture = new CultureInfo(cultureId);
        }

        public virtual string GetNumberFormatString()
        {
            return NumberFormatString;
        }

        public string FormatValue(decimal? value)
        {
            if (value == null)
                return string.Empty;

            var newValue = (decimal)value;

            return newValue.ToString(GetNumberFormatString(), Culture.NumberFormat);
        }

    }
}
