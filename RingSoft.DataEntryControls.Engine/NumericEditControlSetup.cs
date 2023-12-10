// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="NumericEditControlSetup.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Enum DataEntryModes
    /// </summary>
    public enum DataEntryModes
    {
        /// <summary>
        /// The format on entry
        /// </summary>
        FormatOnEntry = 0,
        /// <summary>
        /// The validate only
        /// </summary>
        ValidateOnly = 1,
        /// <summary>
        /// The raw entry
        /// </summary>
        RawEntry = 2
    }

    /// <summary>
    /// Class NumericEditControlSetup.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NumericEditControlSetup<T>
    {
        /// <summary>
        /// Gets the number format string.  Default value is empty.
        /// </summary>
        /// <value>The number format string.</value>
        public string NumberFormatString { get; set; }

        /// <summary>
        /// Gets or sets the data entry mode.
        /// </summary>
        /// <value>The data entry mode.</value>
        public DataEntryModes DataEntryMode { get; set; }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public T MaximumValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public T MinimumValue { get; set; }


        /// <summary>
        /// Sets the culture identifier.
        /// </summary>
        /// <value>The culture identifier.</value>
        public string CultureId
        {
            set => SetupNumericInfo(value);
        }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow null value].
        /// </summary>
        /// <value><c>true</c> if [allow null value]; otherwise, <c>false</c>.</value>
        public bool AllowNullValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericEditControlSetup{T}"/> class.
        /// </summary>
        public NumericEditControlSetup()
        {
            if (Culture == null)
                CultureId = CultureInfo.CurrentCulture.Name;
        }

        /// <summary>
        /// Setups the numeric information.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        protected virtual void SetupNumericInfo(string cultureId)
        {
            Culture = new CultureInfo(cultureId);
            DecimalEditControlSetup.FormatCulture(Culture);
        }

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string GetNumberFormatString()
        {
            return NumberFormatString;
        }

        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public string FormatValue(double? value)
        {
            if (value == null)
                return string.Empty;

            var newValue = (double)value;

            return newValue.ToString(GetNumberFormatString(), Culture.NumberFormat);
        }

    }
}
