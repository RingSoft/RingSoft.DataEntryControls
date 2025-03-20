// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="NumericEditControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// The decimal and integer control base class.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DropDownEditControl" />
    /// Implements the <see cref="INumericControl" />
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DropDownEditControl" />
    /// <seealso cref="INumericControl" />
    /// <seealso cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    public abstract class NumericEditControl<T> : DropDownEditControl, INumericControl, IReadOnlyControl
    {
        /// <summary>
        /// The data entry mode property
        /// </summary>
        public static readonly DependencyProperty DataEntryModeProperty =
            DependencyProperty.Register(nameof(DataEntryMode), typeof(DataEntryModes), typeof(NumericEditControl<T>));

        /// <summary>
        /// Gets or sets the data entry mode.  This is a bind-able property.
        /// </summary>
        /// <value>The data entry mode.</value>
        public DataEntryModes DataEntryMode
        {
            get { return (DataEntryModes)GetValue(DataEntryModeProperty); }
            set { SetValue(DataEntryModeProperty, value); }
        }

        /// <summary>
        /// The number format string property
        /// </summary>
        public static readonly DependencyProperty NumberFormatStringProperty =
            DependencyProperty.Register(nameof(NumberFormatString), typeof(string), typeof(NumericEditControl<T>));

        /// <summary>
        /// Gets or sets the number format string.  This is a bind-able property.
        /// </summary>
        /// <value>The number format string.</value>
        public string NumberFormatString
        {
            get { return (string)GetValue(NumberFormatStringProperty); }
            set { SetValue(NumberFormatStringProperty, value); }
        }

        /// <summary>
        /// The maximum value property
        /// </summary>
        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(nameof(MaximumValue), typeof(T), typeof(NumericEditControl<T>));

        /// <summary>
        /// Gets or sets the maximum value.  This is a bind-able property.
        /// </summary>
        /// <value>The maximum value.</value>
        public T MaximumValue
        {
            get { return (T)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        /// <summary>
        /// The minimum value property
        /// </summary>
        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(nameof(MinimumValue), typeof(T), typeof(NumericEditControl<T>));

        /// <summary>
        /// Gets or sets the minimum value.  This is a bind-able property.
        /// </summary>
        /// <value>The minimum value.</value>
        public T MinimumValue
        {
            get { return (T)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(T), typeof(NumericEditControl<T>),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        /// <summary>
        /// Gets or sets the value.  This is a bind-able property.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Values the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericEditControl = (NumericEditControl<T>)obj;

            if (!numericEditControl._settingText)
            {
                numericEditControl.SetValue();
                numericEditControl._settingText = true;
                numericEditControl.OnValueChanged(numericEditControl.ConvertValueToString());
                numericEditControl._settingText = false;
            }
        }

        /// <summary>
        /// The culture identifier property
        /// </summary>
        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(NumericEditControl<T>),
                new FrameworkPropertyMetadata(CultureIdChangedCallback));

        /// <summary>
        /// Gets or sets the culture identifier.  This is a bind-able property.
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
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericEditControl = (NumericEditControl<T>) obj;
            var culture = new CultureInfo(numericEditControl.CultureId);
            numericEditControl.Culture = culture;
            
            DecimalEditControlSetup.FormatCulture(numericEditControl.Culture);
            if (!numericEditControl._settingText)
                numericEditControl.SetValue();
        }

        /// <summary>
        /// The allow null value property
        /// </summary>
        public static readonly DependencyProperty AllowNullValueProperty =
            DependencyProperty.Register(nameof(AllowNullValue), typeof(bool), typeof(NumericEditControl<T>));

        /// <summary>
        /// Gets or sets a value indicating whether [allow null value].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [allow null value]; otherwise, <c>false</c>.</value>
        public bool AllowNullValue
        {
            get { return (bool)GetValue(AllowNullValueProperty); }
            set { SetValue(AllowNullValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; protected internal set; }

        /// <summary>
        /// The numeric processor
        /// </summary>
        private DataEntryNumericControlProcessor _numericProcessor;
        /// <summary>
        /// The setting text
        /// </summary>
        private bool _settingText;
        /// <summary>
        /// The override sel changed
        /// </summary>
        private bool _overrideSelChanged;

        /// <summary>
        /// Initializes static members of the <see cref="NumericEditControl{T}" /> class.
        /// </summary>
        static NumericEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericEditControl<T>), new FrameworkPropertyMetadata(typeof(NumericEditControl<T>)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericEditControl{T}" /> class.
        /// </summary>
        public NumericEditControl()
        {
            if (Culture == null)
                Culture = new CultureInfo(CultureInfo.CurrentCulture.Name);

            DecimalEditControlSetup.FormatCulture(Culture);

            _numericProcessor = new DataEntryNumericControlProcessor(this);
            _numericProcessor.ValueChanged += (sender, args) =>
            {
                _settingText = true;
                OnValueChanged(args.NewValue);
                _settingText = false;
            };

            LostFocus += NumericEditControl_LostFocus;
        }

        /// <summary>
        /// Loads from setup.
        /// </summary>
        /// <param name="setup">The setup.</param>
        protected virtual void LoadFromSetup(NumericEditControlSetup<T> setup)
        {
            DataEntryMode = setup.DataEntryMode;
            MaximumValue = setup.MaximumValue;
            MinimumValue = setup.MinimumValue;
            NumberFormatString = setup.NumberFormatString;
            Culture = setup.Culture;
            AllowNullValue = setup.AllowNullValue;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        protected abstract void SetValue();

        /// <summary>
        /// Converts the value to string.
        /// </summary>
        /// <returns>System.String.</returns>
        protected abstract string ConvertValueToString();

        /// <summary>
        /// Gets the minimum value properties.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimumValue">The minimum value.</param>
        protected abstract void GetMinimumValueProperties(out double? value, out double? minimumValue);

        /// <summary>
        /// Handles the LostFocus event of the NumericEditControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void NumericEditControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin && TextBox != null)
            {
                if (ValidateMinimumValue())
                {
                    if (Value == null)
                        SetText((double?) null);
                    else
                        SetText(TextBox.Text);
                }
            }
        }

        /// <summary>
        /// Validates the minimum value.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateMinimumValue()
        {
            GetMinimumValueProperties(out var value, out var minimumValue);
            if (value == null && minimumValue != null)
            {
                Value = MinimumValue;
                return false;
            }

            if (minimumValue != null && value < minimumValue)
            {
                Value = MinimumValue;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="text">The text.</param>
        private void SetText(string text)
        {
            SetText(text.ToDecimal(Culture));
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected void SetText(double? newValue)
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
                var value = (double) newValue;
                var newText = value.ToString(setup.GetNumberFormatString(), Culture.NumberFormat);
                if (TextBox.IsFocused)
                    OnFocusedSetText(newText, setup);
                else 
                    TextBox.Text = newText;
            }

            _settingText = false;
        }

        /// <summary>
        /// Called when [text box got focus].
        /// </summary>
        protected override void OnTextBoxGotFocus()
        {
            OnFocusedSetText(TextBox.Text, GetSetup());
            base.OnTextBoxGotFocus();
        }

        /// <summary>
        /// Called when [focused set text].
        /// </summary>
        /// <param name="newText">The new text.</param>
        /// <param name="setup">The setup.</param>
        private void OnFocusedSetText(string newText, DecimalEditControlSetup setup)
        {
            if (TextBox != null)
            {
                _settingText = true;
                TextBox.Text = _numericProcessor.FormatTextForEntry(setup, newText);
                _settingText = false;
                _overrideSelChanged = true;

            }
        }

        /// <summary>
        /// Gets the setup.
        /// </summary>
        /// <returns>DecimalEditControlSetup.</returns>
        private DecimalEditControlSetup GetSetup()
        {
            var result = new DecimalEditControlSetup();
            PopulateSetup(result);
            return result;
        }

        /// <summary>
        /// Populates the setup.
        /// </summary>
        /// <param name="setup">The setup.</param>
        protected virtual void PopulateSetup(DecimalEditControlSetup setup)
        {
            setup.DataEntryMode = DataEntryMode;
            setup.NumberFormatString = NumberFormatString;
            setup.CultureId = Culture.Name;
            setup.AllowNullValue = AllowNullValue;
        }

        /// <summary>
        /// Processes the key character.
        /// </summary>
        /// <param name="keyChar">The key character.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Processes the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Called when [text changed].
        /// </summary>
        /// <param name="newText">The new text.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            if (UiLabel != null)
            {
                UiLabel.IsEnabled = !readOnlyValue;
            }

            IsEnabled = !readOnlyValue;
        }
    }
}
