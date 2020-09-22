using System;
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
    ///     <MyNamespace:DecimalReadOnlyBox/>
    ///
    /// </summary>
    public class DecimalReadOnlyBox : NumericReadOnlyBox<decimal?>
    {
        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register(nameof(Precision), typeof(int), typeof(DecimalReadOnlyBox));

        public int Precision
        {
            get { return (int)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        public static readonly DependencyProperty FormatTypeProperty =
            DependencyProperty.Register(nameof(FormatType), typeof(DecimalEditFormatTypes), typeof(DecimalReadOnlyBox));

        public DecimalEditFormatTypes FormatType
        {
            get { return (DecimalEditFormatTypes)GetValue(FormatTypeProperty); }
            set { SetValue(FormatTypeProperty, value); }
        }

        static DecimalReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalReadOnlyBox), new FrameworkPropertyMetadata(typeof(DecimalReadOnlyBox)));
            PrecisionProperty.OverrideMetadata(typeof(DecimalReadOnlyBox), new FrameworkPropertyMetadata(2));
            TextAlignmentProperty.OverrideMetadata(typeof(DecimalReadOnlyBox), new FrameworkPropertyMetadata(TextAlignment.Right));
        }

        protected override void SetValue()
        {
            throw new NotImplementedException();
        }
    }
}
