using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridIntegerControlHost : DataEntryGridDropDownControlHost<IntegerEditControl>
    {
        public DataEntryGridIntegerCellProps IntegerCellProps { get; private set; }
        public DataEntryGridIntegerControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridIntegerCellProps(IntegerCellProps.Row, IntegerCellProps.ColumnId,
                IntegerCellProps.NumericEditSetup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != IntegerCellProps.Value;
        }

        protected override void OnControlLoaded(IntegerEditControl control, DataEntryGridCellProps cellProps)
        {
            IntegerCellProps = (DataEntryGridIntegerCellProps) cellProps;

            control.Setup = IntegerCellProps.NumericEditSetup;
            control.Value = IntegerCellProps.Value;
            
            base.OnControlLoaded(control, cellProps);
        }

        public override void ProcessValidationFail(DataEntryGridCellProps cellProps)
        {
            var decimalCellProps = (DataEntryGridIntegerCellProps) cellProps;
            Control.Value = decimalCellProps.Value;

            base.ProcessValidationFail(cellProps);
        }
    }
}
