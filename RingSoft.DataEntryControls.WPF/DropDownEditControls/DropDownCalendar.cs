using System;
using System.Windows;
using System.Windows.Controls;
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
    ///     <MyNamespace:DropDownCalendar/>
    ///
    /// </summary>
    [TemplatePart(Name = "Calendar", Type = typeof(Calendar))]
    [TemplatePart(Name = "TodayButton", Type = typeof(Button))]
    public class DropDownCalendar : Control, IDropDownCalendar
    {
        private Calendar _calendar;

        public Calendar Calendar
        {
            get => _calendar;
            set
            {
                if (_calendar != null)
                    _calendar.SelectedDatesChanged -= _calendar_SelectedDatesChanged;

                _calendar = value;

                if (_calendar != null)
                    _calendar.SelectedDatesChanged += _calendar_SelectedDatesChanged;
            }
        }

        private Button _todayButton;

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

        public Control Control => this;

        public DateTime? Value
        {
            get => Calendar?.SelectedDate;
            set
            {
                _settingValue = true;

                if (Calendar != null)
                    Calendar.SelectedDate = value;

                _settingValue = false;
            }
        }

        public event EventHandler ValueChanged;

        private bool _settingValue;

        static DropDownCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownCalendar), new FrameworkPropertyMetadata(typeof(DropDownCalendar)));
        }

        public override void OnApplyTemplate()
        {
            Calendar = GetTemplateChild(nameof(Calendar)) as Calendar;
            TodayButton = GetTemplateChild(nameof(TodayButton)) as Button;

            base.OnApplyTemplate();

            if (TodayButton != null)
                TodayButton.IsTabStop = false;

            Calendar?.Focus();
        }

        private void _calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_settingValue)
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void _todayButton_Click(object sender, RoutedEventArgs e)
        {
            if (Calendar != null)
            {
                Calendar.SelectedDate = DateTime.Today;
                Calendar.Focus();
            }
        }
    }
}
