using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace TestDummyApp
{
    /// <summary>
    /// Interaction logic for ChangedGlobalsWindow.xaml
    /// </summary>
    public partial class DummyWindow : INotifyPropertyChanged
    {
        private decimal _decimalValue;

        public decimal DecimalValue
        {
            get => _decimalValue;
            set
            {
                if (_decimalValue == value)
                    return;

                _decimalValue = value;
                OnPropertyChanged(nameof(DecimalValue));
            }
        }

        private int _integerValue;
        public int IntegerValue
        {
            get => _integerValue;
            set
            {
                if (_integerValue == value)
                    return;

                _integerValue = value;
                OnPropertyChanged(nameof(IntegerValue));
            }
        }

        private DataTable _gridSource = new DataTable();
        public DummyWindow()
        {
            InitializeComponent();

            Grid.ItemsSource = _gridSource.DefaultView;
            //CalculatorDec.Value = (decimal)-2345.67;
            IntegerValue = 12345;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
