﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;

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
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF;assembly=RingSoft.DataEntryControls.WPF"
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
    ///     <MyNamespace:NewDataEntryMemoEditor/>
    ///
    /// </summary>
    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "DateStampButton", Type = typeof(Button))]
    public class DataEntryMemoEditor : Control
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(TextChangedCallback));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void TextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var memoEditor = (DataEntryMemoEditor)obj;
            if (memoEditor._controlLoaded)
                memoEditor.SetText();
        }

        public static readonly DependencyProperty SelectAllOnGotFocusProperty =
            DependencyProperty.Register(nameof(SelectAllOnGotFocus), typeof(bool), typeof(DataEntryMemoEditor));

        public bool SelectAllOnGotFocus
        {
            get { return (bool)GetValue(SelectAllOnGotFocusProperty); }
            set { SetValue(SelectAllOnGotFocusProperty, value); }
        }

        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register(nameof(DateFormat), typeof(string), typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata("G", DateFormatChangedCallback));

        public string DateFormat
        {
            get { return (string)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        private static void DateFormatChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryMemoEditor = (DataEntryMemoEditor)obj;
            DateEditControlSetup.ValidateDateFormat(dataEntryMemoEditor.DateFormat);
        }

        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.Name, CultureIdChangedCallback));

        public string CultureId
        {
            get { return (string)GetValue(CultureIdProperty); }
            set { SetValue(CultureIdProperty, value); }
        }

        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var newDataEntryMemoEditor = (DataEntryMemoEditor)obj;
            var culture = new CultureInfo(newDataEntryMemoEditor.CultureId);
            newDataEntryMemoEditor.Culture = culture;
        }

        public CultureInfo Culture { get; protected internal set; }


        private TextBox _textBox;

        public TextBox TextBox
        {
            get => _textBox;
            set
            {
                if (TextBox != null)
                {
                    TextBox.TextChanged -= TextBox_TextChanged;
                    TextBox.GotFocus -= TextBox_GotFocus;
                }
                _textBox = value;

                if (TextBox != null)
                {
                    TextBox.TextChanged += TextBox_TextChanged;
                    TextBox.GotFocus += TextBox_GotFocus;
                }
            }
        }

        private Button _dateStampButton;

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

        public DataEntryMemoTabItem Notifier { get; set; }

        public event EventHandler TextChanged;

        private bool _controlLoaded;
        private bool _settingText;

        static DataEntryMemoEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(typeof(DataEntryMemoEditor)));

            FocusableProperty.OverrideMetadata(typeof(DataEntryMemoEditor), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DataEntryMemoEditor),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
        }

        public DataEntryMemoEditor()
        {
            Loaded += (sender, args) => OnLoad();
        }

        private void OnLoad()
        {
            if (!_controlLoaded)
            {
                SetText();

                TextBox?.SelectAll();

                Notifier = this.GetLogicalParent<DataEntryMemoTabItem>();

                _controlLoaded = true;
            }
        }

        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild(nameof(TextBox)) as TextBox;
            DateStampButton = GetTemplateChild(nameof(DateStampButton)) as Button;

            SetText();
            base.OnApplyTemplate();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = TextBox.Text;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SelectAllOnGotFocus)
                TextBox.SelectAll();
        }

        private void DateStampButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox != null)
            {
                OnDateStamp();
            }
        }

        protected virtual void OnDateStamp()
        {
            var stamp = $"{DateTime.Now.ToString(DateFormat, Culture)} - ";
            TextBox.Text = $"{stamp}\r\n{TextBox.Text}";
            TextBox.SelectionStart = TextBox.Text.IndexOf(stamp, StringComparison.Ordinal) + stamp.Length;
            TextBox.SelectionLength = 0;
        }

        private void SetText()
        {
            if (_settingText)
                return;

            _settingText = true;
            if (TextBox != null)
                TextBox.Text = Text;
            _settingText = false;

            if (Notifier != null)
                Notifier.MemoHasText = !Text.IsNullOrEmpty();

            TextChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
