using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;
using RingSoft.DbLookup.Controls.WPF;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class DataEntryGridAutoFillHost : DataEntryGridControlHost<AutoFillControl>
    {
        public DataEntryGridAutoFillCellProps AutoFillCellProps { get; private set; }

                public DataEntryGridAutoFillHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridAutoFillCellProps(AutoFillCellProps.Row, AutoFillCellProps.ColumnId,
                Control.Setup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            if (Control.Value == null && AutoFillCellProps.AutoFillValue != null)
                return true;

            if (Control.Value != null && AutoFillCellProps.AutoFillValue == null)
                return true;

            if (Control.Value == null && AutoFillCellProps.AutoFillValue == null)
                return false;

            if (AutoFillCellProps.AutoFillValue != null && Control.Value != null)
            {
                if (Control.Value.PrimaryKeyValue.ContainsValidData() &&
                    AutoFillCellProps.AutoFillValue.PrimaryKeyValue.ContainsValidData())
                {
                    return !Control.Value.PrimaryKeyValue.IsEqualTo(AutoFillCellProps.AutoFillValue
                        .PrimaryKeyValue);
                }

                return Control.Value.Text != AutoFillCellProps.AutoFillValue.Text;
            }

            return false;
        }

        protected override void OnControlLoaded(AutoFillControl control, DataEntryGridCellProps cellProps)
        {
            AutoFillCellProps = (DataEntryGridAutoFillCellProps)cellProps;
            Control.Setup = AutoFillCellProps.AutoFillSetup;
            Control.Value = AutoFillCellProps.AutoFillValue;

            Control.ControlDirty += (sender, args) => OnControlDirty();
        }

        public override bool CanGridProcessKey(Key key)
        {
            switch (key)
            {
                case Key.Escape:
                case Key.Up:
                case Key.Down:
                    if (Control.ContainsBoxIsOpen)
                        return false;
                    break;
            }
            return base.CanGridProcessKey(key);
        }

        public override void ProcessValidationFail(DataEntryGridCellProps cellProps)
        {
            var autoFillCellProps = (DataEntryGridAutoFillCellProps) cellProps;
            Control.Value = autoFillCellProps.AutoFillValue;
            base.ProcessValidationFail(cellProps);
        }
    }
}
