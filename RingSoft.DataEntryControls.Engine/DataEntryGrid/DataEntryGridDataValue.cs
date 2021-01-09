using System;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridDataValue
    {
        public static string CheckDataValue
        {
            get
            {
                var dataValue = new DataEntryGridDataValue();
                return dataValue.CreateDataValue(new DataEntryGridControlCellStyle(), "");
            }

        }
        public bool IsVisible { get; private set; }
        public bool IsEnabled { get; private set; }
        public int DisplayStyleId { get; private set; }
        public string ControlValue { get; private set; }
        public string DataValue { get; private set; }

        public bool ProcessDataValueInput(string dataValue)
        {
            if (dataValue.IsNullOrEmpty())
                return true;

            if (dataValue.Length < CheckDataValue.Length)
                return false;

            DataValue = dataValue;
            IsVisible = dataValue[0].ToString().ToBool();
            IsEnabled = dataValue[1].ToString().ToBool();
            var semiIndex = dataValue.IndexOf(';');

            var displayStyleStr = dataValue.MidStr(2, semiIndex - 2);
            DisplayStyleId = displayStyleStr.ToInt();

            if (dataValue.Length > semiIndex)
                ControlValue = dataValue.RightStr(dataValue.Length - (semiIndex + 1));

            return true;
        }

        public string CreateDataValue(DataEntryGridRow row, int columnId, string controlValue)
        {
            var controlCellStyle = row.GetCellStyle(columnId) as DataEntryGridControlCellStyle;
            if (controlCellStyle == null)
                throw new ArgumentException($"'{row}' : Column Id '{columnId}' GetCellStyle must return a cell style that derives from {nameof(DataEntryGridControlCellStyle)}.");

            return CreateDataValue(controlCellStyle, controlValue);
        }

        public string CreateDataValue(DataEntryGridControlCellStyle controlCellStyle, string controlValue)
        {
            IsVisible = controlCellStyle.IsVisible;
            IsEnabled = controlCellStyle.IsEnabled;
            DisplayStyleId = controlCellStyle.DisplayStyleId;
            ControlValue = controlValue;

            DataValue = $"{(IsVisible ? "1" : "0")}{(IsEnabled ? "1" : "0")}{DisplayStyleId};{ControlValue}";
            return DataValue;
        }
    }
}
