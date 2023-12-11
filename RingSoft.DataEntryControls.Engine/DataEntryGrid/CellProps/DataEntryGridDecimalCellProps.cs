// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="DataEntryGridDecimalCellProps.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Creates a decimal edit control in the data entry grid cell.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    public class DataEntryGridDecimalCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => DataEntryGridEditingCellProps.DecimalEditHostId;

        /// <summary>
        /// Gets the numeric edit setup.
        /// </summary>
        /// <value>The numeric edit setup.</value>
        public DecimalEditControlSetup NumericEditSetup { get; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double? Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridDecimalCellProps"/> class and creates a decimal edit control in the data entry grid cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="setup">The decimal edit setup.</param>
        /// <param name="value">The decimal value.</param>
        public DataEntryGridDecimalCellProps(DataEntryGridRow row, int columnId, DecimalEditControlSetup setup, double? value) : base(row, columnId)
        {
            NumericEditSetup = setup;
            Value = value;
        }

        /// <summary>
        /// /// Gets the control properties.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return NumericEditSetup.FormatValue(Value);
        }

    }
}
