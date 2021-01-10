using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace TestDummyApp
{
    public class LineTypeCellProps : DataEntryGridComboBoxCellProps
    {
        public override int EditingControlId => Globals.LineTypeControlId;

        public AppGridLineTypes LineType { get; set; }

        public LineTypeCellProps(DataEntryGridRow row, int columnId, AppGridLineTypes lineType, ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(row, columnId, changeType)
        {
            LineType = lineType;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            var dataValue = new DataEntryGridDataValue();
            return dataValue.CreateDataValue(row, columnId, ((int)LineType).ToString());
        }
    }
}
