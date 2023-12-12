// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="DecimalReadOnlyBox.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A control that displays decimal values to the user.  User is not allowed to edit.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.NumericReadOnlyBox{System.Double?}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.NumericReadOnlyBox{System.Double?}" />
    /// <font color="red">Badly formed XML comment.</font>
    public class DecimalReadOnlyBox : NumericReadOnlyBox<double?>
    {
        /// <summary>
        /// The precision property
        /// </summary>
        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register(nameof(Precision), typeof(int), typeof(DecimalReadOnlyBox),
                new FrameworkPropertyMetadata(2, PrecisionChangedCallback));

        /// <summary>
        /// Gets or sets the precision.  This is a bind-able property.
        /// </summary>
        /// <value>The precision.</value>
        public int Precision
        {
            get { return (int)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        /// <summary>
        /// Precisions the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PrecisionChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var decimalReadOnlyBox = (DecimalReadOnlyBox)obj;
            decimalReadOnlyBox.SetValue();
        }

        /// <summary>
        /// The format type property
        /// </summary>
        public static readonly DependencyProperty FormatTypeProperty =
            DependencyProperty.Register(nameof(FormatType), typeof(DecimalEditFormatTypes), typeof(DecimalReadOnlyBox),
                new FrameworkPropertyMetadata(FormatTypeChangedCallback));

        /// <summary>
        /// Gets or sets the type of the format.  This is a bind-able property.
        /// </summary>
        /// <value>The type of the format.</value>
        public DecimalEditFormatTypes FormatType
        {
            get { return (DecimalEditFormatTypes)GetValue(FormatTypeProperty); }
            set { SetValue(FormatTypeProperty, value); }
        }

        /// <summary>
        /// Formats the type changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void FormatTypeChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var decimalReadOnlyBox = (DecimalReadOnlyBox)obj;
            decimalReadOnlyBox.SetValue();
        }

        /// <summary>
        /// Initializes static members of the <see cref="DecimalReadOnlyBox"/> class.
        /// </summary>
        static DecimalReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalReadOnlyBox), new FrameworkPropertyMetadata(typeof(DecimalReadOnlyBox)));
            TextAlignmentProperty.OverrideMetadata(typeof(DecimalReadOnlyBox), new FrameworkPropertyMetadata(TextAlignment.Right));
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        protected override void SetValue()
        {
            var text = string.Empty;

            if (Value != null)
            {
                var formatString =
                    DecimalEditControlSetup.GetDecimalFormatString(FormatType, Precision, NumberFormatString);
                var displayValue = (double) Value;

                text = displayValue.ToString(formatString, Culture.NumberFormat);
            }

            Text = text;
        }
    }
}
