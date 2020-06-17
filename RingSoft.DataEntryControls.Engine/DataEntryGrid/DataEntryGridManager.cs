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

        protected abstract DataEntryGridRow GetNewRow();

        public void ClearRows(bool addRowToBottom = true)
        {
            Grid.CancelEdit();

            _rows.Clear();

            if (addRowToBottom)
                InsertNewRow();
        }

        public void InsertNewRow(int startIndex = -1)
        {
            if (Grid != null && Grid.CanUserAddRows)
                AddRow(GetNewRow(), startIndex);
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
            Grid.UpdateRow(newRow, index);
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

        public void RemoveRow(DataEntryGridRow rowToDelete)
        {
            var descendants = rowToDelete.GetDescendants();

            _rows.Remove(rowToDelete);

            foreach (var descendant in descendants)
            {
                _rows.Remove(descendant);
            }
        }
    }
}
