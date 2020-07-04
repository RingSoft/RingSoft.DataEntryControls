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

        private CalculatorOperators? _lastOperator;
        private decimal _currentValue;
        private decimal? _valueAtOperator;

        public CalculatorProcessor(ICalculatorControl control)
        {
            Control = control;
        }

        public void SetValue(decimal? value)
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

            Control.EntryText = text;
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
                    ProcessOperator(CalculatorOperators.Equals);
                    return true;
            }

            if (keyChar.ToString() == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
            {
                ProcessDecimal();
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
                    ProcessCe();
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
            if (_valueAtOperator == null)
                newText = Control.EntryText + digit;
            else
                _valueAtOperator = null;

            var number = newText.ToDecimal();
            SetValue(number);
        }

        private void ProcessDecimal()
        {
            if (!Control.EntryText.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                Control.EntryText += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        }

        private void ProcessCe()
        {
            _currentValue = 0;
            SetValue(_currentValue);
        }

        private void ProcessOperator(CalculatorOperators calculatorOperator)
        {
            AddToTape(calculatorOperator);
            var entryValue = Control.EntryText.ToDecimal();

            if (_lastOperator == null)
                _currentValue = entryValue;
            else 
            {
                PerformOperation(calculatorOperator, entryValue);
            }

            if (calculatorOperator == CalculatorOperators.Equals)
            {
                Control.Value = _currentValue;
            }
            else
            {
                _lastOperator = calculatorOperator;
            }

            _valueAtOperator = entryValue;
        }

        private void PerformOperation(CalculatorOperators currentOperator, decimal entryValue)
        {
            switch (_lastOperator)
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_lastOperator), _lastOperator, null);
            }

            if (currentOperator != CalculatorOperators.Equals)
                SetValue(_currentValue);
        }

        private void AddToTape(CalculatorOperators calculatorOperator)
        {
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

            Control.TapeText += $" {Control.EntryText} {operatorText}";
        }
    }
}
