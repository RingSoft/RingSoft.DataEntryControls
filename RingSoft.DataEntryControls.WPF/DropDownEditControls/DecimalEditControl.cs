using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using System;
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
    public class DecimalEditControl : NumericEditControl
    {
        public override NumericEditTypes EditType => NumericEditTypes.Decimal;

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
            DependencyProperty.Register(nameof(Precision), typeof(int), typeof(DecimalEditControl));

        public int Precision
        {
            get { return (int)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(nameof(MaximumValue), typeof(decimal?), typeof(DecimalEditControl));

        public decimal? MaximumValue
        {
            get { return (decimal?)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(nameof(MinimumValue), typeof(decimal?), typeof(DecimalEditControl));

        public decimal? MinimumValue
        {
            get { return (decimal?)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
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
            decimalEditControl.DataEntryMode = decimalEditControl.Setup.DataEntryMode;
            decimalEditControl.EditFormatType = decimalEditControl.Setup.EditFormatType;
            decimalEditControl.Precision = decimalEditControl.Setup.Precision;
            decimalEditControl.MaximumValue = decimalEditControl.Setup.MaximumValue;
            decimalEditControl.MinimumValue = decimalEditControl.Setup.MinimumValue;
            decimalEditControl.NumberFormatString = decimalEditControl.Setup.NumberFormatString;
            decimalEditControl.Culture = decimalEditControl.Setup.Culture;
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

        public static readonly DependencyProperty EditFormatTypeProperty =
            DependencyProperty.Register(nameof(EditFormatType), typeof(DecimalEditFormatTypes), typeof(DecimalEditControl));

        public DecimalEditFormatTypes EditFormatType
        {
            get { return (DecimalEditFormatTypes)GetValue(EditFormatTypeProperty); }
            set { SetValue(EditFormatTypeProperty, value); }
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
                var precision = Precision;

                CalculatorControl.Precision = precision;
                if (Value != null)
                {
                    var calcValue = (decimal)Value;
                    if (EditFormatType == DecimalEditFormatTypes.Percent)
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

            if (EditFormatType == DecimalEditFormatTypes.Percent)
                newValue /= 100;

            Value = newValue;
        }

        protected override void PopulateSetup(DecimalEditControlSetup setup)
        {
            setup.EditFormatType = EditFormatType;
            setup.MaximumValue = MaximumValue;
            setup.MinimumValue = MinimumValue;
            setup.Precision = Precision;
            base.PopulateSetup(setup);
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
