using System;
using System.Windows;
using System.Windows.Threading;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
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

#if DEBUG
            ProcedureTest.Click += (_, _) => ShowProcedureTest();
#else
            ProcedureTest.Visibility = Visibility.Collapsed;
#endif
        }

        private void ShowSalesEntryWindow()
        {
            LookupControlsGlobals.WindowRegistry.ShowDbMaintenanceWindow(AppGlobals.LookupContext.Orders);
        }

        private void ShowPurchaseOrderWindow()
        {
            LookupControlsGlobals.WindowRegistry.ShowDbMaintenanceWindow(AppGlobals.LookupContext.Purchases);
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

        private void ShowProcedureTest()
        {
            var procTest = new TestProcedure(this, "Testing");
            procTest.Start();
        }
    }
}
