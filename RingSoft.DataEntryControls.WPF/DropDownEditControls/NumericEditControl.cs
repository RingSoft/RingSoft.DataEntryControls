using RingSoft.DataEntryControls.Engine;
using System;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    public enum NumericEditTypes
    {
        Decimal = 0,
        WholeNumber = 1
    }

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
    ///     <MyNamespace:NumericEditControl/>
    ///
    /// </summary>
    public abstract class NumericEditControl : DropDownEditControl
    {
        public abstract NumericEditTypes EditType { get; }

        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(DataEntryNumericEditSetup), typeof(NumericEditControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        private DataEntryNumericEditSetup Setup
        {
            get { return (DataEntryNumericEditSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericEditControl = (NumericEditControl)obj;
            switch (numericEditControl.EditType)
            {
                case NumericEditTypes.Decimal:
                    break;
                case NumericEditTypes.WholeNumber:
                    numericEditControl.Setup.Precision = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            numericEditControl.Precision = numericEditControl.Setup.Precision;
            numericEditControl.MaximumValue = numericEditControl.Setup.MaximumValue;
            numericEditControl.MinimumValue = numericEditControl.Setup.MinimumValue;
            numericEditControl.NumberFormatString = numericEditControl.Setup.GetNumberFormatString();
        }

        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register(nameof(Precision), typeof(int), typeof(NumericEditControl));

        private int Precision
        {
            get { return (int)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        public decimal MaximumValue { get; set;}

        public decimal MinimumValue { get; set; }

        public string NumberFormatString { get; set; }

        static NumericEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericEditControl), new FrameworkPropertyMetadata(typeof(NumericEditControl)));
        }
    }
}
