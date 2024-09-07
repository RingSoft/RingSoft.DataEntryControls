// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="StringReadOnlyBox.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A control that displays string values to the user.  User is not allowed to edit.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.ReadOnlyBox" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.ReadOnlyBox" />
    public class StringReadOnlyBox : ReadOnlyBox
    {
        /// <summary>
        /// The text property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(StringReadOnlyBox),
                new FrameworkPropertyMetadata(TextChangedCallback));

        /// <summary>
        /// Gets or sets the text.  This is a bind-able property.
        /// </summary>
        /// <value>The text.</value>
        public new string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Texts the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void TextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (StringReadOnlyBox)obj;
            readOnlyBox.SetText();
        }

        /// <summary>
        /// Initializes static members of the <see cref="StringReadOnlyBox" /> class.
        /// </summary>
        static StringReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StringReadOnlyBox), new FrameworkPropertyMetadata(typeof(StringReadOnlyBox)));
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        private void SetText()
        {
            base.Text = Text;
        }
    }
}
