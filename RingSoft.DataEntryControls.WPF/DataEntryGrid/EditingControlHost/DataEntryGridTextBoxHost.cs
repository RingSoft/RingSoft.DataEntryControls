// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-19-2023
// ***********************************************************************
// <copyright file="DataEntryGridTextBoxHost.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Grid cell's textbox control host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{RingSoft.DataEntryControls.WPF.StringEditControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{RingSoft.DataEntryControls.WPF.StringEditControl}" />
    public class DataEntryGridTextBoxHost : DataEntryGridEditingControlHost<StringEditControl>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public override bool IsDropDownOpen => false;

        /// <summary>
        /// The text
        /// </summary>
        private string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridTextBoxHost"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridTextBoxHost(DataEntryGrid grid) : base(grid)
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
            //var binding = new Binding()
            //{
            //    Source = this,
            //    Path = new PropertyPath(nameof(Text)),
            //    Mode = BindingMode.TwoWay,
            //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            //};
            //factory.SetBinding(TextBox.TextProperty, binding);
        }

        /// <summary>
        /// Called by the control's loaded event.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void OnControlLoaded(StringEditControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            if (cellProps is DataEntryGridTextCellProps textCellProps)
            {
                control.MaxLength = textCellProps.MaxLength;
                switch (textCellProps.CharacterCasing)
                {
                    case TextCasing.Normal:
                        break;
                    case TextCasing.Upper:
                        control.CharacterCasing = CharacterCasing.Upper;
                        break;
                    case TextCasing.Lower:
                        control.CharacterCasing = CharacterCasing.Lower;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var displayStyle = GetCellDisplayStyle();
                if (displayStyle.SelectionBrush != null)
                {
                    Control.SelectionBrush = displayStyle.SelectionBrush;
                }
                _text = control.Text = textCellProps.Text;
            }

            Control.SelectAll();

            Control.KeyDown += TextBox_KeyDown;
            Control.TextChanged += (sender, args) => OnControlDirty();
        }

        /// <summary>
        /// Imports the data grid cell properties.
        /// </summary>
        /// <param name="dataGridCell">The data grid cell.</param>
        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            if (dataGridCell.Column is DataEntryGridTextColumn dataEntryGridColumn)
            {
                Control.TextAlignment = dataEntryGridColumn.Alignment;
            }

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
                    Control.SelectAll();
                }
                else
                {
                    Control.SelectionLength = 0;
                    Control.SelectionStart = Control.Text.Length;
                }
            }
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridTextCellProps(Row, ColumnId)
            {
                Text = Control.Text
            };
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            if (string.IsNullOrEmpty(_text) && string.IsNullOrEmpty(Control.Text))
                return false;

            return _text != Control.Text;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is DataEntryGridTextCellProps textCellProps) 
                _text = textCellProps.Text;
        }

        /// <summary>
        /// Determines whether the grid can process the specified key.
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
            }
            return base.CanGridProcessKey(key);
        }
    }
}
