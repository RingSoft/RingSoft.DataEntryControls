using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public abstract class DataEntryGridDropDownControlHost<TDropDownControl> : DataEntryGridEditingControlHost<TDropDownControl>
        where TDropDownControl : DropDownEditControl
    {
        public override bool IsDropDownOpen => Control.IsPopupOpen();

        protected DataEntryGridDropDownControlHost(DataEntryGrid grid) : base(grid)
        {
        }

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

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            if (dataGridCell.Column is DataEntryGridColumn dataEntryGridColumn)
                Control.TextAlignment = dataEntryGridColumn.Alignment;

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
