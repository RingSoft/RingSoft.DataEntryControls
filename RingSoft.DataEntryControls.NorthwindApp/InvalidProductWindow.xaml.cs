﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for InvalidProductCorrectionWindow.xaml
    /// </summary>
    public partial class InvalidProductWindow : IInvalidProductView
    {
        public InvalidProductWindow(AutoFillValue invalidProductValue)
        {
            InitializeComponent();

            Loaded += (sender, args) => ViewModel.OnViewLoaded(this, invalidProductValue);
            AddProductButton.Click += (sender, args) =>
            {
                if (ViewModel.AddNewProduct(this))
                    Close();
            };
            AddNonInventoryButton.Click += (sender, args) =>
            {
                if (ViewModel.AddNewNonInventoryCode(this))
                    Close();
            };
            AddSpecialOrderButton.Click += (sender, args) =>
            {
                ViewModel.AddNewSpecialOrder();
                Close();
            };
            AddCommentButton.Click += (sender, args) =>
            {
                if (ViewModel.AddComment())
                    Close();
            };
            CancelButton.Click += (sender, args) => Close();
        }

        public new InvalidProductResult ShowDialog()
        {
            base.ShowDialog();
            return ViewModel.Result;
        }

        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var gridMemoEditor = new DataEntryGridMemoEditor(comment);
            gridMemoEditor.Title = "Edit Comment";
            gridMemoEditor.Owner = this;
            gridMemoEditor.ShowInTaskbar = false;
            return gridMemoEditor.ShowDialog();
        }
    }
}
