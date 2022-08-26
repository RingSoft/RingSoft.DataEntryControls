using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace RingSoft.DataEntryControls.WPF
{
    public class NotificationButton : Button
    {
        public static readonly DependencyProperty NotificationVisibilityProperty =
            DependencyProperty.Register(nameof(NotificationVisibility), typeof(Visibility), typeof(NotificationButton)
                , new FrameworkPropertyMetadata(Visibility.Visible));

        public Visibility NotificationVisibility
        {
            get { return (Visibility) GetValue(NotificationVisibilityProperty); }
            set { SetValue(NotificationVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MemoHasTextProperty =
            DependencyProperty.Register(nameof(MemoHasText), typeof(bool), typeof(NotificationButton));

        public bool MemoHasText
        {
            get { return (bool) GetValue(MemoHasTextProperty); }
            set { SetValue(MemoHasTextProperty, value); }
        }

        public Ellipse Notifier { get; set; }

        static NotificationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationButton),
                new FrameworkPropertyMetadata(typeof(NotificationButton)));
        }

    }
}