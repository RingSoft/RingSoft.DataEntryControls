using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public class DataEntryGridButtonHost : DataEntryGridEditingControlHost<Button>
    {
        public override bool IsDropDownOpen => false;

        private bool _hasDataChanged;

        public DataEntryGridButtonHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridButtonCellProps(Row, ColumnId);
        }

        public override bool HasDataChanged()
        {
            return _hasDataChanged;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            
        }

        protected override void OnControlLoaded(Button control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            if (cellStyle is DataEntryGridButtonCellStyle buttonCellStyle)
            {
                Control.Content = buttonCellStyle.Content;
            }
            else
            {
                throw new Exception(
                    DataEntryGridButtonCellProps.GetCellStyleExceptionMessage(cellProps.Row, cellProps.ColumnId));
            }

            control.Click += (sender, args) =>
            {
                _hasDataChanged = true;
                OnUpdateSource(cellProps);
                _hasDataChanged = false;
            };
        }

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
        }
    }
}
