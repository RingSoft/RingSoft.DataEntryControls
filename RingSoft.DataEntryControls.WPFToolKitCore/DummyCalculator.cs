using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;

namespace RingSoft.DataEntryControls.WPFToolKitCore
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPFToolKitCore"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPFToolKitCore;assembly=RingSoft.DataEntryControls.WPFToolKitCore"
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
    ///     <MyNamespace:DummyCalculator/>
    ///
    /// </summary>
    public class DummyCalculator : Control, IDropDownCalculator
    {
        public Control Control => this;
        public decimal? Value { get; set; }
        public int Precision { get; set; }

        public TextBlock TextBlock { get; set; }

        public Calendar Calendar { get; set; }

        static DummyCalculator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DummyCalculator), new FrameworkPropertyMetadata(typeof(DummyCalculator)));
        }

        public event RoutedPropertyChangedEventHandler<object> ValueChanged;

        public override void OnApplyTemplate()
        {
            TextBlock = GetTemplateChild(nameof(TextBlock)) as TextBlock;
            Calendar = GetTemplateChild(nameof(Calendar)) as Calendar;
            base.OnApplyTemplate();

            Calendar.SelectedDatesChanged += (sender, args) =>
            {
                TextBlock.Text = Calendar.SelectedDate.ToString();
            };
        }
    }
}
