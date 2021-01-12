﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace TestDummyApp
{
    public enum AppGridLineTypes
    {
        [Description("Inventory")]
        Inventory = AppGridManager.InventoryLineTypeId,

        [Description("Non Inventory")]
        NonInventory = AppGridManager.NonInventoryLineTypeId,

        Comment = AppGridManager.CommentLineTypeId
    }

    public enum AppGridColumns
    {
        Disabled = AppGridManager.DisabledColumnId,
        LineType = AppGridManager.LineTypeColumnId,
        StockNumber = AppGridManager.StockNumberColumnId,
        Location = AppGridManager.LocationColumnId,
        CheckBox = AppGridManager.CheckBoxColumnId,
        Price = AppGridManager.PriceColumnId,
        Date = AppGridManager.DateColumnId,
        Integer = AppGridManager.IntegerColumnId,
        Button = AppGridManager.ButtonColumnId
    }

    public interface IAppUserInterface
    {
        public bool ShowYesNoMessage(string text, string caption);

        public void ShowValidationFailMessage(string text, string caption);

        public bool ShowGridMemoEditor(DataEntryGridMemoValue gridMemoValue);

    }

    public class AppGridManager : DataEntryGridManager
    {
        public const int DisabledColumnId = 0;
        public const int LineTypeColumnId = 1;
        public const int StockNumberColumnId = 2;
        public const int LocationColumnId = 3;
        public const int CheckBoxColumnId = 4;
        public const int PriceColumnId = 5;
        public const int DateColumnId = 6;
        public const int IntegerColumnId = 7;
        public const int ButtonColumnId = 8;

        public const int InventoryLineTypeId = 0;
        public const int NonInventoryLineTypeId = 1;
        public const int CommentLineTypeId = 2;

        public const int RedDisplayId = 200;

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
            PreLoadGridFromEntity();

            foreach (var saleDetail in saleDetails)
            {
                var newRow = GetNewAppGridRow(saleDetail.LineType);

                newRow.LoadSale_DetailRow(saleDetail);
                AddRow(newRow);
            }

            PostLoadGridFromEntity();
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
