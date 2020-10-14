using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public abstract class DataEntryGridRow
    {
        public DataEntryGridManager Manager { get; }

        public string RowId { get; }

        public string ParentRowId { get; private set; }

        public abstract DataEntryGridCellProps GetCellProps(int columnId);

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public bool IsNew { get; internal set; }

        public DataEntryGridRow RowReplacedBy { get; internal set; }

        public virtual bool AllowUserDelete { get; } = true;

        public DataEntryGridRow(DataEntryGridManager manager)
        {
            Manager = manager;
            RowId = Guid.NewGuid().ToString();
        }

        public virtual void SetCellValue(DataEntryGridCellProps value)
        {
            var rowIndex = Manager.Rows.IndexOf(this);
            if (rowIndex >= 0)
                Manager.Grid.UpdateRow(this, rowIndex);

            IsNew = false;
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
        }
    }
}
