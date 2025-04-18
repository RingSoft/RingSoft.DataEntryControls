﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridCustomControlCellProps.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    /// <summary>
    /// Creates a custom control in the data entry grid cell.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridComboBoxCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridComboBoxCellProps" />
    public class DataEntryGridCustomControlCellProps : DataEntryGridComboBoxCellProps
    {
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => ContentControlHostId;

        /// <summary>
        /// Gets or sets the selected item identifier.  Used to determine the image identifier.
        /// </summary>
        /// <value>The selected item identifier.</value>
        public int SelectedItemId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridCustomControlCellProps" /> class and creates a custom control in the data entry grid cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="selectedItemId">The selected item identifier.</param>
        /// <param name="changeType">Type of the change.</param>
        public DataEntryGridCustomControlCellProps(DataEntryGridRow row, int columnId, int selectedItemId, ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(row, columnId, changeType)
        {
            SelectedItemId = selectedItemId;
        }

        /// <summary>
        /// Gets the control properties.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            var dataValue = new DataEntryGridDataValue();
            return dataValue.CreateDataValue(row, columnId, SelectedItemId.ToString());
        }
    }
}
