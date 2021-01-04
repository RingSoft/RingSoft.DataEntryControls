using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public class DataEntryGridTextBoxHost : DataEntryGridEditingControlHost<StringEditControl>
    {
        public override bool IsDropDownOpen => false;

        private string _text;

        public DataEntryGridTextBoxHost(DataEntryGrid grid) : base(grid)
        {
        }

        protected override void SetupFrameworkElementFactory(FrameworkElementFactory factory)
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

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            if (dataGridCell.Column is DataEntryGridColumn dataEntryGridColumn)
            {
                Control.TextAlignment = dataEntryGridColumn.Alignment;
            }

            base.ImportDataGridCellProperties(dataGridCell);
        }

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

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridTextCellProps(Row, ColumnId)
            {
                Text = Control.Text
            };
        }

        public override bool HasDataChanged()
        {
            if (string.IsNullOrEmpty(_text) && string.IsNullOrEmpty(Control.Text))
                return false;

            return _text != Control.Text;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is DataEntryGridTextCellProps textCellProps) 
                _text = textCellProps.Text;
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
            }
            return base.CanGridProcessKey(key);
        }
    }
}
