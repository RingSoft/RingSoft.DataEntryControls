using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public class PurchaseOrderDetailsProductRow : PurchaseOrderDetailsRow
    {
        public override PurchaseOrderDetailsLineTypes LineType => PurchaseOrderDetailsLineTypes.Product;

        public AutoFillValue ProductValue { get; private set; }

        public bool ValidProduct =>
            ProductValue?.PrimaryKeyValue != null && ProductValue.PrimaryKeyValue.ContainsValidData();

        private AutoFillSetup _productAutoFillSetup;

        public PurchaseOrderDetailsProductRow(PurchaseOrderDetailsGridManager manager) : base(manager)
        {
            _productAutoFillSetup =
                new AutoFillSetup(PurchaseOrderDetailsManager.PurchaseOrderViewModel.ProductsLookup);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (PurchaseOrderColumns) columnId;

            switch (column)
            {
                case PurchaseOrderColumns.Item:
                    if (PurchaseOrderDetailsManager.PurchaseOrderViewModel.ValidSupplier())
                    {
                        _productAutoFillSetup.AddViewParameter = new ProductInput(PurchaseOrderDetailsManager
                            .PurchaseOrderViewModel.SupplierAutoFillValue);
                        return new DataEntryGridAutoFillCellProps(this, columnId, _productAutoFillSetup, ProductValue);
                    }
                    else 
                        return new DataEntryGridTextCellProps(this, columnId){Text = "Invalid Supplier!"};
                case PurchaseOrderColumns.Quantity:
                    break;
                case PurchaseOrderColumns.Price:
                    break;
                case PurchaseOrderColumns.ExtendedPrice:
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
                            //CorrectInvalidItem(autoFillCellProps);
                        }
                    }
                    PurchaseOrderDetailsManager.PurchaseOrderViewModel.UpdateSupplierEnabled();
                    break;
                case PurchaseOrderColumns.Quantity:
                    break;
                case PurchaseOrderColumns.Price:
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
                //Quantity = 1;
                //if (product.UnitPrice != null)
                //    Price = (decimal)product.UnitPrice;

                //LoadChildRows(product);

                //SalesEntryDetailsManager.SalesEntryViewModel.RefreshTotalControls();
            }
        }

        public override void LoadFromEntity(PurchaseDetails entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(PurchaseDetails entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
