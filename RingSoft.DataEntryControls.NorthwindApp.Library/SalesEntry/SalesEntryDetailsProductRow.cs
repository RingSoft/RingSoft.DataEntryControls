using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryDetailsProductRow : SalesEntryDetailsValueRow
    {
        public override SalesEntryDetailsLineTypes LineType => SalesEntryDetailsLineTypes.Product;
        public AutoFillValue ProductValue { get; private set; }

        public double Discount { get; private set; }

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
                    _productAutoFillSetup.AddViewParameter =
                        SalesEntryDetailsManager.SalesEntryViewModel.ViewModelInput;

                    return new DataEntryGridAutoFillCellProps(this, columnId, _productAutoFillSetup, ProductValue);
                case SalesEntryGridColumns.Discount:
                    return new DataEntryGridDecimalCellProps(this, columnId, _discountSetup, Discount);
            }

            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (SalesEntryGridColumns) columnId;

            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    return new DataEntryGridCellStyle{ColumnHeader = "Product"};
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (SalesEntryGridColumns) value.ColumnId;
            switch (column)
            {
                case SalesEntryGridColumns.Item:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        var validProduct = autoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid;
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
                            Discount = (double)discountDecimalCellProps.Value;
                            SalesEntryDetailsManager.SalesEntryViewModel.RefreshTotalControls();
                        }
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public void LoadFromItemAutoFillValue(AutoFillValue itemAutoFillValue)
        {
            ProductValue = itemAutoFillValue;
            if (ProductValue.PrimaryKeyValue.IsValid)
            {
                var product =
                    AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(ProductValue
                        .PrimaryKeyValue);
                product = AppGlobals.DbContextProcessor.GetProduct(product.ProductId);
                Quantity = 1;
                if (product.UnitPrice != null)
                    Price = (double)product.UnitPrice;

                LoadChildRows(product);

                SalesEntryDetailsManager.SalesEntryViewModel.RefreshTotalControls();
            }
        }

        private void LoadChildRows(Products product)
        {
            if (product.NonInventoryCode != null)
            {
                var niRow = new SalesEntryDetailsNonInventoryRow(SalesEntryDetailsManager);
                AddChildRow(niRow);
                niRow.LoadFromNiCode(product.NonInventoryCode);
                Manager.Grid?.UpdateRow(niRow);
            }

            if (!string.IsNullOrEmpty(product.OrderComment))
            {
                var commentRow = new SalesEntryDetailsCommentRow(SalesEntryDetailsManager);
                AddChildRow(commentRow);
                commentRow.SetValue(product.OrderComment);
            }
        }

        public override void LoadFromEntity(OrderDetails entity)
        {
            if (entity.Product != null)
            {
                var productPrimaryKey =
                    AppGlobals.LookupContext.Products.GetPrimaryKeyValueFromEntity(entity.Product);
                ProductValue = new AutoFillValue(productPrimaryKey, entity.Product.ProductName);
            }

            if (entity.Discount != null)
                Discount = (double)entity.Discount;

            var children = GetDetailChildren(entity);
            foreach (var child in children)
            {
                var childRow =
                    SalesEntryDetailsManager.CreateRowFromLineType((SalesEntryDetailsLineTypes) child.LineType);
                AddChildRow(childRow);
                childRow.LoadFromEntity(child);
                Manager.Grid?.UpdateRow(childRow);
            }

            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            if (ProductValue == null || !ProductValue.PrimaryKeyValue.IsValid)
            {
                //SalesEntryDetailsManager.SalesEntryViewModel.SalesEntryView.GridValidationFail();
                SalesEntryDetailsManager.Grid?.GotoCell(this, (int)SalesEntryGridColumns.Item);

                var message = "Product must contain a valid value.";
                SalesEntryDetailsManager.SalesEntryViewModel.SalesEntryView.OnValidationFail(
                    AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.ProductId), message,
                    "Validation Failure!");

                return false;
            }
            return true;
        }

        public override void SaveToEntity(OrderDetails entity, int rowIndex)
        {
            entity.ProductId = AppGlobals.LookupContext.Products
                .GetEntityFromPrimaryKeyValue(ProductValue.PrimaryKeyValue).ProductId;
            entity.Discount = (float)Discount;
            base.SaveToEntity(entity, rowIndex);
        }
    }
}
