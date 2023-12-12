// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="DecimalEditControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using System;
using System.Media;
using System.Windows;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A control that edits decimal values.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.NumericEditControl{System.Double?}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.NumericEditControl{System.Double?}" />

    [TemplatePart(Name = "Calculator", Type = typeof(IDropDownCalculator))]
    public class DecimalEditControl : NumericEditControl<double?>
    {
        /// <summary>
        /// The calculator control
        /// </summary>
        private IDropDownCalculator _calculatorControl;

        /// <summary>
        /// Gets or sets the calculator control.
        /// </summary>
        /// <value>The calculator control.</value>
        public IDropDownCalculator CalculatorControl
        {
            get => _calculatorControl;
            set
            {
                if (_calculatorControl != null)
                {
                    _calculatorControl.ValueChanged -= _calculatorControl_ValueChanged;
                }

                _calculatorControl = value;
                
                if (_calculatorControl != null)
                {
                    _calculatorControl.ValueChanged += _calculatorControl_ValueChanged;
                }
            }
        }

        /// <summary>
        /// The precision property
        /// </summary>
        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register(nameof(Precision), typeof(int), typeof(DecimalEditControl),
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
            var decimalEditControl = (DecimalEditControl)obj;
            decimalEditControl.SetValue();
        }

        /// <summary>
        /// The setup property
        /// </summary>
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(DecimalEditControlSetup), typeof(DecimalEditControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        /// <summary>
        /// Sets the setup.  This is a bind-able property.
        /// </summary>
        /// <value>The setup.</value>
        public DecimalEditControlSetup Setup
        {
            private get { return (DecimalEditControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        /// <summary>
        /// Setups the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var decimalEditControl = (DecimalEditControl)obj;
            decimalEditControl.LoadFromSetup(decimalEditControl.Setup);
        }

        /// <summary>
        /// The format type property
        /// </summary>
        public static readonly DependencyProperty FormatTypeProperty =
            DependencyProperty.Register(nameof(FormatType), typeof(DecimalEditFormatTypes), typeof(DecimalEditControl),
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
            var decimalEditControl = (DecimalEditControl)obj;
            decimalEditControl.SetValue();
        }

        /// <summary>
        /// Occurs when [calculator value changed].
        /// </summary>
        public event EventHandler CalculatorValueChanged;

        /// <summary>
        /// The pending new value
        /// </summary>
        private double? _pendingNewValue;

        /// <summary>
        /// Initializes static members of the <see cref="DecimalEditControl"/> class.
        /// </summary>
        static DecimalEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalEditControl), new FrameworkPropertyMetadata(typeof(DecimalEditControl)));
            TextAlignmentProperty.OverrideMetadata(typeof(DecimalEditControl), new FrameworkPropertyMetadata(TextAlignment.Right));
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            CalculatorControl = GetTemplateChild("Calculator") as IDropDownCalculator;
            base.OnApplyTemplate();

            if (_pendingNewValue != null)
                SetValue();

            _pendingNewValue = null;
        }

        /// <summary>
        /// Loads from setup.
        /// </summary>
        /// <param name="setup">The setup.</param>
        protected override void LoadFromSetup(NumericEditControlSetup<double?> setup)
        {
            FormatType = Setup.FormatType;
            Precision = Setup.Precision;

            base.LoadFromSetup(setup);
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
                result = ((double) Value).ToString(Culture);

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
        /// Validates the value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        protected double? ValidateValue(double? newValue)
        {
            double? result = null;
            if (MaximumValue != null)
            {
                if (newValue > MaximumValue)
                    result = MaximumValue;
            }

            if (MinimumValue != null)
            {
                if (newValue < MinimumValue)
                    result = MinimumValue;
            }

            return result;
        }

        /// <summary>
        /// Handles the <see cref="E:PreviewKeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            if (e.Key == Key.F4)
            {
                var processHotKey = !(DropDownButton != null && !DropDownButton.IsEnabled);

                if (processHotKey)
                {
                    OnDropDownButtonClick();
                    e.Handled = true;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Called when [drop down button click].
        /// </summary>
        public override void OnDropDownButtonClick()
        {
            base.OnDropDownButtonClick();

            if (CalculatorControl != null && Popup != null && Popup.IsOpen)
            {
                var precision = Precision;

                CalculatorControl.Precision = precision;
                if (Value != null)
                {
                    var calcValue = (double)Value;
                    if (FormatType == DecimalEditFormatTypes.Percent)
                    {
                        calcValue *= 100;
                        calcValue = Math.Round(calcValue, Precision);
                    }
                    CalculatorControl.Value = calcValue;
                }

                CalculatorControl.Control.Focus();
            }
        }

        /// <summary>
        /// Calculators the control value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void _calculatorControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var newValue = CalculatorControl.Value;

            if (FormatType == DecimalEditFormatTypes.Percent)
                newValue /= 100;

            var validatedValue = ValidateValue(newValue);
            if (validatedValue != null)
            {
                newValue = validatedValue;
                SystemSounds.Exclamation.Play();
            }

            var valueChanged = !newValue.Equals(Value);
            Value = newValue;

            if (valueChanged)
            {
                OnValueChanged(Text);
                OnCalculatorValueChanged();
            }
        }

        /// <summary>
        /// Called when [calculator value changed].
        /// </summary>
        protected virtual void OnCalculatorValueChanged()
        {
            CalculatorValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Populates the setup.
        /// </summary>
        /// <param name="setup">The setup.</param>
        protected override void PopulateSetup(DecimalEditControlSetup setup)
        {
            setup.FormatType = FormatType;
            setup.MaximumValue = MaximumValue;
            setup.MinimumValue = MinimumValue;
            setup.Precision = Precision;
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
                Value = newValue.ToDecimal(Culture);

            base.OnValueChanged(newValue);
        }
    }
}
