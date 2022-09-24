using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public class DataEntryGridDateHost : DataEntryGridDropDownControlHost<DateEditControl>
    {
        public override bool AllowReadOnlyEdit => true;

        private DateEditControlSetup _setup;
        private DateTime? _value;
        private bool _gridReadOnlyMode;

        public DataEntryGridDateHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridDateCellProps(Row, ColumnId, _setup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != _value;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            var dateCellProps = (DataEntryGridDateCellProps)cellProps;
            _value = dateCellProps.Value;
        }

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

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            base.ImportDataGridCellProperties(dataGridCell);
            if (_gridReadOnlyMode)
            {
                dataGridCell.BorderThickness = new Thickness(1);
            }
        }

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

        public override bool SetReadOnlyMode(bool readOnlyMode)
        {
            _gridReadOnlyMode = readOnlyMode;

            if (readOnlyMode)
                return true;

            return base.SetReadOnlyMode(readOnlyMode);
        }

    }
}
