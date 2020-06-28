using RingSoft.DataEntryControls.Engine;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    public enum NumericEditTypes
    {
        Decimal = 0,
        WholeNumber = 1
    }

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
    ///     <MyNamespace:NumericEditControl/>
    ///
    /// </summary>
    public abstract class NumericEditControl : DropDownEditControl, INumericControl
    {
        public abstract NumericEditTypes EditType { get; }

        public static readonly DependencyProperty DataEntryModeProperty =
            DependencyProperty.Register(nameof(DataEntryMode), typeof(DataEntryModes), typeof(NumericEditControl));

        public DataEntryModes DataEntryMode
        {
            get { return (DataEntryModes)GetValue(DataEntryModeProperty); }
            set { SetValue(DataEntryModeProperty, value); }
        }

        public static readonly DependencyProperty EditFormatTypeProperty =
            DependencyProperty.Register(nameof(EditFormatType), typeof(NumericEditFormatTypes), typeof(NumericEditControl));

        public NumericEditFormatTypes EditFormatType
        {
            get { return (NumericEditFormatTypes)GetValue(EditFormatTypeProperty); }
            set { SetValue(EditFormatTypeProperty, value); }
        }


        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(DataEntryNumericEditSetup), typeof(NumericEditControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        public DataEntryNumericEditSetup Setup
        {
            get { return (DataEntryNumericEditSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericEditControl = (NumericEditControl)obj;
            switch (numericEditControl.EditType)
            {
                case NumericEditTypes.Decimal:
                    break;
                case NumericEditTypes.WholeNumber:
                    numericEditControl.Setup.Precision = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            numericEditControl.DataEntryMode = numericEditControl.Setup.DataEntryMode;
            numericEditControl.EditFormatType = numericEditControl.Setup.EditFormatType;
            numericEditControl.Precision = numericEditControl.Setup.Precision;
            numericEditControl.MaximumValue = numericEditControl.Setup.MaximumValue;
            numericEditControl.MinimumValue = numericEditControl.Setup.MinimumValue;
            numericEditControl.NumberFormatString = numericEditControl.Setup.GetNumberFormatString();
            numericEditControl.CurrencySymbolLocation = numericEditControl.Setup.CurrencySymbolLocation;
            numericEditControl.Culture = numericEditControl.Setup.Culture;
        }

        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register(nameof(Precision), typeof(int), typeof(NumericEditControl));

        public int Precision
        {
            get { return (int)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(nameof(MaximumValue), typeof(decimal?), typeof(NumericEditControl));

        public decimal? MaximumValue
        {
            get { return (decimal?)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(nameof(MinimumValue), typeof(decimal?), typeof(NumericEditControl));

        public decimal? MinimumValue
        {
            get { return (decimal?)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        public static readonly DependencyProperty NumberFormatStringProperty =
            DependencyProperty.Register(nameof(NumberFormatString), typeof(string), typeof(NumericEditControl));

        public string NumberFormatString
        {
            get { return (string)GetValue(NumberFormatStringProperty); }
            set { SetValue(NumberFormatStringProperty, value); }
        }

        public static readonly DependencyProperty CurrencySymbolLocationProperty =
            DependencyProperty.Register(nameof(CurrencySymbolLocation), typeof(CurrencySymbolLocations), typeof(NumericEditControl));

        public CurrencySymbolLocations CurrencySymbolLocation
        {
            get { return (CurrencySymbolLocations)GetValue(CurrencySymbolLocationProperty); }
            set { SetValue(CurrencySymbolLocationProperty, value); }
        }

        public static readonly DependencyProperty CultureProperty =
            DependencyProperty.Register(nameof(Culture), typeof(CultureInfo), typeof(NumericEditControl));

        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        public string Text
        {
            get
            {
                if (TextBox == null)
                    return string.Empty;

                return TextBox.Text;
            }
            set
            {
                if (TextBox != null)
                    TextBox.Text = value;
            }
        }

        public int SelectionStart
        {
            get
            {
                if (TextBox == null)
                    return 0;

                return TextBox.SelectionStart;
            }
            set
            {
                if (TextBox != null)
                    TextBox.SelectionStart = value;
            }
        }

        public int SelectionLength
        {
            get
            {
                if (TextBox == null)
                    return 0;

                return TextBox.SelectionLength;
            }
            set
            {
                if (TextBox != null)
                    TextBox.SelectionLength = value;
            }
        }

        private DataEntryNumericControlProcessor _numericProcessor;
        private bool _settingText;

        static NumericEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericEditControl), new FrameworkPropertyMetadata(typeof(NumericEditControl)));
            CultureProperty.OverrideMetadata(typeof(NumericEditControl), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture));
        }

        public NumericEditControl()
        {
            _numericProcessor = new DataEntryNumericControlProcessor(this);
            _numericProcessor.ValueChanged += (sender, args) => OnValueChanged(args.NewValue);

            LostFocus += NumericEditControl_LostFocus;
        }

        private void NumericEditControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin && TextBox != null)
                SetText(TextBox.Text);
        }

        private void SetText(string text)
        {
            text = text.NumTextToString(Culture);
            decimal.TryParse(text, out var newResult);
            SetText(newResult);
        }

        protected void SetText(decimal? newValue)
        {
            if (TextBox == null)
                return;

            _settingText = true;

            if (newValue == null)
                TextBox.Text = string.Empty;
            else
            {
                var value = (decimal) newValue;
                TextBox.Text = value.ToString(NumberFormatString, Culture.NumberFormat);
            }

            _settingText = false;
        }

        private DataEntryNumericEditSetup GetSetup()
        {
            return new DataEntryNumericEditSetup()
            {
                DataEntryMode = DataEntryMode,
                EditFormatType = EditFormatType,
                MaximumValue = MaximumValue,
                MinimumValue = MinimumValue,
                Precision = Precision,
                NumberFormatString = NumberFormatString,
                CurrencySymbolLocation = CurrencySymbolLocation,
                Culture = Culture
            };
        }

        protected override bool ProcessKeyChar(char keyChar)
        {
            switch (DataEntryMode)
            {
                case DataEntryModes.FormatOnEntry:
                case DataEntryModes.ValidateOnly:
                    switch (_numericProcessor.ProcessChar(GetSetup(), keyChar))
                    {
                        case ProcessCharResults.Ignored:
                            return false;
                        case ProcessCharResults.Processed:
                            return true;
                        case ProcessCharResults.ValidationFailed:
                            System.Media.SystemSounds.Exclamation.Play();
                            return true;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case DataEntryModes.RawEntry:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return base.ProcessKeyChar(keyChar);
        }

        protected override bool ProcessKey(Key key)
        {
            switch (DataEntryMode)
            {
                case DataEntryModes.FormatOnEntry:
                    switch (key)
                    {
                        case Key.Space:
                            return ProcessKeyChar(' ');
                        case Key.Back:
                            _numericProcessor.OnBackspaceKeyDown(GetSetup());
                            return true;
                        case Key.Delete:
                            _numericProcessor.OnDeleteKeyDown(GetSetup());
                            return true;
                        case Key.Decimal:
                            if (_numericProcessor.ProcessDecimal(GetSetup()) == ProcessCharResults.ValidationFailed)
                                System.Media.SystemSounds.Exclamation.Play();

                            return true;
                    }
                    break;
                case DataEntryModes.ValidateOnly:
                    switch (key)
                    {
                        case Key.Decimal:
                            if (_numericProcessor.ProcessDecimal(GetSetup()) == ProcessCharResults.ValidationFailed)
                                System.Media.SystemSounds.Exclamation.Play();

                            return true;
                    }
                    break;
                case DataEntryModes.RawEntry:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.ProcessKey(key);
        }

        protected override void OnTextChanged(string newText)
        {
            if (_settingText)
            {
                base.OnTextChanged(newText);
                return;
            }

            switch (DataEntryMode)
            {
                case DataEntryModes.FormatOnEntry:
                    break;
                case DataEntryModes.ValidateOnly:
                case DataEntryModes.RawEntry:
                    OnValueChanged(newText);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.OnTextChanged(newText);
        }
    }
}
