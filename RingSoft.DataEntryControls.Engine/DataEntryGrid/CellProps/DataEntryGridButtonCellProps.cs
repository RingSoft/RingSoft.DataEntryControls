using System;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridButtonCellProps : DataEntryGridTextCellProps
    {
        public override int EditingControlId => DataEntryGridEditingCellProps.ButtonHostId;

        public DataEntryGridButtonCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            if (!controlMode)
                return Text;

            var cellStyle = row.GetCellStyle(columnId) as DataEntryGridButtonCellStyle;
            if (cellStyle == null)
                throw new Exception(GetCellStyleExceptionMessage(row, columnId));

            var dataValue = new DataEntryGridDataValue();
            dataValue.CreateDataValue(cellStyle, cellStyle.Content);
            return dataValue.DataValue;
        }

        public static string GetCellStyleExceptionMessage(DataEntryGridRow row, int columnId)
        {
            return
                $"Row '{row}' ColumnId '{columnId}' GetCellStyle must return a {nameof(DataEntryGridButtonCellStyle)} object.";
        }
    }
}
