using System.Collections.Generic;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum DataEntryGridDataActions
    {
        RowUpdated = 0,
        Refresh = 3
    }
    public class DataEntryGridDataChangedArgs
    {
        public DataEntryGridDataActions Action { get; }

        public int StartRowIndex { get; internal set; } = -1;

        public IReadOnlyList<DataEntryGridRow> AffectedRows { get; internal set; }

        internal DataEntryGridDataChangedArgs(DataEntryGridDataActions action)
        {
            Action = action;
        }
    }
}
