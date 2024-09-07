// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DataEntryGridButtonHost.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Grid cell's button host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{System.Windows.Controls.Button}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{System.Windows.Controls.Button}" />
    public class DataEntryGridButtonHost : DataEntryGridEditingControlHost<Button>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public override bool IsDropDownOpen => false;

        /// <summary>
        /// The has data changed
        /// </summary>
        private bool _hasDataChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridButtonHost" /> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridButtonHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridButtonCellProps(Row, ColumnId);
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return _hasDataChanged;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            
        }

        /// <summary>
        /// Called by the control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <exception cref="System.Exception"></exception>
        protected override void OnControlLoaded(Button control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            if (cellStyle is DataEntryGridButtonCellStyle buttonCellStyle)
            {
                Control.Content = buttonCellStyle.Content;
            }
            else
            {
                throw new Exception(
                    DataEntryGridButtonCellProps.GetCellStyleExceptionMessage(cellProps.Row, cellProps.ColumnId));
            }

            control.Click += (sender, args) =>
            {
                _hasDataChanged = true;
                OnUpdateSource(cellProps);
                _hasDataChanged = false;
            };
        }

        /// <summary>
        /// Imports the data grid cell properties.
        /// </summary>
        /// <param name="dataGridCell">The data grid cell.</param>
        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
        }
    }
}
