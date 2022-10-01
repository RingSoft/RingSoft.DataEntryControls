using System;
using System.Windows.Threading;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public event EventHandler Done;

        public MainWindow()
        {
            InitializeComponent();

            var timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 10),
                IsEnabled = true
            };

            timer.Tick += (_, _) =>
            {
                timer.Stop();
                Done?.Invoke(this, EventArgs.Empty);
            };

            ContentRendered += (_, _) =>
            {
                Activate();
                timer.Start();
            };

            SalesEntryButton.Click += (_, _) => ShowSalesEntryWindow();
            SalesEntryMenu.Click += (_, _) => ShowSalesEntryWindow();
            PoMenu.Click += (_, _) => ShowPurchaseOrderWindow();
            PoButton.Click += (_, _) => ShowPurchaseOrderWindow();
            OptionsMenu.Click += (_, _) => ShowOptionsWindow();
            AdvancedFindMenuItem.Click += (_, _) => ShowAdvancedFindWindow();
            ExitButton.Click += (_, _) => Close();
            ExitMenu.Click += (_, _) => Close();
        }

        private void ShowSalesEntryWindow()
        {
            var salesEntryWindow = new SalesEntryWindow();
            salesEntryWindow.Owner = this;
            salesEntryWindow.ShowDialog();
        }

        private void ShowPurchaseOrderWindow()
        {
            var purchaseOrderWindow = new PurchaseOrderWindow();
            purchaseOrderWindow.Owner = this;
            purchaseOrderWindow.ShowDialog();
        }

        private void ShowOptionsWindow()
        {
            var optionsWindow = new OptionsWindow();
            optionsWindow.Owner = this;
            optionsWindow.ShowDialog();
        }

        private void ShowAdvancedFindWindow()
        {
            var advancedFindWindow = new AdvancedFindWindow();
            advancedFindWindow.Owner = this;
            advancedFindWindow.ShowInTaskbar = false;
            advancedFindWindow.Closed += (sender, args) => Activate();
            advancedFindWindow.Show();
        }
    }
}
