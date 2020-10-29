﻿using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public abstract class DataEntryGridDropDownControlHost<TDropDownControl> : DataEntryGridControlHost<TDropDownControl>
        where TDropDownControl : DropDownEditControl
    {
        public override bool IsDropDownOpen => Control.IsPopupOpen();

        protected DataEntryGridDropDownControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        protected override void OnControlLoaded(TDropDownControl control, DataEntryGridCellProps cellProps)
        {
            if (Control.TextBox != null)
            {
                Control.TextBox.KeyDown += TextBox_KeyDown;
            }

            Control.ValueChanged += (sender, args) => OnControlDirty();
        }

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            if (Control.TextBox != null)
            {
                if (dataGridCell.Column is DataEntryGridColumn dataEntryGridColumn)
                    Control.TextBox.TextAlignment = dataEntryGridColumn.Alignment;

                Control.TextBox.Background = dataGridCell.Background;
                Control.TextBox.Foreground = dataGridCell.Foreground;
            }

            base.ImportDataGridCellProperties(dataGridCell);
        }

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
