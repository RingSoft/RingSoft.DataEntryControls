using System;
using System.Diagnostics;
using System.Windows;
using RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow
    {
        public OptionsWindow()
        {
            InitializeComponent();

            ApplyNumericButton.Click += (sender, args) => ProcessValidationResult(ViewModel.OnApplyNumberFormat());

            ApplyDateButton.Click += (sender, args) => ProcessValidationResult(ViewModel.OnApplyDateFormat());
            OkButton.Click += OkButton_Click;
            CancelButton.Click += (sender, args) => Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var result = ViewModel.OnOk();
            if (result != ValidationResults.Success)
            {
                ProcessValidationResult(result);
                return;
            }

            var message =
                "This application must be restarted in order for changes to take effect.\r\n\r\nDo you wish to restart now?";

            if (MessageBox.Show(this, message, "Restart Application?", MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            Close();
        }

        private void ProcessValidationResult(ValidationResults result)
        {
            switch (result)
            {
                case ValidationResults.Success:
                    break;
                case ValidationResults.NumberCultureFail:
                    OtherNumberCultureTextBox.Focus();
                    break;
                case ValidationResults.DateCultureFail:
                    OtherDateCultureTextBox.Focus();
                    break;
                case ValidationResults.DateEntryFormatFail:
                    CustomDateEntryFormatTextBox.Focus();
                    break;
                case ValidationResults.DateDisplayFormatFail:
                    CustomDateDisplayFormatTextBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
