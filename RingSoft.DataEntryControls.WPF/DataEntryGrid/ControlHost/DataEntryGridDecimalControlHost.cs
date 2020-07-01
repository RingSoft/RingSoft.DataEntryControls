using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridDecimalControlHost : DataEntryGridControlHost<DecimalEditControl>
    {
        public override bool IsDropDownOpen => Control.Popup != null && Control.Popup.IsOpen;

        public DataEntryGridDecimalCellProps DecimalCellProps { get; private set; }
        public DataEntryGridDecimalControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridDecimalCellProps(DecimalCellProps.Row, DecimalCellProps.ColumnId,
                DecimalCellProps.NumericEditSetup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != DecimalCellProps.Value;
        }

        protected override void OnControlLoaded(DecimalEditControl control, DataEntryGridCellProps cellProps)
        {
            if (Control.TextBox != null)
            {
                Control.TextBox.KeyDown += TextBox_KeyDown;
            }

            DecimalCellProps = (DataEntryGridDecimalCellProps) cellProps;

            control.Setup = DecimalCellProps.NumericEditSetup;
            control.Value = DecimalCellProps.Value;
            Control.ValueChanged += (sender, args) => OnControlDirty();
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

        public override void ProcessValidationFail(DataEntryGridCellProps cellProps)
        {
            var decimalCellProps = (DataEntryGridDecimalCellProps) cellProps;
            Control.Value = decimalCellProps.Value;

            base.ProcessValidationFail(cellProps);
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
                    if (Control.Popup != null && Control.Popup.IsOpen)
                        return false;
                    break;
            }
            return base.CanGridProcessKey(key);
        }
    }
}
