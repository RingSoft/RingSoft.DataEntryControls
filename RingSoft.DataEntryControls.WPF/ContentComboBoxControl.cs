// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-19-2023
// ***********************************************************************
// <copyright file="ContentComboBoxControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class ContentComboBoxControl.
    /// Implements the <see cref="ComboBox" />
    /// </summary>
    /// <seealso cref="ComboBox" />
    /// <font color="red">Badly formed XML comment.</font>
    public class ContentComboBoxControl : ComboBox
    {
        /// <summary>
        /// The selected item identifier property
        /// </summary>
        public static readonly DependencyProperty SelectedItemIdProperty =
            DependencyProperty.Register(nameof(SelectedItemId), typeof(int), typeof(ContentComboBoxControl),
                new FrameworkPropertyMetadata(SelectedItemIdChangedCallback));

        /// <summary>
        /// Gets or sets the selected item identifier.
        /// </summary>
        /// <value>The selected item identifier.</value>
        public int SelectedItemId
        {
            get { return (int)GetValue(SelectedItemIdProperty); }
            set { SetValue(SelectedItemIdProperty, value); }
        }

        /// <summary>
        /// Selecteds the item identifier changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SelectedItemIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var contentComboBoxControl = (ContentComboBoxControl)obj;
            contentComboBoxControl.SelectItem(contentComboBoxControl.SelectedItemId);
        }

        /// <summary>
        /// The content template property
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataEntryCustomContentTemplate), typeof(ContentComboBoxControl),
                new FrameworkPropertyMetadata(ContentTemplateChangedCallback));

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        /// <value>The content template.</value>
        public DataEntryCustomContentTemplate ContentTemplate
        {
            get { return (DataEntryCustomContentTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Contents the template changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ContentTemplateChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var contentComboBoxControl = (ContentComboBoxControl)obj;
            contentComboBoxControl.SetContent();
        }

        /// <summary>
        /// The UI command property
        /// </summary>
        public static readonly DependencyProperty UiCommandProperty =
            DependencyProperty.Register(nameof(UiCommand), typeof(UiCommand), typeof(ContentComboBoxControl),
                new FrameworkPropertyMetadata(UiCommandChangedCallback));

        /// <summary>
        /// Gets or sets the UI command.
        /// </summary>
        /// <value>The UI command.</value>
        public UiCommand UiCommand
        {
            get { return (UiCommand)GetValue(UiCommandProperty); }
            set { SetValue(UiCommandProperty, value); }
        }

        /// <summary>
        /// UIs the command changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void UiCommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customComboBoxControl = (ContentComboBoxControl)obj;
            if (customComboBoxControl._vmUiControl == null)
            {
                customComboBoxControl._vmUiControl = WPFControlsGlobals.VmUiFactory.CreateUiControl(
                    customComboBoxControl, customComboBoxControl.UiCommand);
                if (customComboBoxControl.UiLabel != null)
                {
                    customComboBoxControl._vmUiControl.SetLabel(customComboBoxControl.UiLabel);
                }
            }
        }

        /// <summary>
        /// The UI label property
        /// </summary>
        public static readonly DependencyProperty UiLabelProperty =
            DependencyProperty.Register(nameof(UiLabel), typeof(Label), typeof(ContentComboBoxControl),
                new FrameworkPropertyMetadata(UiLabelChangedCallback));

        /// <summary>
        /// Gets or sets the UI label.
        /// </summary>
        /// <value>The UI label.</value>
        public Label UiLabel
        {
            get { return (Label)GetValue(UiLabelProperty); }
            set { SetValue(UiLabelProperty, value); }
        }

        /// <summary>
        /// UIs the label changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void UiLabelChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customContentControl = (ContentComboBoxControl)obj;
            if (customContentControl._vmUiControl != null)
                customContentControl._vmUiControl.SetLabel(customContentControl.UiLabel);
        }


        /// <summary>
        /// The control loaded
        /// </summary>
        private bool _controlLoaded;
        /// <summary>
        /// The height
        /// </summary>
        private double _height;
        /// <summary>
        /// The vm UI control
        /// </summary>
        private VmUiControl _vmUiControl;

        /// <summary>
        /// Initializes static members of the <see cref="ContentComboBoxControl"/> class.
        /// </summary>
        static ContentComboBoxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentComboBoxControl), new FrameworkPropertyMetadata(typeof(ContentComboBoxControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentComboBoxControl"/> class.
        /// </summary>
        public ContentComboBoxControl()
        {

            Loaded += (sender, args) => OnLoaded();
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
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

        /// <summary>
        /// Handles the GotFocus event of the ContentComboBoxControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ContentComboBoxControl_GotFocus(object sender, RoutedEventArgs e)
        {
            var border = this.GetVisualChild<Border>();
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = new SolidColorBrush(Colors.Blue);
            Height = _height + 5;
            UpdateLayout();

        }

        /// <summary>
        /// Sets the content.
        /// </summary>
        /// <exception cref="System.Exception">The {nameof(ContentTemplate)} Property has not been set.</exception>
        private void SetContent()
        {
            if (_controlLoaded)
            {
                ItemsSource = ContentTemplate ?? throw new Exception($"The {nameof(ContentTemplate)} Property has not been set.");
                SelectItem(SelectedItemId);
            }
        }

        /// <summary>
        /// Responds to a <see cref="T:System.Windows.Controls.ComboBox" /> selection change by raising a <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> event.
        /// </summary>
        /// <param name="e">Provides data for <see cref="T:System.Windows.Controls.SelectionChangedEventArgs" />.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (SelectedItem is DataEntryCustomContentTemplateItem customContent)
                SelectedItemId = customContent.ItemId;

            base.OnSelectionChanged(e);
        }

        /// <summary>
        /// Selects the item.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        protected void SelectItem(int itemId)
        {
            if (!_controlLoaded || ContentTemplate == null)
                return;

            var selectedItem = ContentTemplate.FirstOrDefault(f => f.ItemId == itemId);

            SelectedItem = selectedItem;
        }

        /// <summary>
        /// Handles the <see cref="E:KeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
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
