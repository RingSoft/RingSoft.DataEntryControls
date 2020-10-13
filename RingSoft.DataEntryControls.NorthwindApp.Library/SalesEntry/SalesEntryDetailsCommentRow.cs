using System.Drawing;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsCommentRow : SalesEntryDetailsRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.Comment;

        public string Comment { get; set; }

        public GridMemoValue Value { get; private set; }

        public const int MaxCharactersPerLine = 20;


        public SalesEntryDetailsCommentRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            BackgroundColor = Color.Green;
            ForegroundColor = Color.White;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;

            return base.GetCellProps(columnId);
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
