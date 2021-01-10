using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;

namespace TestDummyApp
{
    public class LineTypeControlHost : DataEntryGridEditingControlHost<LineTypeControl>
    {
        private AppGridLineTypes _lineType;
        private ComboBoxValueChangedTypes _valueChangeType;

        public LineTypeControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override bool IsDropDownOpen => Control.ComboBox.IsDropDownOpen;

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new LineTypeCellProps(Row, ColumnId, Control.LineType, _valueChangeType);
        }

        public override bool HasDataChanged()
        {
            return _lineType != Control.LineType;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is LineTypeCellProps lineTypeCellProps)
            {
                _lineType = Control.LineType = lineTypeCellProps.LineType;
            }
        }

        protected override void OnControlLoaded(LineTypeControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            UpdateFromCellProps(cellProps);
            if (cellProps is LineTypeCellProps lineTypeCellProps)
            {
                _valueChangeType = lineTypeCellProps.ChangeType;
            }
        }

        public override bool CanGridProcessKey(Key key)
        {
            return base.CanGridProcessKey(key);
        }
    }
}
