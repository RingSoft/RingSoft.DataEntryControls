using System.Windows;
using RingSoft.DataEntryControls.NorthwindApp.Library;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for AppSplashWindow.xaml
    /// </summary>
    public partial class AppSplashWindow : IAppSplashWindow
    {
        public bool IsDisposed => false;
        public bool Disposing => false;

        public AppSplashWindow()
        {
            InitializeComponent();
        }

        public void SetProgress(string progressText)
        {
            Dispatcher.Invoke(() => ProgressTextBlock.Text = progressText);
        }

        public void CloseSplash()
        {
            Dispatcher.Invoke(() => Close());
        }

        public void ShowError(string message, string caption)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(this, message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }
}
