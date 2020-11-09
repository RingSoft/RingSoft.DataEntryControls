using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public interface IDataEntryGrid
    {
        bool CanUserAddRows { get; }

        void UpdateRow(DataEntryGridRow gridRow);

        void UpdateRow(DataEntryGridRow gridRow, int rowIndex);

        void RefreshDataSource();

        bool CancelEdit();

        bool CommitCellEdit(CellLostFocusTypes cellLostFocusType);

        void ResetGridFocus();

        void GotoCell(DataEntryGridRow row, int columnId);

        void SetBulkInsertMode(bool value = true);
    }
}
