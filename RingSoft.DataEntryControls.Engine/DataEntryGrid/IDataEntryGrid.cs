namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public interface IDataEntryGrid
    {
        bool CanUserAddRows { get; }

        void UpdateRow(DataEntryGridRow gridRow);

        void UpdateRow(DataEntryGridRow gridRow, int rowIndex);

        void RefreshDataSource();

        bool CancelEdit(bool skipValidation);

        bool CommitEdit();

        void ResetGridFocus();

        void GotoCell(DataEntryGridRow row, int columnId);

        void SetBulkInsertMode(bool value = true);
    }
}
