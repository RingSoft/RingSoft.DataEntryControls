using System.Data;

namespace RingSoft.DataEntryControls.App.WPF
{
    /// <summary>
    /// Interaction logic for ChangedGlobalsWindow.xaml
    /// </summary>
    public partial class DummyWindow
    {
        private DataTable _gridSource = new DataTable();
        public DummyWindow()
        {
            InitializeComponent();

            Grid.ItemsSource = _gridSource.DefaultView;
            CalculatorDec.Value = (decimal)-2345.67;
        }
    }
}
