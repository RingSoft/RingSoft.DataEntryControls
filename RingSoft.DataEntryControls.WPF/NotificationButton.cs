// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="NotificationButton.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Button that displays a red ellipse notification when set.
    /// Implements the <see cref="Button" />
    /// </summary>
    /// <seealso cref="Button" />
    public class NotificationButton : Button
    {
        /// <summary>
        /// The notification visibility property
        /// </summary>
        public static readonly DependencyProperty NotificationVisibilityProperty =
            DependencyProperty.Register(nameof(NotificationVisibility), typeof(Visibility), typeof(NotificationButton)
                , new FrameworkPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the notification visibility.  This is a bind-able property.
        /// </summary>
        /// <value>The notification visibility.</value>
        public Visibility NotificationVisibility
        {
            get { return (Visibility) GetValue(NotificationVisibilityProperty); }
            set { SetValue(NotificationVisibilityProperty, value); }
        }

        /// <summary>
        /// The memo has text property
        /// </summary>
        public static readonly DependencyProperty MemoHasTextProperty =
            DependencyProperty.Register(nameof(MemoHasText), typeof(bool), typeof(NotificationButton));

        /// <summary>
        /// Gets or sets a value indicating whether [memo has text].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [memo has text]; otherwise, <c>false</c>.</value>
        public bool MemoHasText
        {
            get { return (bool) GetValue(MemoHasTextProperty); }
            set { SetValue(MemoHasTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the notifier.
        /// </summary>
        /// <value>The notifier.</value>
        public Ellipse Notifier { get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="NotificationButton"/> class.
        /// </summary>
        static NotificationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationButton),
                new FrameworkPropertyMetadata(typeof(NotificationButton)));
        }

    }
}