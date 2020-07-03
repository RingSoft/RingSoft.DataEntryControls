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
        Divide = 3
    }

    public class CalculatorProcessor
    {
        public ICalculatorControl Control { get; }

        public int Precision { get; set; } = -1;

        private CalculatorOperators? _lastOperator;
        private decimal _currentValue;

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
                case CalculatorButtons.Equals:
                    break;
                case CalculatorButtons.Add:
                    break;
                case CalculatorButtons.Subtract:
                    break;
                case CalculatorButtons.Multiply:
                    break;
                case CalculatorButtons.Divide:
                    break;
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
            var newText = Control.EntryText + digit;
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
            SetValue(0);
        }

        private void ProcessOperator(CalculatorOperators calculatorOperator)
        {
            AddToTape(calculatorOperator);

            if (_lastOperator == null)
            {
                _lastOperator = calculatorOperator;
                return;
            }

            var entryValue = Control.EntryText.ToDecimal();
            switch (calculatorOperator)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculatorOperator), calculatorOperator, null);
            }

            SetValue(_currentValue);
        }

        private void AddToTape(CalculatorOperators calculatorOperator)
        {
            var operatorText = string.Empty;

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculatorOperator), calculatorOperator, null);
            }

            Control.TapeText += $" {Control.EntryText} {operatorText}";
        }
    }
}
