using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class DataEntryGridAutoFillCellProps : DataEntryGridCellProps
    {
        public AutoFillSetup AutoFillSetup { get; }

        public AutoFillValue AutoFillValue { get; }

        public const int AutoFillControlHostId = 50;

        public DataEntryGridAutoFillCellProps(DataEntryGridRow row, int columnId, AutoFillSetup setup, AutoFillValue value) : base(row, columnId)
        {
            AutoFillSetup = setup;
            AutoFillValue = value;
        }

        public override int EditingControlId => AutoFillControlHostId;
    }
}
