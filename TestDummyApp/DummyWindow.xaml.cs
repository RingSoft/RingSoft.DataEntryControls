using RingSoft.DataEntryControls.Engine;
using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TestDummyApp
{
    /// <summary>
    /// Interaction logic for ChangedGlobalsWindow.xaml
    /// </summary>
    public partial class DummyWindow : INotifyPropertyChanged
    {
        private TextComboBoxControlSetup _comboBoxSetup;

        public TextComboBoxControlSetup ComboBoxSetup
        {
            get => _comboBoxSetup;
            set
            {
                if (_comboBoxSetup == value)
                    return;

                _comboBoxSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _selectedComboBoxItem;

        public TextComboBoxItem SelectedComboBoxItem
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


        private double _decimalValue;

        public double DecimalValue
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
            //CalculatorDec.Value = (double)-2345.67;
            IntegerValue = 12345;
            DecimalValue = 1234;

            Loaded += (_, _) =>
            {
                ComboBoxSetup = new TextComboBoxControlSetup();
                ComboBoxSetup.LoadFromEnum<DayOfWeek>();
                SelectedComboBoxItem = ComboBoxSetup.GetItem((int) DayOfWeek.Wednesday);
            };

            ExpandButton.Click += (_, _) =>
            {
                ExpandPanel.Visibility = Visibility.Visible;
            };

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
