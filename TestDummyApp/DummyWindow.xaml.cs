using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace TestDummyApp
{
    /// <summary>
    /// Interaction logic for ChangedGlobalsWindow.xaml
    /// </summary>
    public partial class DummyWindow : INotifyPropertyChanged
    {
        private List<ComboBoxItem> _comboBoxItems;

        public List<ComboBoxItem> ComboBoxItems
        {
            get => _comboBoxItems;
            set
            {
                if (_comboBoxItems == value)
                    return;

                _comboBoxItems = value;
                OnPropertyChanged();
            }
        }

        private ComboBoxItem _selectedComboBoxItem;

        public ComboBoxItem SelectedComboBoxItem
        {
            get => _selectedComboBoxItem;
            set
            {
                if (_selectedComboBoxItem == value)
                    return;

                _selectedComboBoxItem = value;
                OnPropertyChanged();
            }
        }


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

            Loaded += (sender, args) =>
            {
                ComboBoxItems = new List<ComboBoxItem>();
                ComboBoxItems.Add(new ComboBoxItem
                {
                    NumericValue = 0,
                    TextValue = "Item 0"
                });
                ComboBoxItems.Add(new ComboBoxItem
                {
                    NumericValue = 1,
                    TextValue = "Item 1"
                });
                ComboBoxItems.Add(new ComboBoxItem
                {
                    NumericValue = 2,
                    TextValue = "Item 2"
                });
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
