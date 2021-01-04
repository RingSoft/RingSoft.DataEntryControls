// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridCheckBoxCellProps : DataEntryGridEditingCellProps
    {
        public override string DataValue => Value.ToString();

        public override int EditingControlId => DataEntryGridEditingCellProps.CheckBoxHostId;

        public bool Value { get; set; }

        public DataEntryGridCheckBoxCellProps(DataEntryGridRow row, int columnId, bool value) : base(row, columnId)
        {
            Value = value;
        }
    }
}
