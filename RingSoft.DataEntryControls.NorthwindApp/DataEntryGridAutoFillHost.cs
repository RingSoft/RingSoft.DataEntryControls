using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class DataEntryGridAutoFillHost : DataEntryGridControlHost<AutoFillControl>
    {
        public DataEntryGridAutoFillCellProps AutoFillCellProps { get; private set; }

        public AutoFillControl AutoFillControl { get; private set; }

        public DataEntryGridAutoFillHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridAutoFillCellProps(AutoFillCellProps.Row, AutoFillCellProps.ColumnId,
                AutoFillControl.Setup, AutoFillControl.Value);
        }

        public override bool HasDataChanged()
        {
            if (AutoFillControl.Value == null && AutoFillCellProps.AutoFillValue != null)
                return true;

            if (AutoFillControl.Value != null && AutoFillCellProps.AutoFillValue == null)
                return true;

            if (AutoFillControl.Value == null && AutoFillCellProps.AutoFillValue == null)
                return false;

            return AutoFillCellProps.AutoFillValue != null && (AutoFillControl.Value != null && AutoFillControl.Value.PrimaryKeyValue.IsEqualTo(AutoFillCellProps.AutoFillValue.PrimaryKeyValue));
        }

        protected override void OnControlLoaded(AutoFillControl control, DataEntryGridCellProps cellProps)
        {
            AutoFillControl = control;
            AutoFillCellProps = (DataEntryGridAutoFillCellProps) cellProps;

            AutoFillControl.Setup = AutoFillCellProps.AutoFillSetup;
            AutoFillControl.Value = AutoFillCellProps.AutoFillValue;
        }
    }
}
