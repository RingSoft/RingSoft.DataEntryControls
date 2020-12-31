using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public class PurchaseOrderDetailsProductRow : PurchaseOrderDetailsRow
    {
        public override PurchaseOrderDetailsLineTypes LineType => PurchaseOrderDetailsLineTypes.Product;

        public AutoFillValue ProductValue { get; private set; }

        public decimal Quantity { get; private set; }

        public decimal Price { get; private set; }

        public decimal ExtendedPrice => Quantity * Price;

        public bool ValidProduct =>
            ProductValue?.PrimaryKeyValue != null && ProductValue.PrimaryKeyValue.IsValid;

        private AutoFillSetup _productAutoFillSetup;
        private DecimalEditControlSetup _quantitySetup;
        private DecimalEditControlSetup _priceSetup;

        public PurchaseOrderDetailsProductRow(PurchaseOrderDetailsGridManager manager) : base(manager)
        {
            _productAutoFillSetup =
                new AutoFillSetup(PurchaseOrderDetailsManager.PurchaseOrderViewModel.ProductsLookup);
            _quantitySetup = AppGlobals.CreateNewDecimalEditControlSetup();
            _priceSetup = AppGlobals.CreateNewDecimalEditControlSetup();
            _priceSetup.FormatType = DecimalEditFormatTypes.Currency;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;
            var validSupplier = PurchaseOrderDetailsManager.PurchaseOrderViewModel.ValidSupplier();
            var validProduct = validSupplier && ProductValue?.PrimaryKeyValue != null &&
                               ProductValue.PrimaryKeyValue.IsValid;
            switch (column)
            {
                case PurchaseOrderColumns.Item:
                    if (validSupplier)
                    {
                        _productAutoFillSetup.AddViewParameter = new ProductInput(PurchaseOrderDetailsManager
                            .PurchaseOrderViewModel.SupplierAutoFillValue);
                        return new DataEntryGridAutoFillCellProps(this, columnId, _productAutoFillSetup, ProductValue);
                    }
                    else 
                        return new DataEntryGridTextCellProps(this, columnId){Text = "Invalid Supplier!"};
                case PurchaseOrderColumns.Quantity:
                    if (validProduct)
                        return new DataEntryGridDecimalCellProps(this, columnId, _quantitySetup, Quantity);
                    break;
                case PurchaseOrderColumns.Price:
                    if (validProduct)
                        return new DataEntryGridDecimalCellProps(this, columnId, _priceSetup, Price);
                    break;
                case PurchaseOrderColumns.ExtendedPrice:
                    if (validProduct)
                        return new DataEntryGridDecimalCellProps(this, columnId, _priceSetup, ExtendedPrice);
                    break;
            }

            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;
            DataEntryGridCellStyle cellStyle = null;
            switch (column)
            {
                case PurchaseOrderColumns.LineType:
                    break;
                case PurchaseOrderColumns.Item:
                    cellStyle = new DataEntryGridCellStyle { ColumnHeader = "Product" };
                    break;
                case PurchaseOrderColumns.ExtendedPrice:
                    return new DataEntryGridCellStyle{CellStyle = DataEntryGridCellStyles.Disabled};
                default:
                    cellStyle = new DataEntryGridCellStyle();
                    if (!ValidProduct && IsNew)
                        cellStyle.CellStyle = DataEntryGridCellStyles.Disabled;

                    break;
            }

            if (cellStyle != null)
            {
                SetupCellStyle(cellStyle);
                return cellStyle;
            }
            return base.GetCellStyle(columnId);
        }

        private void SetupCellStyle(DataEntryGridCellStyle cellStyle)
        {
            if (!PurchaseOrderDetailsManager.PurchaseOrderViewModel.ValidSupplier())
                cellStyle.CellStyle = DataEntryGridCellStyles.Disabled;
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (PurchaseOrderColumns) value.ColumnId;

            switch (column)
            {
                case PurchaseOrderColumns.Item:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        var validProduct = autoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid;
                        if (validProduct)
                        {
                            LoadFromItemAutoFillValue(autoFillCellProps.AutoFillValue);
                        }
                        else if (string.IsNullOrEmpty(autoFillCellProps.AutoFillValue.Text))
                        {
                            ProductValue = null;
                        }
                        else
                        {
                            var message =
                                $"{PurchaseOrderDetailsManager.PurchaseOrderViewModel.SupplierAutoFillValue.Text} does not carry this Product.  Do you wish to erase this entry?";
                            var messageResult =
                                ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Invalid Product");
                            switch (messageResult)
                            {
                                case MessageBoxButtonsResult.No:
                                    value.OverrideCellMovement = true;
                                    break;
                                default:
                                    ProductValue = null;
                                    break;
                            }

                            return;
                        }
                    }
                    PurchaseOrderDetailsManager.PurchaseOrderViewModel.UpdateSupplierEnabled();
                    break;
                case PurchaseOrderColumns.Quantity:
                    if (value is DataEntryGridDecimalCellProps quantityCellProps)
                        if (quantityCellProps.Value != null)
                            Quantity = (decimal) quantityCellProps.Value;
                    break;
                case PurchaseOrderColumns.Price:
                    if (value is DataEntryGridDecimalCellProps priceCellProps)
                        if (priceCellProps.Value != null)
                            Price = (decimal) priceCellProps.Value;
                    break;
            }
            PurchaseOrderDetailsManager.PurchaseOrderViewModel.RefreshTotalControls();
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
                    Price = (decimal)product.UnitPrice;

                LoadChildRows(product);

                PurchaseOrderDetailsManager.PurchaseOrderViewModel.RefreshTotalControls();
            }
        }

        private void LoadChildRows(Products product)
        {
            if (!string.IsNullOrEmpty(product.PurchaseComment))
            {
                var commentRow = new PurchaseOrderDetailsCommentRow(PurchaseOrderDetailsManager);
                AddChildRow(commentRow);
                commentRow.SetValue(product.PurchaseComment);
            }
        }

        public override void LoadFromEntity(PurchaseDetails entity)
        {
            ProductValue =
                new AutoFillValue(AppGlobals.LookupContext.Products.GetPrimaryKeyValueFromEntity(entity.Product),
                    entity.Product.ProductName);

            if (entity.Quantity != null) 
                Quantity = (decimal) entity.Quantity;

            if (entity.Price != null) 
                Price = (decimal) entity.Price;

            var children = GetDetailChildren(entity);
            foreach (var child in children)
            {
                var childRow =
                    PurchaseOrderDetailsManager.CreateRowFromLineType((PurchaseOrderDetailsLineTypes)child.LineType);
                AddChildRow(childRow);
                childRow.LoadFromEntity(child);
                Manager.Grid.UpdateRow(childRow);
            }

            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            if (ProductValue == null || !ProductValue.PrimaryKeyValue.IsValid)
            {
                PurchaseOrderDetailsManager.PurchaseOrderViewModel.PurchaseOrderView.GridValidationFail();
                PurchaseOrderDetailsManager.Grid.GotoCell(this, (int)PurchaseOrderColumns.Item);

                var message = "Product must contain a valid value.";
                PurchaseOrderDetailsManager.PurchaseOrderViewModel.PurchaseOrderView.OnValidationFail(
                    AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.ProductId), message,
                    "Validation Failure!");

                return false;
            }
            return true;
        }

        public override void SaveToEntity(PurchaseDetails entity, int rowIndex)
        {
            entity.ProductId = AppGlobals.LookupContext.Products
                .GetEntityFromPrimaryKeyValue(ProductValue.PrimaryKeyValue).ProductId;
            entity.Quantity = Quantity;
            entity.Price = Price;

            base.SaveToEntity(entity, rowIndex);
        }
    }
}
