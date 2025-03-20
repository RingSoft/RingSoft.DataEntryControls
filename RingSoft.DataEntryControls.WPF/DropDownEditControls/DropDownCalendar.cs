// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DropDownCalendar.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// The calendar popup control that appears when the user clicks on the date edit control's calendar button.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="IDropDownCalendar" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="IDropDownCalendar" />
    [TemplatePart(Name = "Calendar", Type = typeof(Calendar))]
    [TemplatePart(Name = "TodayButton", Type = typeof(Button))]
    public class DropDownCalendar : Control, IDropDownCalendar
    {
        /// <summary>
        /// Gets or sets the minimum date.
        /// </summary>
        /// <value>The minimum date.</value>
        public DateTime? MinimumDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum date.
        /// </summary>
        /// <value>The maximum date.</value>
        public DateTime? MaximumDate { get; set; }

        /// <summary>
        /// The calendar
        /// </summary>
        private Calendar _calendar;

        /// <summary>
        /// Gets or sets the calendar.
        /// </summary>
        /// <value>The calendar.</value>
        public Calendar Calendar
        {
            get => _calendar;
            set
            {
                if (_calendar != null)
                {
                    _calendar.SelectedDatesChanged -= _calendar_SelectedDatesChanged;
                    _calendar.PreviewMouseUp -= _calendar_PreviewMouseUp;
                    _calendar.PreviewKeyDown -= _calendar_PreviewKeyDown;
                }

                _calendar = value;

                if (_calendar != null)
                {
                    _calendar.SelectedDatesChanged += _calendar_SelectedDatesChanged;
                    _calendar.PreviewMouseUp += _calendar_PreviewMouseUp;
                    _calendar.PreviewKeyDown += _calendar_PreviewKeyDown;
                }
            }
        }

        /// <summary>
        /// The today button
        /// </summary>
        private Button _todayButton;

        /// <summary>
        /// Gets or sets the today button.
        /// </summary>
        /// <value>The today button.</value>
        public Button TodayButton
        {
            get => _todayButton;
            set
            {
                if (_todayButton != null)
                    _todayButton.Click -= _todayButton_Click;

                _todayButton = value;

                if (_todayButton != null)
                    _todayButton.Click += _todayButton_Click;
            }
        }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public Control Control => this;

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        /// <value>The selected date.</value>
        public DateTime? SelectedDate
        {
            get => Calendar?.SelectedDate;
            set
            {
                _settingValue = true;

                if (Calendar != null)
                {
                    Calendar.SelectedDate = value;
                    if (value != null)
                        Calendar.DisplayDate = (DateTime) value;
                }

                _settingValue = false;
            }
        }

        /// <summary>
        /// Occurs when [selected date changed].
        /// </summary>
        public event EventHandler SelectedDateChanged;
        /// <summary>
        /// Occurs when [date picked].
        /// </summary>
        public event EventHandler DatePicked;

        /// <summary>
        /// The setting value
        /// </summary>
        private bool _settingValue;

        /// <summary>
        /// Initializes static members of the <see cref="DropDownCalendar" /> class.
        /// </summary>
        static DropDownCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownCalendar), new FrameworkPropertyMetadata(typeof(DropDownCalendar)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Calendar = GetTemplateChild(nameof(Calendar)) as Calendar;
            TodayButton = GetTemplateChild(nameof(TodayButton)) as Button;

            base.OnApplyTemplate();

            if (TodayButton != null)
                TodayButton.IsTabStop = false;

        }

        /// <summary>
        /// Invoked whenever an unhandled <see cref="E:System.Windows.UIElement.GotFocus" /> event reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (Calendar != null)
            {
                Calendar.DisplayDateStart = MinimumDate;
                Calendar.DisplayDateEnd = MaximumDate;
            }

            Calendar?.Focus();
            base.OnGotFocus(e);
        }

        /// <summary>
        /// Handles the SelectedDatesChanged event of the _calendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void _calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_settingValue)
            {
                SelectedDateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the Click event of the _todayButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void _todayButton_Click(object sender, RoutedEventArgs e)
        {
            if (Calendar != null)
            {
                Calendar.SelectedDate = Calendar.DisplayDate = DateTime.Now;
                Calendar.Focus();
            }
        }

        /// <summary>
        /// Handles the PreviewMouseUp event of the _calendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void _calendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject originalSource = e.OriginalSource as DependencyObject;
            CalendarDayButton day = originalSource.GetParentOfType<CalendarDayButton>();
            if (day != null)
            {
                DatePicked?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the _calendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        private void _calendar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DatePicked?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }
    }
}
