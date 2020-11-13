using System;
using System.Globalization;
using System.Windows;
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
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.ReadOnlyControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.ReadOnlyControls;assembly=RingSoft.DataEntryControls.WPF.ReadOnlyControls"
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
    ///     <MyNamespace:DateReadOnlyBox/>
    ///
    /// </summary>
    public class DateReadOnlyBox : ReadOnlyBox
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(DateTime?), typeof(DateReadOnlyBox),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateReadOnlyBox = (DateReadOnlyBox)obj;
            dateReadOnlyBox.SetValue();
        }

        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register(nameof(DateFormat), typeof(string), typeof(DateReadOnlyBox),
                new FrameworkPropertyMetadata(DateFormatChangedCallback));

        public string DateFormat
        {
            get { return (string)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        private static void DateFormatChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateReadOnlyBox = (DateReadOnlyBox)obj;
            DateEditControlSetup.ValidateDateFormat(dateReadOnlyBox.DateFormat);
            dateReadOnlyBox.SetValue();
        }

        public static readonly DependencyProperty DateFormatTypeProperty =
            DependencyProperty.Register(nameof(DateFormatType), typeof(DateFormatTypes), typeof(DateReadOnlyBox),
                new FrameworkPropertyMetadata(DateFormatTypeChangedCallback));

        public DateFormatTypes DateFormatType
        {
            get { return (DateFormatTypes)GetValue(DateFormatTypeProperty); }
            set { SetValue(DateFormatTypeProperty, value); }
        }

        private static void DateFormatTypeChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateReadOnlyBox = (DateReadOnlyBox)obj;
            dateReadOnlyBox.SetValue();
        }

        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(DateReadOnlyBox),
                new FrameworkPropertyMetadata(CultureIdChangedCallback));

        public string CultureId
        {
            get { return (string)GetValue(CultureIdProperty); }
            set { SetValue(CultureIdProperty, value); }
        }

        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dateEditBox = (DateReadOnlyBox)obj;
            var culture = new CultureInfo(dateEditBox.CultureId);
            dateEditBox.Culture = culture; 
            dateEditBox.SetValue();
        }

        public CultureInfo Culture { get; protected internal set; }

        static DateReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateReadOnlyBox), new FrameworkPropertyMetadata(typeof(DateReadOnlyBox)));
        }

        public DateReadOnlyBox()
        {
            if (Culture == null)
                Culture = CultureInfo.CurrentCulture;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetValue();
        }

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

    }
}
