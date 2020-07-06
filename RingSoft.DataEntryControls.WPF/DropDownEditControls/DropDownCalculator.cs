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
    [TemplatePart(Name = "TapeTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "EntryTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "McButton", Type = typeof(Button))]
    [TemplatePart(Name = "MrButton", Type = typeof(Button))]
    [TemplatePart(Name = "MAddButton", Type = typeof(Button))]
    [TemplatePart(Name = "MSubtractButton", Type = typeof(Button))]
    [TemplatePart(Name = "MsButton", Type = typeof(Button))]

    [TemplatePart(Name = "PlusMinusButton", Type = typeof(Button))]
    [TemplatePart(Name = "EqualsButton", Type = typeof(Button))]
    public class DropDownCalculator : Control, IDropDownCalculator, ICalculatorControl
    {
        public TextBlock MemoryStatusTextBlock { get; set; }
        public TextBlock TapeTextBlock { get; set; }
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


        public Control Control => this;

        public decimal? Value
        {
            get => Processor.ComittedValue;
            set => Processor.InitializeValue(value);
        }

        public int Precision
        {
            get => Processor.Precision;
            set => Processor.Precision = value;
        }

        public string TapeText
        {
            get
            {
                if (TapeTextBlock == null)
                    return string.Empty;

                return TapeTextBlock.Text;
            }
            set
            {
                if (TapeTextBlock != null)
                    TapeTextBlock.Text = value;
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

        protected CalculatorProcessor Processor { get; }

        public event RoutedPropertyChangedEventHandler<object> ValueChanged;
        
        static DropDownCalculator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownCalculator), new FrameworkPropertyMetadata(typeof(DropDownCalculator)));
        }

        public DropDownCalculator()
        {
            Processor = new CalculatorProcessor(this);
        }

        public override void OnApplyTemplate()
        {
            MemoryStatusTextBlock = GetTemplateChild(nameof(MemoryStatusTextBlock)) as TextBlock;
            TapeTextBlock = GetTemplateChild(nameof(TapeTextBlock)) as TextBlock;
            EntryTextBlock = GetTemplateChild(nameof(EntryTextBlock)) as TextBlock;

            McButton = GetTemplateChild(nameof(McButton)) as Button;
            MrButton = GetTemplateChild(nameof(MrButton)) as Button;
            MAddButton = GetTemplateChild(nameof(MAddButton)) as Button;
            MSubtractButton = GetTemplateChild(nameof(MSubtractButton)) as Button;
            MsButton = GetTemplateChild(nameof(MsButton)) as Button;

            PlusMinusButton = GetTemplateChild(nameof(PlusMinusButton)) as Button;
            EqualsButton = GetTemplateChild(nameof(EqualsButton)) as Button;

            Processor.OnMemoryChanged();

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
                        Processor.ProcessButton(CalculatorButtons.CeButton);
                        break;
                }
            }
            base.OnPreviewKeyDown(e);
        }

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

        private void _plusMinusButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessPlusMinusButton();
        }

        private void _equalsButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('=');
        }

        public void OnValueChanged(decimal? oldValue, decimal? newValue)
        {
            ValueChanged?.Invoke(this, new RoutedPropertyChangedEventArgs<object>(oldValue, newValue));
        }
    }
}
