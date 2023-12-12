// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DateReadOnlyBox.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using Calendar = System.Windows.Controls.Calendar;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class DateReadOnlyBox.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.ReadOnlyBox" />
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.ReadOnlyBox" />
    /// <seealso cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    /// <font color="red">Badly formed XML comment.</font>
    public class DateReadOnlyBox : ReadOnlyBox, IReadOnlyControl
    {
        /// <summary>
        /// Gets or sets the drop down button.
        /// </summary>
        /// <value>The drop down button.</value>
        public Button DropDownButton { get; set; }
        /// <summary>
        /// Gets or sets the popup.
        /// </summary>
        /// <value>The popup.</value>
        public Popup Popup { get; set; }
        /// <summary>
        /// Gets or sets the calendar.
        /// </summary>
        /// <value>The calendar.</value>
        public DropDownCalendar Calendar { get; set; }

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(DateTime?), typeof(DateReadOnlyBox),
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
            var dateReadOnlyBox = (DateReadOnlyBox)obj;
            dateReadOnlyBox.SetValue();
        }

        /// <summary>
        /// The date format property
        /// </summary>
        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register(nameof(DateFormat), typeof(string), typeof(DateReadOnlyBox),
                new FrameworkPropertyMetadata(DateFormatChangedCallback));

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>The date format.</value>
        public string DateFormat
        {
            get { return (string)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        /// <summary>
        /// Dates the format changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DateFormatChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateReadOnlyBox = (DateReadOnlyBox)obj;
            DateEditControlSetup.ValidateDateFormat(dateReadOnlyBox.DateFormat);
            dateReadOnlyBox.SetValue();
        }

        /// <summary>
        /// The date format type property
        /// </summary>
        public static readonly DependencyProperty DateFormatTypeProperty =
            DependencyProperty.Register(nameof(DateFormatType), typeof(DateFormatTypes), typeof(DateReadOnlyBox),
                new FrameworkPropertyMetadata(DateFormatTypeChangedCallback));

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
        /// Dates the format type changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DateFormatTypeChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateReadOnlyBox = (DateReadOnlyBox)obj;
            dateReadOnlyBox.SetValue();
        }

        /// <summary>
        /// The culture identifier property
        /// </summary>
        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(DateReadOnlyBox),
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
            var dateEditBox = (DateReadOnlyBox)obj;
            var culture = new CultureInfo(dateEditBox.CultureId);
            dateEditBox.Culture = culture; 
            dateEditBox.SetValue();
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; protected internal set; }

        /// <summary>
        /// Initializes static members of the <see cref="DateReadOnlyBox"/> class.
        /// </summary>
        static DateReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateReadOnlyBox), new FrameworkPropertyMetadata(typeof(DateReadOnlyBox)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateReadOnlyBox"/> class.
        /// </summary>
        public DateReadOnlyBox()
        {
            if (Culture == null)
                Culture = CultureInfo.CurrentCulture;

            PreviewKeyDown += (sender, args) =>
            {
                if (args.Key == Key.Escape && Popup.IsOpen)
                {
                    Popup.IsOpen = false;
                    args.Handled = true;
                }
            };
        }

        /// <summary>
        /// The is popup opened
        /// </summary>
        private bool _isPopupOpened;

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            DropDownButton = GetTemplateChild(nameof(DropDownButton)) as Button;
            Popup = GetTemplateChild(nameof(Popup)) as Popup;
            Calendar = GetTemplateChild(nameof(Calendar)) as DropDownCalendar;

            DropDownButton.Click += (sender, args) => OnDropDownButtonClick();
            base.OnApplyTemplate();
            SetValue();
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        private void SetValue()
        {
            var text = string.Empty;

            if (Value != null)
            {
                var formatString = DateFormat;
                if (formatString.IsNullOrEmpty())
                    formatString = DateEditControlSetup.GetDefaultFormatForType(DateFormatType);

                var displayValue = (DateTime)Value;
                text = displayValue.ToString(formatString, Culture.DateTimeFormat);
            }

            Text = text;
        }

        /// <summary>
        /// Called when [drop down button click].
        /// </summary>
        public void OnDropDownButtonClick()
        {
            if (Calendar != null && Popup != null)
            {
                _isPopupOpened = !_isPopupOpened;
                Popup.IsOpen = _isPopupOpened;

                if (_isPopupOpened)
                {
                    Calendar.SelectedDate = Value ?? DateTime.Today;
                    Calendar.Focus();
                }
            }
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            DropDownButton.IsEnabled = true;
        }
    }
}
