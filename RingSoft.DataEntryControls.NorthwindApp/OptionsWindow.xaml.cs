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
                if (!ViewModel.OnApplyNumberFormat())
                    OtherNumberCultureTextBox.Focus();
            };

            ApplyDateButton.Click += ApplyDateButton_Click;
        }

        private void ApplyDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (ViewModel.OnApplyDateFormat())
            {
                case DateValidationResults.Success:
                    break;
                case DateValidationResults.CultureFail:
                    OtherDateCultureTextBox.Focus();
                    break;
                case DateValidationResults.DateEntryFormatFail:
                    CustomDateEntryFormatTextBox.Focus();
                    break;
                case DateValidationResults.DateDisplayFormatFail:
                    CustomDateDisplayFormatTextBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
