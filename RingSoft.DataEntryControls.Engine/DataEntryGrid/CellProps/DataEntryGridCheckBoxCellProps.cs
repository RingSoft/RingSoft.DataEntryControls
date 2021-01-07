// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridCheckBoxCellProps : DataEntryGridEditingCellProps
    {
        public override string DataValue => new DataEntryGridDataValue(Row, ColumnId, Value.ToString()).DataValue;

        public override int EditingControlId => DataEntryGridEditingCellProps.CheckBoxHostId;

        public bool Value { get; set; }

        public DataEntryGridCheckBoxCellProps(DataEntryGridRow row, int columnId, bool value) : base(row, columnId)
        {
            Value = value;
        }
    }
}
