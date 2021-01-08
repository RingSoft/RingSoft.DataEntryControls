// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridCheckBoxCellProps : DataEntryGridEditingCellProps
    {
        public override int EditingControlId => DataEntryGridEditingCellProps.CheckBoxHostId;

        public bool Value { get; set; }

        public DataEntryGridCheckBoxCellProps(DataEntryGridRow row, int columnId, bool value) : base(row, columnId)
        {
            Value = value;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            var dataValue = new DataEntryGridDataValue();
            dataValue.CreateDataValue(row, columnId, Value.ToString());

            return dataValue.DataValue;
        }
    }
}
