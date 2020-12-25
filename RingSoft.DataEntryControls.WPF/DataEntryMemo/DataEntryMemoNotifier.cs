using System.Windows;
using System.Windows.Controls;

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
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DataEntryMemo"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DataEntryMemo;assembly=RingSoft.DataEntryControls.WPF.DataEntryMemo"
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
    ///     <MyNamespace:DataEntryMemoNotifier/>
    ///
    /// </summary>
    [TemplatePart(Name = "ContainerPanel", Type = typeof(Panel))]
    public class DataEntryMemoNotifier : Control
    {
        public Panel ContainerPanel { get; set; }

        static DataEntryMemoNotifier()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryMemoNotifier), new FrameworkPropertyMetadata(typeof(DataEntryMemoNotifier)));

            FocusableProperty.OverrideMetadata(typeof(DataEntryMemoNotifier), new FrameworkPropertyMetadata(false));
        }

        public override void OnApplyTemplate()
        {
            ContainerPanel = GetTemplateChild(nameof(ContainerPanel)) as Panel;

            OnMemoChanged(false);
            base.OnApplyTemplate();
        }

        public virtual void OnMemoChanged(bool memoContainsText)
        {
            Visibility = memoContainsText ? Visibility.Visible : Visibility.Collapsed;
            //if (ContainerPanel != null)
            //{
            //    ContainerPanel.Visibility = memoContainsText ? Visibility.Visible : Visibility.Collapsed;
            //}
        }
    }
}
