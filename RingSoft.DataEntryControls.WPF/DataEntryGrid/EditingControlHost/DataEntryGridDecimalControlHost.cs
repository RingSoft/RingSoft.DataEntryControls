// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridDecimalControlHost.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Class DataEntryGridDecimalControlHost.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridDropDownControlHost{RingSoft.DataEntryControls.WPF.DecimalEditControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridDropDownControlHost{RingSoft.DataEntryControls.WPF.DecimalEditControl}" />
    public class DataEntryGridDecimalControlHost : DataEntryGridDropDownControlHost<DecimalEditControl>
    {
        /// <summary>
        /// Gets the decimal cell props.
        /// </summary>
        /// <value>The decimal cell props.</value>
        public DataEntryGridDecimalCellProps DecimalCellProps { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridDecimalControlHost"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridDecimalControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridDecimalCellProps(Row, ColumnId,
                DecimalCellProps.NumericEditSetup, Control.Value);
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return Control.Value != DecimalCellProps.Value;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            DecimalCellProps = (DataEntryGridDecimalCellProps)cellProps;
        }

        /// <summary>
        /// Called when [control loaded].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected override void OnControlLoaded(DecimalEditControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            DecimalCellProps = (DataEntryGridDecimalCellProps) cellProps;

            control.Setup = DecimalCellProps.NumericEditSetup;
            control.Value = DecimalCellProps.Value;

            control.CalculatorValueChanged += (sender, args) => OnUpdateSource(GetCellValue());
            
            base.OnControlLoaded(control, cellProps, cellStyle);
        }
    }
}
