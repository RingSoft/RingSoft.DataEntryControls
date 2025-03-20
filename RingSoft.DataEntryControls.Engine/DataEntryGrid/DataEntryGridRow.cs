﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-22-2024
// ***********************************************************************
// <copyright file="DataEntryGridRow.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Manages a data entry grid row.
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <seealso cref="IDisposable" />
    public abstract class DataEntryGridRow : IDisposable
    {
        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public DataEntryGridManager Manager { get; }

        /// <summary>
        /// Gets the row identifier.
        /// </summary>
        /// <value>The row identifier.  This is a GUID.</value>
        public string RowId { get; }

        /// <summary>
        /// Gets the parent row identifier.
        /// </summary>
        /// <value>The parent row identifier.  This is a GUID</value>
        public string ParentRowId { get; private set; }

        /// <summary>
        /// Gets the cell props.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellProps.</returns>
        public abstract DataEntryGridCellProps GetCellProps(int columnId);

        /// <summary>
        /// Gets or sets the display style identifier.
        /// </summary>
        /// <value>The display style identifier.</value>
        public virtual int DisplayStyleId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is new.
        /// </summary>
        /// <value><c>true</c> if this instance is new; otherwise, <c>false</c>.</value>
        private bool _isNew;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is new.
        /// </summary>
        /// <value><c>true</c> if this instance is new; otherwise, <c>false</c>.</value>
        public bool IsNew
        {
            get { return _isNew; }
            set
            {
                OnIsNewChanged(_isNew, value);
                _isNew = value;
            }
        }


        /// <summary>
        /// Gets the row replaced by.
        /// </summary>
        /// <value>The row replaced by.</value>
        public DataEntryGridRow RowReplacedBy { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether allow user delete.
        /// </summary>
        /// <value><c>true</c> if allow user delete; otherwise, <c>false</c>.</value>
        public virtual bool AllowUserDelete { get; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridRow" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public DataEntryGridRow(DataEntryGridManager manager)
        {
            Manager = manager;
            RowId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Called when [is new changed].
        /// </summary>
        /// <param name="oldValue">if set to <c>true</c> [old value].</param>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        public virtual void OnIsNewChanged(bool oldValue, bool newValue)
        {

        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void SetCellValue(DataEntryGridEditingCellProps value)
        {
            IsNew = false;
            var rowIndex = Manager.Rows.IndexOf(this);
            if (rowIndex >= 0)
                if (Manager.Grid != null)
                    Manager.Grid.UpdateRow(this, rowIndex);
        }

        /// <summary>
        /// Gets the cell style.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellStyle.</returns>
        public virtual DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle();
        }

        /// <summary>
        /// Determines whether this instance has children.
        /// </summary>
        /// <returns><c>true</c> if this instance has children; otherwise, <c>false</c>.</returns>
        public bool HasChildren()
        {
            return Manager.Rows.Any(a => a.ParentRowId == RowId);
        }

        /// <summary>
        /// Adds a child row to grid.
        /// </summary>
        /// <param name="childRow">The child row.</param>
        /// <exception cref="System.Exception">This row must be added to the Rows collection before child rows can be added to it.</exception>
        public void AddChildRow(DataEntryGridRow childRow)
        {
            if (Manager.Rows.IndexOf(this) < 0)
                throw new Exception(
                    "This row must be added to the Rows collection before child rows can be added to it.");

            var rowIndex = Manager.Rows.IndexOf(this) + 1;
            rowIndex += GetDescendants().Count;

            childRow.ParentRowId = RowId;
            Manager.AddRow(childRow, rowIndex);
            Manager.RaiseDirtyFlag();
        }

        /// <summary>
        /// Gets the descendants.
        /// </summary>
        /// <returns>List&lt;DataEntryGridRow&gt;.</returns>
        public List<DataEntryGridRow> GetDescendants()
        {
            var result = new List<DataEntryGridRow>();
            var descendants = Manager.Rows.Where(w => w.ParentRowId == RowId);
            foreach (var childRow in descendants)
            {
                result.Add(childRow);
                var grandChildren = childRow.GetDescendants();
                foreach (var grandChildRow in grandChildren)
                {
                    result.Add(grandChildRow);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the parent row.
        /// </summary>
        /// <returns>DataEntryGridRow.</returns>
        public DataEntryGridRow GetParentRow()
        {
            return Manager.Rows.FirstOrDefault(f => f.RowId == ParentRowId);
        }

        /// <summary>
        /// Gets the oldest ancestor row.
        /// </summary>
        /// <returns>DataEntryGridRow.</returns>
        public DataEntryGridRow GetOldestAncestorRow()
        {
            var ancestorRow = GetParentRow();
            if (ancestorRow != null)
            {
                var grandparentRow = ancestorRow.GetOldestAncestorRow();
                if (grandparentRow != null)
                    ancestorRow = grandparentRow;
            }

            return ancestorRow;
        }

        /// <summary>
        /// Deletes the descendants.
        /// </summary>
        public void DeleteDescendants()
        {
            var descendants = GetDescendants();
            foreach (var gridRow in descendants)
            {
                Manager.RemoveRow(gridRow);
            }
            Manager.RaiseDirtyFlag();
        }

        /// <summary>
        /// Adds the context menu items.
        /// </summary>
        /// <param name="contextMenuItems">The context menu items.</param>
        /// <param name="columnId">The column identifier.</param>
        public virtual void AddContextMenuItems(List<DataEntryGridContextMenuItem> contextMenuItems, int columnId)
        {

        }

        /// <summary>
        /// Allows the end edit.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool AllowEndEdit(DataEntryGridEditingCellProps cellProps)
        {
            return true;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public virtual void Dispose()
        {
            
        }

        //Peter Ringering - 11/22/2024 09:03:21 PM - E-78
        /// <summary>
        /// Called when [deleting row].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool OnDeletingRow()
        {
            return true;
        }
    }
}
