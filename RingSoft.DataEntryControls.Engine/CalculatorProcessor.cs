using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public enum CalculatorOperators
    {
        Add = 0,
        Subtract = 1,
        Multiply =  2,
        Divide = 3,
        Equals = 4
    }

    public class CalculatorProcessor
    {
        public ICalculatorControl Control { get; }

        public int Precision { get; set; } = -1;

        public decimal? ComittedValue { get; private set; }

        private decimal? _memory;

        public decimal? Memory
        {
            get => _memory;
            private set
            {
                _memory = value;
                OnMemoryChanged();
            }
        }

        private bool _calculationError;

        public bool CalculationError
        {
            get => _calculationError;
            private set
            {
                _calculationError = value;
                OnCalculationStatusChanged();
            }
        }

        private CalculatorOperators? _lastOperator;
        private decimal _currentValue;
        private decimal? _valueAtOperator;
        private decimal? _initialValue;
        private decimal? _valueAtEquals;
        private bool _equalsProcessed;
        private bool _enteringData;

        public CalculatorProcessor(ICalculatorControl control)
        {
            Control = control;
        }

        public void Initialize()
        {
            OnMemoryChanged();
            Reset();
            SetEntryText(ComittedValue);
        }

        public void ReinitializeValue(decimal? value)
        {
            if (ComittedValue != value)
            {
                Reset();
                SetEqualsValue(value);
                _enteringData = true;
            }
        }

        private void Reset()
        {
            _currentValue = 0;
            _valueAtEquals = _valueAtOperator = null;
            _lastOperator = null;
            Control.EquationText = string.Empty;
            _equalsProcessed = false;
            CalculationError = false;
        }

        private void SetEntryText(decimal? value)
        {
            if (CalculationError)
                Reset();

            Control.EntryText = FormatValue(value);
        }

        private decimal GetEntryValue()
        {
            decimal result = 0;
            if (!CalculationError)
                result = Control.EntryText.ToDecimal();

            return result;
        }

        private string FormatValue(decimal? value)
        {
            var text = "0";
            if (value != null)
            {
                var newValue = (decimal) value;
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

        private void ProcessEntryValue(decimal number)
        {
            PreProcessEntryValue();

            SetEntryText(number);
        }

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

        public void ProcessCeButton()
        {
            if (_equalsProcessed)
                Control.EquationText = string.Empty;

            SetEntryText(0);
        }

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
                    var valueAtEquals = (decimal) _valueAtEquals;
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

        private void SetEqualsValue(decimal? value)
        {
            if (Precision >= 0 && value != null)
            {
                var newValue = (decimal)value;
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


        private bool PerformOperation(CalculatorOperators currentOperator, decimal entryValue)
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

        private void AddToEquationText(CalculatorOperators calculatorOperator, decimal? entryValue = null)
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

        public void ProcessCButton()
        {
            Reset();
            SetEqualsValue(0);
        }

        public void ProcessPlusMinusButton()
        {
            var entryValue = GetEntryValue();
            entryValue *= -1;
            SetEntryText(entryValue);
        }

        public void ProcessMemoryStore()
        {
            Memory = GetEntryValue();
        }

        public void ProcessMemoryClear()
        {
            Memory = null;
        }

        public void ProcessMemoryRecall()
        {
            if (Memory != null)
            {
                var memory = (decimal) Memory;
                ProcessEntryValue(memory);
                _initialValue = memory;
            }
        }

        public void ProcessMemoryAdd()
        {
            decimal newMemory = 0;
            if (Memory != null)
                newMemory = (decimal) Memory;

            newMemory += GetEntryValue();
            Memory = newMemory;
        }

        public void ProcessMemorySubtract()
        {
            decimal newMemory = 0;
            if (Memory != null)
                newMemory = (decimal)Memory;

            newMemory -= GetEntryValue();
            Memory = newMemory;
        }

        private void OnMemoryChanged()
        {
            Control.MemoryStatusVisible = Memory != null;

            Control.MemoryRecallEnabled = Control.MemoryClearEnabled = Memory != null;
        }

        private void OnCalculationStatusChanged()
        {
            Control.MemoryPlusEnabled = Control.MemoryMinusEnabled = Control.MemoryStoreEnabled = !CalculationError;
        }

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
