// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridDateHost.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Grid cell's date edit control host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridDropDownControlHost{RingSoft.DataEntryControls.WPF.DateEditControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridDropDownControlHost{RingSoft.DataEntryControls.WPF.DateEditControl}" />
    public class DataEntryGridDateHost : DataEntryGridDropDownControlHost<DateEditControl>
    {
        /// <summary>
        /// Gets a value indicating whether [allow read only edit].
        /// </summary>
        /// <value><c>true</c> if [allow read only edit]; otherwise, <c>false</c>.</value>
        public override bool AllowReadOnlyEdit => true;

        /// <summary>
        /// The setup
        /// </summary>
        private DateEditControlSetup _setup;
        /// <summary>
        /// The value
        /// </summary>
        private DateTime? _value;
        /// <summary>
        /// The grid read only mode
        /// </summary>
        private bool _gridReadOnlyMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridDateHost" /> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridDateHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridDateCellProps(Row, ColumnId, _setup, Control.Value);
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return Control.Value != _value;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            var dateCellProps = (DataEntryGridDateCellProps)cellProps;
            _value = dateCellProps.Value;
        }

        /// <summary>
        /// Called by the control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void OnControlLoaded(DateEditControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var dateCellProps = (DataEntryGridDateCellProps) cellProps;

            control.Setup = _setup = dateCellProps.Setup;
            control.Value = _value = dateCellProps.Value;

            switch (cellStyle.State)
            {
                case DataEntryGridCellStates.Enabled:
                    break;
                case DataEntryGridCellStates.ReadOnly:
                case DataEntryGridCellStates.Disabled:
                    _gridReadOnlyMode = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (_gridReadOnlyMode)
            {
                Control.SetReadOnlyMode(true);
                Control.KeyDown += (sender, args) =>
                {
                    if (args.Key == Key.F4)
                    {
                        Control.OnDropDownButtonClick();
                        args.Handled = true;
                    }
                };
            }

            base.OnControlLoaded(control, cellProps, cellStyle);
        }

        /// <summary>
        /// Imports the data grid cell properties.
        /// </summary>
        /// <param name="dataGridCell">The data grid cell.</param>
        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            base.ImportDataGridCellProperties(dataGridCell);
            if (_gridReadOnlyMode)
            {
                dataGridCell.BorderThickness = new Thickness(1);
            }
        }

        /// <summary>
        /// Determines whether the grid can process the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if this instance [can grid process key] the specified key; otherwise, <c>false</c>.</returns>
        public override bool CanGridProcessKey(Key key)
        {
            if (Control.IsPopupOpen())
            {
                switch (key)
                {
                    case Key.Left:
                    case Key.Right:
                    case Key.Up:
                    case Key.Down:
                        return false;
                }
            }
            return base.CanGridProcessKey(key);
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyMode">if set to <c>true</c> [read only mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool SetReadOnlyMode(bool readOnlyMode)
        {
            _gridReadOnlyMode = readOnlyMode;

            if (readOnlyMode)
                return true;

            return base.SetReadOnlyMode(readOnlyMode);
        }

    }
}
