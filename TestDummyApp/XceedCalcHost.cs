using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;
using Xceed.Wpf.Toolkit;

namespace RingSoft.DataEntryControls.App.WPF
{
    public class XceedCalcHost : DataEntryGridControlHost<CalculatorUpDown>
    {
        public XceedCalcHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridTextCellProps(CellProps.Row, CellProps.ColumnId);
        }

        public override bool HasDataChanged()
        {
            return false;
        }

        protected override void OnControlLoaded(CalculatorUpDown control, DataEntryGridCellProps cellProps)
        {
            
        }
    }
}
