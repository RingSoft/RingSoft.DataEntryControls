using System;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridDataValue
    {
        public bool IsVisible { get; private set; }
        public bool IsEnabled { get; private set; }
        public int DisplayStyleId { get; private set; }
        public string ControlValue { get; private set; }
        public string DataValue { get; private set; }

        public DataEntryGridDataValue()
        {
            
        }

        public void CreateDataValue(DataEntryGridRow row, int columnId, string controlValue)
        {
            var controlCellStyle = row.GetCellStyle(columnId) as DataEntryGridControlCellStyle;
            if (controlCellStyle == null)
                throw new ArgumentException($"'{row}' : Column Id '{columnId}' GetCellStyle must return a cell style that derives from {nameof(DataEntryGridControlCellStyle)}.");

            CreateDataValue(controlCellStyle, controlValue);
        }

        public DataEntryGridDataValue(string dataValue)
        {
            if (dataValue.IsNullOrEmpty())
                return;

            DataValue = dataValue;
            IsVisible = dataValue[0].ToString().ToBool();
            IsEnabled = dataValue[1].ToString().ToBool();
            var semiIndex = dataValue.IndexOf(';');

            var displayStyleStr = dataValue.MidStr(2, semiIndex - 2);
            DisplayStyleId = displayStyleStr.ToInt();

            ControlValue = dataValue.RightStr(dataValue.Length - (semiIndex + 1));
        }

        public void CreateDataValue(DataEntryGridControlCellStyle controlCellStyle, string controlValue)
        {
            IsVisible = controlCellStyle.IsVisible;
            IsEnabled = controlCellStyle.IsEnabled;
            DisplayStyleId = controlCellStyle.DisplayStyleId;
            ControlValue = controlValue;

            DataValue = $"{(IsVisible ? "1" : "0")}{(IsEnabled ? "1" : "0")}{DisplayStyleId};{ControlValue}";
        }
    }
}
