// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridDataValue.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Data entry grid display properties and text.
    /// </summary>
    public class DataEntryGridDataValue
    {
        /// <summary>
        /// Gets the check data value.
        /// </summary>
        /// <value>The check data value.</value>
        public static string CheckDataValue
        {
            get
            {
                var dataValue = new DataEntryGridDataValue();
                return dataValue.CreateDataValue(new DataEntryGridControlCellStyle(), "");
            }

        }
        /// <summary>
        /// Gets a value indicating whether this instance is visible.
        /// </summary>
        /// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled { get; private set; }
        /// <summary>
        /// Gets the display style identifier.
        /// </summary>
        /// <value>The display style identifier.</value>
        public int DisplayStyleId { get; private set; }
        /// <summary>
        /// Gets the control value.
        /// </summary>
        /// <value>The control value.</value>
        public string ControlValue { get; private set; }
        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <value>The data value.</value>
        public string DataValue { get; private set; }

        /// <summary>
        /// Processes the data value input.
        /// </summary>
        /// <param name="dataValue">The data value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ProcessDataValueInput(string dataValue)
        {
            if (dataValue.IsNullOrEmpty())
                return true;

            if (dataValue.Length < CheckDataValue.Length)
                return false;

            DataValue = dataValue;
            IsVisible = dataValue[0].ToString().ToBool();
            IsEnabled = dataValue[1].ToString().ToBool();
            var semiIndex = dataValue.IndexOf(';');

            var displayStyleStr = dataValue.MidStr(2, semiIndex - 2);
            DisplayStyleId = displayStyleStr.ToInt();

            if (dataValue.Length > semiIndex)
                ControlValue = dataValue.RightStr(dataValue.Length - (semiIndex + 1));

            return true;
        }

        /// <summary>
        /// Creates the data value.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlValue">The control value.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentException">'{row}' : Column Id '{columnId}' GetCellStyle must return a cell style that derives from {nameof(DataEntryGridControlCellStyle)}.</exception>
        public string CreateDataValue(DataEntryGridRow row, int columnId, string controlValue)
        {
            var controlCellStyle = row.GetCellStyle(columnId) as DataEntryGridControlCellStyle;
            if (controlCellStyle == null)
                throw new ArgumentException($"'{row}' : Column Id '{columnId}' GetCellStyle must return a cell style that derives from {nameof(DataEntryGridControlCellStyle)}.");

            return CreateDataValue(controlCellStyle, controlValue);
        }

        /// <summary>
        /// Creates the data value.
        /// </summary>
        /// <param name="controlCellStyle">The control cell style.</param>
        /// <param name="controlValue">The control value.</param>
        /// <returns>System.String.</returns>
        public string CreateDataValue(DataEntryGridControlCellStyle controlCellStyle, string controlValue)
        {
            IsVisible = controlCellStyle.IsVisible;
            IsEnabled = controlCellStyle.IsEnabled;
            DisplayStyleId = controlCellStyle.DisplayStyleId;
            ControlValue = controlValue;

            DataValue = $"{(IsVisible ? "1" : "0")}{(IsEnabled ? "1" : "0")}{DisplayStyleId};{ControlValue}";
            return DataValue;
        }
    }
}
