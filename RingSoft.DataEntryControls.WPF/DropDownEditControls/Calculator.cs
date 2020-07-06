using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DropDownEditControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DropDownEditControls;assembly=RingSoft.DataEntryControls.WPF.DropDownEditControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DropDownCalculator/>
    ///
    /// </summary>

    [TemplatePart(Name = "MemoryStatusTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "EquationTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "EntryTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "McButton", Type = typeof(Button))]
    [TemplatePart(Name = "MrButton", Type = typeof(Button))]
    [TemplatePart(Name = "MAddButton", Type = typeof(Button))]
    [TemplatePart(Name = "MSubtractButton", Type = typeof(Button))]
    [TemplatePart(Name = "MsButton", Type = typeof(Button))]
    [TemplatePart(Name = "CeButton", Type = typeof(Button))]
    [TemplatePart(Name = "ClrButton", Type = typeof(Button))]
    [TemplatePart(Name = "BackButton", Type = typeof(Button))]
    [TemplatePart(Name = "DivideButton", Type = typeof(Button))]
    [TemplatePart(Name = "Button7", Type = typeof(Button))]
    [TemplatePart(Name = "Button8", Type = typeof(Button))]
    [TemplatePart(Name = "Button9", Type = typeof(Button))]
    [TemplatePart(Name = "MultiplyButton", Type = typeof(Button))]
    [TemplatePart(Name = "Button4", Type = typeof(Button))]
    [TemplatePart(Name = "Button5", Type = typeof(Button))]
    [TemplatePart(Name = "Button6", Type = typeof(Button))]
    [TemplatePart(Name = "SubtractButton", Type = typeof(Button))]
    [TemplatePart(Name = "Button1", Type = typeof(Button))]
    [TemplatePart(Name = "Button2", Type = typeof(Button))]
    [TemplatePart(Name = "Button3", Type = typeof(Button))]
    [TemplatePart(Name = "AdditionButton", Type = typeof(Button))]
    [TemplatePart(Name = "PlusMinusButton", Type = typeof(Button))]
    [TemplatePart(Name = "Button0", Type = typeof(Button))]
    [TemplatePart(Name = "DecimalButton", Type = typeof(Button))]
    [TemplatePart(Name = "EqualsButton", Type = typeof(Button))]
    public class Calculator : Control, IDropDownCalculator, ICalculatorControl
    {
        #region Controls

        public TextBlock MemoryStatusTextBlock { get; set; }
        public TextBlock EquationTextBlock { get; set; }
        public TextBlock EntryTextBlock { get; set; }

        private Button _mcButton;

        public Button McButton
        {
            get => _mcButton;
            set
            {
                if (_mcButton != null)
                    _mcButton.Click -= _mcButton_Click;

                _mcButton = value;

                if (_mcButton != null)
                    _mcButton.Click += _mcButton_Click;
            }
        }

        private Button _mrButton;

        public Button MrButton
        {
            get => _mrButton;
            set
            {
                if (_mrButton != null)
                    _mrButton.Click -= _mrButton_Click;

                _mrButton = value;

                if (_mrButton != null)
                    _mrButton.Click += _mrButton_Click;
            }
        }

        private Button _mAddButton;

        public Button MAddButton
        {
            get => _mAddButton;
            set
            {
                if (_mAddButton != null)
                    _mAddButton.Click -= _mAddButton_Click;

                _mAddButton = value;

                if (_mAddButton != null)
                    _mAddButton.Click += _mAddButton_Click;
            }
        }

        private Button _mSubtractButton;

        public Button MSubtractButton
        {
            get => _mSubtractButton;
            set
            {
                if (_mSubtractButton != null)
                    _mSubtractButton.Click -= _mSubtractButton_Click;

                _mSubtractButton = value;

                if (_mSubtractButton != null)
                    _mSubtractButton.Click += _mSubtractButton_Click;
            }
        }

        private Button _msButton;

        public Button MsButton
        {
            get => _msButton;
            set
            {
                if (_msButton != null)
                    _msButton.Click -= _msButton_Click;

                _msButton = value;
                
                if (_msButton != null)
                    _msButton.Click += _msButton_Click;
            }
        }

        private Button _plusMinusButton;

        public Button PlusMinusButton
        {
            get => _plusMinusButton;
            set
            {
                if (_plusMinusButton != null)
                    _plusMinusButton.Click -= _plusMinusButton_Click;

                _plusMinusButton = value;

                if (_plusMinusButton != null)
                    _plusMinusButton.Click += _plusMinusButton_Click;
            }
        }

        private Button _equalsButton;

        public Button EqualsButton
        {
            get => _equalsButton;
            set
            {
                if (_equalsButton != null)
                    _equalsButton.Click -= _equalsButton_Click;

                _equalsButton = value;

                if (_equalsButton != null)
                    _equalsButton.Click += _equalsButton_Click;
            }
        }

        private Button _ceButton;

        public Button CeButton
        {
            get => _ceButton;
            set
            {
                if (_ceButton != null)
                    _ceButton.Click -= _ceButton_Click;

                _ceButton = value;
                
                if (_ceButton != null)
                    _ceButton.Click += _ceButton_Click;
            }
        }

        private Button _clrButton;

        public Button ClrButton
        {
            get => _clrButton;
            set
            {
                if (_clrButton != null)
                    _clrButton.Click -= _clrButton_Click;

                _clrButton = value;

                if (_clrButton != null)
                    _clrButton.Click += _clrButton_Click;
            }
        }

        private Button _backButton;

        public Button BackButton
        {
            get => _backButton;
            set
            {
                if (_backButton != null)
                    _backButton.Click -= _backButton_Click;

                _backButton = value;

                if (_backButton != null)
                    _backButton.Click += _backButton_Click;
            }
        }

        private Button _divideButton;

        public Button DivideButton
        {
            get => _divideButton;
            set
            {
                if (_divideButton != null)
                    _divideButton.Click -= _divideButton_Click;

                _divideButton = value;

                if (_divideButton != null)
                    _divideButton.Click += _divideButton_Click;
            }
        }

        private Button _button7;

        public Button Button7
        {
            get => _button7;
            set
            {
                if (_button7 != null)
                    _button7.Click -= _button7_Click;

                _button7 = value;
                
                if (_button7 != null)
                    _button7.Click += _button7_Click;
            }
        }

        private Button _button8;
        public Button Button8
        {
            get => _button8;
            set
            {
                if (_button8 != null)
                    _button8.Click -= _button8_Click;

                _button8 = value;

                if (_button8 != null)
                    _button8.Click += _button8_Click;
            }
        }

        private Button _button9;
        public Button Button9
        {
            get => _button9;
            set
            {
                if (_button9 != null)
                    _button9.Click -= _button9_Click;

                _button9 = value;

                if (_button9 != null)
                    _button9.Click += _button9_Click;
            }
        }

        private Button _multiplyButton;

        public Button MultiplyButton
        {
            get => _multiplyButton;
            set
            {
                if (_multiplyButton != null)
                    _multiplyButton.Click -= _multiplyButton_Click;

                _multiplyButton = value;

                if (_multiplyButton != null)
                    _multiplyButton.Click += _multiplyButton_Click;
            }
        }

        private Button _button4;
        public Button Button4
        {
            get => _button4;
            set
            {
                if (_button4 != null)
                    _button4.Click -= _button4_Click;

                _button4 = value;

                if (_button4 != null)
                    _button4.Click += _button4_Click;
            }
        }

        private Button _button5;
        public Button Button5
        {
            get => _button5;
            set
            {
                if (_button5 != null)
                    _button5.Click -= _button5_Click;

                _button5 = value;

                if (_button5 != null)
                    _button5.Click += _button5_Click;
            }
        }

        private Button _button6;
        public Button Button6
        {
            get => _button6;
            set
            {
                if (_button6 != null)
                    _button6.Click -= _button6_Click;

                _button6 = value;

                if (_button6 != null)
                    _button6.Click += _button6_Click;
            }
        }

        private Button _subtractButton;

        public Button SubtractButton
        {
            get => _subtractButton;
            set
            {
                if (_subtractButton != null)
                    _subtractButton.Click -= _subtractButton_Click;

                _subtractButton = value;
                
                if (_subtractButton != null)
                    _subtractButton.Click += _subtractButton_Click;
            }
        }

        private Button _button1;
        public Button Button1
        {
            get => _button1;
            set
            {
                if (_button1 != null)
                    _button1.Click -= _button1_Click;

                _button1 = value;

                if (_button1 != null)
                    _button1.Click += _button1_Click;
            }
        }

        private Button _button2;
        public Button Button2
        {
            get => _button2;
            set
            {
                if (_button2 != null)
                    _button2.Click -= _button2_Click;

                _button2 = value;

                if (_button2 != null)
                    _button2.Click += _button2_Click;
            }
        }

        private Button _button3;
        public Button Button3
        {
            get => _button3;
            set
            {
                if (_button3 != null)
                    _button3.Click -= _button3_Click;

                _button3 = value;

                if (_button3 != null)
                    _button3.Click += _button3_Click;
            }
        }

        private Button _additionButton;

        public Button AdditionButton
        {
            get => _additionButton;
            set
            {
                if (_additionButton != null)
                    _additionButton.Click -= _additionButton_Click;

                _additionButton = value;

                if (_additionButton !=null)
                    _additionButton.Click += _additionButton_Click;
            }
        }

        private Button _button0;

        public Button Button0
        {
            get => _button0;
            set
            {
                if (_button0 != null)
                    _button0.Click -= _button0_Click;

                _button0 = value;

                if (_button0 != null)
                    _button0.Click += _button0_Click;
            }
        }

        private Button _decimalButton;

        public Button DecimalButton
        {
            get => _decimalButton;
            set
            {
                if (_decimalButton != null)
                    _decimalButton.Click -= _decimalButton_Click;

                _decimalButton = value;

                if (_decimalButton != null)
                    _decimalButton.Click += _decimalButton_Click;
            }
        }

        #endregion

        public Control Control => this;

        #region ICalculatorControl Interface

        public decimal? Value
        {
            get => Processor.ComittedValue;
            set => Processor.ReinitializeValue(value);
        }

        public int Precision
        {
            get => Processor.Precision;
            set => Processor.Precision = value;
        }

        public string EquationText
        {
            get
            {
                if (EquationTextBlock == null)
                    return string.Empty;

                return EquationTextBlock.Text;
            }
            set
            {
                if (EquationTextBlock != null)
                    EquationTextBlock.Text = value;
            }
        }

        public string EntryText
        {
            get
            {
                if (EntryTextBlock == null)
                    return string.Empty;

                return EntryTextBlock.Text;
            }
            set
            {
                if (EntryTextBlock != null)
                    EntryTextBlock.Text = value;
            }
        }

        public bool MemoryRecallEnabled
        {
            get
            {
                if (MrButton != null)
                    return MrButton.IsEnabled;

                return false;
            }
            set
            {
                if (MrButton != null)
                    MrButton.IsEnabled = value;
            }
        }

        public bool MemoryClearEnabled
        {
            get
            {
                if (McButton != null)
                    return McButton.IsEnabled;

                return false;
            }
            set
            {
                if (McButton != null)
                    McButton.IsEnabled = value;
            }
        }

        public bool MemoryStoreEnabled
        {
            get
            {
                if (MsButton != null)
                    return MsButton.IsEnabled;

                return false;
            }
            set
            {
                if (MsButton != null)
                    MsButton.IsEnabled = value;
            }
        }

        public bool MemoryPlusEnabled
        {
            get
            {
                if (MAddButton != null)
                    return MAddButton.IsEnabled;

                return false;
            }
            set
            {
                if (MAddButton != null)
                    MAddButton.IsEnabled = value;
            }
        }

        public bool MemoryMinusEnabled
        {
            get
            {
                if (MSubtractButton != null)
                    return MSubtractButton.IsEnabled;

                return false;
            }
            set
            {
                if (MSubtractButton != null)
                    MSubtractButton.IsEnabled = value;
            }
        }

        public bool MemoryStatusVisible
        {
            get
            {
                if (MemoryStatusTextBlock != null)
                    return MemoryStatusTextBlock.Visibility == Visibility.Visible;

                return false;
            }
            set
            {
                if (MemoryStatusTextBlock != null)
                {
                    if (value)
                        MemoryStatusTextBlock.Visibility = Visibility.Visible;
                    else
                        MemoryStatusTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion

        protected CalculatorProcessor Processor { get; }

        public event RoutedPropertyChangedEventHandler<object> ValueChanged;
        
        static Calculator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calculator), new FrameworkPropertyMetadata(typeof(Calculator)));
        }

        public Calculator()
        {
            Processor = new CalculatorProcessor(this);
        }

        public override void OnApplyTemplate()
        {
            MemoryStatusTextBlock = GetTemplateChild(nameof(MemoryStatusTextBlock)) as TextBlock;
            EquationTextBlock = GetTemplateChild(nameof(EquationTextBlock)) as TextBlock;
            EntryTextBlock = GetTemplateChild(nameof(EntryTextBlock)) as TextBlock;

            McButton = GetTemplateChild(nameof(McButton)) as Button;
            MrButton = GetTemplateChild(nameof(MrButton)) as Button;
            MAddButton = GetTemplateChild(nameof(MAddButton)) as Button;
            MSubtractButton = GetTemplateChild(nameof(MSubtractButton)) as Button;
            MsButton = GetTemplateChild(nameof(MsButton)) as Button;

            CeButton = GetTemplateChild(nameof(CeButton)) as Button;
            ClrButton = GetTemplateChild(nameof(ClrButton)) as Button;
            BackButton = GetTemplateChild(nameof(BackButton)) as Button;
            DivideButton = GetTemplateChild(nameof(DivideButton)) as Button;

            Button7 = GetTemplateChild(nameof(Button7)) as Button;
            Button8 = GetTemplateChild(nameof(Button8)) as Button;
            Button9 = GetTemplateChild(nameof(Button9)) as Button;
            MultiplyButton = GetTemplateChild(nameof(MultiplyButton)) as Button;

            Button4 = GetTemplateChild(nameof(Button4)) as Button;
            Button5 = GetTemplateChild(nameof(Button5)) as Button;
            Button6 = GetTemplateChild(nameof(Button6)) as Button;
            SubtractButton = GetTemplateChild(nameof(SubtractButton)) as Button;

            Button1 = GetTemplateChild(nameof(Button1)) as Button;
            Button2 = GetTemplateChild(nameof(Button2)) as Button;
            Button3 = GetTemplateChild(nameof(Button3)) as Button;
            AdditionButton = GetTemplateChild(nameof(AdditionButton)) as Button;
            
            PlusMinusButton = GetTemplateChild(nameof(PlusMinusButton)) as Button;
            Button0 = GetTemplateChild(nameof(Button0)) as Button;
            DecimalButton = GetTemplateChild(nameof(DecimalButton)) as Button;
            EqualsButton = GetTemplateChild(nameof(EqualsButton)) as Button;

            Processor.Initialize();

            base.OnApplyTemplate();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var keyChar = e.Key.GetCharFromKey();
            if (!Processor.ProcessChar(keyChar))
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        Processor.ProcessChar('=');
                        e.Handled = true;
                        break;
                    case Key.Delete:
                        Processor.ProcessCeButton();
                        break;
                    case Key.Back:
                        Processor.ProcessBackspace();
                        break;
                }
            }
            base.OnPreviewKeyDown(e);
        }

        #region Button Click Events

        private void _mcButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemoryClear();
        }

        private void _mrButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemoryRecall();
        }

        private void _mAddButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemoryAdd();
        }

        private void _mSubtractButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemorySubtract();
        }

        private void _msButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemoryStore();
        }

        private void _ceButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessCeButton();
        }

        private void _clrButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessCButton();
        }

        private void _backButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessBackspace();
        }

        private void _divideButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('/');
        }

        private void _button7_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('7');
        }

        private void _button8_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('8');
        }

        private void _button9_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('9');
        }

        private void _multiplyButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('*');
        }

        private void _button4_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('4');
        }

        private void _button5_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('5');
        }

        private void _button6_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('6');
        }

        private void _subtractButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('-');
        }

        private void _button1_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('1');
        }

        private void _button2_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('2');
        }

        private void _button3_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('3');
        }

        private void _additionButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('+');
        }

        private void _plusMinusButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessPlusMinusButton();
        }

        private void _button0_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('0');
        }

        private void _decimalButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessDecimal();
        }

        private void _equalsButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('=');
        }

        #endregion

        public void OnValueChanged(decimal? oldValue, decimal? newValue)
        {
            ValueChanged?.Invoke(this, new RoutedPropertyChangedEventArgs<object>(oldValue, newValue));
        }
    }
}
