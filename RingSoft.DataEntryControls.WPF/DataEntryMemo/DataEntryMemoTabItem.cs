using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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

        public static readonly DependencyProperty NotificationVisibilityProperty =
            DependencyProperty.Register(nameof(NotificationVisibility), typeof(Visibility), typeof(DataEntryMemoTabItem),
                new FrameworkPropertyMetadata(System.Windows.Visibility.Collapsed));

        public Visibility NotificationVisibility
        {
            get { return (Visibility)GetValue(NotificationVisibilityProperty); }
            set { SetValue(NotificationVisibilityProperty, value); }
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
            tabItem.UpdateNotification();

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

        //public DataEntryMemoNotifier Notifier { get; set; }

        public Ellipse Notifier { get; set; }

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
                //Notifier = content.GetVisualChild<DataEntryMemoNotifier>();
                Notifier = content.GetVisualChild<Ellipse>();
            }
        }

        private void UpdateNotification()
        {
            NotificationVisibility = MemoHasText ? Visibility.Visible : Visibility.Collapsed;

            //var tabControl = this.GetParentOfType<TabControl>();
            //ContentPresenter myContentPresenter = HeaderTemplate.FindName("PART_SelectedContentHost", this) as ContentPresenter;
            //if (myContentPresenter != null)
            //{
            //    if (myContentPresenter.ContentTemplate == HeaderTemplate)
            //    {
            //        myContentPresenter.ApplyTemplate();
            //        var notifier = myContentPresenter.ContentTemplate.FindName("Notifier", myContentPresenter) as Ellipse;
            //        if (notifier != null)
            //            notifier.Visibility = MemoHasText ? Visibility.Visible : Visibility.Collapsed;
            //    }
            //}

            //if (Notifier != null)
            //    Notifier.Visibility = MemoHasText ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
