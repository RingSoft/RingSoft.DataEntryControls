// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-05-2023
// ***********************************************************************
// <copyright file="DateEditControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.Date;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class DateEditControl.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DropDownEditControl" />
    /// Implements the <see cref="IDateEditControl" />
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DropDownEditControl" />
    /// <seealso cref="IDateEditControl" />
    /// <seealso cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    /// <font color="red">Badly formed XML comment.</font>
    [TemplatePart(Name = "Calendar", Type = typeof(IDropDownCalendar))]
    public class DateEditControl : DropDownEditControl, IDateEditControl, IReadOnlyControl
    {
        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(DateTime?), typeof(DateEditControl),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Values the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditControl = (DateEditControl)obj;
            if (!dateEditControl._textSettingValue)
            {
                dateEditControl._innerValidateLostFocus = false;
                dateEditControl.SetValue();
                dateEditControl.OnValueChanged(dateEditControl.Text);
            }
        }

        /// <summary>
        /// The entry format property
        /// </summary>
        public static readonly DependencyProperty EntryFormatProperty =
            DependencyProperty.Register(nameof(EntryFormat), typeof(string), typeof(DateEditControl),
                new FrameworkPropertyMetadata(EntryFormatChangedCallback));

        /// <summary>
        /// Gets or sets the entry format.
        /// </summary>
        /// <value>The entry format.</value>
        public string EntryFormat
        {
            get { return (string)GetValue(EntryFormatProperty); }
            set { SetValue(EntryFormatProperty, value); }
        }

        /// <summary>
        /// Entries the format changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// The display format property
        /// </summary>
        public static readonly DependencyProperty DisplayFormatProperty =
            DependencyProperty.Register(nameof(DisplayFormat), typeof(string), typeof(DateEditControl),
                new FrameworkPropertyMetadata(DisplayFormatChangedCallback));

        /// <summary>
        /// Gets or sets the display format.
        /// </summary>
        /// <value>The display format.</value>
        public string DisplayFormat
        {
            get { return (string)GetValue(DisplayFormatProperty); }
            set { SetValue(DisplayFormatProperty, value); }
        }

        /// <summary>
        /// Displays the format changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DisplayFormatChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditControl = (DateEditControl)obj;
            DateEditControlSetup.ValidateDateFormat(dateEditControl.DisplayFormat);
            if (!dateEditControl._textSettingValue)
                dateEditControl.SetValue();
        }

        /// <summary>
        /// The date format type property
        /// </summary>
        public static readonly DependencyProperty DateFormatTypeProperty =
            DependencyProperty.Register(nameof(DateFormatType), typeof(DateFormatTypes), typeof(DateEditControl));

        /// <summary>
        /// Gets or sets the type of the date format.
        /// </summary>
        /// <value>The type of the date format.</value>
        public DateFormatTypes DateFormatType
        {
            get { return (DateFormatTypes)GetValue(DateFormatTypeProperty); }
            set { SetValue(DateFormatTypeProperty, value); }
        }

        /// <summary>
        /// The minimum date property
        /// </summary>
        public static readonly DependencyProperty MinimumDateProperty =
            DependencyProperty.Register(nameof(MinimumDate), typeof(DateTime?), typeof(DateEditControl));

        /// <summary>
        /// Gets or sets the minimum date.
        /// </summary>
        /// <value>The minimum date.</value>
        public DateTime? MinimumDate
        {
            get { return (DateTime?)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        /// <summary>
        /// The maximum date property
        /// </summary>
        public static readonly DependencyProperty MaximumDateProperty =
            DependencyProperty.Register(nameof(MaximumDate), typeof(DateTime?), typeof(DateEditControl));

        /// <summary>
        /// Gets or sets the maximum date.
        /// </summary>
        /// <value>The maximum date.</value>
        public DateTime? MaximumDate
        {
            get { return (DateTime?)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        /// <summary>
        /// The culture identifier property
        /// </summary>
        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(DateEditControl),
                new FrameworkPropertyMetadata(CultureIdChangedCallback));

        /// <summary>
        /// Gets or sets the culture identifier.
        /// </summary>
        /// <value>The culture identifier.</value>
        public string CultureId
        {
            get { return (string)GetValue(CultureIdProperty); }
            set { SetValue(CultureIdProperty, value); }
        }

        /// <summary>
        /// Cultures the identifier changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditControl = (DateEditControl)obj;
            var culture = new CultureInfo(dateEditControl.CultureId);
            dateEditControl.Culture = culture;
            if (!dateEditControl._textSettingValue)
                dateEditControl.SetValue();
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; protected internal set; }


        /// <summary>
        /// The setup property
        /// </summary>
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(DateEditControlSetup), typeof(DateEditControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        /// <summary>
        /// Sets the setup.
        /// </summary>
        /// <value>The setup.</value>
        public DateEditControlSetup Setup
        {
            private get { return (DateEditControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        /// <summary>
        /// Setups the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// The allow null value property
        /// </summary>
        public static readonly DependencyProperty AllowNullValueProperty =
            DependencyProperty.Register(nameof(AllowNullValue), typeof(bool), typeof(DateEditControl));

        /// <summary>
        /// Gets or sets a value indicating whether [allow null value].
        /// </summary>
        /// <value><c>true</c> if [allow null value]; otherwise, <c>false</c>.</value>
        public bool AllowNullValue
        {
            get { return (bool)GetValue(AllowNullValueProperty); }
            set { SetValue(AllowNullValueProperty, value); }
        }

        /// <summary>
        /// The play validation sound on lost focus property
        /// </summary>
        public static readonly DependencyProperty PlayValidationSoundOnLostFocusProperty =
            DependencyProperty.Register(nameof(PlayValidationSoundOnLostFocus), typeof(bool), typeof(DateEditControl));

        /// <summary>
        /// Gets or sets a value indicating whether [play validation sound on lost focus].
        /// </summary>
        /// <value><c>true</c> if [play validation sound on lost focus]; otherwise, <c>false</c>.</value>
        public bool PlayValidationSoundOnLostFocus
        {
            get => (bool) GetValue(PlayValidationSoundOnLostFocusProperty);
            set => SetValue(PlayValidationSoundOnLostFocusProperty, value);
        }

        /// <summary>
        /// The calendar
        /// </summary>
        private IDropDownCalendar _calendar;

        /// <summary>
        /// Gets or sets the calendar.
        /// </summary>
        /// <value>The calendar.</value>
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

        /// <summary>
        /// The inner validate lost focus
        /// </summary>
        private bool _innerValidateLostFocus;
        /// <summary>
        /// The pending new value
        /// </summary>
        private DateTime? _pendingNewValue;
        /// <summary>
        /// The text setting value
        /// </summary>
        private bool _textSettingValue;
        /// <summary>
        /// The validating entry format
        /// </summary>
        private bool _validatingEntryFormat;
        /// <summary>
        /// The processor
        /// </summary>
        private DateEditProcessor _processor;
        /// <summary>
        /// The override sel changed
        /// </summary>
        private bool _overrideSelChanged;

        /// <summary>
        /// Initializes static members of the <see cref="DateEditControl"/> class.
        /// </summary>
        static DateEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateEditControl),
                new FrameworkPropertyMetadata(typeof(DateEditControl)));

            PlayValidationSoundOnLostFocusProperty.OverrideMetadata(typeof(DateEditControl),
                new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateEditControl"/> class.
        /// </summary>
        public DateEditControl()
        {
            _processor = new DateEditProcessor(this);
            _processor.ValueChanged += _processor_ValueChanged;

            LostFocus += (sender, args) => OnLostFocusSetText(GetSetup(), Value);
            GotFocus += (sender, args) => _innerValidateLostFocus = false;
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            Calendar = GetTemplateChild(nameof(Calendar)) as IDropDownCalendar;
            base.OnApplyTemplate();

            if (_pendingNewValue != null)
                SetValue();

            SetReadOnlyMode(ReadOnlyMode);
            _pendingNewValue = null;
        }

        /// <summary>
        /// Gets the setup.
        /// </summary>
        /// <returns>DateEditControlSetup.</returns>
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

        /// <summary>
        /// Sets the value.
        /// </summary>
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

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="newValue">The new value.</param>
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

        /// <summary>
        /// Called when [text box got focus].
        /// </summary>
        protected override void OnTextBoxGotFocus()
        {
            OnFocusedSetText(GetSetup(), Value);
            base.OnTextBoxGotFocus();
        }

        /// <summary>
        /// Called when [focused set text].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="value">The value.</param>
        private void OnFocusedSetText(DateEditControlSetup setup, DateTime? value)
        {
            _textSettingValue = true;

            _processor.OnSetFocus(setup, value);

            _textSettingValue = false;
        }

        /// <summary>
        /// Called when [lost focus set text].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="value">The value.</param>
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

        /// <summary>
        /// Handles the <see cref="E:PreviewKeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Called when [drop down button click].
        /// </summary>
        public override void OnDropDownButtonClick()
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

        /// <summary>
        /// Handles the SelectedDateChanged event of the _calendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _calendar_SelectedDateChanged(object sender, EventArgs e)
        {
            if (ReadOnlyMode)
            {
                return;
            }
            SetValueChanged();
            _innerValidateLostFocus = true;
        }

        /// <summary>
        /// Handles the DatePicked event of the _calendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _calendar_DatePicked(object sender, EventArgs e)
        {
            if (ReadOnlyMode)
            {
                return;
            }
            SetValueChanged();
            OnDropDownButtonClick();
        }

        /// <summary>
        /// Sets the value changed.
        /// </summary>
        private void SetValueChanged()
        {
            var changedValue = Value != Calendar.SelectedDate;
            Value = Calendar.SelectedDate;
            if (changedValue)
                OnValueChanged(Text);
        }

        /// <summary>
        /// Processes the key character.
        /// </summary>
        /// <param name="keyChar">The key character.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override bool ProcessKeyChar(char keyChar)
        {
            if (!ReadOnlyMode)
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
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Processes the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool ProcessKey(Key key)
        {
            if (!ReadOnlyMode)
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
            }

            return base.ProcessKey(key);
        }

        /// <summary>
        /// Called when [text changed].
        /// </summary>
        /// <param name="newText">The new text.</param>
        protected override void OnTextChanged(string newText)
        {
            if (_textSettingValue)
            {
                base.OnTextChanged(newText);
                return;
            }

            _textSettingValue = true;

            if (ReadOnlyMode)
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
            else
            {
                if (!_processor.PasteText(GetSetup(), newText))
                    System.Media.SystemSounds.Exclamation.Play();
            }


            _textSettingValue = false;

            base.OnTextChanged(newText);
        }


        /// <summary>
        /// Handles the ValueChanged event of the _processor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _processor_ValueChanged(object sender, EventArgs e)
        {
            _textSettingValue = true;

            Value = _processor.Value;
            OnValueChanged(Text);

            _textSettingValue = false;
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            ReadOnlyMode = readOnlyValue;
        }

        /// <summary>
        /// Sets the select all.
        /// </summary>
        public void SetSelectAll()
        {
            //TextBox.ScrollToTop();
        }
    }
}
