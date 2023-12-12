// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 06-28-2023
// ***********************************************************************
// <copyright file="DataEntryMemoEditor.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A memo editor control.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "DateStampButton", Type = typeof(Button))]
    public class DataEntryMemoEditor : Control, IReadOnlyControl
    {
        /// <summary>
        /// The text property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(TextChangedCallback));

        /// <summary>
        /// Gets or sets the text.  This is a bind-able property.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Texts the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void TextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var memoEditor = (DataEntryMemoEditor)obj;
            if (memoEditor._controlLoaded)
                memoEditor.SetText();
        }

        /// <summary>
        /// The select all on got focus property
        /// </summary>
        public static readonly DependencyProperty SelectAllOnGotFocusProperty =
            DependencyProperty.Register(nameof(SelectAllOnGotFocus), typeof(bool), typeof(DataEntryMemoEditor));

        /// <summary>
        /// Gets or sets a value indicating whether [select all on got focus].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [select all on got focus]; otherwise, <c>false</c>.</value>
        public bool SelectAllOnGotFocus
        {
            get { return (bool)GetValue(SelectAllOnGotFocusProperty); }
            set { SetValue(SelectAllOnGotFocusProperty, value); }
        }

        /// <summary>
        /// The date format property
        /// </summary>
        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register(nameof(DateFormat), typeof(string), typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata("G", DateFormatChangedCallback));

        /// <summary>
        /// Gets or sets the date format.  This is a bind-able property.
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
            var dataEntryMemoEditor = (DataEntryMemoEditor)obj;
            DateEditControlSetup.ValidateDateFormat(dataEntryMemoEditor.DateFormat);
        }

        /// <summary>
        /// The culture identifier property
        /// </summary>
        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.Name, CultureIdChangedCallback));

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
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var newDataEntryMemoEditor = (DataEntryMemoEditor)obj;
            var culture = new CultureInfo(newDataEntryMemoEditor.CultureId);
            newDataEntryMemoEditor.Culture = culture;
        }

        /// <summary>
        /// The read only mode property
        /// </summary>
        public static readonly DependencyProperty ReadOnlyModeProperty =
            DependencyProperty.Register(nameof(ReadOnlyMode), typeof(bool), typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(ReadOnlyModeChangedCallback));

        /// <summary>
        /// Gets or sets a value indicating whether [read only mode].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [read only mode]; otherwise, <c>false</c>.</value>
        public bool ReadOnlyMode
        {
            get { return (bool)GetValue(ReadOnlyModeProperty); }
            set { SetValue(ReadOnlyModeProperty, value); }
        }

        /// <summary>
        /// Reads the only mode changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ReadOnlyModeChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryGrid = (DataEntryGrid.DataEntryGrid)obj;
            dataEntryGrid.SetReadOnlyMode(dataEntryGrid.ReadOnlyMode);
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; protected internal set; }


        /// <summary>
        /// The text box
        /// </summary>
        private TextBox _textBox;

        /// <summary>
        /// Gets or sets the text box.
        /// </summary>
        /// <value>The text box.</value>
        public TextBox TextBox
        {
            get => _textBox;
            set
            {
                if (TextBox != null)
                {
                    TextBox.TextChanged -= TextBox_TextChanged;
                    TextBox.GotFocus -= TextBox_GotFocus;
                    TextBox.KeyDown -= TextBox_KeyDown;
                }
                _textBox = value;

                if (TextBox != null)
                {
                    TextBox.TextChanged += TextBox_TextChanged;
                    TextBox.GotFocus += TextBox_GotFocus;
                    TextBox.KeyDown += TextBox_KeyDown;
                }
            }
        }

        /// <summary>
        /// The date stamp button
        /// </summary>
        private Button _dateStampButton;

        /// <summary>
        /// Gets or sets the date stamp button.
        /// </summary>
        /// <value>The date stamp button.</value>
        public Button DateStampButton
        {
            get => _dateStampButton;
            set
            {
                if (DateStampButton != null)
                {
                    DateStampButton.Click -= DateStampButton_Click;
                }

                _dateStampButton = value;

                if (DateStampButton != null)
                {
                    DateStampButton.Click += DateStampButton_Click;
                }
            }
        }

        /// <summary>
        /// Gets or sets the notifier.
        /// </summary>
        /// <value>The notifier.</value>
        public DataEntryMemoTabItem Notifier { get; set; }

        /// <summary>
        /// Occurs when [text changed].
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// The control loaded
        /// </summary>
        private bool _controlLoaded;
        /// <summary>
        /// The setting text
        /// </summary>
        private bool _settingText;
        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;

        /// <summary>
        /// Initializes static members of the <see cref="DataEntryMemoEditor"/> class.
        /// </summary>
        static DataEntryMemoEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(typeof(DataEntryMemoEditor)));

            FocusableProperty.OverrideMetadata(typeof(DataEntryMemoEditor), new FrameworkPropertyMetadata(false));

            SelectAllOnGotFocusProperty.OverrideMetadata(typeof(DataEntryMemoEditor), new FrameworkPropertyMetadata(true));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
        }

        /// <summary>
        /// The collapse date button
        /// </summary>
        private bool _collapseDateButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryMemoEditor"/> class.
        /// </summary>
        public DataEntryMemoEditor()
        {
            Loaded += (sender, args) => OnLoad();
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        private void OnLoad()
        {
            if (!_controlLoaded)
            {
                SetText();

                TextBox?.SelectAll();

                Notifier = this.GetParentOfType<DataEntryMemoTabItem>();

                NotifyHasText();

                _controlLoaded = true;
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild(nameof(TextBox)) as TextBox;
            DateStampButton = GetTemplateChild(nameof(DateStampButton)) as Button;

            SetText();
            SetReadOnlyMode(_readOnlyMode);
            if (_collapseDateButton)
            {
                DateStampButton.Visibility = Visibility.Collapsed;
            }
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Handles the TextChanged event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = TextBox.Text;
        }

        /// <summary>
        /// Handles the GotFocus event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SelectAllOnGotFocus)
            {
                TextBox.ScrollToTop();
            }
        }

        /// <summary>
        /// Handles the Click event of the DateStampButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DateStampButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox != null)
            {
                OnDateStamp();
            }
        }

        /// <summary>
        /// Called when user clicks the Date/Time Stamp button.
        /// </summary>
        protected virtual void OnDateStamp()
        {
            var stamp = $"{DateTime.Now.ToString(DateFormat, Culture)} - ";
            TextBox.Text = $"{stamp}\r\n{TextBox.Text}";
            TextBox.SelectionStart = TextBox.Text.IndexOf(stamp, StringComparison.Ordinal) + stamp.Length;
            TextBox.SelectionLength = 0;
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        private void SetText()
        {
            if (_settingText)
                return;

            _settingText = true;
            if (TextBox != null)
                TextBox.Text = Text;
            _settingText = false;

            NotifyHasText();

            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Notifies the has text.
        /// </summary>
        private void NotifyHasText()
        {
            if (Notifier != null)
                Notifier.MemoHasText = !Text.IsNullOrEmpty();
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            if (!readOnlyValue && ReadOnlyMode)
                readOnlyValue = ReadOnlyMode;

            _readOnlyMode = readOnlyValue;

            if (TextBox != null)
            {
                TextBox.IsReadOnly = _readOnlyMode;
                TextBox.IsReadOnlyCaretVisible = true;
            }

            if (DateStampButton != null)
            {
                DateStampButton.IsEnabled = !_readOnlyMode;
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (_readOnlyMode)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                    case Key.Tab:
                        break;
                    case Key.Back:
                        SystemSounds.Exclamation.Play();
                        break;
                    default:
                        var keyChar = e.Key.GetCharFromKey();
                        if (keyChar != ' ')
                            SystemSounds.Exclamation.Play();

                        break;
                }
            }
        }

        /// <summary>
        /// Collapses the date button.
        /// </summary>
        public void CollapseDateButton()
        {
            if (DateStampButton != null)
            {
                DateStampButton.Visibility = Visibility.Collapsed;
            }
            else
                _collapseDateButton = true;
        }
    }
}
