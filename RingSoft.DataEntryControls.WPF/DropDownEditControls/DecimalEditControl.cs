using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using System;
using System.Media;
using System.Windows;
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
    ///     <MyNamespace:DecimalEditControl/>
    ///
    /// </summary>

    [TemplatePart(Name = "Calculator", Type = typeof(IDropDownCalculator))]
    public class DecimalEditControl : NumericEditControl<decimal?>
    {
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

        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register(nameof(Precision), typeof(int), typeof(DecimalEditControl),
                new FrameworkPropertyMetadata(2, PrecisionChangedCallback));

        public int Precision
        {
            get { return (int)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        private static void PrecisionChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var decimalEditControl = (DecimalEditControl)obj;
            decimalEditControl.SetValue();
        }

        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(DecimalEditControlSetup), typeof(DecimalEditControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        public DecimalEditControlSetup Setup
        {
            private get { return (DecimalEditControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var decimalEditControl = (DecimalEditControl)obj;
            decimalEditControl.LoadFromSetup(decimalEditControl.Setup);
        }

        public static readonly DependencyProperty FormatTypeProperty =
            DependencyProperty.Register(nameof(FormatType), typeof(DecimalEditFormatTypes), typeof(DecimalEditControl),
                new FrameworkPropertyMetadata(FormatTypeChangedCallback));

        public DecimalEditFormatTypes FormatType
        {
            get { return (DecimalEditFormatTypes)GetValue(FormatTypeProperty); }
            set { SetValue(FormatTypeProperty, value); }
        }

        private static void FormatTypeChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var decimalEditControl = (DecimalEditControl)obj;
            decimalEditControl.SetValue();
        }

        public event EventHandler CalculatorValueChanged;

        private decimal? _pendingNewValue;

        static DecimalEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalEditControl), new FrameworkPropertyMetadata(typeof(DecimalEditControl)));
            TextAlignmentProperty.OverrideMetadata(typeof(DecimalEditControl), new FrameworkPropertyMetadata(TextAlignment.Right));
        }

        public override void OnApplyTemplate()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            CalculatorControl = GetTemplateChild("Calculator") as IDropDownCalculator;
            base.OnApplyTemplate();

            if (_pendingNewValue != null)
                SetValue();

            _pendingNewValue = null;
        }

        protected override void LoadFromSetup(NumericEditControlSetup<decimal?> setup)
        {
            FormatType = Setup.FormatType;
            Precision = Setup.Precision;

            base.LoadFromSetup(setup);
        }

        protected override void SetValue()
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

        protected override void SetDefaultValue()
        {
            Value = 0;
        }

        protected decimal? ValidateValue(decimal? newValue)
        {
            decimal? result = null;
            if (MaximumValue != null)
            {
                if (newValue > MaximumValue)
                    result = MaximumValue;
            }

            if (MinimumValue != null)
            {
                if (newValue < MinimumValue)
                    result = MinimumValue;
            }

            return result;
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
                var precision = Precision;

                CalculatorControl.Precision = precision;
                if (Value != null)
                {
                    var calcValue = (decimal)Value;
                    if (FormatType == DecimalEditFormatTypes.Percent)
                    {
                        calcValue *= 100;
                        calcValue = Math.Round(calcValue, Precision);
                    }
                    CalculatorControl.Value = calcValue;
                }

                CalculatorControl.Control.Focus();
            }
        }

        private void _calculatorControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var newValue = CalculatorControl.Value;

            if (FormatType == DecimalEditFormatTypes.Percent)
                newValue /= 100;

            var validatedValue = ValidateValue(newValue);
            if (validatedValue != null)
            {
                newValue = validatedValue;
                SystemSounds.Exclamation.Play();
            }

            var valueChanged = !newValue.Equals(Value);
            Value = newValue;

            if (valueChanged)
            {
                OnValueChanged(Text);
                OnCalculatorValueChanged();
            }
        }

        protected virtual void OnCalculatorValueChanged()
        {
            CalculatorValueChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void PopulateSetup(DecimalEditControlSetup setup)
        {
            setup.FormatType = FormatType;
            setup.MaximumValue = MaximumValue;
            setup.MinimumValue = MinimumValue;
            setup.Precision = Precision;
            base.PopulateSetup(setup);
        }

        public override void OnValueChanged(string newValue)
        {
            if (newValue.IsNullOrEmpty())
                Value = null;
            else 
                Value = newValue.ToDecimal(Culture);

            base.OnValueChanged(newValue);
        }
    }
}
