﻿using RingSoft.DataEntryControls.Engine;
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
                dateEditControl.SetValue();
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
            DateEditControlSetup.ValidateEntryFormat(dateEditControl.EntryFormat);
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
        }

        public static readonly DependencyProperty DateFormatTypeProperty =
            DependencyProperty.Register(nameof(DateFormatType), typeof(DateFormatTypes), typeof(DateEditControl));

        public DateFormatTypes DateFormatType
        {
            get { return (DateFormatTypes)GetValue(DateFormatTypeProperty); }
            set { SetValue(DateFormatTypeProperty, value); }
        }

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

        private DateTime? _pendingNewValue;
        private bool _textSettingValue;
        private DateEditProcessor _processor;

        static DateEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateEditControl), new FrameworkPropertyMetadata(typeof(DateEditControl)));
        }

        public DateEditControl()
        {
            _processor = new DateEditProcessor(this);
            _processor.ValueChanged += _processor_ValueChanged;
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
                EntryFormat = EntryFormat
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

            _textSettingValue = true;

            var setup = GetSetup();
            if (newValue == null)
                TextBox.Text = string.Empty;
            else
            {
                var value = (DateTime)newValue;
                var newText = value.ToString(setup.GetDisplayFormat());
                TextBox.Text = newText;
            }

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
                Calendar.Control.Focus();
            }
        }

        private void _calendar_SelectedDateChanged(object sender, EventArgs e)
        {
            Value = Calendar.SelectedDate;
        }

        private void _calendar_DatePicked(object sender, EventArgs e)
        {
            Value = Calendar.SelectedDate;
            OnDropDownButtonClick();
        }

        protected override bool ProcessKeyChar(char keyChar)
        {
            switch (_processor.ProcessChar(GetSetup(), keyChar))
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
        }

        protected override bool ProcessKey(Key key)
        {
            switch (key)
            {
                case Key.Space:
                    System.Media.SystemSounds.Exclamation.Play();
                    return true;
                case Key.Back:
                    if (_processor.OnBackspaceKeyDown(GetSetup()) == ProcessCharResults.ValidationFailed)
                        System.Media.SystemSounds.Exclamation.Play();
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

            _textSettingValue = false;
        }
    }
}
