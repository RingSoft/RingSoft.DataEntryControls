namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public interface IDataEntryGrid
    {
        bool DataEntryCanUserAddRows { get; set; }

        void UpdateRow(DataEntryGridRow gridRow);

        void UpdateRow(DataEntryGridRow gridRow, int rowIndex);

        void RefreshDataSource();

        void DataEntryGridCancelEdit();

        bool CommitCellEdit();

        void ResetGridFocus();

        void GotoCell(DataEntryGridRow row, int columnId);

        void SetBulkInsertMode(bool value = true);

        void TakeCellSnapshot(bool doOnlyWhenGridHasFocus = true);

        void RestoreCellSnapshot(bool doOnlyWhenGridHasFocus = true);
    }
}
