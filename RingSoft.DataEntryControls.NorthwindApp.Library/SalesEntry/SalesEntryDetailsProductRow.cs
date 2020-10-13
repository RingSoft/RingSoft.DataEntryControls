using RingSoft.DataEntryControls.Engine;
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
        private DecimalEditControlSetup _discountSetup;

        public SalesEntryDetailsProductRow(SalesEntryDetailsGridManager manager) : base(manager)
        {
            _productAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.ProductId));
            _discountSetup = AppGlobals.CreateNewDecimalEditControlSetup();
            _discountSetup.FormatType = DecimalEditFormatTypes.Currency;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    return new DataEntryGridAutoFillCellProps(this, columnId, _productAutoFillSetup, ProductValue);
                case SalesEntryGridColumns.Discount:
                    return new DataEntryGridDecimalCellProps(this, columnId, _discountSetup, Discount);
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
                        if (validProduct)
                        {
                            LoadFromItemAutoFillValue(autoFillCellProps.AutoFillValue);
                        }
                        else if (string.IsNullOrEmpty(autoFillCellProps.AutoFillValue.Text))
                        {
                            ProductValue = autoFillCellProps.AutoFillValue;
                        }
                        else
                        {
                            CorrectInvalidItem(autoFillCellProps);
                        }
                    }
                    break;
                case SalesEntryGridColumns.Discount:
                    if (value is DataEntryGridDecimalCellProps discountDecimalCellProps)
                    {
                        if (discountDecimalCellProps.Value != null)
                        {
                            Discount = (decimal)discountDecimalCellProps.Value;
                            SalesEntryDetailsManager.ViewModel.RefreshTotalControls();
                        }
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public void LoadFromItemAutoFillValue(AutoFillValue itemAutoFillValue)
        {
            ProductValue = itemAutoFillValue;
            if (ProductValue.PrimaryKeyValue.ContainsValidData())
            {
                var product =
                    AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(ProductValue
                        .PrimaryKeyValue);
                product = AppGlobals.DbContextProcessor.GetProduct(product.ProductId);
                Quantity = 1;
                if (product.UnitPrice != null)
                    Price = (decimal)product.UnitPrice;
                SalesEntryDetailsManager.ViewModel.RefreshTotalControls();
            }
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

        public override bool ValidateRow()
        {
            if (ProductValue == null || !ProductValue.PrimaryKeyValue.ContainsValidData())
            {
                SalesEntryDetailsManager.ViewModel.SalesEntryView.GridValidationFail();
                SalesEntryDetailsManager.Grid.GotoCell(this, (int)SalesEntryGridColumns.Item);

                var message = "Product must contain a valid value.";
                SalesEntryDetailsManager.ViewModel.SalesEntryView.OnValidationFail(
                    AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.ProductId), message,
                    "Validation Failure!");

                return false;
            }
            return true;
        }

        public override void SaveToOrderDetail(OrderDetails orderDetail)
        {
            orderDetail.ProductId = AppGlobals.LookupContext.Products
                .GetEntityFromPrimaryKeyValue(ProductValue.PrimaryKeyValue).ProductId;
            orderDetail.Discount = (float)Discount;

            base.SaveToOrderDetail(orderDetail);
        }
    }
}
