using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
    [TemplatePart(Name = "DataEntryMemoEditor", Type = typeof(DataEntryMemoEditor))]
    public class DataEntryMemoTabItem : TabItem
    {
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(DataEntryMemoTabItem));

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public static readonly DependencyProperty NotificationTextProperty =
            DependencyProperty.Register(nameof(NotificationText), typeof(string), typeof(DataEntryMemoTabItem));

        public string NotificationText
        {
            get { return (string)GetValue(NotificationTextProperty); }
            set { SetValue(NotificationTextProperty, value); }
        }

        public static readonly DependencyProperty NotificationColorProperty =
            DependencyProperty.Register(nameof(NotificationColor), typeof(Brush), typeof(DataEntryMemoTabItem));

        public Brush NotificationColor
        {
            get { return (Brush)GetValue(NotificationColorProperty); }
            set { SetValue(NotificationColorProperty, value); }
        }

        public static readonly DependencyProperty MemoHasTextProperty =
            DependencyProperty.Register(nameof(MemoHasText), typeof(bool), typeof(DataEntryMemoTabItem),
                new FrameworkPropertyMetadata(MemoHasTextCallback));

        public bool MemoHasText
        {
            get { return (bool)GetValue(MemoHasTextProperty); }
            set { SetValue(MemoHasTextProperty, value); }
        }

        private static void MemoHasTextCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var tabItem = (DataEntryMemoTabItem)obj;
            if (tabItem.Notifier != null)
                tabItem.Notifier.Visibility = tabItem.MemoHasText ? Visibility.Visible : Visibility.Collapsed;

            //tabItem.Notifier?.OnMemoChanged(tabItem.MemoHasText);

            //if (tabItem.MemoHasText)
            //{
            //    tabItem.NotificationColor = new SolidColorBrush(Colors.Green);
            //    tabItem.NotificationText = "*";
            //}
            //else
            //{
            //    tabItem.NotificationColor = new SolidColorBrush(Colors.Transparent);
            //    tabItem.NotificationText = string.Empty;
            //}
        }

        public DataEntryMemoNotifier Notifier { get; set; }

        static DataEntryMemoTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryMemoTabItem), new FrameworkPropertyMetadata(typeof(DataEntryMemoTabItem)));
        }

        public DataEntryMemoTabItem()
        {
            Loaded += (sender, args) => OnLoad();
        }

        protected virtual void OnLoad()
        {
            if (HeaderTemplate != null && HeaderTemplate.HasContent)
            {
                var content = HeaderTemplate.LoadContent();
                Notifier = content.GetVisualChild<DataEntryMemoNotifier>();
            }
        }
    }
}
