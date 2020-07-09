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
    ///     <MyNamespace:DecimalEditControl/>
    ///
    /// </summary>

    [TemplatePart(Name = "Calculator", Type = typeof(IDropDownCalculator))]
    public class DecimalEditControl : NumericEditControl
    {
        public override NumericEditTypes EditType => NumericEditTypes.Decimal;

        private Expander _expander;

        public Expander Expander
        {
            get => _expander;
            set
            {
                _expander = value;
            }
        }
        private IDropDownCalculator _calculatorControl;

        public IDropDownCalculator CalculatorControl
        {
            get => _calculatorControl;
            set
            {
                if (_calculatorControl != null)
                {
                    _calculatorControl.ValueChanged -= _calculatorControl_ValueChanged;
                }

                _calculatorControl = value;
                
                if (_calculatorControl != null)
                {
                    _calculatorControl.ValueChanged += _calculatorControl_ValueChanged;
                }
            }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(decimal?), typeof(DecimalEditControl),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        public decimal? Value
        {
            get { return (decimal?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var decimalEditControl = (DecimalEditControl) obj;
            if (!decimalEditControl._textSettingValue)
                decimalEditControl.SetValue();
        }

        private decimal? _pendingNewValue;
        private bool _textSettingValue;

        static DecimalEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalEditControl), new FrameworkPropertyMetadata(typeof(DecimalEditControl)));
            PrecisionProperty.OverrideMetadata(typeof(DecimalEditControl), new FrameworkPropertyMetadata(2));
            TextAlignmentProperty.OverrideMetadata(typeof(DecimalEditControl), new FrameworkPropertyMetadata(TextAlignment.Right));
        }

        public override void OnApplyTemplate()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            CalculatorControl = GetTemplateChild("Calculator") as IDropDownCalculator;
            Expander = GetTemplateChild("Expander") as Expander;
            base.OnApplyTemplate();

            if (_pendingNewValue != null)
                SetValue();

            _pendingNewValue = null;
        }

        private void SetValue()
        {
            if (TextBox == null)
            {
                _pendingNewValue = Value;
            }
            else
            {
                SetText(Value);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            if (e.Key == Key.F4)
            {
                OnDropDownButtonClick();
                e.Handled = true;
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnDropDownButtonClick()
        {
            base.OnDropDownButtonClick();

            if (CalculatorControl != null && Popup != null && Popup.IsOpen)
            {
                CalculatorControl.Precision = Precision;
                if (Value != null)
                    CalculatorControl.Value = Value;

                CalculatorControl.Control.Focus();
            }
        }

        private void _calculatorControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Value = CalculatorControl.Value;
        }

        public override void OnValueChanged(string newValue)
        {
            _textSettingValue = true;

            var decimalValue = newValue.ToDecimal(Culture);
            
            Value = decimalValue;

            _textSettingValue = false;

            base.OnValueChanged(newValue);
        }
    }
}
