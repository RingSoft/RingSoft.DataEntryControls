// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridCheckBoxCellProps.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Class DataEntryGridCheckBoxCellProps.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    public class DataEntryGridCheckBoxCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => DataEntryGridEditingCellProps.CheckBoxHostId;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataEntryGridCheckBoxCellProps"/> is value.
        /// </summary>
        /// <value><c>true</c> if value; otherwise, <c>false</c>.</value>
        public bool Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridCheckBoxCellProps"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public DataEntryGridCheckBoxCellProps(DataEntryGridRow row, int columnId, bool value) : base(row, columnId)
        {
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
            var dataValue = new DataEntryGridDataValue();
            dataValue.CreateDataValue(row, columnId, Value.ToString());

            return dataValue.DataValue;
        }
    }
}
