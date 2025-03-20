// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridCheckBoxHost.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Grid cell's checkbox host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{System.Windows.Controls.CheckBox}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{System.Windows.Controls.CheckBox}" />
    public class DataEntryGridCheckBoxHost : DataEntryGridEditingControlHost<CheckBox>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public override bool IsDropDownOpen => false;
        /// <summary>
        /// Gets a value indicating whether [set selection].
        /// </summary>
        /// <value><c>true</c> if [set selection]; otherwise, <c>false</c>.</value>
        public override bool SetSelection => true;

        /// <summary>
        /// The value
        /// </summary>
        private bool _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridCheckBoxHost" /> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridCheckBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Setups the framework element factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="column">The column.</param>
        protected override void SetupFrameworkElementFactory(FrameworkElementFactory factory,
            DataEntryGridColumn column)
        {
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            base.SetupFrameworkElementFactory(factory, column);
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            bool checkBoxValue = Control.IsChecked != null && (bool) Control.IsChecked;

            return new DataEntryGridCheckBoxCellProps(Row, ColumnId, checkBoxValue);
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return _value != Control.IsChecked;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            _value = cellProps is DataEntryGridCheckBoxCellProps checkBoxCellProps && checkBoxCellProps.Value;
        }

        /// <summary>
        /// Called by the control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected override void OnControlLoaded(CheckBox control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var checkBoxCellProps = cellProps as DataEntryGridCheckBoxCellProps;
            control.IsChecked = _value = checkBoxCellProps != null && checkBoxCellProps.Value;
            switch (cellStyle.State)
            {
                case DataEntryGridCellStates.Enabled:
                    break;
                case DataEntryGridCellStates.ReadOnly:
                case DataEntryGridCellStates.Disabled:
                    Control.IsEnabled = false;
                    break;
            }

            Control.Checked += (sender, args) =>
            {
                OnControlDirty();
                OnUpdateSource(GetCellValue());
                _value = (bool) control.IsChecked;
            };
            Control.Unchecked += (sender, args) =>
            {
                OnControlDirty();
                OnUpdateSource(GetCellValue());
                _value = (bool) control.IsChecked;
            };

            if (Mouse.LeftButton == MouseButtonState.Pressed)
                control.IsChecked = !control.IsChecked;

            Control.HorizontalAlignment = HorizontalAlignment.Center;
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
