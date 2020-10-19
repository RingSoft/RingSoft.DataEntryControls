using RingSoft.DataEntryControls.Engine;
using System;
using System.ComponentModel;
using System.Globalization;
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
    ///     <MyNamespace:NumericEditControl/>
    ///
    /// </summary>
    public abstract class NumericEditControl<T> : DropDownEditControl, INumericControl
    {
        public static readonly DependencyProperty DataEntryModeProperty =
            DependencyProperty.Register(nameof(DataEntryMode), typeof(DataEntryModes), typeof(NumericEditControl<T>));

        public DataEntryModes DataEntryMode
        {
            get { return (DataEntryModes)GetValue(DataEntryModeProperty); }
            set { SetValue(DataEntryModeProperty, value); }
        }

        public static readonly DependencyProperty NumberFormatStringProperty =
            DependencyProperty.Register(nameof(NumberFormatString), typeof(string), typeof(NumericEditControl<T>));

        public string NumberFormatString
        {
            get { return (string)GetValue(NumberFormatStringProperty); }
            set { SetValue(NumberFormatStringProperty, value); }
        }

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(nameof(MaximumValue), typeof(T), typeof(NumericEditControl<T>));

        public T MaximumValue
        {
            get { return (T)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(nameof(MinimumValue), typeof(T), typeof(NumericEditControl<T>));

        public T MinimumValue
        {
            get { return (T)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(T), typeof(NumericEditControl<T>),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericEditControl = (NumericEditControl<T>)obj;

            if (!numericEditControl._settingText)
            {
                numericEditControl.SetValue();
            }
        }

        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(NumericEditControl<T>),
                new FrameworkPropertyMetadata(CultureIdChangedCallback));

        public string CultureId
        {
            get { return (string)GetValue(CultureIdProperty); }
            set { SetValue(CultureIdProperty, value); }
        }

        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericEditControl = (NumericEditControl<T>) obj;
            var culture = new CultureInfo(numericEditControl.CultureId);
            numericEditControl.Culture = culture;
            if (!numericEditControl._settingText)
                numericEditControl.SetValue();
        }

        public CultureInfo Culture { get; protected internal set; }

        private DataEntryNumericControlProcessor _numericProcessor;
        private bool _settingText;

        static NumericEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericEditControl<T>), new FrameworkPropertyMetadata(typeof(NumericEditControl<T>)));
        }

        public NumericEditControl()
        {
            if (Culture == null)
                Culture = CultureInfo.CurrentCulture;

            _numericProcessor = new DataEntryNumericControlProcessor(this);
            _numericProcessor.ValueChanged += (sender, args) =>
            {
                _settingText = true;
                OnValueChanged(args.NewValue);
                _settingText = false;
            };

            LostFocus += NumericEditControl_LostFocus;
        }

        protected virtual void LoadFromSetup(NumericEditControlSetup<T> setup)
        {
            DataEntryMode = setup.DataEntryMode;
            MaximumValue = setup.MaximumValue;
            MinimumValue = setup.MinimumValue;
            NumberFormatString = setup.NumberFormatString;
            Culture = setup.Culture;
        }

        protected abstract void SetValue();

        private void NumericEditControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin && TextBox != null)
            {
                if (Value == null)
                    SetText((decimal?)null);
                else 
                    SetText(TextBox.Text);
            }
        }

        private void SetText(string text)
        {
            SetText(text.ToDecimal(Culture));
        }

        protected void SetText(decimal? newValue)
        {
            if (TextBox == null)
                return;

            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty())
                return;

            _settingText = true;

            var setup = GetSetup();
            if (newValue == null)
                TextBox.Text = String.Empty;
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
                            SystemSounds.Exclamation.Play();
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
                            SystemSounds.Exclamation.Play();
                            return true;
                        case Key.Back:
                            if (_numericProcessor.OnBackspaceKeyDown(GetSetup()) == ProcessCharResults.ValidationFailed)
                                SystemSounds.Exclamation.Play();
                            return true;
                        case Key.Delete:
                            if (_numericProcessor.OnDeleteKeyDown(GetSetup()) == ProcessCharResults.ValidationFailed)
                                SystemSounds.Exclamation.Play();
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
                        SystemSounds.Exclamation.Play();
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
