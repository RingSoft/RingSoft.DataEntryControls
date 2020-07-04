using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;

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
    
    [TemplatePart(Name = "TapeTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "EntryTextBlock", Type = typeof(TextBlock))]
    public class DropDownCalculator : Control, IDropDownCalculator, ICalculatorControl
    {
        public TextBlock TapeTextBlock { get; set; }

        public TextBlock EntryTextBlock { get; set; }

        public Control Control => this;

        private decimal? _value;

        public decimal? Value
        {
            get => _value;
            set
            {
                var oldValue = _value;
                _value = value;

                Processor.SetValue(value);
                ValueChanged?.Invoke(this, new RoutedPropertyChangedEventArgs<object>(oldValue, _value));
            }
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
            TapeTextBlock = GetTemplateChild(nameof(TapeTextBlock)) as TextBlock;
            EntryTextBlock = GetTemplateChild(nameof(EntryTextBlock)) as TextBlock;
            
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
                        break;
                    case Key.Delete:
                        Processor.ProcessButton(CalculatorButtons.CeButton);
                        break;
                }
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
