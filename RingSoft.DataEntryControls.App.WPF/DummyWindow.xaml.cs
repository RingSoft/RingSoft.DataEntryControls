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
            Calculator.Value = (decimal)1234.12;
        }
    }
}
