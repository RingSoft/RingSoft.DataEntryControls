// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridControlValue.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Class DataEntryGridControlValue.
    /// </summary>
    public class DataEntryGridControlValue
    {
        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is visible.
        /// </summary>
        /// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible { get; }

        /// <summary>
        /// Gets the display style identifier.
        /// </summary>
        /// <value>The display style identifier.</value>
        public int DisplayStyleId { get; }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <value>The cell value.</value>
        public string CellValue { get; }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <value>The data value.</value>
        public string DataValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridControlValue"/> class.
        /// </summary>
        /// <param name="cellStyle">The cell style.</param>
        /// <param name="cellValue">The cell value.</param>
        public DataEntryGridControlValue(DataEntryGridControlCellStyle cellStyle, string cellValue)
        {
            IsEnabled = cellStyle.IsEnabled;
            IsVisible = cellStyle.IsVisible;
            DisplayStyleId = cellStyle.DisplayStyleId;
            CellValue = cellValue;

            var visibleBit = IsVisible ? "1" : "0";
            var enabledBit = IsEnabled ? "1" : "0";
            DataValue = $"{visibleBit}{enabledBit}{DisplayStyleId};{CellValue}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridControlValue"/> class.
        /// </summary>
        /// <param name="dataValue">The data value.</param>
        public DataEntryGridControlValue(string dataValue)
        {
            DataValue = dataValue;
            var semiIndex = dataValue.IndexOf(';');
            //if (dataValue.Length < 5 || semiIndex < 0)
            //    throw new Exception("Unable to parse DataValue.  Invalid format string.");

            IsVisible = dataValue[0].ToString().ToBool();
            IsEnabled = dataValue[1].ToString().ToBool();
            var displayStyleText = dataValue.MidStr(2, semiIndex - 2);
            DisplayStyleId = displayStyleText.ToInt();
            var cellValueIndex = dataValue.Length - (semiIndex + 1);
            CellValue = dataValue.RightStr(cellValueIndex);
        }
    }
}
