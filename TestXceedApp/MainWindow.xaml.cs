using System.Data;

namespace TestXceedApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private DataTable _gridSource = new DataTable();

        public MainWindow()
        {
            InitializeComponent();

            Grid.ItemsSource = _gridSource.DefaultView;
        }
    }
}
