using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

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
    ///     <MyNamespace:ContentComboBoxControl/>
    ///
    /// </summary>
    public class ContentComboBoxControl : ComboBox
    {
        public static readonly DependencyProperty SelectedItemIdProperty =
            DependencyProperty.Register(nameof(SelectedItemId), typeof(int), typeof(ContentComboBoxControl),
                new FrameworkPropertyMetadata(SelectedItemIdChangedCallback));

        public int SelectedItemId
        {
            get { return (int)GetValue(SelectedItemIdProperty); }
            set { SetValue(SelectedItemIdProperty, value); }
        }

        private static void SelectedItemIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var contentComboBoxControl = (ContentComboBoxControl)obj;
            contentComboBoxControl.SelectItem(contentComboBoxControl.SelectedItemId);
        }

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataEntryCustomContentTemplate), typeof(ContentComboBoxControl),
                new FrameworkPropertyMetadata(ContentTemplateChangedCallback));

        public DataEntryCustomContentTemplate ContentTemplate
        {
            get { return (DataEntryCustomContentTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        private static void ContentTemplateChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var contentComboBoxControl = (ContentComboBoxControl)obj;
            contentComboBoxControl.SetContent();
        }

        
        private bool _controlLoaded;
        private double _height;

        static ContentComboBoxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentComboBoxControl), new FrameworkPropertyMetadata(typeof(ContentComboBoxControl)));
        }

        public ContentComboBoxControl()
        {

            Loaded += (sender, args) => OnLoaded();
        }

        private void OnLoaded()
        {
            var dataTemplate = new DataTemplate();
            var contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetBinding(ContentPresenter.ContentTemplateProperty,
                new Binding(nameof(DataEntryCustomContentTemplateItem.DataTemplate)));

            dataTemplate.VisualTree = contentPresenterFactory;
            ItemTemplate = dataTemplate;

            _controlLoaded = true;
            SetContent();

            UpdateLayout();
            _height = ActualHeight;
            if (IsFocused)
            {
                ContentComboBoxControl_GotFocus(this, new RoutedEventArgs());
            }

            GotFocus += ContentComboBoxControl_GotFocus;
            LostFocus += (o, eventArgs) =>
            {
                var border = this.GetVisualChild<Border>();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = new SolidColorBrush(Colors.Transparent);
            };
            DropDownOpened += (o, eventArgs) =>
            {
                if (SelectedItem == null)
                {
                    SelectedItem = Items[0];
                }
            };

        }

        private void ContentComboBoxControl_GotFocus(object sender, RoutedEventArgs e)
        {
            var border = this.GetVisualChild<Border>();
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = new SolidColorBrush(Colors.Blue);
            Height = _height + 5;
            UpdateLayout();

        }

        private void SetContent()
        {
            if (_controlLoaded)
            {
                ItemsSource = ContentTemplate ?? throw new Exception($"The {nameof(ContentTemplate)} Property has not been set.");
                SelectItem(SelectedItemId);
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (SelectedItem is DataEntryCustomContentTemplateItem customContent)
                SelectedItemId = customContent.ItemId;

            base.OnSelectionChanged(e);
        }

        protected void SelectItem(int itemId)
        {
            if (!_controlLoaded || ContentTemplate == null)
                return;

            var selectedItem = ContentTemplate.FirstOrDefault(f => f.ItemId == itemId);

            SelectedItem = selectedItem;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var item = ContentTemplate.FirstOrDefault(f => f.HotKey == e.Key);

            if (item != null)
                SelectedItem = item;

            base.OnKeyDown(e);
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.I:
        //            InventoryItem.IsSelected = true;
        //            break;
        //        case Key.N:
        //            NonInventoryItem.IsSelected = true;
        //            break;
        //        case Key.C:
        //            CommentItem.IsSelected = true;
        //            break;
        //    }
        //    base.OnKeyDown(e);
        //}
    }
}
