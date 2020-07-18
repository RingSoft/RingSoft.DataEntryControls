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

            ApplyNumericButton.Click += (sender, args) => ViewModel.OnApplyNumberFormat();
        }
    }
}
