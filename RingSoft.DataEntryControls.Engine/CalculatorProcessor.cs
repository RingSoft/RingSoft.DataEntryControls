// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="CalculatorProcessor.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Enum CalculatorOperators
    /// </summary>
    public enum CalculatorOperators
    {
        /// <summary>
        /// The add
        /// </summary>
        Add = 0,
        /// <summary>
        /// The subtract
        /// </summary>
        Subtract = 1,
        /// <summary>
        /// The multiply
        /// </summary>
        Multiply =  2,
        /// <summary>
        /// The divide
        /// </summary>
        Divide = 3,
        /// <summary>
        /// The equals
        /// </summary>
        Equals = 4
    }

    /// <summary>
    /// Class CalculatorProcessor.
    /// </summary>
    public class CalculatorProcessor
    {
        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public ICalculatorControl Control { get; }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        /// <value>The precision.</value>
        public int Precision { get; set; } = -1;

        /// <summary>
        /// Gets the comitted value.
        /// </summary>
        /// <value>The comitted value.</value>
        public double? ComittedValue { get; private set; }

        /// <summary>
        /// The memory
        /// </summary>
        private double? _memory;

        /// <summary>
        /// Gets the memory.
        /// </summary>
        /// <value>The memory.</value>
        public double? Memory
        {
            get => _memory;
            private set
            {
                _memory = value;
                OnMemoryChanged();
            }
        }

        /// <summary>
        /// The calculation error
        /// </summary>
        private bool _calculationError;

        /// <summary>
        /// Gets a value indicating whether [calculation error].
        /// </summary>
        /// <value><c>true</c> if [calculation error]; otherwise, <c>false</c>.</value>
        public bool CalculationError
        {
            get => _calculationError;
            private set
            {
                _calculationError = value;
                OnCalculationStatusChanged();
            }
        }

        /// <summary>
        /// The last operator
        /// </summary>
        private CalculatorOperators? _lastOperator;
        /// <summary>
        /// The current value
        /// </summary>
        private double _currentValue;
        /// <summary>
        /// The value at operator
        /// </summary>
        private double? _valueAtOperator;
        /// <summary>
        /// The initial value
        /// </summary>
        private double? _initialValue;
        /// <summary>
        /// The value at equals
        /// </summary>
        private double? _valueAtEquals;
        /// <summary>
        /// The equals processed
        /// </summary>
        private bool _equalsProcessed;
        /// <summary>
        /// The entering data
        /// </summary>
        private bool _enteringData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorProcessor"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public CalculatorProcessor(ICalculatorControl control)
        {
            Control = control;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            OnMemoryChanged();
            Reset();
            SetEntryText(ComittedValue);
        }

        /// <summary>
        /// Reinitializes the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void ReinitializeValue(double? value)
        {
            if (ComittedValue != value)
            {
                Reset();
                SetEqualsValue(value);
                _enteringData = true;
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        private void Reset()
        {
            _currentValue = 0;
            _valueAtEquals = _valueAtOperator = null;
            _lastOperator = null;
            Control.EquationText = string.Empty;
            _equalsProcessed = false;
            CalculationError = false;
        }

        /// <summary>
        /// Sets the entry text.
        /// </summary>
        /// <param name="value">The value.</param>
        private void SetEntryText(double? value)
        {
            if (CalculationError)
                Reset();

            Control.EntryText = FormatValue(value);
        }

        /// <summary>
        /// Gets the entry value.
        /// </summary>
        /// <returns>System.Double.</returns>
        private double GetEntryValue()
        {
            double result = 0;
            if (!CalculationError)
                result = Control.EntryText.ToDecimal();

            return result;
        }

        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        private string FormatValue(double? value)
        {
            var text = "0";
            if (value != null)
            {
                var newValue = (double) value;
                var wholeNumberText = text = newValue.ToString(CultureInfo.CurrentCulture);
                var decimalIndex = text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                    StringComparison.Ordinal);
                var decimalText = string.Empty;
                if (decimalIndex >= 0)
                {
                    wholeNumberText = text.LeftStr(decimalIndex);
                    decimalText = text.GetRightText(decimalIndex, 1);
                }

                var wholeNumber = wholeNumberText.ToDecimal();
                text = wholeNumber.ToString("N0", CultureInfo.CurrentCulture);
                if (!decimalText.IsNullOrEmpty())
                    text += $".{decimalText}";
            }
            return text;
        }

        /// <summary>
        /// Processes the character.
        /// </summary>
        /// <param name="keyChar">The key character.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ProcessChar(char keyChar)
        {
            switch (keyChar)
            {
                case '0':
                    if (Control.EntryText != "0")
                        ProcessDigit(keyChar.ToString());

                    return true;
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ProcessDigit(keyChar.ToString());
                    return true;
                case '+':
                    ProcessOperator(CalculatorOperators.Add);
                    return true;
                case '-':
                    ProcessOperator(CalculatorOperators.Subtract);
                    return true;
                case '*':
                    ProcessOperator(CalculatorOperators.Multiply);
                    return true;
                case '/':
                    ProcessOperator(CalculatorOperators.Divide);
                    return true;
                case '=':
                    ProcessEquals();
                    return true;
            }

            if (keyChar.ToString() == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
            {
                ProcessDecimal();
                return true;
            }
            else if (keyChar.ToString().ToUpperInvariant() == "C")
            {
                ProcessCButton();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Processes the digit.
        /// </summary>
        /// <param name="digit">The digit.</param>
        private void ProcessDigit(string digit)
        {
            var newText = digit;
            if (!CalculationError && _valueAtOperator == null && _initialValue == null)
                newText = Control.EntryText + digit;
            else
                _valueAtOperator = null;

            var number = newText.ToDecimal();
            ProcessEntryValue(number);
            _enteringData = true;
        }

        /// <summary>
        /// Processes the entry value.
        /// </summary>
        /// <param name="number">The number.</param>
        private void ProcessEntryValue(double number)
        {
            PreProcessEntryValue();

            SetEntryText(number);
        }

        /// <summary>
        /// Pres the process entry value.
        /// </summary>
        private void PreProcessEntryValue()
        {
            if (_valueAtEquals == null)
                _equalsProcessed = false;//Otherwise it performs calculation after operator.

            _valueAtOperator = null;
            if (_initialValue != null)
            {
                Control.EquationText = string.Empty;
                _initialValue = null;
            }
        }

        /// <summary>
        /// Processes the decimal.
        /// </summary>
        public void ProcessDecimal()
        {
            var newText = $"0{NumberFormatInfo.CurrentInfo.NumberDecimalSeparator}";
            if (!CalculationError && _valueAtOperator == null && _initialValue == null)
            {
                if (Control.EntryText.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                    newText = string.Empty;
                else 
                    newText = Control.EntryText += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            }
            else
            {
                _valueAtOperator = null;
            }

            if (!newText.IsNullOrEmpty())
            {
                PreProcessEntryValue();

                if (CalculationError)
                    Reset();
                Control.EntryText = newText;
            }
            _enteringData = true;
        }

        /// <summary>
        /// Processes the ce button.
        /// </summary>
        public void ProcessCeButton()
        {
            if (_equalsProcessed)
                Control.EquationText = string.Empty;

            SetEntryText(0);
        }

        /// <summary>
        /// Processes the operator.
        /// </summary>
        /// <param name="calculatorOperator">The calculator operator.</param>
        private void ProcessOperator(CalculatorOperators calculatorOperator)
        {
            if (CalculationError)
                return;

            _enteringData = false;

            var entryValue = GetEntryValue();
            if (_equalsProcessed)
            {
                //Pressed operator button right after equals button
                Control.EquationText = string.Empty;
                _currentValue = entryValue;
                AddToEquationText(calculatorOperator);
            }
            else
            {
                AddToEquationText(calculatorOperator);

                if (_lastOperator == null)
                    _currentValue = entryValue;
                else
                {
                    var lastOperator = (CalculatorOperators) _lastOperator;
                    if (!PerformOperation(lastOperator, entryValue))
                        return;
                }
            }

            _lastOperator = calculatorOperator;
            _valueAtOperator = entryValue;
            _valueAtEquals = null;
            _initialValue = null; //Once we press any operator, initial value should be reset so tape doesn't get cleared when entering digits.
        }

        /// <summary>
        /// Processes the equals.
        /// </summary>
        private void ProcessEquals()
        {
            if (CalculationError)
            {
                SetEqualsValue(0);
                Reset();
                return;
            }
            _enteringData = false;

            var lastOperator = CalculatorOperators.Equals;
            if (_lastOperator != null)
                lastOperator = (CalculatorOperators)_lastOperator;

            var entryValue = GetEntryValue();

            if (_valueAtEquals != null)
            {
                Control.EquationText = string.Empty;
                _currentValue = entryValue;
                AddToEquationText(lastOperator);
                AddToEquationText(CalculatorOperators.Equals, _valueAtEquals);
                if (_valueAtEquals != null)
                {
                    var valueAtEquals = (double) _valueAtEquals;
                    if (!PerformOperation(lastOperator, valueAtEquals))
                        return;
                }
            }
            else
            {
                if (_lastOperator == null)
                    Control.EquationText = string.Empty;

                AddToEquationText(CalculatorOperators.Equals);
                if (!PerformOperation(lastOperator, entryValue))
                    return;

                if (lastOperator != CalculatorOperators.Equals)
                    _valueAtEquals = entryValue;
            }

            SetEqualsValue(_currentValue);
            _equalsProcessed = true;
        }

        /// <summary>
        /// Sets the equals value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void SetEqualsValue(double? value)
        {
            if (Precision >= 0 && value != null)
            {
                var newValue = (double)value;
                var text = newValue.ToString(CultureInfo.CurrentCulture);
                var decimalIndex = text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                    StringComparison.Ordinal);
                if (decimalIndex >= 0)
                {
                    var decimalText = text.GetRightText(decimalIndex, 1);
                    if (decimalText.Length > Precision)
                        value = Math.Round(newValue, Precision);
                }
            }
            var oldValue = ComittedValue;
            ComittedValue = _initialValue = value;
            SetEntryText(value);
            Control.OnValueChanged(oldValue, value);
        }


        /// <summary>
        /// Performs the operation.
        /// </summary>
        /// <param name="currentOperator">The current operator.</param>
        /// <param name="entryValue">The entry value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">_lastOperator - null</exception>
        private bool PerformOperation(CalculatorOperators currentOperator, double entryValue)
        {
            switch (currentOperator)
            {
                case CalculatorOperators.Add:
                    _currentValue += entryValue;
                    break;
                case CalculatorOperators.Subtract:
                    _currentValue -= entryValue;
                    break;
                case CalculatorOperators.Multiply:
                    _currentValue *= entryValue;
                    break;
                case CalculatorOperators.Divide:
                    if (entryValue.Equals(0))
                    {
                        CalculationError = true;
                        Control.EntryText = "Div / 0!";
                        return false;
                    }
                    _currentValue /= entryValue;
                    break;
                case CalculatorOperators.Equals:
                    _currentValue = entryValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_lastOperator), _lastOperator, null);
            }

            SetEntryText(_currentValue);
            return true;
        }

        /// <summary>
        /// Adds to equation text.
        /// </summary>
        /// <param name="calculatorOperator">The calculator operator.</param>
        /// <param name="entryValue">The entry value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">calculatorOperator - null</exception>
        private void AddToEquationText(CalculatorOperators calculatorOperator, double? entryValue = null)
        {
            var entryText = Control.EntryText;
            if (entryValue != null)
            {
                entryText = FormatValue(entryValue);
            }
            string operatorText;

            switch (calculatorOperator)
            {
                case CalculatorOperators.Add:
                    operatorText = "+";
                    break;
                case CalculatorOperators.Subtract:
                    operatorText = "-";
                    break;
                case CalculatorOperators.Multiply:
                    operatorText = "X";
                    break;
                case CalculatorOperators.Divide:
                    operatorText = "/";
                    break;
                case CalculatorOperators.Equals:
                    operatorText = "=";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculatorOperator), calculatorOperator, null);
            }

            Control.EquationText += $" {entryText} {operatorText}";
        }

        /// <summary>
        /// Processes the c button.
        /// </summary>
        public void ProcessCButton()
        {
            Reset();
            SetEqualsValue(0);
        }

        /// <summary>
        /// Processes the plus minus button.
        /// </summary>
        public void ProcessPlusMinusButton()
        {
            var entryValue = GetEntryValue();
            entryValue *= -1;
            SetEntryText(entryValue);
        }

        /// <summary>
        /// Processes the memory store.
        /// </summary>
        public void ProcessMemoryStore()
        {
            Memory = GetEntryValue();
        }

        /// <summary>
        /// Processes the memory clear.
        /// </summary>
        public void ProcessMemoryClear()
        {
            Memory = null;
        }

        /// <summary>
        /// Processes the memory recall.
        /// </summary>
        public void ProcessMemoryRecall()
        {
            if (Memory != null)
            {
                var memory = (double) Memory;
                ProcessEntryValue(memory);
                _initialValue = memory;
            }
        }

        /// <summary>
        /// Processes the memory add.
        /// </summary>
        public void ProcessMemoryAdd()
        {
            double newMemory = 0;
            if (Memory != null)
                newMemory = (double) Memory;

            newMemory += GetEntryValue();
            Memory = newMemory;
        }

        /// <summary>
        /// Processes the memory subtract.
        /// </summary>
        public void ProcessMemorySubtract()
        {
            double newMemory = 0;
            if (Memory != null)
                newMemory = (double)Memory;

            newMemory -= GetEntryValue();
            Memory = newMemory;
        }

        /// <summary>
        /// Called when [memory changed].
        /// </summary>
        private void OnMemoryChanged()
        {
            Control.MemoryStatusVisible = Memory != null;

            Control.MemoryRecallEnabled = Control.MemoryClearEnabled = Memory != null;
        }

        /// <summary>
        /// Called when [calculation status changed].
        /// </summary>
        private void OnCalculationStatusChanged()
        {
            Control.MemoryPlusEnabled = Control.MemoryMinusEnabled = Control.MemoryStoreEnabled = !CalculationError;
        }

        /// <summary>
        /// Processes the backspace.
        /// </summary>
        public void ProcessBackspace()
        {
            if (_valueAtEquals != null)
            {
                Control.EquationText = string.Empty;
                return;
            }

            if (CalculationError)
            {
                SetEntryText(0);
                return;
            }

            if (_enteringData)
            {
                if (Control.EntryText != "0")
                {
                    var decimalIndex = Control.EntryText.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                        StringComparison.Ordinal);

                    if (decimalIndex < 0)
                    {
                        var newValue = GetEntryValue();
                        var wholeText = newValue.ToString(CultureInfo.InvariantCulture);
                        wholeText = wholeText.LeftStr(wholeText.Length - 1);
                        newValue = wholeText.ToDecimal();
                        SetEntryText(newValue);
                    }
                    else
                    {
                        var newText = Control.EntryText.LeftStr(Control.EntryText.Length - 1);
                        Control.EntryText = newText;
                    }
                }
            }
        }
    }
}
