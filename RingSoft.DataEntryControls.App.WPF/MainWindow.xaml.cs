﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RingSoft.DataEntryControls.App.WPF
{
    public class SaleDetail
    {
        public AppGridLineTypes LineType { get; set; }

        public string StockNumber { get; set; }

        public string Location { get; set; }

        public double Price { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged, IAppUserInterface
    {
        private AppGridManager _gridManager;

        public AppGridManager GridManager
        {
            get => _gridManager;
            set
            {
                if (_gridManager == value)
                    return;

                _gridManager = value;
                OnPropertyChanged(nameof(GridManager));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            GridManager = new AppGridManager(this);

            var saleDetails = new List<SaleDetail>();

            for (int i = 0; i < 2; i++)
            {
                saleDetails.Add(new SaleDetail()
                {
                    LineType = AppGridLineTypes.Inventory,
                    StockNumber = "Chair #1 Swivel",
                    Location = "Boise, ID",
                    Price = 50
                });
                saleDetails.Add(new SaleDetail()
                {
                    LineType = AppGridLineTypes.NonInventory,
                    StockNumber = "Warranty",
                    Price = 5
                });
            }


            GridManager.LoadSaleDetails(saleDetails);

            ClearGridButton.Click += (sender, args) =>
            {
                GridManager.ClearRows();
            };

            LoadGridButton.Click += (sender, args) =>
            {
                GridManager.LoadSaleDetails(saleDetails);
            };
        }

        public bool ShowYesNoMessage(string text, string caption)
        {
            var result = MessageBox.Show(this, text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        public void ShowValidationFailMessage(string text, string caption)
        {
            MessageBox.Show(this, text, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public bool ShowGridMemoEditor(GridMemoValue gridMemoValue)
        {
            var memoEditor = new DataEntryGridMemoEditor(gridMemoValue);
            memoEditor.Owner = this;
            memoEditor.Title = "Edit Comment";
            return memoEditor.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
