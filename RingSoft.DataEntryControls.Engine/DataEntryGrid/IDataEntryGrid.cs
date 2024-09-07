// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="IDataEntryGrid.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Class ColumnMap.
    /// </summary>
    public class ColumnMap
    {
        /// <summary>
        /// Gets the column identifier.
        /// </summary>
        /// <value>The column identifier.</value>
        public int ColumnId { get; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnMap" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public ColumnMap(int id, string name)
        {
            ColumnId = id;
            ColumnName = name;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ColumnName;
        }
    }
    /// <summary>
    /// Connects grid manager to grid.
    /// </summary>
    public interface IDataEntryGrid
    {
        /// <summary>
        /// Gets or sets a value indicating whether [data entry can user add rows].
        /// </summary>
        /// <value><c>true</c> if [data entry can user add rows]; otherwise, <c>false</c>.</value>
        bool DataEntryCanUserAddRows { get; set; }

        /// <summary>
        /// Gets the current row.
        /// </summary>
        /// <value>The current row.</value>
        DataEntryGridRow CurrentRow { get; }

        /// <summary>
        /// Gets the index of the current row.
        /// </summary>
        /// <value>The index of the current row.</value>
        int CurrentRowIndex { get; }

        /// <summary>
        /// Gets the current column identifier.
        /// </summary>
        /// <value>The current column identifier.</value>
        int CurrentColumnId { get; }

        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="gridRow">The grid row.</param>
        void UpdateRow(DataEntryGridRow gridRow);

        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="gridRow">The grid row.</param>
        /// <param name="rowIndex">Index of the row.</param>
        void UpdateRow(DataEntryGridRow gridRow, int rowIndex);

        /// <summary>
        /// Refreshes the data source.
        /// </summary>
        void RefreshDataSource();

        /// <summary>
        /// Datas the entry grid cancel edit.
        /// </summary>
        void DataEntryGridCancelEdit();

        /// <summary>
        /// Commits the cell edit.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool CommitCellEdit();

        /// <summary>
        /// Resets the grid focus.
        /// </summary>
        void ResetGridFocus();

        /// <summary>
        /// Goes to the cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        void GotoCell(DataEntryGridRow row, int columnId);

        /// <summary>
        /// Sets the bulk insert mode.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        void SetBulkInsertMode(bool value = true);

        /// <summary>
        /// Takes the cell snapshot.
        /// </summary>
        /// <param name="doOnlyWhenGridHasFocus">if set to <c>true</c> [do only when grid has focus].</param>
        void TakeCellSnapshot(bool doOnlyWhenGridHasFocus = true);

        /// <summary>
        /// Restores the cell snapshot.
        /// </summary>
        /// <param name="doOnlyWhenGridHasFocus">if set to <c>true</c> [do only when grid has focus].</param>
        void RestoreCellSnapshot(bool doOnlyWhenGridHasFocus = true);

        /// <summary>
        /// Refreshes the grid view.
        /// </summary>
        void RefreshGridView();

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <returns>List&lt;ColumnMap&gt;.</returns>
        List<ColumnMap> GetColumns();
    }
}
