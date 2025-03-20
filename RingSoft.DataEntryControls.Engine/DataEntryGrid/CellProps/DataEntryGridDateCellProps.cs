// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridDateCellProps.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Creates a date edit control in the data entry grid cell.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    public class DataEntryGridDateCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => DataEntryGridEditingCellProps.DateEditHostId;

        /// <summary>
        /// Gets the setup.
        /// </summary>
        /// <value>The setup.</value>
        public DateEditControlSetup Setup { get; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public DateTime? Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridDateCellProps" /> class and creates a date edit control in the data entry grid cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="setup">The date edit control setup.</param>
        /// <param name="value">The datetime value.</param>
        public DataEntryGridDateCellProps(DataEntryGridRow row, int columnId, DateEditControlSetup setup,
            DateTime? value) : base(row, columnId)
        {
            Setup = setup;
            Value = value;
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
            return Setup.FormatValueForDisplay(Value);
        }
    }
}
