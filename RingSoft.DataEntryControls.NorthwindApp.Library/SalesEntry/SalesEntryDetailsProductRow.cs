using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsProductRow : SalesEntryDetailsValueRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.Product;
        public AutoFillValue ProductValue { get; private set; }

        public decimal Discount { get; private set; }

        private AutoFillSetup _productAutoFillSetup;

        public SalesEntryDetailsProductRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            _productAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.ProductId));
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    return new DataEntryGridAutoFillCellProps(this, columnId, _productAutoFillSetup, ProductValue);
                case SalesEntryGridColumns.Discount:
                    return new DataEntryGridTextCellProps(this, columnId){Text = Discount.ToString("C")};
            }

            return base.GetCellProps(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (SalesEntryGridColumns) value.ColumnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        var validProduct = autoFillCellProps.AutoFillValue.PrimaryKeyValue.ContainsValidData();
                        if (!validProduct)
                        {
                            if (!value.SkipValidation)
                            {
                                var correctedValue =
                                    SalesEntryDetailsManager.ViewModel.SalesEntryView.CorrectInvalidProduct(
                                        autoFillCellProps.AutoFillValue);

                                switch (correctedValue.ReturnCode)
                                {
                                    case InvalidProductResultReturnCodes.Cancel:
                                        break;
                                    case InvalidProductResultReturnCodes.NewProduct:
                                        break;
                                    case InvalidProductResultReturnCodes.NewNonInventory:
                                        break;
                                    case InvalidProductResultReturnCodes.NewSpecialOrder:
                                        break;
                                    case InvalidProductResultReturnCodes.NewComment:
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }

                            //if (!validProduct)
                            {
                                autoFillCellProps.AutoFillValue = ProductValue;
                                autoFillCellProps.ValidationResult = false;
                            }
                        }
                        if (validProduct)
                            ProductValue = autoFillCellProps.AutoFillValue;
                    }

                    break;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromOrderDetail(OrderDetails orderDetail)
        {
            if (orderDetail.Product != null)
            {
                var productPrimaryKey =
                    AppGlobals.LookupContext.Products.GetPrimaryKeyValueFromEntity(orderDetail.Product);
                ProductValue = new AutoFillValue(productPrimaryKey, orderDetail.Product.ProductName);
            }

            if (orderDetail.Discount != null)
                Discount = (decimal) orderDetail.Discount;

            base.LoadFromOrderDetail(orderDetail);
        }
    }
}
