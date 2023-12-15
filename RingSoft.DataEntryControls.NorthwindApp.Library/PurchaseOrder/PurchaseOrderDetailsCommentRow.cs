using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System.Collections.Generic;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public class PurchaseOrderDetailsCommentRow : PurchaseOrderDetailsRow
    {
        public override PurchaseOrderDetailsLineTypes LineType => PurchaseOrderDetailsLineTypes.Comment;

        public override bool AllowUserDelete => Value != null;

        public string Comment { get; private set; }

        public bool CommentCrLf { get; private set; }

        public DataEntryGridMemoValue Value { get; private set; }

        public const int MaxCharactersPerLine = 35;

        public PurchaseOrderDetailsCommentRow(PurchaseOrderDetailsGridManager manager) : base(manager)
        {
            DisplayStyleId = AppGlobals.CommentDisplayStyleId;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;
            DataEntryGridCellProps result = null;

            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    if (Value == null && !IsNew)
                        result= new DataEntryGridTextCellProps(this, columnId);
                    break;
                case PurchaseOrderColumns.Item:
                    if (Value == null)
                        result = new DataEntryGridTextCellProps(this, columnId, Comment);
                    else
                        result = new DataEntryGridButtonCellProps(this, columnId) { Text = Comment };
                    break;
                case PurchaseOrderColumns.Received:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, Received);
                default:
                    result = new DataEntryGridTextCellProps(this, columnId);
                    break;
            }
            if (result != null)
                return result;

            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    if ((Value == null && !IsNew) || !string.IsNullOrEmpty(ParentRowId))
                        return new DataEntryGridControlCellStyle{State = DataEntryGridCellStates.ReadOnly};
                    break;
                case PurchaseOrderColumns.Item:
                    if (Value == null)
                    {
                        return new DataEntryGridCellStyle()
                        {
                            ColumnHeader = "Comment",
                            State = DataEntryGridCellStates.ReadOnly
                        };
                    }
                    else
                    {
                        return new DataEntryGridButtonCellStyle
                        {
                            ColumnHeader = "Comment",
                            Content = "Edit Comment..."
                        };
                    }
                case PurchaseOrderColumns.Received:
                    return new DataEntryGridControlCellStyle
                    {
                        State = DataEntryGridCellStates.ReadOnly,
                        IsVisible = false
                    };
                default:
                    return new DataEntryGridCellStyle() { State = DataEntryGridCellStates.ReadOnly };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (PurchaseOrderColumns) value.ColumnId;
            switch (column)
            {
                case PurchaseOrderColumns.Item:
                    if (PurchaseOrderDetailsManager.PurchaseOrderViewModel.PurchaseOrderView.ShowCommentEditor(Value))
                    {
                        UpdateFromValue();
                    }
                    else
                    {
                        value.OverrideCellMovement = true;
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override bool AllowEndEdit(DataEntryGridEditingCellProps cellProps)
        {
            var column = (PurchaseOrderColumns)cellProps.ColumnId;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    if (IsNew)
                    {
                        var nextRow = Manager.Rows[Manager.Rows.IndexOf(this) + 1];
                        var value = new DataEntryGridMemoValue(MaxCharactersPerLine);
                        if (PurchaseOrderDetailsManager.PurchaseOrderViewModel.PurchaseOrderView.ShowCommentEditor(value))
                        {
                            Value = value;
                            UpdateFromValue();
                            IsNew = false;
                            cellProps.OverrideCellMovement = true;
                            Manager.Grid.GotoCell(nextRow, cellProps.ColumnId);
                        }
                        return false;
                    }
                    break;
            }
            return base.AllowEndEdit(cellProps);
        }

        public void SetValue(string text)
        {
            if (Value == null)
                Value = new DataEntryGridMemoValue(MaxCharactersPerLine);

            Value.Text = text;

            UpdateFromValue();
        }

        public void SetValue(DataEntryGridMemoValue value)
        {
            Value = value;
            UpdateFromValue();
        }

        private void UpdateFromValue()
        {
            DeleteDescendants();
            var firstLine = true;
            foreach (var gridMemoValueLine in Value.Lines)
            {
                if (firstLine)
                {
                    Comment = gridMemoValueLine.Text;
                    CommentCrLf = gridMemoValueLine.CrLf;
                    Manager.Grid?.UpdateRow(this);
                    firstLine = false;
                }
                else
                {
                    var childCommentRow = new PurchaseOrderDetailsCommentRow(PurchaseOrderDetailsManager)
                    {
                        Comment = gridMemoValueLine.Text,
                        CommentCrLf = gridMemoValueLine.CrLf
                    };
                    AddChildRow(childCommentRow);
                }
            }
        }

        public override void AddContextMenuItems(List<DataEntryGridContextMenuItem> contextMenuItems, int columnId)
        {
            contextMenuItems.Add(new DataEntryGridContextMenuItem("_Edit Comment",
                    new RelayCommand<int>(h => EditComment(columnId)))
            { CommandParameter = columnId });
            base.AddContextMenuItems(contextMenuItems, columnId);
        }

        private void EditComment(int columnId)
        {
            var parentRow = this;
            if (!string.IsNullOrEmpty(ParentRowId))
            {
                var gridRow = GetParentRow();
                if (gridRow is PurchaseOrderDetailsCommentRow parentCommentRow)
                    parentRow = parentCommentRow;
            }

            if (parentRow == this)
            {
                if (PurchaseOrderDetailsManager.PurchaseOrderViewModel.PurchaseOrderView.ShowCommentEditor(Value))
                {
                    UpdateFromValue();
                    Manager.Grid.GotoCell(this, columnId);
                }
            }
            else
            {
                parentRow.EditComment(columnId);
            }
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(PurchaseDetails entity, int rowIndex)
        {
            entity.Comment = Comment;
            entity.CommentCrLf = CommentCrLf;

            base.SaveToEntity(entity, rowIndex);
        }

        public override void LoadFromEntity(PurchaseDetails entity)
        {
            var gridMemoValue = new DataEntryGridMemoValue(MaxCharactersPerLine);
            var commentCrLf = false;
            if (entity.CommentCrLf != null)
                commentCrLf = (bool) entity.CommentCrLf;

            gridMemoValue.AddLine(entity.Comment, commentCrLf);

            var children = GetDetailChildren(entity);
            foreach (var child in children)
            {
                commentCrLf = false;
                if (child.CommentCrLf != null)
                    commentCrLf = (bool)child.CommentCrLf;

                gridMemoValue.AddLine(child.Comment, commentCrLf);
            }
            SetValue(gridMemoValue);

            base.LoadFromEntity(entity);
        }
    }
}
