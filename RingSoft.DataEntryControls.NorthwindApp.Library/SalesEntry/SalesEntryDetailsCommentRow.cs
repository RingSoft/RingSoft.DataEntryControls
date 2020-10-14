using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using System.Drawing;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsCommentRow : SalesEntryDetailsRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.Comment;

        public string Comment { get; set; }

        public GridMemoValue Value { get; private set; }

        public const int MaxCharactersPerLine = 35;


        public SalesEntryDetailsCommentRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            BackgroundColor = Color.Green;
            ForegroundColor = Color.White;
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
                    if (Value == null)
                        return new DataEntryGridCellStyle()
                        {
                            ColumnHeader = "Comment",
                            CellStyle = DataEntryGridCellStyles.ReadOnly
                        };
                    break;
                default:
                    return new DataEntryGridCellStyle() { CellStyle = DataEntryGridCellStyles.ReadOnly };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (SalesEntryGridColumns)value.ColumnId;

            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    if (SalesEntryDetailsManager.ViewModel.SalesEntryView.ShowCommentEditor(Value))
                    {
                        UpdateFromValue();
                    }
                    else
                    {
                        value.ValidationResult = false;
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public void SetValue(string text)
        {
            if (Value == null)
                Value = new GridMemoValue(MaxCharactersPerLine);

            Value.Text = text;

            UpdateFromValue();
        }

        public void SetValue(GridMemoValue value)
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
                    Manager.Grid.UpdateRow(this);
                    firstLine = false;
                }
                else
                {
                    var childCommentRow = new SalesEntryDetailsCommentRow(SalesEntryDetailsManager)
                    {
                        Comment = gridMemoValueLine.Text
                    };
                    AddChildRow(childCommentRow);
                }
            }
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToOrderDetail(OrderDetails orderDetail)
        {
            throw new System.NotImplementedException();
        }
    }
}
