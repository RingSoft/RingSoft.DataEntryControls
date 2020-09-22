using System.Globalization;
using System.Windows;

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
    ///     <MyNamespace:NumericReadOnlyBox/>
    ///
    /// </summary>
    public abstract class NumericReadOnlyBox<T> : ReadOnlyBox
    {
        public static readonly DependencyProperty NumberFormatStringProperty =
            DependencyProperty.Register(nameof(NumberFormatString), typeof(string), typeof(NumericReadOnlyBox<T>));

        public string NumberFormatString
        {
            get { return (string)GetValue(NumberFormatStringProperty); }
            set { SetValue(NumberFormatStringProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(T), typeof(NumericReadOnlyBox<T>),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericReadOnlyBox = (NumericReadOnlyBox<T>)obj;
            numericReadOnlyBox.SetValue();
        }

        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(NumericReadOnlyBox<T>),
                new FrameworkPropertyMetadata(CultureIdChangedCallback));

        public string CultureId
        {
            get { return (string)GetValue(CultureIdProperty); }
            set { SetValue(CultureIdProperty, value); }
        }

        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericReadOnlyBox = (NumericReadOnlyBox<T>)obj;
            var culture = new CultureInfo(numericReadOnlyBox.CultureId);
            numericReadOnlyBox.Culture = culture;
            numericReadOnlyBox.SetValue();
        }

        public CultureInfo Culture { get; protected internal set; }


        static NumericReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericReadOnlyBox<T>), new FrameworkPropertyMetadata(typeof(NumericReadOnlyBox<T>)));
        }

        public NumericReadOnlyBox()
        {
            if (Culture == null)
                Culture = CultureInfo.CurrentCulture;
        }

        protected abstract void SetValue();
    }
}
