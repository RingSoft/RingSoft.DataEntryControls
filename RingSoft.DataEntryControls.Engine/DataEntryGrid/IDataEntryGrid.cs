using System.Collections.Generic;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class ColumnMap
    {
        public int ColumnId { get; }

        public string ColumnName { get; }

        public ColumnMap(int id, string name)
        {
            ColumnId = id;
            ColumnName = name;
        }

        public override string ToString()
        {
            return ColumnName;
        }
    }
    public interface IDataEntryGrid
    {
        bool DataEntryCanUserAddRows { get; set; }

        DataEntryGridRow CurrentRow { get; }

        int CurrentRowIndex { get; }

        int CurrentColumnId { get; }

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

        void RefreshGridView();

        List<ColumnMap> GetColumns();
    }
}
