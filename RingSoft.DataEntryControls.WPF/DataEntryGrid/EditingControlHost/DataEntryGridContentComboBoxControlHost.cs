// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridContentComboBoxControlHost.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// Grid cell's content combo box control's host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridComboBoxHost{RingSoft.DataEntryControls.WPF.ContentComboBoxControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridComboBoxHost{RingSoft.DataEntryControls.WPF.ContentComboBoxControl}" />
    public class DataEntryGridContentComboBoxControlHost : DataEntryGridComboBoxHost<ContentComboBoxControl>
    {
        /// <summary>
        /// Gets the ComboBox.
        /// </summary>
        /// <value>The ComboBox.</value>
        protected override ComboBox ComboBox => Control;

        /// <summary>
        /// The selected item identifier
        /// </summary>
        private int _selectedItemId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridContentComboBoxControlHost" /> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridContentComboBoxControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return Control.SelectedItemId != _selectedItemId;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is DataEntryGridCustomControlCellProps customControlCellProps)
                Control.SelectedItemId = customControlCellProps.SelectedItemId;
        }

        /// <summary>
        /// Gets the ComboBox cell props.
        /// </summary>
        /// <param name="valueChangeType">Type of the value change.</param>
        /// <returns>DataEntryGridComboBoxCellProps.</returns>
        protected override DataEntryGridComboBoxCellProps GetComboBoxCellProps(ComboBoxValueChangedTypes valueChangeType)
        {
            return new DataEntryGridCustomControlCellProps(Row, ColumnId, Control.SelectedItemId, valueChangeType);
        }

        /// <summary>
        /// Validates the ComboBox cell props.
        /// </summary>
        /// <param name="comboBoxCellProps">The combo box cell props.</param>
        /// <exception cref="System.Exception">Row: {comboBoxCellProps.Row} ColumnId: {comboBoxCellProps.ColumnId} {nameof(DataEntryGridRow.GetCellProps)} must return a valid {nameof(DataEntryGridCustomControlCellProps)} object.</exception>
        protected override void ValidateComboBoxCellProps(DataEntryGridComboBoxCellProps comboBoxCellProps)
        {
            if (!(comboBoxCellProps is DataEntryGridCustomControlCellProps))
                throw new Exception(
                    $"Row: {comboBoxCellProps.Row} ColumnId: {comboBoxCellProps.ColumnId} {nameof(DataEntryGridRow.GetCellProps)} must return a valid {nameof(DataEntryGridCustomControlCellProps)} object.");
        }

        /// <summary>
        /// Sets the selected item.
        /// </summary>
        /// <param name="overrideCellMovement">if set to <c>true</c> [override cell movement].</param>
        protected override void SetSelectedItem(bool overrideCellMovement)
        {
            if (overrideCellMovement)
            {
                Control.SelectedItemId = _selectedItemId;
            }
            else
            {
                _selectedItemId = Control.SelectedItemId;
            }
        }

        /// <summary>
        /// Called by the control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected override void OnComboControlLoaded(ContentComboBoxControl control, DataEntryGridComboBoxCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            if (cellProps is DataEntryGridCustomControlCellProps customControlCellProps)
                _selectedItemId = Control.SelectedItemId = customControlCellProps.SelectedItemId;
        }

        /// <summary>
        /// Setups the framework element factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="column">The column.</param>
        /// <exception cref="System.Exception">DataEntryGridRow: {Row} GetCellProps for ColumnId: {column.ColumnId} cannot return a {nameof(DataEntryGridCustomControlCellProps)} object.  It is only compatible with {nameof(DataEntryGridCustomControlColumn)} column types.</exception>
        protected override void SetupFrameworkElementFactory(FrameworkElementFactory factory, DataEntryGridColumn column)
        {
            if (column is DataEntryGridCustomControlColumn customControlColumn)
            {
                factory.SetValue(ContentComboBoxControl.ContentTemplateProperty, customControlColumn.ContentTemplate);
            }
            else
            {
                throw new Exception(
                    $"DataEntryGridRow: {Row} GetCellProps for ColumnId: {column.ColumnId} cannot return a {nameof(DataEntryGridCustomControlCellProps)} object.  It is only compatible with {nameof(DataEntryGridCustomControlColumn)} column types.");
            }
            base.SetupFrameworkElementFactory(factory, column);
        }
    }
}
