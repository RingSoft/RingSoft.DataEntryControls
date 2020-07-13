using RingSoft.DataEntryControls.Engine;
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
    ///     <MyNamespace:IntegerEditControl/>
    ///
    /// </summary>
    public class IntegerEditControl : NumericEditControl<int?>
    {
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(IntegerEditControlSetup), typeof(IntegerEditControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        public IntegerEditControlSetup Setup
        {
            private get { return (IntegerEditControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var intEditControl = (IntegerEditControl)obj;
            intEditControl.LoadFromSetup(intEditControl.Setup);
        }

        private int? _pendingNewValue;

        static IntegerEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerEditControl), new FrameworkPropertyMetadata(typeof(IntegerEditControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_pendingNewValue != null)
                SetValue();

            _pendingNewValue = null;
        }


        protected override void SetValue()
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

        protected override void PopulateSetup(DecimalEditControlSetup setup)
        {
            setup.EditFormatType = DecimalEditFormatTypes.Number;
            setup.MaximumValue = MaximumValue;
            setup.MinimumValue = MinimumValue;
            setup.Precision = 0;
            base.PopulateSetup(setup);
        }

        public override void OnValueChanged(string newValue)
        {
            Value = newValue.ToInt(Culture);

            base.OnValueChanged(newValue);
        }
    }
}
