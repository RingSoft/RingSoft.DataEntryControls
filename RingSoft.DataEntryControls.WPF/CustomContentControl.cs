// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="CustomContentControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A control that displays custom content.
    /// Implements the <see cref="Control" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <font color="red">Badly formed XML comment.</font>
    [TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter))]
    public class CustomContentControl : Control
    {
        /// <summary>
        /// The content template property
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataEntryCustomContentTemplate), typeof(CustomContentControl),
                new FrameworkPropertyMetadata(ContentTemplateChangedCallback));

        /// <summary>
        /// Gets or sets the content template.  This is a bind-able property.
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
            var customControl = (CustomContentControl)obj;
            customControl.SelectItem(customControl.SelectedItemId);
        }

        /// <summary>
        /// The selected item identifier property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemIdProperty =
            DependencyProperty.Register(nameof(SelectedItemId), typeof(int), typeof(CustomContentControl),
                new FrameworkPropertyMetadata(SelectedItemIdChangedCallback));

        /// <summary>
        /// Gets or sets the selected item identifier.  This is a bind-able property.
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
            var customControl = (CustomContentControl)obj;
            customControl.SelectItem(customControl.SelectedItemId);
        }

        //public int SelectedItemId
        //{
        //    get => _selectedItemId;
        //    set
        //    {
        //        if (_selectedItemId == value)
        //            return;

        //        _selectedItemId = value;

        //        SelectItem(SelectedItemId);
        //    }
        //}


        /// <summary>
        /// Gets or sets the content presenter.
        /// </summary>
        /// <value>The content presenter.</value>
        public ContentPresenter ContentPresenter { get; set; }

        /// <summary>
        /// The control loaded
        /// </summary>
        private bool _controlLoaded;


        /// <summary>
        /// Initializes static members of the <see cref="CustomContentControl"/> class.
        /// </summary>
        static CustomContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomContentControl), new FrameworkPropertyMetadata(typeof(CustomContentControl)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            ContentPresenter = GetTemplateChild(nameof(ContentPresenter)) as ContentPresenter;

            _controlLoaded = true;

            SelectItem(SelectedItemId);

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Selects the item.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
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
