using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.Date;

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
    ///     <MyNamespace:DateEditControl/>
    ///
    /// </summary>
    [TemplatePart(Name = "Calendar", Type = typeof(IDropDownCalendar))]
    public class DateEditControl : DropDownEditControl, IDateEditControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(DateTime?), typeof(DateEditControl),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditControl = (DateEditControl)obj;
            if (!dateEditControl._textSettingValue)
            {
                dateEditControl.SetValue();
                dateEditControl.OnValueChanged(dateEditControl.Text);
            }
        }

        public static readonly DependencyProperty EntryFormatProperty =
            DependencyProperty.Register(nameof(EntryFormat), typeof(string), typeof(DateEditControl),
                new FrameworkPropertyMetadata(EntryFormatChangedCallback));

        public string EntryFormat
        {
            get { return (string)GetValue(EntryFormatProperty); }
            set { SetValue(EntryFormatProperty, value); }
        }

        private static void EntryFormatChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditControl = (DateEditControl)obj;

            if (!dateEditControl._validatingEntryFormat)
            {
                dateEditControl._validatingEntryFormat = true;

                try
                {
                    DateEditControlSetup.ValidateEntryFormat(dateEditControl.EntryFormat);
                    if (!dateEditControl._textSettingValue)
                       dateEditControl.SetValue();
                }
                finally
                {
                    dateEditControl._validatingEntryFormat = false;
                }
            }
        }

        public static readonly DependencyProperty DisplayFormatProperty =
            DependencyProperty.Register(nameof(DisplayFormat), typeof(string), typeof(DateEditControl),
                new FrameworkPropertyMetadata(DisplayFormatChangedCallback));

        public string DisplayFormat
        {
            get { return (string)GetValue(DisplayFormatProperty); }
            set { SetValue(DisplayFormatProperty, value); }
        }

        private static void DisplayFormatChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditControl = (DateEditControl)obj;
            DateEditControlSetup.ValidateDateFormat(dateEditControl.DisplayFormat);
            if (!dateEditControl._textSettingValue)
                dateEditControl.SetValue();
        }

        public static readonly DependencyProperty DateFormatTypeProperty =
            DependencyProperty.Register(nameof(DateFormatType), typeof(DateFormatTypes), typeof(DateEditControl));

        public DateFormatTypes DateFormatType
        {
            get { return (DateFormatTypes)GetValue(DateFormatTypeProperty); }
            set { SetValue(DateFormatTypeProperty, value); }
        }

        public static readonly DependencyProperty MinimumDateProperty =
            DependencyProperty.Register(nameof(MinimumDate), typeof(DateTime?), typeof(DateEditControl));

        public DateTime? MinimumDate
        {
            get { return (DateTime?)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        public static readonly DependencyProperty MaximumDateProperty =
            DependencyProperty.Register(nameof(MaximumDate), typeof(DateTime?), typeof(DateEditControl));

        public DateTime? MaximumDate
        {
            get { return (DateTime?)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(DateEditControl),
                new FrameworkPropertyMetadata(CultureIdChangedCallback));

        public string CultureId
        {
            get { return (string)GetValue(CultureIdProperty); }
            set { SetValue(CultureIdProperty, value); }
        }

        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditControl = (DateEditControl)obj;
            var culture = new CultureInfo(dateEditControl.CultureId);
            dateEditControl.Culture = culture;
            if (!dateEditControl._textSettingValue)
                dateEditControl.SetValue();
        }

        public CultureInfo Culture { get; protected internal set; }


        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(DateEditControlSetup), typeof(DateEditControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        public DateEditControlSetup Setup
        {
            private get { return (DateEditControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditControl = (DateEditControl)obj;
            dateEditControl.DateFormatType = dateEditControl.Setup.DateFormatType;
            dateEditControl.DisplayFormat = dateEditControl.Setup.DisplayFormat;
            dateEditControl.EntryFormat = dateEditControl.Setup.EntryFormat;
            dateEditControl.MaximumDate = dateEditControl.Setup.MaximumDate;
            dateEditControl.MinimumDate = dateEditControl.Setup.MinimumDate;
            dateEditControl.CultureId = dateEditControl.Setup.CultureId;
            dateEditControl.AllowNullValue = dateEditControl.Setup.AllowNullValue;
        }

        public static readonly DependencyProperty AllowNullValueProperty =
            DependencyProperty.Register(nameof(AllowNullValue), typeof(bool), typeof(DateEditControl));

        public bool AllowNullValue
        {
            get { return (bool)GetValue(AllowNullValueProperty); }
            set { SetValue(AllowNullValueProperty, value); }
        }

        public static readonly DependencyProperty PlayValidationSoundOnLostFocusProperty =
            DependencyProperty.Register(nameof(PlayValidationSoundOnLostFocus), typeof(bool), typeof(DateEditControl));

        public bool PlayValidationSoundOnLostFocus
        {
            get => (bool) GetValue(PlayValidationSoundOnLostFocusProperty);
            set => SetValue(PlayValidationSoundOnLostFocusProperty, value);
        }

        private IDropDownCalendar _calendar;

        public IDropDownCalendar Calendar
        {
            get => _calendar;
            set
            {
                if (_calendar != null)
                {
                    _calendar.SelectedDateChanged -= _calendar_SelectedDateChanged;
                    _calendar.DatePicked -= _calendar_DatePicked;
                }

                _calendar = value;

                if (_calendar != null)
                {
                    _calendar.SelectedDateChanged += _calendar_SelectedDateChanged;
                    _calendar.DatePicked += _calendar_DatePicked;
                }
            }
        }


        private bool _innerValidateLostFocus;
        private DateTime? _pendingNewValue;
        private bool _textSettingValue;
        private bool _validatingEntryFormat;
        private DateEditProcessor _processor;

        static DateEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateEditControl),
                new FrameworkPropertyMetadata(typeof(DateEditControl)));

            PlayValidationSoundOnLostFocusProperty.OverrideMetadata(typeof(DateEditControl),
                new FrameworkPropertyMetadata(true));
        }

        public DateEditControl()
        {
            _processor = new DateEditProcessor(this);
            _processor.ValueChanged += _processor_ValueChanged;

            LostFocus += (sender, args) => OnLostFocusSetText(GetSetup(), Value);
            GotFocus += (sender, args) => _innerValidateLostFocus = false;
        }

        public override void OnApplyTemplate()
        {
            Calendar = GetTemplateChild(nameof(Calendar)) as IDropDownCalendar;
            base.OnApplyTemplate();

            if (_pendingNewValue != null)
                SetValue();

            _pendingNewValue = null;
        }

        private DateEditControlSetup GetSetup()
        {
            return new DateEditControlSetup()
            {
                DateFormatType = DateFormatType,
                DisplayFormat = DisplayFormat,
                EntryFormat = EntryFormat,
                MaximumDate = MaximumDate,
                MinimumDate = MinimumDate,
                CultureId = CultureId,
                AllowNullValue = AllowNullValue
            };
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

        protected void SetText(DateTime? newValue)
        {
            if (TextBox == null)
                return;

            if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty())
                return;

            var popupIsOpen = false;
            if (Popup != null)
                popupIsOpen = Popup.IsOpen;

            var setup = GetSetup();
            if (TextBox.IsFocused || popupIsOpen)
                OnFocusedSetText(setup, newValue);
            else
                OnLostFocusSetText(setup, newValue);
        }

        protected override void OnTextBoxGotFocus()
        {
            OnFocusedSetText(GetSetup(), Value);
            base.OnTextBoxGotFocus();
        }

        private void OnFocusedSetText(DateEditControlSetup setup, DateTime? value)
        {
            _textSettingValue = true;

            _processor.OnSetFocus(setup, value);

            _textSettingValue = false;
        }

        private void OnLostFocusSetText(DateEditControlSetup setup, DateTime? value)
        {
            if (Popup != null && Popup.IsOpen)
                return;

            _textSettingValue = true;

            var result = _processor.OnLostFocus(setup, value);
            if (result != null)
            {
                Value = result;
            }

            if (PlayValidationSoundOnLostFocus && (result != null || value == null) && _innerValidateLostFocus)
                System.Media.SystemSounds.Exclamation.Play();

            _textSettingValue = false;
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

            if (Calendar != null && Popup != null && Popup.IsOpen)
            {
                Calendar.SelectedDate = Value ?? DateTime.Today;
                Calendar.MaximumDate = MaximumDate;
                Calendar.MinimumDate = MinimumDate;

                Calendar.Control.Focus();
            }
        }

        private void _calendar_SelectedDateChanged(object sender, EventArgs e)
        {
            SetValueChanged();
            _innerValidateLostFocus = true;
        }

        private void _calendar_DatePicked(object sender, EventArgs e)
        {
            SetValueChanged();
            OnDropDownButtonClick();
        }

        private void SetValueChanged()
        {
            var changedValue = Value != Calendar.SelectedDate;
            Value = Calendar.SelectedDate;
            if (changedValue)
                OnValueChanged(Text);
        }

        protected override bool ProcessKeyChar(char keyChar)
        {
            switch (_processor.ProcessChar(GetSetup(), keyChar))
            {
                case ProcessCharResults.Ignored:
                    return false;
                case ProcessCharResults.Processed:
                    _innerValidateLostFocus = true;
                    return true;
                case ProcessCharResults.ValidationFailed:
                    System.Media.SystemSounds.Exclamation.Play();
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override bool ProcessKey(Key key)
        {
            switch (key)
            {
                case Key.Space:
                    if (_processor.OnSpaceKey(GetSetup()) == ProcessCharResults.ValidationFailed)
                        System.Media.SystemSounds.Exclamation.Play();
                    else if (Text != _processor.GetNullDatePattern())
                        _innerValidateLostFocus = true;
                    return true;
                case Key.Back:
                    if (_processor.OnBackspaceKeyDown(GetSetup()) == ProcessCharResults.ValidationFailed)
                        System.Media.SystemSounds.Exclamation.Play();
                    else if (Text != _processor.GetNullDatePattern())
                        _innerValidateLostFocus = true;
                    return true;
                case Key.Delete:
                    if (_processor.OnDeleteKeyDown(GetSetup()) == ProcessCharResults.ValidationFailed)
                        System.Media.SystemSounds.Exclamation.Play();

                    return true;
            }
            return base.ProcessKey(key);
        }

        protected override void OnTextChanged(string newText)
        {
            if (_textSettingValue)
            {
                base.OnTextChanged(newText);
                return;
            }

            _textSettingValue = true;

            if (!_processor.PasteText(GetSetup(), newText))
                System.Media.SystemSounds.Exclamation.Play();

            _textSettingValue = false;

            base.OnTextChanged(newText);
        }


        private void _processor_ValueChanged(object sender, EventArgs e)
        {
            _textSettingValue = true;

            Value = _processor.Value;
            OnValueChanged(Text);

            _textSettingValue = false;
        }
    }
}
