using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public abstract class DataEntryGridManager
    {
        private ObservableCollection<DataEntryGridRow> _rows = new ObservableCollection<DataEntryGridRow>();

        public ReadOnlyObservableCollection<DataEntryGridRow> Rows { get; }

        public IDataEntryGrid Grid { get; private set; }

        public event EventHandler<NotifyCollectionChangedEventArgs> RowsChanged;
        public DataEntryGridManager()
        {
            _rows.CollectionChanged += _rows_CollectionChanged;
            Rows = new ReadOnlyObservableCollection<DataEntryGridRow>(_rows);
        }

        private void _rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnRowsChanged(e);
        }

        protected virtual void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            RowsChanged?.Invoke(this, e);
        }

        public void SetupGrid(IDataEntryGrid grid)
        {
            Grid = grid;
            
            Initialize();
            InsertNewRow();
        }

        protected virtual void Initialize()
        {
        }

        public virtual void RaiseDirtyFlag()
        {

        }

        protected abstract DataEntryGridRow GetNewRow();

        private void ClearRows(bool addRowToBottom = true)
        {
            Grid?.DataEntryGridCancelEdit();

            _rows.Clear();

            if (Grid != null)
                addRowToBottom = addRowToBottom && Grid.DataEntryCanUserAddRows;

            if (addRowToBottom)
                InsertNewRow();
        }

        public void SetupForNewRecord()
        {
            ClearRows();

            if (Grid != null && Grid.DataEntryCanUserAddRows)
                Grid.ResetGridFocus();
        }

        protected void PreLoadGridFromEntity()
        {
            Grid?.TakeCellSnapshot(false);
            ClearRows(false);
        }

        protected void PostLoadGridFromEntity()
        {
            InsertNewRow();
            Grid?.RestoreCellSnapshot(false);
        }

        public void InsertNewRow(int startIndex = -1)
        {
            if (Grid != null && Grid.DataEntryCanUserAddRows)
            {
                var newRow = GetNewRow();
                newRow.IsNew = true;
                AddRow(newRow, startIndex);
            }
        }

        public void AddRow(DataEntryGridRow newRow, int startIndex = -1)
        {
            if (startIndex < 0)
                _rows.Add(newRow);
            else
                _rows.Insert(startIndex, newRow);
        }

        public void ReplaceRow(DataEntryGridRow existingRow, DataEntryGridRow newRow)
        {
            existingRow.DeleteDescendants();
            var index = _rows.IndexOf(existingRow);

            _rows[index] = newRow;
            Grid?.UpdateRow(newRow, index);
            existingRow.RowReplacedBy = newRow;
        }

        private void ValidateRowIndex(int rowIndex)
        {
            if (rowIndex > _rows.Count - 1)
                throw new Exception($"Row index: {rowIndex} is outside the Rows collection.");
        }
        public void RemoveRow(int rowIndex)
        {
            ValidateRowIndex(rowIndex);

            RemoveRow(_rows[rowIndex]);
        }

        public virtual void RemoveRow(DataEntryGridRow rowToDelete)
        {
            var descendants = rowToDelete.GetDescendants();

            rowToDelete.Dispose();
            _rows.Remove(rowToDelete);

            foreach (var descendant in descendants)
            {
                descendant.Dispose();
                _rows.Remove(descendant);
            }
            Grid?.RefreshGridView();
        }
    }
}
