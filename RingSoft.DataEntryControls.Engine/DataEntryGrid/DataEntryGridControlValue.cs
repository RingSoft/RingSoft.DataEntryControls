namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridControlValue
    {
        public bool IsEnabled { get; }

        public bool IsVisible { get; }

        public int DisplayStyleId { get; }

        public string CellValue { get; }

        public string DataValue { get; }

        public DataEntryGridControlValue(DataEntryGridControlCellStyle cellStyle, string cellValue)
        {
            IsEnabled = cellStyle.IsEnabled;
            IsVisible = cellStyle.IsVisible;
            DisplayStyleId = cellStyle.DisplayStyleId;
            CellValue = cellValue;

            var visibleBit = IsVisible ? "1" : "0";
            var enabledBit = IsEnabled ? "1" : "0";
            DataValue = $"{visibleBit}{enabledBit}{DisplayStyleId};{CellValue}";
        }

        public DataEntryGridControlValue(string dataValue)
        {
            DataValue = dataValue;
            var semiIndex = dataValue.IndexOf(';');
            //if (dataValue.Length < 5 || semiIndex < 0)
            //    throw new Exception("Unable to parse DataValue.  Invalid format string.");

            IsVisible = dataValue[0].ToString().ToBool();
            IsEnabled = dataValue[1].ToString().ToBool();
            var displayStyleText = dataValue.MidStr(2, semiIndex - 2);
            DisplayStyleId = displayStyleText.ToInt();
            var cellValueIndex = dataValue.Length - (semiIndex + 1);
            CellValue = dataValue.RightStr(cellValueIndex);
        }
    }
}
