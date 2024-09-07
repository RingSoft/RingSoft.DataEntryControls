// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="IntegerEditControl.cs" company="Peter Ringering">
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
    /// A control that edits integer values.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.NumericEditControl{System.Int32?}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.NumericEditControl{System.Int32?}" />
    public class IntegerEditControl : NumericEditControl<int?>
    {
        /// <summary>
        /// The setup property
        /// </summary>
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(IntegerEditControlSetup), typeof(IntegerEditControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        /// <summary>
        /// Sets the setup.  This is a bind-able property.
        /// </summary>
        /// <value>The setup.</value>
        public IntegerEditControlSetup Setup
        {
            private get { return (IntegerEditControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        /// <summary>
        /// Setups the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var intEditControl = (IntegerEditControl)obj;
            intEditControl.LoadFromSetup(intEditControl.Setup);
        }

        /// <summary>
        /// The pending new value
        /// </summary>
        private int? _pendingNewValue;

        /// <summary>
        /// Initializes static members of the <see cref="IntegerEditControl" /> class.
        /// </summary>
        static IntegerEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerEditControl), new FrameworkPropertyMetadata(typeof(IntegerEditControl)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_pendingNewValue != null)
                SetValue();

            _pendingNewValue = null;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        protected override void SetValue()
        {
            if (TextBox == null)
            {
                _pendingNewValue = Value;
            }
            else
            {
                SetText(Value);
            }
        }

        /// <summary>
        /// Converts the value to string.
        /// </summary>
        /// <returns>System.String.</returns>
        protected override string ConvertValueToString()
        {
            var result = string.Empty;
            if (Value != null)
                result = ((int)Value).ToString(Culture);

            return result;
        }

        /// <summary>
        /// Gets the minimum value properties.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimumValue">The minimum value.</param>
        protected override void GetMinimumValueProperties(out double? value, out double? minimumValue)
        {
            value = Value;
            minimumValue = MinimumValue;
        }

        /// <summary>
        /// Populates the setup.
        /// </summary>
        /// <param name="setup">The setup.</param>
        protected override void PopulateSetup(DecimalEditControlSetup setup)
        {
            setup.FormatType = DecimalEditFormatTypes.Number;
            setup.MaximumValue = MaximumValue;
            setup.MinimumValue = MinimumValue;
            setup.Precision = 0;
            base.PopulateSetup(setup);
        }

        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public override void OnValueChanged(string newValue)
        {
            if (newValue.IsNullOrEmpty())
                Value = null;
            else
                Value = newValue.ToInt(Culture);

            base.OnValueChanged(newValue);
        }
    }
}
