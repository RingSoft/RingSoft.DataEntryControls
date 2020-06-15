namespace RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps
{
    public enum ComboBoxValueChangedTypes
    {
        EndEdit = 0,
        SelectedItemChanged = 1
    }
    public class DataEntryGridComboBoxCellProps : DataEntryGridCellProps
    {
        public override int EditingControlId => ComboBoxHostId;

        public DataEntryComboBoxSetup ComboBoxSetup { get; }

        public ComboBoxItem SelectedItem { get; set; }

        public override string Text => SelectedItem.TextValue;

        public ComboBoxValueChangedTypes ChangeType { get; set; }

        public DataEntryGridComboBoxCellProps(DataEntryGridRow row, int columnId, DataEntryComboBoxSetup comboBoxSetup,
            ComboBoxItem selectedItem) : base(row, columnId)
        {
            ComboBoxSetup = comboBoxSetup;
            SelectedItem = selectedItem;
        }
    }
}
