using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public enum DataEntryModes
    {
        FormatOnEntry = 0,
        ValidateOnly = 1,
        RawEntry = 2
    }

    public class NumericEditControlSetup<T>
    {
        /// <summary>
        /// Gets the number format string.  Default value is empty.
        /// </summary>
        /// <value>
        /// The number format string.
        /// </value>
        public string NumberFormatString { get; set; }

        public DataEntryModes DataEntryMode { get; set; }

        public T MaximumValue { get; set; }

        public T MinimumValue { get; set; }


        public string CultureId
        {
            set => SetupNumericInfo(value);
        }

        public CultureInfo Culture { get; private set; }

        public bool AllowNullValue { get; set; }

        public NumericEditControlSetup()
        {
            if (Culture == null)
                CultureId = CultureInfo.CurrentCulture.Name;
        }

        protected virtual void SetupNumericInfo(string cultureId)
        {
            Culture = new CultureInfo(cultureId);
            Culture.NumberFormat.CurrencyNegativePattern = 1;
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
