using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public enum ProcessCharResults
    {
        Processed = 0,
        Ignored = 1,
        ValidationFailed = 2
    }

    public class ValueChangedArgs
    {
        public string NewValue { get; }

        public ValueChangedArgs(string newValue)
        {
            NewValue = newValue;
        }
    }

    public class NumericTextProperties
    {
        public string LeftText { get; set; }
        public string SelectedText { get; set; }
        public string RightText { get; set; }
        public int DecimalPosition { get; set; } = -1;
    }

    public class DataEntryNumericControlProcessor
    {
        public INumericControl Control { get; }

        public event EventHandler<ValueChangedArgs> ValueChanged;

        private DataEntryNumericEditSetup _setup;

        public DataEntryNumericControlProcessor(INumericControl control)
        {
            Control = control;
        }

        public ProcessCharResults ProcessChar(DataEntryNumericEditSetup setup, char keyChar)
        {
            var stringChar = keyChar.ToString();
            _setup = setup;
            switch (keyChar)
            {
                case '\b':
                    OnBackspaceKeyDown();
                    return ProcessCharResults.Processed;
                case '\u001b':  //Escape
                case '\t':
                case '\r':
                case '\n':
                    return ProcessCharResults.Ignored;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ProcessNumber(keyChar);
            }

            if (stringChar == NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator)
                return ProcessDecimal(keyChar);

            return ProcessNonNumericChar(keyChar);
        }

        protected NumericTextProperties GetNumericTextProperties()
        {
            return new NumericTextProperties()
            {
                LeftText = Control.Text.LeftStr(Control.SelectionStart),
                SelectedText = Control.Text.MidStr(Control.SelectionStart, Control.SelectionLength),
                RightText = Control.Text.GetRightText(Control.SelectionStart, Control.SelectionLength)
            };
        }

        private int GetDecimalPosition(string numberText)
        {
            return numberText.IndexOf(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator, StringComparison.Ordinal);
        }

        protected virtual ProcessCharResults ProcessNonNumericChar(char keyChar)
        {
            return ProcessCharResults.ValidationFailed;
        }

        protected virtual ProcessCharResults ProcessNumber(char numberChar)
        {
            if (!ValidateNumber(numberChar))
                return ProcessCharResults.ValidationFailed;

            var numericTextProperties = GetNumericTextProperties();
            var newText = numericTextProperties.LeftText + numberChar + numericTextProperties.RightText;

            var oldCurrencyPosition =
                newText.IndexOf(NumberFormatInfo.CurrentInfo.CurrencySymbol, StringComparison.Ordinal);
            var oldSymbolCount = CountNumberSymbols(newText);

            var newValue = newText = StripNonNumericCharacters(newText);

            newText = ProcessNewText(newText);

            var newCurrencyPosition =
                newText.IndexOf(NumberFormatInfo.CurrentInfo.CurrencySymbol, StringComparison.Ordinal);
            var newSymbolCount = CountNumberSymbols(newText);

            var newSelectionStart = Control.SelectionStart + 1;
            var selectionIncrement = newSymbolCount - oldSymbolCount;
            if (selectionIncrement > 0)
                newSelectionStart += selectionIncrement;

            if (newCurrencyPosition == 0 && oldCurrencyPosition == -1)
                newSelectionStart++;

            Control.Text = newText;
            Control.SelectionStart = newSelectionStart;
            OnValueChanged(newValue);

            return ProcessCharResults.Processed;
        }

        private string ProcessNewText(string newText)
        {
            var wholeNumberText = newText;
            var decimalText = string.Empty;

            var decimalPosition = GetDecimalPosition(newText);
            if (decimalPosition >= 0)
            {
                wholeNumberText = newText.LeftStr(decimalPosition);
                decimalText = newText.GetRightText(decimalPosition, 0);
            }

            var wholeNumber = decimal.Parse(wholeNumberText);
            switch (_setup.EditFormatType)
            {
                case NumericEditFormatTypes.Number:
                    wholeNumberText = wholeNumber.ToString("N0");
                    newText = wholeNumberText + decimalText;
                    break;
                case NumericEditFormatTypes.Currency:
                    wholeNumberText = FormatLeftOfDecimalCurrency(wholeNumber);
                    newText = wholeNumberText + decimalText;
                    newText = AddCurrencySymbolToRightOfNumberText(newText);
                    break;
                case NumericEditFormatTypes.Percent:
                    var percentNumber = decimal.Parse(newText);
                    newText = percentNumber.ToString($"P{_setup.Precision}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return newText;
        }

        protected virtual string FormatLeftOfDecimalCurrency(decimal number)
        {
            return number.ToString("C0");
        }

        protected virtual string AddCurrencySymbolToRightOfNumberText(string numberText)
        {
            return numberText;
        }

        private bool ValidateNumber(char numberChar)
        {
            var numericText = GetNumericTextProperties();
            var newText = StripNonNumericCharacters(numericText.LeftText + numberChar + numericText.RightText);

            if (newText == "00")
                return false;

            return ValidateNewText(newText);
        }

        protected virtual ProcessCharResults ProcessDecimal(char decimalChar)
        {
            if (!ValidateDecimal(decimalChar))
                return ProcessCharResults.ValidationFailed;

            var numericProperties = GetNumericTextProperties();
            var newText = numericProperties.LeftText + decimalChar + numericProperties.RightText;
            var oldCurrencyPosition =
                newText.IndexOf(NumberFormatInfo.CurrentInfo.CurrencySymbol, StringComparison.Ordinal);
            var oldSymbolCount = CountNumberSymbols(newText);

            var selectionIncrement = 0;
            var newValue = newText = StripNonNumericCharacters(newText);
            if (newValue == decimalChar.ToString())
            {
                newValue = newText = $"0{decimalChar}";
                selectionIncrement++;
            }

            newText = ProcessNewText(newText);

            var newCurrencyPosition =
                newText.IndexOf(NumberFormatInfo.CurrentInfo.CurrencySymbol, StringComparison.Ordinal);
            var newSymbolCount = CountNumberSymbols(newText);

            var newSelectionStart = Control.SelectionStart + 1;
            selectionIncrement += newSymbolCount - oldSymbolCount;
            newSelectionStart += selectionIncrement;

            if (newCurrencyPosition == 0 && oldCurrencyPosition == -1)
                newSelectionStart++;

            Control.Text = newText;
            Control.SelectionStart = newSelectionStart;
            OnValueChanged(newValue);

            return ProcessCharResults.Processed;
        }

        public virtual string StripNonNumericCharacters(string formattedText)
        {
            return formattedText.NumTextToString();
        }

        public virtual int CountNumberSymbols(string formattedText)
        {
            var searchString = NumberFormatInfo.CurrentInfo.NumberGroupSeparator;
            if (_setup.EditFormatType == NumericEditFormatTypes.Currency)
                searchString = NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator;

            return formattedText.CountTextForChars(searchString);
        }

        private bool ValidateDecimal(char decimalChar)
        {
            if (_setup.Precision <= 0)
                return false;

            var numericText = GetNumericTextProperties();
            var checkNewText = numericText.LeftText + numericText.RightText;

            if (checkNewText.Contains(decimalChar.ToString()))
                return false;

            var newText = numericText.LeftText + decimalChar.ToString() + numericText.RightText;
            return ValidateNewText(newText);
        }

        private bool ValidateNewText(string newText)
        {
            newText = StripNonNumericCharacters(newText);
            var decimalPosition = GetDecimalPosition(newText);
            if (decimalPosition >= 0)
            {
                var decimalText = newText.GetRightText(decimalPosition + 1, 0);
                if (decimalText.Length > _setup.Precision)
                    return false;
            }

            if (decimal.TryParse(newText, out var newValue))
            {
                if (_setup.MaximumValue > 0 && newValue > _setup.MaximumValue)
                    return false;

                if (_setup.MinimumValue > 0 && newValue < _setup.MinimumValue)
                    return false;
            }

            return true;
        }

        public virtual void OnBackspaceKeyDown()
        {
            if (Control.SelectionStart > 0)
            {
                if (Control.SelectionLength == 0)
                {
                    Control.SelectionStart--;
                    Control.SelectionLength = 1;
                }
            }

            if (Control.SelectionLength > 0)
            {
                var numericTextProperties = GetNumericTextProperties();
                var newText = numericTextProperties.LeftText + numericTextProperties.RightText;
                var newValue = newText = StripNonNumericCharacters(newText);

                if (newText.IsNullOrEmpty())
                {
                    newValue = "0";
                }
                else
                {
                    newText = ProcessNewText(newText);
                }

                var selectionStart = Control.SelectionStart;
                Control.Text = newText;
                Control.SelectionStart = selectionStart;
                Control.SelectionLength = 0;

                OnValueChanged(newValue);
            }
        }

        public virtual void OnDeleteKeyDown()
        {

        }

        public virtual void OnValueChanged(string newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedArgs(newValue));
        }
    }
}
