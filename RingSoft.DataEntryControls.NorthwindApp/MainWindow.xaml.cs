using System;
using System.Windows.Threading;

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

            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                Done?.Invoke(this, EventArgs.Empty);
            };

            ContentRendered += (sender, args) =>
            {
                Activate();
                timer.Start();
            };

            SalesEntryButton.Click += (sender, args) => ShowSalesEntryWindow();
            SalesEntryMenu.Click += (sender, args) => ShowSalesEntryWindow();
        }

        private void ShowSalesEntryWindow()
        {
            var salesEntryWindow = new SalesEntryWindow();
            salesEntryWindow.Owner = this;
            salesEntryWindow.ShowDialog();
        }
    }
}
