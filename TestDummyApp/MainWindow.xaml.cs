using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TestDummyApp
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

        private double? _calcValue;

        public double? CalcValue
        {
            get => _calcValue;
            set
            {
                if (_calcValue == value)
                    return;

                _calcValue = value;
                OnPropertyChanged(nameof(CalcValue));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            GridManager = new AppGridManager(this);
            CalcValue = (double)-2345.67;
            //CalcValue = (double)0.67;
            //DateEditControl.EntryFormat = "M/d/yy";
            //DateEditControl.DisplayFormat = "D";
            //DateEditControl.Value = DateTime.Parse("01/01/1980");

            //DateEditControl.PreviewLostKeyboardFocus += (sender, args) =>
            //{
            //    if (MessageBox.Show("OK?", "OK?", MessageBoxButton.YesNo) == MessageBoxResult.No)
            //    {
            //        args.Handled = true;
            //    }
            //};

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

            ClearGridButton.Click += (_, _) =>
            {
                GridManager.SetupForNewRecord();
                DateEditControl.Focus();
            };

            LoadGridButton.Click += (_, _) =>
            {
                GridManager.LoadSaleDetails(saleDetails);
            };

            TestButton.Click += (_, _) =>
            {
                //GridManager.Grid.TakeCellSnapshot(true, true);
                //GridManager.RemoveRow(0);
                //GridManager.Grid.RestoreCellSnapshot(true, true);
                IntegerEditControl.Focus();

                MessageBox.Show(
                    $"Current Row Index = {GridManager.Grid.CurrentRowIndex}.  Current Column ID = {GridManager.Grid.CurrentColumnId}.");

                var win = new DummyWindow();
                win.Owner = this;
                win.ShowInTaskbar = false;
                win.ShowDialog();
            };

            FocusButton.Click += (_, _) =>
            {
                //DateEditControl.Focus();
                DataEntryGrid.Focus();
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

        public bool ShowGridMemoEditor(DataEntryGridMemoValue gridMemoValue)
        {
            var memoEditor = new DataEntryGridMemoEditor(gridMemoValue);
            memoEditor.Owner = this;
            memoEditor.Title = "Edit Comment";
            //return memoEditor.ShowDialog();
            memoEditor.Deactivated += (sender, args) =>
            {

            };
            memoEditor.Show();
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
