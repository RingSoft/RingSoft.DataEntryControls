// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridButtonCellProps.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Creates a button in the data entry grid cell.
    /// </summary>
    public class DataEntryGridButtonCellProps : DataEntryGridTextCellProps
    {
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => DataEntryGridEditingCellProps.ButtonHostId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridButtonCellProps" /> class and Creates a button in the data entry grid cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        public DataEntryGridButtonCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }

        /// <summary>
        /// Gets the data value that's displayed in the grid cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            if (!controlMode)
                return Text;

            var cellStyle = row.GetCellStyle(columnId) as DataEntryGridButtonCellStyle;
            if (cellStyle == null)
                throw new Exception(GetCellStyleExceptionMessage(row, columnId));

            var dataValue = new DataEntryGridDataValue();
            dataValue.CreateDataValue(cellStyle, cellStyle.Content);
            return dataValue.DataValue;
        }

        /// <summary>
        /// Gets the cell style exception message.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>System.String.</returns>
        public static string GetCellStyleExceptionMessage(DataEntryGridRow row, int columnId)
        {
            return
                $"Row '{row}' ColumnId '{columnId}' GetCellStyle must return a {nameof(DataEntryGridButtonCellStyle)} object.";
        }
    }
}
