using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

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
    ///     <MyNamespace:DataEntryDecimalEditControl/>
    ///
    /// </summary>
    public class DataEntryDecimalEditControl : CalculatorUpDown, IDecimalEditControl
    {
        public Control EditControl => this;

        public int SelectionStart
        {
            get => TextBox.SelectionStart;
            set => TextBox.SelectionStart = value;
        }

        public int SelectionLength
        {
            get => TextBox.SelectionLength;
            set => TextBox.SelectionLength = value;
        }

        public void OnInvalidChar()
        {
            System.Media.SystemSounds.Exclamation.Play();
        }

        private DataEntryNumericEditSetup _numericEditSetup;

        public DataEntryNumericEditSetup NumericSetup
        {
            get => _numericEditSetup;
            set
            {
                if (_numericEditSetup == value)
                    return;

                _numericEditSetup = value;
                FormatString = _numericEditSetup.GetNumberFormatString();
            }
        }

        public DataEntryNumericControlProcessor Processor { get; }

        public DataEntryDecimalEditControl()
        {
            Loaded += (sender, args) =>
            {
                TextBox.PreviewTextInput += TextBox_PreviewTextInput;
            };

            Processor = new DataEntryNumericControlProcessor(this, NumericSetup);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                return;

            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                return;

            if (!Processor.IsValidChar(e.Text[0]))
                e.Handled = true;
        }
    }
}
