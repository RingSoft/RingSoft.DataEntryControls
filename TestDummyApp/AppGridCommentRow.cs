using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Collections.Generic;


namespace TestDummyApp
{
    public class AppGridCommentRow : AppGridRow
    {
        public override AppGridLineTypes LineType => AppGridLineTypes.Comment;

        public string Comment { get; set; }

        public DataEntryGridMemoValue Value { get; private set; }

        public const int MaxCharactersPerLine = 20;

        public AppGridCommentRow(AppGridManager manager) : base(manager)
        {
            DisplayStyleId = Globals.CommentDisplayStyleId;
        }

        public override void LoadSale_DetailRow(SaleDetail saleDetail)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AppGridColumns) columnId;
            DataEntryGridCellProps result = null;

            switch (column)
            {
                case AppGridColumns.LineType:
                    //if (!ParentRowId.IsNullOrEmpty())
                    //{
                    //    var text = string.Empty;
                    //    if (Value != null)
                    //        text = "Comment";

                    //    result = new DataEntryGridTextCellProps(this, columnId, text);
                    //}
                    break;
                case AppGridColumns.Disabled:
                case AppGridColumns.CheckBox:
                case AppGridColumns.Location:
                case AppGridColumns.Price:
                case AppGridColumns.Date:
                case AppGridColumns.Integer:
                    result = new DataEntryGridTextCellProps(this, columnId);
                    break;
                case AppGridColumns.StockNumber:
                    if (Value == null)
                        result = new DataEntryGridTextCellProps(this, columnId, Comment);
                    else
                        result = new DataEntryGridButtonCellProps(this, columnId) { Text = Comment };
                    break;
            }


            if (result != null)
                return result;

            return base.GetCellProps(columnId); 
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (AppGridColumns)value.ColumnId;

            switch (column)
            {
                case AppGridColumns.StockNumber:
                    if (AppGridManager.UserInterface.ShowGridMemoEditor(Value))
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
            var column = (AppGridColumns) cellProps.ColumnId;
            if (column == AppGridColumns.LineType && IsNew)
            {
                var gridMemoValue = new DataEntryGridMemoValue(AppGridCommentRow.MaxCharactersPerLine);
                if (AppGridManager.UserInterface.ShowGridMemoEditor(gridMemoValue))
                {
                    SetValue(gridMemoValue);
                    IsNew = false;
                }
                else
                {
                    return false;
                }
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

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (AppGridColumns) columnId;

            switch (column)
            {
                case AppGridColumns.Disabled:
                    break;
                case AppGridColumns.LineType:
                    if (Value == null)
                        return new DataEntryGridControlCellStyle()
                        {
                            State = DataEntryGridCellStates.ReadOnly,
                            IsVisible = false
                        };
                    break;
                case AppGridColumns.StockNumber:
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
                
                case AppGridColumns.CheckBox:
                    return new DataEntryGridControlCellStyle
                    {
                        IsVisible = false,
                        State = DataEntryGridCellStates.ReadOnly
                    };
                case AppGridColumns.Button:
                    return new DataEntryGridButtonCellStyle
                    {
                        IsVisible = false,
                        State = DataEntryGridCellStates.ReadOnly,
                    };
                default:
                    return new DataEntryGridCellStyle {State = DataEntryGridCellStates.ReadOnly};
            }
            return base.GetCellStyle(columnId);
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
                    Manager.Grid.UpdateRow(this);
                    firstLine = false;
                }
                else
                {
                    var childCommentRow = new AppGridCommentRow(AppGridManager)
                    {
                        Comment = gridMemoValueLine.Text
                    };
                    AddChildRow(childCommentRow);
                }
            }
        }

        public override void AddContextMenuItems(List<DataEntryGridContextMenuItem> contextMenuItems, int columnId)
        {
            contextMenuItems.Add(new DataEntryGridContextMenuItem("_Edit Comment",
                new RelayCommand<int>(h => EditComment(columnId))){CommandParameter = columnId});
            base.AddContextMenuItems(contextMenuItems, columnId);
        }

        private void EditComment(int columnId)
        {
            var parentRow = this;
            if (!string.IsNullOrEmpty(ParentRowId))
            {
                var gridRow = GetParentRow();
                if (gridRow is AppGridCommentRow parentCommentRow)
                    parentRow = parentCommentRow;
            }

            if (parentRow == this)
            {
                if (AppGridManager.UserInterface.ShowGridMemoEditor(Value))
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
    }
}
