// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridDropDownControlHost.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Class DataEntryGridDropDownControlHost.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{TDropDownControl}" />
    /// </summary>
    /// <typeparam name="TDropDownControl">The type of the t drop down control.</typeparam>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{TDropDownControl}" />
    public abstract class DataEntryGridDropDownControlHost<TDropDownControl> : DataEntryGridEditingControlHost<TDropDownControl>
        where TDropDownControl : DropDownEditControl
    {
        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public override bool IsDropDownOpen => Control.IsPopupOpen();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridDropDownControlHost{TDropDownControl}"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        protected DataEntryGridDropDownControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Called when [control loaded].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected override void OnControlLoaded(TDropDownControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            if (Control.TextBox != null)
            {
                Control.TextBox.KeyDown += TextBox_KeyDown;
                Control.TextBox.SelectAll();
            }
            
            Control.ValueChanged += (sender, args) => OnControlDirty();

            var displayStyle = GetCellDisplayStyle();
            if (displayStyle.SelectionBrush != null)
            {
                Control.SelectionBrush = displayStyle.SelectionBrush;
            }
        }

        /// <summary>
        /// Imports the data grid cell properties.
        /// </summary>
        /// <param name="dataGridCell">The data grid cell.</param>
        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            if (dataGridCell.Column is DataEntryGridTextColumn dataEntryGridColumn)
                Control.TextAlignment = dataEntryGridColumn.Alignment;

            base.ImportDataGridCellProperties(dataGridCell);
        }

        /// <summary>
        /// Handles the KeyDown event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                if (Control.SelectionLength < Control.Text.Length)
                {
                    Control.TextBox.SelectAll();
                }
                else
                {
                    Control.SelectionLength = 0;
                    Control.SelectionStart = Control.Text.Length;
                }
            }
        }

        /// <summary>
        /// Determines whether this instance [can grid process key] the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if this instance [can grid process key] the specified key; otherwise, <c>false</c>.</returns>
        public override bool CanGridProcessKey(Key key)
        {
            var editingCell = Control.Text.Length > 0 && Control.SelectionLength != Control.Text.Length;
            switch (key)
            {
                case Key.Left:
                    if (editingCell)
                    {
                        if (Control.SelectionStart <= 0)
                            return true;

                        return false;
                    }

                    break;
                case Key.Right:
                    if (editingCell)
                    {
                        if (Control.SelectionStart >= Control.Text.Length - 1)
                            return true;
                        return false;
                    }
                    break;
                case Key.Enter:
                case Key.Escape:
                    return !Control.IsPopupOpen();
            }
            return base.CanGridProcessKey(key);
        }
    }
}
