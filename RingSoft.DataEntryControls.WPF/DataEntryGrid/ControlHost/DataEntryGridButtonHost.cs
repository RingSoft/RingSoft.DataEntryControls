using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridButtonHost : DataEntryGridControlHost<Button>
    {
        public override bool IsDropDownOpen => false;

        private bool _hasDataChanged;

        public DataEntryGridButtonHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridButtonCellProps(Row, ColumnId, Control.Content.ToString());
        }

        public override bool HasDataChanged()
        {
            return _hasDataChanged;
        }

        protected override void OnControlLoaded(Button control, DataEntryGridCellProps cellProps)
        {
            if (cellProps is DataEntryGridButtonCellProps buttonCellProps)
            {
                control.Content = buttonCellProps.ButtonContent;
            }

            control.Click += (sender, args) =>
            {
                _hasDataChanged = true;
                OnUpdateSource(cellProps);
                _hasDataChanged = false;
            };
        }
    }
}
