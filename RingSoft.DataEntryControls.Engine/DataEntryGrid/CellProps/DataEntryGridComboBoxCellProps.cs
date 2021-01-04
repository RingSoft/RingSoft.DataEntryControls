// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum ComboBoxValueChangedTypes
    {
        EndEdit = 0,
        SelectedItemChanged = 1
    }
    public class DataEntryGridComboBoxCellProps : DataEntryGridEditingCellProps
    {
        public override string DataValue => SelectedItem?.TextValue;

        public override int EditingControlId => DataEntryGridEditingCellProps.ComboBoxHostId;

        public ComboBoxControlSetup ComboBoxSetup { get; }

        public ComboBoxItem SelectedItem { get; set; }

        public ComboBoxValueChangedTypes ChangeType { get; }

        public DataEntryGridComboBoxCellProps(DataEntryGridRow row, int columnId, ComboBoxControlSetup comboBoxSetup,
            ComboBoxItem selectedItem, ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(
            row, columnId)
        {
            ComboBoxSetup = comboBoxSetup;
            SelectedItem = selectedItem;
            ChangeType = changeType;
        }
    }
}
