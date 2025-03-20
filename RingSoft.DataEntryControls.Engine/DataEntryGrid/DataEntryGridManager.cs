// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-22-2024
// ***********************************************************************
// <copyright file="DataEntryGridManager.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Data Entry Grid Manager
    /// </summary>
    public abstract class DataEntryGridManager
    {
        /// <summary>
        /// The rows
        /// </summary>
        private ObservableCollection<DataEntryGridRow> _rows = new ObservableCollection<DataEntryGridRow>();

        /// <summary>
        /// Gets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public ReadOnlyObservableCollection<DataEntryGridRow> Rows { get; }

        /// <summary>
        /// Gets the grid.
        /// </summary>
        /// <value>The grid.</value>
        public IDataEntryGrid Grid { get; private set; }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public List<ColumnMap> Columns { get; private set; }

        /// <summary>
        /// Fired when row changes.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> RowsChanged;

        /// <summary>
        /// The initialize row
        /// </summary>
        private DataEntryGridRow _initRow;
        /// <summary>
        /// The initialize column identifier
        /// </summary>
        private int _initColumnId = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridManager" /> class.
        /// </summary>
        public DataEntryGridManager()
        {
            _rows.CollectionChanged += _rows_CollectionChanged;
            Rows = new ReadOnlyObservableCollection<DataEntryGridRow>(_rows);
        }

        /// <summary>
        /// Handles the CollectionChanged event of the _rows control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        private void _rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnRowsChanged(e);
        }

        /// <summary>
        /// Handles the <see cref="E:RowsChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            RowsChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Setups the grid.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public void SetupGrid(IDataEntryGrid grid)
        {
            Grid = grid;
            
            Initialize();
            InsertNewRow();

            if (_initRow != null && _initColumnId != -1)
            {
                Grid.GotoCell(_initRow, _initColumnId);
                _initRow = null;
                _initColumnId = -1;
            }
        }

        /// <summary>
        /// Sets grid cell focus.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        public void GotoCell(DataEntryGridRow row, int columnId)
        {
            if (Grid == null)
            {
                _initRow = row;
                _initColumnId = columnId;
            }
            else
            {
                Grid.GotoCell(row, columnId);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected virtual void Initialize()
        {
            if (Grid != null) Columns = Grid.GetColumns();
        }

        /// <summary>
        /// Raises the dirty flag.
        /// </summary>
        public virtual void RaiseDirtyFlag()
        {

        }

        /// <summary>
        /// Creates a new row.
        /// </summary>
        /// <returns>DataEntryGridRow.</returns>
        protected abstract DataEntryGridRow GetNewRow();

        /// <summary>
        /// Clears the rows.
        /// </summary>
        /// <param name="addRowToBottom">if set to <c>true</c> [add row to bottom].</param>
        protected virtual void ClearRows(bool addRowToBottom = true)
        {
            Grid?.DataEntryGridCancelEdit();

            _rows.Clear();

            if (Grid != null)
                addRowToBottom = addRowToBottom && Grid.DataEntryCanUserAddRows;

            if (addRowToBottom)
                InsertNewRow();
        }

        /// <summary>
        /// Setups for new record.
        /// </summary>
        public void SetupForNewRecord()
        {
            ClearRows();

            if (Grid != null && Grid.DataEntryCanUserAddRows)
                Grid.ResetGridFocus();
        }

        /// <summary>
        /// Preload grid from entity.
        /// </summary>
        protected void PreLoadGridFromEntity()
        {
            Grid?.TakeCellSnapshot(false);
            ClearRows(false);
        }

        /// <summary>
        /// Postloads grid from entity.
        /// </summary>
        protected void PostLoadGridFromEntity()
        {
            InsertNewRow();
            Grid?.RestoreCellSnapshot(false);
        }

        /// <summary>
        /// Determines whether this instance [can insert row] the specified start index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <returns><c>true</c> if this instance [can insert row] the specified start index; otherwise, <c>false</c>.</returns>
        protected virtual bool CanInsertRow(int startIndex)
        {
            return true;
        }

        /// <summary>
        /// Inserts the new row.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        public void InsertNewRow(int startIndex = -1)
        {
            if (Grid != null && Grid.DataEntryCanUserAddRows && CanInsertRow(startIndex))
            {
                var newRow = GetNewRow();
                newRow.IsNew = true;
                AddRow(newRow, startIndex);
            }
        }

        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="newRow">The new row.</param>
        /// <param name="startIndex">The start index.</param>
        public void AddRow(DataEntryGridRow newRow, int startIndex = -1)
        {
            if (startIndex < 0)
                _rows.Add(newRow);
            else
                _rows.Insert(startIndex, newRow);
        }

        /// <summary>
        /// Replaces the row.
        /// </summary>
        /// <param name="existingRow">The existing row.</param>
        /// <param name="newRow">The new row.</param>
        public void ReplaceRow(DataEntryGridRow existingRow, DataEntryGridRow newRow)
        {
            existingRow.DeleteDescendants();
            var index = _rows.IndexOf(existingRow);

            _rows[index] = newRow;
            Grid?.UpdateRow(newRow, index);
            existingRow.RowReplacedBy = newRow;
        }

        /// <summary>
        /// Validates the index of the row.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <exception cref="System.Exception">Row index: {rowIndex} is outside the Rows collection.</exception>
        private void ValidateRowIndex(int rowIndex)
        {
            if (rowIndex > _rows.Count - 1)
                throw new Exception($"Row index: {rowIndex} is outside the Rows collection.");
        }

        /// <summary>
        /// Removes the row.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        public void RemoveRow(int rowIndex)
        {
            ValidateRowIndex(rowIndex);

            RemoveRow(_rows[rowIndex]);
        }

        /// <summary>
        /// Removes the row.
        /// </summary>
        /// <param name="rowToDelete">The row to delete.</param>
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

        /// <summary>
        /// Determines whether is delete ok at the specified row index.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns><c>true</c> if [is delete ok] [the specified row index]; otherwise, <c>false</c>.</returns>
        public virtual bool IsDeleteOk(int rowIndex)
        {
            var deleteOk = rowIndex < Rows.Count - 1 && rowIndex >= 0;
            return deleteOk;
        }

        //Peter Ringering - 11/22/2024 08:58:12 PM - E-78
        /// <summary>
        /// Called when [deleting row].
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool OnDeletingRow(int rowIndex)
        {
            var row = Rows[rowIndex];
            if (!row.IsNew)
            {
                return row.OnDeletingRow();
            }

            return true;
        }
    }
}
