// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum ComboBoxValueChangedTypes
    {
        EndEdit = 0,
        SelectedItemChanged = 1
    }

    public abstract class DataEntryGridComboBoxCellProps : DataEntryGridEditingCellProps
    {
        public ComboBoxValueChangedTypes ChangeType { get; }

        protected DataEntryGridComboBoxCellProps(DataEntryGridRow row, int columnId,
            ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(row, columnId)
        {
            ChangeType = changeType;
        }
    }

    public class DataEntryGridTextComboBoxCellProps : DataEntryGridComboBoxCellProps
    {
        public override int EditingControlId => DataEntryGridEditingCellProps.ComboBoxHostId;

        public TextComboBoxControlSetup ComboBoxSetup { get; }

        public TextComboBoxItem SelectedItem { get; set; }

        public DataEntryGridTextComboBoxCellProps(DataEntryGridRow row, int columnId, TextComboBoxControlSetup comboBoxSetup,
            TextComboBoxItem selectedItem, ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(
            row, columnId, changeType)
        {
            ComboBoxSetup = comboBoxSetup;
            SelectedItem = selectedItem;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return SelectedItem?.TextValue;
        }
    }
}
