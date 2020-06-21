﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public enum InvalidProductResultReturnCodes
    {
        Cancel = 0,
        NewProduct = 1,
        NewNonInventory = 2,
        NewSpecialOrder = 3,
        NewComment = 4
    }

    public class InvalidProductResult
    {
        public InvalidProductResultReturnCodes ReturnCode { get; set; }

        public AutoFillValue NewItemValue { get; set; }

        public string NewSpecialOrderText { get; set; }

        public GridMemoValue Comment { get; set; }
    }

    public interface IInvalidProductView
    {
        bool ShowCommentEditor(GridMemoValue comment);
    }

    public class InvalidProductViewModel : INotifyPropertyChanged
    {
        private string _headerLabel;

        public string HeaderLabel
        {
            get => _headerLabel;
            set
            {
                if (_headerLabel == value)
                    return;

                _headerLabel = value;
                OnPropertyChanged(nameof(HeaderLabel));
            }
        }

        public InvalidProductResult Result { get; } = new InvalidProductResult();

        public AutoFillValue InvalidProductValue { get; private set; }

        public IInvalidProductView View { get; private set; }
        
        public void OnViewLoaded(IInvalidProductView view, AutoFillValue invalidProductValue)
        {
            View = view;
            InvalidProductValue = invalidProductValue;

            HeaderLabel = $"'{invalidProductValue.Text}' was not found in the database.  What do you wish to do?";
        }

        public bool AddNewProduct()
        {
            return false;
        }

        public bool AddNewNonInventoryCode()
        {
            return false;
        }

        public void AddNewSpecialOrder()
        {
            Result.ReturnCode = InvalidProductResultReturnCodes.NewSpecialOrder;
            Result.NewSpecialOrderText = InvalidProductValue.Text;
        }

        public bool AddComment()
        {
            Result.Comment = new GridMemoValue(20);
            Result.Comment.Text = InvalidProductValue.Text;
            return View.ShowCommentEditor(Result.Comment);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}