﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public abstract class DataEntryGridRow : IDisposable
    {
        public DataEntryGridManager Manager { get; }

        public string RowId { get; }

        public string ParentRowId { get; private set; }

        public abstract DataEntryGridCellProps GetCellProps(int columnId);

        public int DisplayStyleId { get; set; }

        public bool IsNew { get; set; }

        public DataEntryGridRow RowReplacedBy { get; internal set; }

        public virtual bool AllowUserDelete { get; } = true;

        public DataEntryGridRow(DataEntryGridManager manager)
        {
            Manager = manager;
            RowId = Guid.NewGuid().ToString();
        }

        public virtual void SetCellValue(DataEntryGridEditingCellProps value)
        {
            IsNew = false;
            var rowIndex = Manager.Rows.IndexOf(this);
            if (rowIndex >= 0)
                Manager.Grid.UpdateRow(this, rowIndex);
        }

        public virtual DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle();
        }

        public bool HasChildren()
        {
            return Manager.Rows.Any(a => a.ParentRowId == RowId);
        }

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

        public DataEntryGridRow GetParentRow()
        {
            return Manager.Rows.FirstOrDefault(f => f.RowId == ParentRowId);
        }

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

        public void DeleteDescendants()
        {
            var descendants = GetDescendants();
            foreach (var gridRow in descendants)
            {
                Manager.RemoveRow(gridRow);
            }
            Manager.RaiseDirtyFlag();
        }

        public virtual void AddContextMenuItems(List<DataEntryGridContextMenuItem> contextMenuItems, int columnId)
        {

        }

        public virtual bool AllowEndEdit(DataEntryGridEditingCellProps cellProps)
        {
            return true;
        }

        public virtual void Dispose()
        {
            
        }
    }
}
