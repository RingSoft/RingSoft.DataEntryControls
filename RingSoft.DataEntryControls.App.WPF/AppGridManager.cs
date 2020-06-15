﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RingSoft.DataEntryControls.App.WPF
{
    public enum AppGridLineTypes
    {
        [Description("Inventory")]
        Inventory = 0,

        [Description("Non Inventory")]
        NonInventory = 1,

        Comment = 2
    }

    public enum AppGridColumns
    {
        Disabled = AppGridManager.DisabledColumnId,
        LineType = AppGridManager.LineTypeColumnId,
        StockNumber = AppGridManager.StockNumberColumnId,
        Location = AppGridManager.LocationColumnId,
        CheckBox = AppGridManager.CheckBoxColumnId,
        Price = AppGridManager.PriceColumnId
    }

    public interface IAppUserInterface
    {
        public bool ShowYesNoMessage(string text, string caption);

        public void ShowValidationFailMessage(string text, string caption);

        public bool ShowGridMemoEditor(GridMemoValue gridMemoValue);

    }

    public class AppGridManager : DataEntryGridManager
    {
        public const int DisabledColumnId = 0;
        public const int LineTypeColumnId = 1;
        public const int StockNumberColumnId = 2;
        public const int LocationColumnId = 3;
        public const int CheckBoxColumnId = 4;
        public const int PriceColumnId = 5;

        public IAppUserInterface UserInterface { get; }

        public AppGridManager(IAppUserInterface userInterface)
        {
            UserInterface = userInterface;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new AppGridInventoryRow(this);
        }

        public void LoadSaleDetails(IEnumerable<SaleDetail> saleDetails)
        {
            ClearRows(false);

            foreach (var saleDetail in saleDetails)
            {
                var newRow = GetNewAppGridRow(saleDetail.LineType);

                newRow.LoadSale_DetailRow(saleDetail);
                AddRow(newRow);
            }

            InsertNewRow();
        }

        public AppGridRow GetNewAppGridRow(AppGridLineTypes lineType)
        {
            AppGridRow result;
            switch (lineType)
            {
                case AppGridLineTypes.Inventory:
                    result = new AppGridInventoryRow(this);
                    break;
                case AppGridLineTypes.NonInventory:
                    result = new AppGridNonInventoryRow(this);
                    break;
                case AppGridLineTypes.Comment:
                    result = new AppGridCommentRow(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}
