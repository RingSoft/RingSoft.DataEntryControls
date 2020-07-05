using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public enum CalculatorButtons
    {
        Equals = 0,
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4,
        CButton = 5,
        CeButton = 6,
        Backspace = 7
    }

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

        private CalculatorOperators? _lastOperator;
        private decimal _currentValue;
        private decimal? _valueAtOperator;
        private decimal? _initialValue;
        private decimal? _valueAtEquals;
        private bool _equalsProcessed;

        public CalculatorProcessor(ICalculatorControl control)
        {
            Control = control;
        }

        public void InitializeValue(decimal? value)
        {
            if (ComittedValue != value)
            {
                Reset();
                SetEqualsValue(value);
            }
        }

        private void Reset()
        {
            _currentValue = 0;
            _valueAtEquals = _valueAtOperator = null;
            _lastOperator = null;
            Control.TapeText = string.Empty;
            _equalsProcessed = false;
        }

        private void SetEntryText(decimal? value)
        {
            Control.EntryText = FormatValue(value);
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

        public void ProcessButton(CalculatorButtons button)
        {
            switch (button)
            {
                case CalculatorButtons.CButton:
                    break;
                case CalculatorButtons.CeButton:
                    ProcessCeButton();
                    break;
                case CalculatorButtons.Backspace:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        private void ProcessDigit(string digit)
        {
            var newText = digit;
            if (_valueAtOperator == null && _initialValue == null)
                newText = Control.EntryText + digit;
            else
                _valueAtOperator = null;

            var number = newText.ToDecimal();
            ProcessEntryValue(number);
        }

        private void ProcessEntryValue(decimal number)
        {
            _equalsProcessed = false;
            _valueAtOperator = null;
            if (_initialValue != null)
            {
                Control.TapeText = string.Empty;
                _initialValue = null;
            }

            SetEntryText(number);
        }

        private void ProcessDecimal()
        {
            if (!Control.EntryText.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                Control.EntryText += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        }

        public void ProcessCeButton()
        {
            if (_equalsProcessed)
                Control.TapeText = string.Empty;

            SetEntryText(0);
        }

        private void ProcessOperator(CalculatorOperators calculatorOperator)
        {
            var entryValue = Control.EntryText.ToDecimal();
            if (_equalsProcessed)
            {
                //Pressed operator button right after equals button
                Control.TapeText = string.Empty;
                _currentValue = entryValue;
                AddToTape(calculatorOperator);
            }
            else
            {
                AddToTape(calculatorOperator);

                if (_lastOperator == null)
                    _currentValue = entryValue;
                else
                {
                    PerformOperation(calculatorOperator, entryValue);
                }
            }

            _lastOperator = calculatorOperator;
            _valueAtOperator = entryValue;
            _valueAtEquals = null;
            _initialValue = null; //Once we press any operator, initial value should be reset so tape doesn't get cleared when entering digits.
        }

        private void ProcessEquals()
        {
            var lastOperator = CalculatorOperators.Equals;
            if (_lastOperator != null)
                lastOperator = (CalculatorOperators)_lastOperator;

            var entryValue = Control.EntryText.ToDecimal();

            if (_valueAtEquals != null)
            {
                Control.TapeText = string.Empty;
                _currentValue = entryValue;
                AddToTape(lastOperator);
                AddToTape(CalculatorOperators.Equals, _valueAtEquals);
                if (_valueAtEquals != null)
                {
                    var valueAtEquals = (decimal) _valueAtEquals;
                    PerformOperation(lastOperator, valueAtEquals);
                }
            }
            else
            {
                if (_lastOperator == null)
                    Control.TapeText = string.Empty;

                AddToTape(CalculatorOperators.Equals);
                PerformOperation(lastOperator, entryValue);

                if (lastOperator != CalculatorOperators.Equals)
                    _valueAtEquals = entryValue;
            }

            SetEqualsValue(_currentValue);
            _equalsProcessed = true;
        }

        private void SetEqualsValue(decimal? value)
        {
            var oldValue = ComittedValue;
            ComittedValue = _initialValue = value;
            SetEntryText(value);
            Control.OnValueChanged(oldValue, value);
        }


        private void PerformOperation(CalculatorOperators currentOperator, decimal entryValue)
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
                    _currentValue /= entryValue;
                    break;
                case CalculatorOperators.Equals:
                    _currentValue = entryValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_lastOperator), _lastOperator, null);
            }

            SetEntryText(_currentValue);
        }

        private void AddToTape(CalculatorOperators calculatorOperator, decimal? entryValue = null)
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

            Control.TapeText += $" {entryText} {operatorText}";
        }

        public void ProcessCButton()
        {
            Reset();
            SetEqualsValue(0);
        }
    }
}
