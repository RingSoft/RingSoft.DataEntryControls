// ReSharper disable once CheckNamespace


// ReSharper disable once CheckNamespace

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum DataEntryGridCellStates
    {
        Enabled = 0,
        ReadOnly = 1,
        Disabled = 2
    }

    public class DataEntryGridCellStyle
    {
        public int DisplayStyleId { get; set; }

        public DataEntryGridCellStates State { get; set; }

        public string ColumnHeader { get; set; }
    }

    public class DataEntryGridControlCellStyle : DataEntryGridCellStyle
    {
        public bool IsEnabled
        {
            get
            {
                switch (State)
                {
                    case DataEntryGridCellStates.Disabled:
                    case DataEntryGridCellStates.ReadOnly:
                        return false;
                }

                return true;
            }
        }

        public bool IsVisible { get; set; } = true;
    }

    public class DataEntryGridButtonCellStyle : DataEntryGridControlCellStyle
    {
        public string Content { get; set; }
    }
}
