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
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF;assembly=RingSoft.DataEntryControls.WPF"
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
    ///     <MyNamespace:DataEntryMemoTabItem/>
    ///
    /// </summary>
    public class DataEntryMemoTabItem : TabItem
    {
        public static readonly DependencyProperty NotificationVisibilityProperty =
            DependencyProperty.Register(nameof(NotificationVisibility), typeof(Visibility), typeof(DataEntryMemoTabItem),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        public Visibility NotificationVisibility
        {
            get { return (Visibility)GetValue(NotificationVisibilityProperty); }
            set { SetValue(NotificationVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MemoHasTextProperty =
            DependencyProperty.Register(nameof(MemoHasText), typeof(bool), typeof(DataEntryMemoTabItem));

        public bool MemoHasText
        {
            get { return (bool)GetValue(MemoHasTextProperty); }
            set { SetValue(MemoHasTextProperty, value); }
        }

        static DataEntryMemoTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryMemoTabItem), new FrameworkPropertyMetadata(typeof(DataEntryMemoTabItem)));
        }

    }
}
