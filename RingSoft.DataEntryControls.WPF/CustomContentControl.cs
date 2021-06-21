using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
    ///     <MyNamespace:CustomContentControl/>
    ///
    /// </summary>
    [TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter))]
    public class CustomContentControl : Control
    {
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataEntryCustomContentTemplate), typeof(CustomContentControl),
                new FrameworkPropertyMetadata(ContentTemplateChangedCallback));

        public DataEntryCustomContentTemplate ContentTemplate
        {
            get { return (DataEntryCustomContentTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        private static void ContentTemplateChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customControl = (CustomContentControl)obj;
            customControl.SelectItem(customControl.SelectedItemId);
        }

        private int _selectedItemId;

        public int SelectedItemId
        {
            get => _selectedItemId;
            set
            {
                if (_selectedItemId == value)
                    return;

                _selectedItemId = value;

                SelectItem(SelectedItemId);
            }
        }


        public ContentPresenter ContentPresenter { get; set; }

        private bool _controlLoaded;


        static CustomContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomContentControl), new FrameworkPropertyMetadata(typeof(CustomContentControl)));
        }

        public override void OnApplyTemplate()
        {
            ContentPresenter = GetTemplateChild(nameof(ContentPresenter)) as ContentPresenter;

            _controlLoaded = true;

            SelectItem(SelectedItemId);

            base.OnApplyTemplate();
        }

        protected void SelectItem(int itemId)
        {
            if (!_controlLoaded || ContentTemplate == null)
                return;

            var contentItem = ContentTemplate.FirstOrDefault(f => f.ItemId == itemId);
            if (contentItem != null)
                ContentPresenter.ContentTemplate = contentItem.DataTemplate;
        }

    }
}
