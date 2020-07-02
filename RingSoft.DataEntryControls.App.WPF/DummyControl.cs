using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.App.WPF
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
    ///     <MyNamespace:CustomCalculator/>
    ///
    /// </summary>
    public class DummyControl : Control
    {
        public TextBox TextBox { get; set; }

        public Popup Popup { get; set; }

        //public Calculator Calculator { get; set; }

        static DummyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DummyControl), new FrameworkPropertyMetadata(typeof(DummyControl)));
        }


        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild(nameof(TextBox)) as TextBox;
            Popup = GetTemplateChild(nameof(Popup)) as Popup;
            //Calculator = GetTemplateChild(nameof(Calculator)) as Calculator;

            base.OnApplyTemplate();

            TextBox.KeyDown += DummyTextBox_KeyDown;
        }

        private void DummyTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                Popup.IsOpen = !Popup.IsOpen;
                //Calculator.Value = (decimal)298.32;
                //Calculator.Focus();
            }
        }
    }
}
