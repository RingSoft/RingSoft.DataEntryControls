// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridIntegerControlHost.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Grid cell's integer edit control host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridDropDownControlHost{RingSoft.DataEntryControls.WPF.IntegerEditControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridDropDownControlHost{RingSoft.DataEntryControls.WPF.IntegerEditControl}" />
    public class DataEntryGridIntegerControlHost : DataEntryGridDropDownControlHost<IntegerEditControl>
    {
        /// <summary>
        /// Gets the integer cell props.
        /// </summary>
        /// <value>The integer cell props.</value>
        public DataEntryGridIntegerCellProps IntegerCellProps { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridIntegerControlHost"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridIntegerControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridIntegerCellProps(Row, ColumnId,
                IntegerCellProps.NumericEditSetup, Control.Value);
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return Control.Value != IntegerCellProps.Value;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            IntegerCellProps = (DataEntryGridIntegerCellProps)cellProps;
        }

        /// <summary>
        /// Called by the control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected override void OnControlLoaded(IntegerEditControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            IntegerCellProps = (DataEntryGridIntegerCellProps) cellProps;

            control.Setup = IntegerCellProps.NumericEditSetup;
            control.Value = IntegerCellProps.Value;
            
            base.OnControlLoaded(control, cellProps, cellStyle);
        }
    }
}
