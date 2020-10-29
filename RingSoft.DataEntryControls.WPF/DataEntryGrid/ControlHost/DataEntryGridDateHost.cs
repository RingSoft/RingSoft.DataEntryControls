using System;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridDateHost : DataEntryGridDropDownControlHost<DateEditControl>
    {
        private DateEditControlSetup _setup;
        private DateTime? _value;

        public DataEntryGridDateHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridDateCellProps(Row, ColumnId, _setup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != _value;
        }

        protected override void OnControlLoaded(DateEditControl control, DataEntryGridCellProps cellProps,
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
