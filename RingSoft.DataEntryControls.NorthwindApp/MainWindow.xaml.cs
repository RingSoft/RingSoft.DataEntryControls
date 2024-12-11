using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using RingSoft.DataEntryControls.Engine;
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
        public HotKeyProcessor HotKeyProcessor { get; } = new HotKeyProcessor();

        public event EventHandler Done;

        public MainWindow()
        {
            InitializeComponent();
            HotKeyProcessor.TopLevel = true;

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

            LookupControlsGlobals.SetTabSwitcherWindow(this, TabControl);

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
            var hotKey = new HotKey(new RelayCommand((() =>
            {
                MessageBox.Show("Main Hot Key");
            })));
            hotKey.AddKey(Key.M);
            hotKey.AddKey(Key.T);
            HotKeyProcessor.AddHotKey(hotKey);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            HotKeyProcessor.OnKeyPressed(e);
            base.OnPreviewKeyDown(e);
        }

        private void ShowSalesEntryWindow()
        {
            TabControl.ShowTableControl(AppGlobals.LookupContext.Orders);
        }

        private void ShowPurchaseOrderWindow()
        {
            TabControl.ShowTableControl(AppGlobals.LookupContext.Purchases);
        }

        private void ShowOptionsWindow()
        {
            var optionsWindow = new OptionsWindow();
            optionsWindow.Owner = this;
            optionsWindow.ShowDialog();
        }

        private void ShowAdvancedFindWindow()
        {
            //var advancedFindWindow = new AdvancedFindWindow();
            //advancedFindWindow.Owner = this;
            //advancedFindWindow.ShowInTaskbar = false;
            //advancedFindWindow.Closed += (sender, args) => Activate();
            //advancedFindWindow.Show();
            TabControl.ShowTableControl(AppGlobals.LookupContext.AdvancedFinds);
        }

        private void ShowProcedureTest()
        {
            var procTest = new TestProcedure(this, "Testing");
            procTest.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!TabControl.CloseAllTabs())
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }
    }
}
