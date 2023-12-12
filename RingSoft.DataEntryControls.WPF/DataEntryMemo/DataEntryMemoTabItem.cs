// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryMemoTabItem.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A tab item control that hosts a Memo Editor control. Puts a red dot on the right side of the header when there is tet inside the memo editor.
    /// Implements the <see cref="TabItem" />
    /// </summary>
    /// <seealso cref="TabItem" />
    /// <font color="red">Badly formed XML comment.</font>
    public class DataEntryMemoTabItem : TabItem
    {
        /// <summary>
        /// The notification visibility property
        /// </summary>
        public static readonly DependencyProperty NotificationVisibilityProperty =
            DependencyProperty.Register(nameof(NotificationVisibility), typeof(Visibility), typeof(DataEntryMemoTabItem),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Gets or sets the notification visibility.  This is a bind-able property.
        /// </summary>
        /// <value>The notification visibility.</value>
        public Visibility NotificationVisibility
        {
            get { return (Visibility)GetValue(NotificationVisibilityProperty); }
            set { SetValue(NotificationVisibilityProperty, value); }
        }

        /// <summary>
        /// The memo has text property
        /// </summary>
        public static readonly DependencyProperty MemoHasTextProperty =
            DependencyProperty.Register(nameof(MemoHasText), typeof(bool), typeof(DataEntryMemoTabItem));

        /// <summary>
        /// Gets or sets a value indicating whether [memo has text].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [memo has text]; otherwise, <c>false</c>.</value>
        public bool MemoHasText
        {
            get { return (bool)GetValue(MemoHasTextProperty); }
            set { SetValue(MemoHasTextProperty, value); }
        }

        /// <summary>
        /// Initializes static members of the <see cref="DataEntryMemoTabItem"/> class.
        /// </summary>
        static DataEntryMemoTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryMemoTabItem), new FrameworkPropertyMetadata(typeof(DataEntryMemoTabItem)));
        }

    }
}
