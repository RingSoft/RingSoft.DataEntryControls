﻿using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using System;
using System.Drawing;

namespace RingSoft.DataEntryControls.App.WPF
{
    public class AppGridNonInventoryRow : AppGridRow
    {
        public override AppGridLineTypes LineType => AppGridLineTypes.NonInventory;

        public string NonInventoryCode { get; set; }

        public double Price { get; set; }

        public DataEntryNumericEditSetup PriceSetup { get; } = Globals.GetNumericEditSetup();

        public AppGridNonInventoryRow(AppGridManager manager) : base(manager)
        {
            PriceSetup.EditFormatType = NumericEditFormatTypes.Currency;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            AppGridColumns column = (AppGridColumns)columnId;
            DataEntryGridCellProps result = null;

            switch (column)
            {
                case AppGridColumns.StockNumber:
                    result = new DataEntryGridTextCellProps(this, columnId) {Text = NonInventoryCode};
                    break;
                case AppGridColumns.Location:
                    result = new DataEntryGridTextCellProps(this, columnId)
                    {
                        Text = "Disabled",
                    };
                    break;
                case AppGridColumns.Price:
                    result = new DataEntryGridDecimalCellProps(this, columnId, PriceSetup, (decimal)Price);
                    break;
            }

            if (result != null)
                return result;

            return base.GetCellProps(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            AppGridColumns column = (AppGridColumns)value.ColumnId;
            switch (column)
            {
                case AppGridColumns.LineType:
                case AppGridColumns.CheckBox:
                    break;
                case AppGridColumns.StockNumber:
                    NonInventoryCode = value.Text;
                    break;
                case AppGridColumns.Location:
                    break;
                case AppGridColumns.Price:
                    var decimalCellProps = (DataEntryGridDecimalCellProps)value;
                    if (decimalCellProps.Value != null)
                        Price = (double)decimalCellProps.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            if (!string.IsNullOrEmpty(ParentRowId))
            {
                BackgroundColor = Color.Blue;
                ForegroundColor = Color.White;
            }

            AppGridColumns column = (AppGridColumns)columnId;
            DataEntryGridCellStyle result = null;

            switch (column)
            {
                case AppGridColumns.LineType:
                    if (!ParentRowId.IsNullOrEmpty())
                        result = new DataEntryGridCellStyle(){CellStyle = DataEntryGridCellStyles.ReadOnly};
                    break;
                case AppGridColumns.StockNumber:
                    result = new DataEntryGridCellStyle(){ColumnHeader = "Non Inventory Code" };
                    break;
                case AppGridColumns.Location:
                    result = new DataEntryGridCellStyle();
                    if (ParentRowId.IsNullOrEmpty())
                    {
                        result.CellStyle = DataEntryGridCellStyles.Disabled;
                        result.ForegroundColor = Color.Red;
                    }
                    else
                    {
                        result.CellStyle = DataEntryGridCellStyles.ReadOnly;
                    }
                    break;
                case AppGridColumns.Price:
                    if (CheckBoxValue)
                        return new DataEntryGridCellStyle() { ForegroundColor = Color.Red };
                    break;
            }

            if (result != null)
                return result;

            return base.GetCellStyle(columnId);
        }

        public override void LoadSale_DetailRow(SaleDetail saleDetail)
        {
            NonInventoryCode = saleDetail.StockNumber;
            Price = saleDetail.Price;
            base.LoadSale_DetailRow(saleDetail);
        }
    }
}
