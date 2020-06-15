namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public interface IDataEntryGrid
    {
        bool CanUserAddRows { get; }

        void UpdateRow(DataEntryGridRow gridRow);

        void UpdateRow(DataEntryGridRow gridRow, int rowIndex);

        void RefreshDataSource();
    }
}
