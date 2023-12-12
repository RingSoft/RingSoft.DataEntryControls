// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="ValueToForegroundColorConverter.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class ValueToForegroundParameter.
    /// </summary>
    public class ValueToForegroundParameter
    {
        /// <summary>
        /// Gets or sets a value indicating whether [show negative values in red].
        /// </summary>
        /// <value><c>true</c> if [show negative values in red]; otherwise, <c>false</c>.</value>
        public bool ShowNegativeValuesInRed { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show positive values in green].
        /// </summary>
        /// <value><c>true</c> if [show positive values in green]; otherwise, <c>false</c>.</value>
        public bool ShowPositiveValuesInGreen { get; set; }
        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>The parameter.</value>
        public object Parameter { get; set; }
    }
    /// <summary>
    /// Gets color based on double value.  Red if negative.  Green if positive.
    /// Implements the <see cref="IValueConverter" />
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class ValueToForegroundConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var showNegativeValuesInRed = false;
            var showPositiveValuesInGreen = false;
            object inputParameter = null;

            if (parameter is ValueToForegroundParameter valueToForegroundParameter)
            {
                showNegativeValuesInRed = valueToForegroundParameter.ShowNegativeValuesInRed;
                showPositiveValuesInGreen = valueToForegroundParameter.ShowPositiveValuesInGreen;
                inputParameter = valueToForegroundParameter.Parameter;
            }
            var doubleValue = 0.0;
            if (value != null) 
                double.TryParse(value.ToString().NumTextToString(culture), out doubleValue);

            if (doubleValue < 0 && showNegativeValuesInRed)
                return new SolidColorBrush(Colors.Red);
            else if (doubleValue > 0 && showPositiveValuesInGreen)
                return new SolidColorBrush(Colors.Green);


            return inputParameter ?? DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Converts positive values to green color.
    /// Implements the <see cref="IValueConverter" />
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class ValueToForegroundColorConverterGreen : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var doubleValue = 0.0;
            if (value != null)
                double.TryParse(value.ToString().NumTextToString(culture), out doubleValue);

            if (doubleValue > 0)
                return new SolidColorBrush(Colors.Green);

            return parameter ?? DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
