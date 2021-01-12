namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    public class DataEntryGridCustomControlCellProps : DataEntryGridEditingCellProps
    {
        public override int EditingControlId => ContentControlHostId;

        public int SelectedItemId { get; }

        public DataEntryGridCustomControlCellProps(DataEntryGridRow row, int columnId, int selectedItemId, ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(row, columnId)
        {
            SelectedItemId = selectedItemId;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            var dataValue = new DataEntryGridDataValue();
            return dataValue.CreateDataValue(row, columnId, SelectedItemId.ToString());
        }
    }
}
