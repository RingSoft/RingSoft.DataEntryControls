// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="NumericReadOnlyBox.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Globalization;
using System.Windows;
using RingSoft.DataEntryControls.Engine;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class NumericReadOnlyBox.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.ReadOnlyBox" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.ReadOnlyBox" />
    /// <font color="red">Badly formed XML comment.</font>
    public abstract class NumericReadOnlyBox<T> : ReadOnlyBox
    {
        /// <summary>
        /// The number format string property
        /// </summary>
        public static readonly DependencyProperty NumberFormatStringProperty =
            DependencyProperty.Register(nameof(NumberFormatString), typeof(string), typeof(NumericReadOnlyBox<T>));

        /// <summary>
        /// Gets or sets the number format string.
        /// </summary>
        /// <value>The number format string.</value>
        public string NumberFormatString
        {
            get { return (string)GetValue(NumberFormatStringProperty); }
            set { SetValue(NumberFormatStringProperty, value); }
        }

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(T), typeof(NumericReadOnlyBox<T>),
                new FrameworkPropertyMetadata(ValueChangedCallback));

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Values the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericReadOnlyBox = (NumericReadOnlyBox<T>)obj;
            numericReadOnlyBox.SetValue();
        }

        /// <summary>
        /// The culture identifier property
        /// </summary>
        public static readonly DependencyProperty CultureIdProperty =
            DependencyProperty.Register(nameof(CultureId), typeof(string), typeof(NumericReadOnlyBox<T>),
                new FrameworkPropertyMetadata(CultureIdChangedCallback));

        /// <summary>
        /// Gets or sets the culture identifier.
        /// </summary>
        /// <value>The culture identifier.</value>
        public string CultureId
        {
            get { return (string)GetValue(CultureIdProperty); }
            set { SetValue(CultureIdProperty, value); }
        }

        /// <summary>
        /// Cultures the identifier changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void CultureIdChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var numericReadOnlyBox = (NumericReadOnlyBox<T>)obj;
            var culture = new CultureInfo(numericReadOnlyBox.CultureId);
            numericReadOnlyBox.Culture = culture;
            
            DecimalEditControlSetup.FormatCulture(numericReadOnlyBox.Culture);
            numericReadOnlyBox.SetValue();
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; protected internal set; }


        /// <summary>
        /// Initializes static members of the <see cref="NumericReadOnlyBox{T}"/> class.
        /// </summary>
        static NumericReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericReadOnlyBox<T>), new FrameworkPropertyMetadata(typeof(NumericReadOnlyBox<T>)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericReadOnlyBox{T}"/> class.
        /// </summary>
        public NumericReadOnlyBox()
        {
            if (Culture == null)
                Culture = new CultureInfo(CultureInfo.CurrentCulture.Name);

            DecimalEditControlSetup.FormatCulture(Culture);
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetValue();
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        protected abstract void SetValue();
    }
}
