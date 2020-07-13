using RingSoft.DataEntryControls.Engine;
using System.Drawing;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;

namespace RingSoft.DataEntryControls.App.WPF
{
    public class AppGridCommentRow : AppGridRow
    {
        public override AppGridLineTypes LineType => AppGridLineTypes.Comment;

        public string Comment { get; set; }

        public GridMemoValue Value { get; private set; }

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

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (AppGridColumns) columnId;

            switch (column)
            {
                case AppGridColumns.Disabled:
                    break;
                case AppGridColumns.LineType:
                    if (!ParentRowId.IsNullOrEmpty())
                        return new DataEntryGridCellStyle(){CellStyle = DataEntryGridCellStyles.ReadOnly};
                    break;
                case AppGridColumns.StockNumber:
                    if (Value == null)
                        return new DataEntryGridCellStyle()
                        {
                            ColumnHeader = "Comment",
                            CellStyle = DataEntryGridCellStyles.ReadOnly
                        };
                    break;
                default:
                    return new DataEntryGridCellStyle(){CellStyle = DataEntryGridCellStyles.ReadOnly};
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
    }
}
