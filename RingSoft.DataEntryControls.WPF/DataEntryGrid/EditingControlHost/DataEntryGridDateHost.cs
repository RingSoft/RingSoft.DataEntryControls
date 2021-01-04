using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public class DataEntryGridDateHost : DataEntryGridDropDownControlHost<DateEditControl>
    {
        private DateEditControlSetup _setup;
        private DateTime? _value;

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

            base.OnControlLoaded(control, cellProps, cellStyle);
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
    }
}
