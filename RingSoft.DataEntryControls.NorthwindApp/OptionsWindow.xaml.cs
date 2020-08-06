using System;
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

            ApplyNumericButton.Click += (sender, args) =>
            {
                if (ViewModel.OnApplyNumberFormat() == ValidationResults.NumberCultureFail)
                    OtherNumberCultureTextBox.Focus();
            };

            ApplyDateButton.Click += ApplyDateButton_Click;
        }

        private void ApplyDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (ViewModel.OnApplyDateFormat())
            {
                case ValidationResults.Success:
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
