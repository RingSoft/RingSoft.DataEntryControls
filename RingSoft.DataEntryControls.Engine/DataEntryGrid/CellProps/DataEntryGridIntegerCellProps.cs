// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridIntegerCellProps.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Class DataEntryGridIntegerCellProps.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    public class DataEntryGridIntegerCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => DataEntryGridEditingCellProps.IntegerEditHostId;

        /// <summary>
        /// Gets the numeric edit setup.
        /// </summary>
        /// <value>The numeric edit setup.</value>
        public IntegerEditControlSetup NumericEditSetup { get; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int? Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridIntegerCellProps"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="setup">The setup.</param>
        /// <param name="value">The value.</param>
        public DataEntryGridIntegerCellProps(DataEntryGridRow row, int columnId, IntegerEditControlSetup setup, int? value) : base(row, columnId)
        {
            NumericEditSetup = setup;
            Value = value;
        }

        /// <summary>
        /// Gets the data value.
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
