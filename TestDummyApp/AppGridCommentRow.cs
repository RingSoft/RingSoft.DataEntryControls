using System.Collections.Generic;
using System.Drawing;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;


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
            BackgroundColor = Color.Green;
            ForegroundColor = Color.White;
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
                    if (!ParentRowId.IsNullOrEmpty())
                    {
                        result = new DataEntryGridTextCellProps(this, columnId);
                        if (Value == null)
                            result.Text = string.Empty;
                        else 
                            result.Text = "Comment";
                    }
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
                    if (Value != null)
                        result = new DataEntryGridButtonCellProps(this, columnId, "Edit Comment...");
                    else
                        result = new DataEntryGridTextCellProps(this, columnId);
                    result.Text = Comment;
                    break;
            }


            if (result != null)
                return result;

            return base.GetCellProps(columnId); 
        }

        public override void SetCellValue(DataEntryGridCellProps value)
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

        public override bool AllowEndEdit(DataEntryGridCellProps cellProps)
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
                    if (!ParentRowId.IsNullOrEmpty())
                        return new DataEntryGridCellStyle(){CellStyleType = DataEntryGridCellStyleTypes.ReadOnly};
                    break;
                case AppGridColumns.StockNumber:
                    if (Value == null)
                        return new DataEntryGridCellStyle()
                        {
                            ColumnHeader = "Comment",
                            CellStyleType = DataEntryGridCellStyleTypes.ReadOnly
                        };
                    break;
                case AppGridColumns.CheckBox:
                    return new DataEntryGridCheckBoxCellStyle{ControlVisible = false};
                case AppGridColumns.Button:
                    return new DataEntryGridButtonCellStyle{ControlVisible = false};
                default:
                    return new DataEntryGridCellStyle {CellStyleType = DataEntryGridCellStyleTypes.ReadOnly};
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
