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

        public static readonly DependencyProperty NumberFormatStringProperty =
            DependencyProperty.Register(nameof(NumberFormatString), typeof(string), typeof(NumericEditControl));

        public string NumberFormatString
        {
            get { return (string)GetValue(NumberFormatStringProperty); }
            set { SetValue(NumberFormatStringProperty, value); }
        }

        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(NumericEditControl),
                new FrameworkPropertyMetadata(CultureIdChangedCallback));

        public string CultureId
        {
            get { return (string)GetValue(CultureIdProperty); }
            set { SetValue(CultureIdProperty, value); }
        }

        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericEditControl = (NumericEditControl) obj;
            var culture = new CultureInfo(numericEditControl.CultureId);
            numericEditControl.Culture = culture;
        }

        public CultureInfo Culture { get; protected internal set; }

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
        }

        public NumericEditControl()
        {
            if (Culture == null)
                Culture = CultureInfo.CurrentCulture;

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
            SetText(text.ToDecimal(Culture));
        }

        protected void SetText(decimal? newValue)
        {
            if (TextBox == null)
                return;

            _settingText = true;

            var setup = GetSetup();
            if (newValue == null)
                TextBox.Text = string.Empty;
            else
            {
                var value = (decimal) newValue;
                var newText = value.ToString(setup.GetNumberFormatString(), Culture.NumberFormat);
                if (TextBox.IsFocused)
                    OnFocusedSetText(newText, setup);
                else 
                    TextBox.Text = newText;
            }

            _settingText = false;
        }

        protected override void OnTextBoxGotFocus()
        {
            OnFocusedSetText(TextBox.Text, GetSetup());
            base.OnTextBoxGotFocus();
        }

        private void OnFocusedSetText(string newText, DecimalEditControlSetup setup)
        {
            if (TextBox != null)
            {
                _settingText = true;
                TextBox.Text = _numericProcessor.FormatTextForEntry(setup, newText);
                _settingText = false;
            }
        }

        private DecimalEditControlSetup GetSetup()
        {
            var result = new DecimalEditControlSetup();
            PopulateSetup(result);
            return result;
        }

        protected virtual void PopulateSetup(DecimalEditControlSetup setup)
        {
            setup.DataEntryMode = DataEntryMode;
            setup.NumberFormatString = NumberFormatString;
            setup.CultureId = Culture.Name;
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
                            System.Media.SystemSounds.Exclamation.Play();
                            return true;
                        case Key.Back:
                            if (_numericProcessor.OnBackspaceKeyDown(GetSetup()) == ProcessCharResults.ValidationFailed)
                                System.Media.SystemSounds.Exclamation.Play();
                            return true;
                        case Key.Delete:
                            if (_numericProcessor.OnDeleteKeyDown(GetSetup()) == ProcessCharResults.ValidationFailed)
                                System.Media.SystemSounds.Exclamation.Play();
                            return true;
                    }
                    break;
                case DataEntryModes.ValidateOnly:
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

            _settingText = true;
            switch (DataEntryMode)
            {
                case DataEntryModes.FormatOnEntry:
                case DataEntryModes.ValidateOnly:
                    if (!_numericProcessor.PasteText(GetSetup(), newText))
                        System.Media.SystemSounds.Exclamation.Play();
                    break;
                case DataEntryModes.RawEntry:
                    OnValueChanged(newText);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _settingText = false;

            base.OnTextChanged(newText);
        }
    }
}
