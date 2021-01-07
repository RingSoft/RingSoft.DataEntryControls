// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridCheckBoxCellProps : DataEntryGridEditingCellProps
    {
        public override string DataValue
        {
            get
            {
                var style = Row.GetCellStyle(ColumnId);
                var isVisible = true;
                var isEnabled = true;
                if (style is DataEntryGridControlCellStyle controlCellStyle)
                {
                    isEnabled = controlCellStyle.IsEnabled;
                    isVisible = controlCellStyle.IsVisible;
                }
                return $"{(isEnabled?"1":"0")}{(isVisible?"1":"0")}{(Value?"1":"0")}";
            }
        }

        public override int EditingControlId => DataEntryGridEditingCellProps.CheckBoxHostId;

        public bool Value { get; set; }

        public DataEntryGridCheckBoxCellProps(DataEntryGridRow row, int columnId, bool value) : base(row, columnId)
        {
            Value = value;
        }
    }
}
