// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
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

        public ComboBoxValueChangedTypes ChangeType { get; }

        public DataEntryGridComboBoxCellProps(DataEntryGridRow row, int columnId, DataEntryComboBoxSetup comboBoxSetup,
            ComboBoxItem selectedItem, ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(
            row, columnId)
        {
            ComboBoxSetup = comboBoxSetup;
            SelectedItem = selectedItem;
            ChangeType = changeType;
        }
    }
}
