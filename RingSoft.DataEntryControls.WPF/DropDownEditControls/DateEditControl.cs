using System;
using System.Windows;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;

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
    public class DateEditControl : DropDownEditControl
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

        private IDropDownCalendar _calendar;

        public IDropDownCalendar Calendar
        {
            get => _calendar;
            set
            {
                if (_calendar != null)
                    _calendar.ValueChanged -= _calendar_ValueChanged;

                _calendar = value;

                if (_calendar != null)
                    _calendar.ValueChanged += _calendar_ValueChanged;
            }
        }

        private DateTime? _pendingNewValue;
        private bool _textSettingValue;
        private bool _settingText;

        static DateEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateEditControl), new FrameworkPropertyMetadata(typeof(DateEditControl)));
        }

        public override void OnApplyTemplate()
        {
            Calendar = GetTemplateChild(nameof(Calendar)) as IDropDownCalendar;
            base.OnApplyTemplate();

            if (_pendingNewValue != null)
                SetValue();

            _pendingNewValue = null;
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

            _settingText = true;

            //var setup = GetSetup();
            if (newValue == null)
                TextBox.Text = string.Empty;
            else
            {
                var value = (DateTime)newValue;
                var newText = value.ToString(); // value.ToString(setup.GetNumberFormatString(), Culture.NumberFormat);
                if (TextBox.IsFocused)
                    OnFocusedSetText(newText);
                else
                    TextBox.Text = newText;
            }

            _settingText = false;
        }

        protected override void OnTextBoxGotFocus()
        {
            OnFocusedSetText(TextBox.Text);
            base.OnTextBoxGotFocus();
        }

        private void OnFocusedSetText(string newText)
        {
            if (TextBox != null)
            {
                _settingText = true;
                TextBox.Text = newText; //_numericProcessor.FormatTextForEntry(setup, newText);
                _settingText = false;
            }
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
                Calendar.Control.Focus();
            }
        }

        private void _calendar_ValueChanged(object sender, EventArgs e)
        {
            Value = Calendar.Value;
        }

        public override void OnValueChanged(string newValue)
        {
            _textSettingValue = true;

            DateTime.TryParse(newValue, out var dateValue);

            Value = dateValue;

            _textSettingValue = false;

            base.OnValueChanged(newValue);
        }

    }
}
