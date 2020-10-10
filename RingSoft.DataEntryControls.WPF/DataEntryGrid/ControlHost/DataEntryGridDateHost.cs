using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridDateHost : DataEntryGridDropDownControlHost<DateEditControl>
    {
        public DataEntryGridDateCellProps DateCellProps { get; private set; }
        public DataEntryGridDateHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridDateCellProps(DateCellProps.Row, DateCellProps.ColumnId, DateCellProps.Setup,
                Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != DateCellProps.Value;
        }

        protected override void OnControlLoaded(DateEditControl control, DataEntryGridCellProps cellProps)
        {
            DateCellProps = (DataEntryGridDateCellProps) cellProps;

            control.Setup = DateCellProps.Setup;
            control.Value = DateCellProps.Value;

            base.OnControlLoaded(control, cellProps);
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
