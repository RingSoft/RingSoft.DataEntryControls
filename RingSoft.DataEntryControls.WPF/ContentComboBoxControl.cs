﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RingSoft.DataEntryControls.WPF
{
    public class CustomContent : ObservableCollection<CustomContentItem>
    {

    }
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

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(CustomContent), typeof(ContentComboBoxControl),
                new FrameworkPropertyMetadata(ContentChangedCallback));

        public CustomContent Content
        {
            get { return (CustomContent)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private static void ContentChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var contentComboBoxControl = (ContentComboBoxControl)obj;
            contentComboBoxControl.SetContent();
        }

        
        private bool _controlLoaded;

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
                new Binding(nameof(CustomContentItem.DataTemplate)));

            dataTemplate.VisualTree = contentPresenterFactory;
            ItemTemplate = dataTemplate;

            _controlLoaded = true;
            SetContent();
        }

        private void SetContent()
        {
            if (_controlLoaded)
            {
                ItemsSource = Content ?? throw new Exception("The Content Property has not been set.");
                SelectItem(SelectedItemId);
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (SelectedItem is CustomContentItem customContent)
                SelectedItemId = customContent.Id;

            base.OnSelectionChanged(e);
        }

        protected void SelectItem(int itemId)
        {
            if (!_controlLoaded || Content == null)
                return;

            var selectedItem = Content.FirstOrDefault(f => f.Id == itemId);

            SelectedItem = selectedItem;
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