using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System.Collections.Generic;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsCommentRow : SalesEntryDetailsRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.Comment;

        public override bool AllowUserDelete => Value != null;

        public string Comment { get; private set; }

        public bool CommentCrLf { get; private set; }

        public DataEntryGridMemoValue Value { get; private set; }

        public const int MaxCharactersPerLine = 35;


        public SalesEntryDetailsCommentRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            DisplayStyleId = AppGlobals.CommentDisplayStyleId;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            DataEntryGridCellProps result = null;
            
            switch (column)
            {
                case SalesEntryGridColumns.LineType:
                    if (!ParentRowId.IsNullOrEmpty())
                    {
                        result = new DataEntryGridTextCellProps(this, columnId);
                        if (Value == null)
                            result.Text = string.Empty;
                        else
                            result.Text = "Comment";
                    }
                    break;
                case SalesEntryGridColumns.Item:
                    if (Value != null)
                        result = new DataEntryGridButtonCellProps(this, columnId, "Edit Comment...");
                    else
                        result = new DataEntryGridTextCellProps(this, columnId);
                    result.Text = Comment;
                    break;
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
            var column = (SalesEntryGridColumns)columnId;
            switch (column)
            {
                case SalesEntryGridColumns.LineType:
                    break;
                case SalesEntryGridColumns.Item:
                    var style = new DataEntryGridCellStyle { ColumnHeader = "Comment" };
                    if (Value == null)
                    {
                        style.CellStyleType = DataEntryGridCellStyleTypes.ReadOnly;
                    }

                    return style;
                default:
                    return new DataEntryGridCellStyle() { CellStyleType = DataEntryGridCellStyleTypes.ReadOnly };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (SalesEntryGridColumns)value.ColumnId;

            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    if (SalesEntryDetailsManager.SalesEntryViewModel.SalesEntryView.ShowCommentEditor(Value))
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
                    var childCommentRow = new SalesEntryDetailsCommentRow(SalesEntryDetailsManager)
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
                if (gridRow is SalesEntryDetailsCommentRow parentCommentRow)
                    parentRow = parentCommentRow;
            }

            if (parentRow == this)
            {
                if (SalesEntryDetailsManager.SalesEntryViewModel.SalesEntryView.ShowCommentEditor(Value))
                {
                    UpdateFromValue();
                    Manager.Grid?.GotoCell(this, columnId);
                }
            }
            else
            {
                parentRow.EditComment(columnId);
            }
        }

        public override void LoadFromEntity(OrderDetails entity)
        {
            var gridMemoValue = new DataEntryGridMemoValue(MaxCharactersPerLine);
            gridMemoValue.AddLine(entity.Comment, entity.CommentCrLf);

            var children = GetDetailChildren(entity);
            foreach (var child in children)
            {
                gridMemoValue.AddLine(child.Comment, child.CommentCrLf);
            }
            SetValue(gridMemoValue);

            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(OrderDetails entity, int rowIndex)
        {
            entity.Comment = Comment;
            entity.CommentCrLf = CommentCrLf;
            base.SaveToEntity(entity, rowIndex);
        }
    }
}
