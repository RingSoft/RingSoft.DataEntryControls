﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     <MyNamespace:DropDownEditControl/>
    ///
    /// </summary>

    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "DropDownButton", Type = typeof(Button))]
    [TemplatePart(Name = "Popup", Type = typeof(Popup))]
    public abstract class DropDownEditControl : Control
    {
        private TextBox _textBox;
        public TextBox TextBox
        {
            get => _textBox;
            set
            {
                if (_textBox != null)
                {
                    _textBox.PreviewTextInput -= _textBox_PreviewTextInput;
                    _textBox.PreviewKeyDown -= _textBox_PreviewKeyDown;
                    _textBox.GotFocus -= _textBox_GotFocus;
                }

                _textBox = value;

                if (_textBox != null)
                {
                    _textBox.PreviewTextInput += _textBox_PreviewTextInput;
                    _textBox.PreviewKeyDown += _textBox_PreviewKeyDown;
                    _textBox.GotFocus += _textBox_GotFocus;
                }
            }
        }

        private Button _dropDownButton;

        public Button DropDownButton
        {
            get => _dropDownButton;
            set
            {
                if (_dropDownButton != null)
                    _dropDownButton.Click -= _dropDownButton_Click;

                _dropDownButton = value;

                if (_dropDownButton != null)
                {
                    _dropDownButton.IsTabStop = false;
                    _dropDownButton.Click += _dropDownButton_Click;
                }
            }
        }

        public Popup Popup { get; set; }

        static DropDownEditControl()
        {
            FocusableProperty.OverrideMetadata(typeof(DropDownEditControl), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DropDownEditControl), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
        }

        public DropDownEditControl()
        {
            LostFocus += (sender, args) =>
            {
                if (Popup != null && !IsKeyboardFocusWithin)
                    Popup.IsOpen = false;
            };
        }

        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild("TextBox") as TextBox;
            DropDownButton = GetTemplateChild("DropDownButton") as Button;
            Popup = GetTemplateChild("Popup") as Popup;

            base.OnApplyTemplate();
        }

        private void _dropDownButton_Click(object sender, RoutedEventArgs e)
        {
            OnDropDownButtonClick();
        }

        private void _textBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (ProcessKey(e.Key))
                e.Handled = true;
        }

        private void _textBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (ProcessKeyChar(e.Text[0]))
                e.Handled = true;
        }

        private void _textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _textBox.SelectionStart = 0;
            _textBox.SelectionLength = _textBox.Text.Length;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Popup != null && Popup.IsOpen && e.Key == Key.Escape)
            {
                OnDropDownButtonClick();
                e.Handled = true;
            }

            base.OnPreviewKeyDown(e);
        }

        protected virtual void OnDropDownButtonClick()
        {
            if (Popup != null)
            {
                Popup.IsOpen = !Popup.IsOpen;
                if (!Popup.IsOpen)
                    TextBox.Focus();
            }
        }

        protected virtual bool ProcessKeyChar(char keyChar)
        {
            return false;
        }

        protected virtual bool ProcessKey(System.Windows.Input.Key key)
        {
            return false;
        }
    }
}
