using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsSpecialOrderRow : SalesEntryDetailsValueRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.SpecialOrder;

        public string SpecialOrderText { get; set; }

        public SalesEntryDetailsSpecialOrderRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            DisplayStyleId = SalesEntryDetailsGridManager.SpecialOrderDisplayId;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns)columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    return new DataEntryGridTextCellProps(this, columnId)
                    {
                        Text = SpecialOrderText,
                        MaxLength = AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.SpecialOrderText)
                            .MaxLength
                    };
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (SalesEntryGridColumns)columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    return new DataEntryGridCellStyle { ColumnHeader = "Special Order" };
                case SalesEntryGridColumns.ExtendedPrice:
                case SalesEntryGridColumns.Discount:
                    return new DataEntryGridCellStyle { CellStyleType = DataEntryGridCellStyleTypes.ReadOnly };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (SalesEntryGridColumns)value.ColumnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        SpecialOrderText = textCellProps.Text;
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override bool ValidateRow()
        {
            if (string.IsNullOrEmpty(SpecialOrderText))
            {
                SalesEntryDetailsManager.SalesEntryViewModel.SalesEntryView.GridValidationFail();
                SalesEntryDetailsManager.Grid.GotoCell(this, (int)SalesEntryGridColumns.Item);

                var message = "Special Order text cannot be empty.";
                SalesEntryDetailsManager.SalesEntryViewModel.SalesEntryView.OnValidationFail(
                    AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.ProductId), message,
                    "Validation Failure!");

                return false;
            }
            return true;
        }

        public override void SaveToEntity(OrderDetails entity, int rowIndex)
        {
            entity.SpecialOrderText = SpecialOrderText;
            base.SaveToEntity(entity, rowIndex);
        }

        public override void LoadFromEntity(OrderDetails entity)
        {
            SpecialOrderText = entity.SpecialOrderText;
            base.LoadFromEntity(entity);
        }
    }
}
