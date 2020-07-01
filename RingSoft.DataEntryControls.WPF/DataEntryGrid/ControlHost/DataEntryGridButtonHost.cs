using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridButtonHost : DataEntryGridControlHost<Button>
    {
        public DataEntryGridButtonCellProps ButtonCellProps { get; private set; }

        public override bool IsDropDownOpen => false;

        private bool _hasDataChanged;

        public DataEntryGridButtonHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return CellProps;
        }

        public override bool HasDataChanged()
        {
            return _hasDataChanged;
        }

        protected override void OnControlLoaded(Button control, DataEntryGridCellProps cellProps)
        {
            if (cellProps is DataEntryGridButtonCellProps buttonCellProps)
            {
                ButtonCellProps = buttonCellProps;
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
