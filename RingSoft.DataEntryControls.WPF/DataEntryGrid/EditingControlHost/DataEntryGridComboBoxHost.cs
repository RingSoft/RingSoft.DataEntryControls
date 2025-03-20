// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridComboBoxHost.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Grid cell's text combo box host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridComboBoxHost{RingSoft.DataEntryControls.WPF.TextComboBoxControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridComboBoxHost{RingSoft.DataEntryControls.WPF.TextComboBoxControl}" />
    public class DataEntryGridTextComboBoxHost : DataEntryGridComboBoxHost<TextComboBoxControl>
    {
        /// <summary>
        /// Gets the ComboBox.
        /// </summary>
        /// <value>The ComboBox.</value>
        protected override ComboBox ComboBox => Control;
        /// <summary>
        /// The combo box setup
        /// </summary>
        private TextComboBoxControlSetup _comboBoxSetup;
        /// <summary>
        /// The selected item
        /// </summary>
        private TextComboBoxItem _selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridTextComboBoxHost" /> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridTextComboBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the ComboBox cell props.
        /// </summary>
        /// <param name="valueChangeType">Type of the value change.</param>
        /// <returns>DataEntryGridComboBoxCellProps.</returns>
        protected override DataEntryGridComboBoxCellProps GetComboBoxCellProps(
            ComboBoxValueChangedTypes valueChangeType)
        {
            return new DataEntryGridTextComboBoxCellProps(Row, ColumnId,
                _comboBoxSetup, Control.SelectedItem, valueChangeType);
        }

        /// <summary>
        /// Validates the ComboBox cell props.
        /// </summary>
        /// <param name="comboBoxCellProps">The combo box cell props.</param>
        /// <exception cref="System.ArgumentException">ComboBox Selected Item cannot be null.</exception>
        protected override void ValidateComboBoxCellProps(DataEntryGridComboBoxCellProps comboBoxCellProps)
        {
            if (comboBoxCellProps is DataEntryGridTextComboBoxCellProps textComboBoxCellProps)
                if (textComboBoxCellProps.SelectedItem == null)
                    throw new ArgumentException($"ComboBox Selected Item cannot be null.");
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return Control.SelectedItem != _selectedItem;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);
            if (comboBoxCellProps is DataEntryGridTextComboBoxCellProps textComboBoxCellProps)
                _selectedItem = textComboBoxCellProps.SelectedItem;
        }

        /// <summary>
        /// Sets the selected item.
        /// </summary>
        /// <param name="overrideCellMovement">if set to <c>true</c> [override cell movement].</param>
        protected override void SetSelectedItem(bool overrideCellMovement)
        {
            if (overrideCellMovement)
                Control.SelectedItem = _selectedItem;
            else
            {
                _selectedItem = Control.SelectedItem;
            }
        }

        /// <summary>
        /// Called by the control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected override void OnComboControlLoaded(TextComboBoxControl control, DataEntryGridComboBoxCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);

            if (comboBoxCellProps is DataEntryGridTextComboBoxCellProps textComboBoxCellProps)
            {
                control.Setup = _comboBoxSetup = textComboBoxCellProps.ComboBoxSetup;
                control.SelectedItem = _selectedItem = textComboBoxCellProps.SelectedItem;
            }
        }
    }

    /// <summary>
    /// Grid cell's combo box host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{TControl}" />
    /// </summary>
    /// <typeparam name="TControl">The type of the t control.</typeparam>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{TControl}" />
    public abstract class DataEntryGridComboBoxHost<TControl> : DataEntryGridEditingControlHost<TControl>
        where TControl : Control
    {
        /// <summary>
        /// Gets the ComboBox.
        /// </summary>
        /// <value>The ComboBox.</value>
        protected abstract ComboBox ComboBox { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public sealed override bool IsDropDownOpen => ComboBox.IsDropDownOpen;

        /// <summary>
        /// The value change type
        /// </summary>
        private ComboBoxValueChangedTypes _valueChangeType;
        /// <summary>
        /// The proceessing validation fail
        /// </summary>
        private bool _proceessingValidationFail;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridComboBoxHost{TControl}" /> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        protected DataEntryGridComboBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public sealed override DataEntryGridEditingCellProps GetCellValue()
        {
            return GetComboBoxCellProps(_valueChangeType);
        }

        /// <summary>
        /// Gets the ComboBox cell props.
        /// </summary>
        /// <param name="valueChangeType">Type of the value change.</param>
        /// <returns>DataEntryGridComboBoxCellProps.</returns>
        protected abstract DataEntryGridComboBoxCellProps GetComboBoxCellProps(ComboBoxValueChangedTypes valueChangeType);

        /// <summary>
        /// Validates the ComboBox cell props.
        /// </summary>
        /// <param name="comboBoxCellProps">The combo box cell props.</param>
        protected abstract void ValidateComboBoxCellProps(DataEntryGridComboBoxCellProps comboBoxCellProps);

        /// <summary>
        /// Gets the ComboBox cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        /// <returns>DataEntryGridComboBoxCellProps.</returns>
        /// <exception cref="System.ArgumentException"></exception>
        protected DataEntryGridComboBoxCellProps GetComboBoxCellProps(DataEntryGridCellProps cellProps)
        {
            var comboBoxProps = cellProps as DataEntryGridComboBoxCellProps;
            if (comboBoxProps == null)
            {
                var rowName = cellProps.Row.ToString();
                throw new ArgumentException(
                    $"{nameof(DataEntryGridTextComboBoxCellProps)} not setup for Row: {rowName} Column Id={cellProps.ColumnId}");
            }

            ValidateComboBoxCellProps(comboBoxProps);

            return comboBoxProps;
        }

        /// <summary>
        /// Sets the selected item.
        /// </summary>
        /// <param name="overrideCellMovement">if set to <c>true</c> [override cell movement].</param>
        protected abstract void SetSelectedItem(bool overrideCellMovement);

        /// <summary>
        /// Called by the control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected sealed override void OnControlLoaded(TControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var comboBoxCellProps = GetComboBoxCellProps(cellProps);

            _valueChangeType = comboBoxCellProps.ChangeType;

            OnComboControlLoaded(control, comboBoxCellProps, cellStyle);

            ComboBox.SelectionChanged += (sender, args) => OnSelectionChanged();
        }

        /// <summary>
        /// Called by the combo box control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected abstract void OnComboControlLoaded(TControl control, DataEntryGridComboBoxCellProps cellProps,
            DataEntryGridCellStyle cellStyle);

        /// <summary>
        /// Called when [selection changed].
        /// </summary>
        private void OnSelectionChanged()
        {
            if (_proceessingValidationFail || _valueChangeType == ComboBoxValueChangedTypes.EndEdit)
                return;

            OnControlDirty();
            var controlValue = GetComboBoxCellProps(GetCellValue());
            OnUpdateSource(controlValue);

            if (controlValue.OverrideCellMovement)
                SetSelectedItem(true);
            else
            {
                _proceessingValidationFail = true;
                SetSelectedItem(false);
                _proceessingValidationFail = false;
            }

            Grid.UpdateRow(Row);
        }

        /// <summary>
        /// Determines whether the grid can process the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if this instance [can grid process key] the specified key; otherwise, <c>false</c>.</returns>
        public sealed override bool CanGridProcessKey(Key key)
        {
            switch (key)
            {
                case Key.Up:
                case Key.Down:
                    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                        return false;
                    if (ComboBox.IsDropDownOpen)
                        return false;

                    break;
                case Key.Escape:
                case Key.Enter:
                    if (ComboBox.IsDropDownOpen)
                        return false;
                    break;
            }
            return base.CanGridProcessKey(key);
        }

        /// <summary>
        /// Imports the data grid cell properties.
        /// </summary>
        /// <param name="dataGridCell">The data grid cell.</param>
        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            Control.MinHeight = dataGridCell.MinHeight;
            Control.MinWidth = dataGridCell.MinWidth;

            //base.ImportDataGridCellProperties(dataGridCell);
        }
    }
}
