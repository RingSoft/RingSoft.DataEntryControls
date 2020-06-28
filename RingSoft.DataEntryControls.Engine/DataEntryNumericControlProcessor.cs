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

        public decimal Value { get; private set; }

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
                    OnBackspaceKeyDown(setup);
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
                    return ProcessNumberDigit(keyChar);
            }

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
            return numberText.IndexOf(_setup.Culture.NumberFormat.CurrencyDecimalSeparator, StringComparison.Ordinal);
        }

        protected virtual ProcessCharResults ProcessNonNumericChar(char keyChar)
        {
            return ProcessCharResults.ValidationFailed;
        }

        protected virtual ProcessCharResults ProcessNumberDigit(char numberChar)
        {
            if (!ValidateNumber(numberChar))
                return ProcessCharResults.ValidationFailed;

            if (_setup.DataEntryMode != DataEntryModes.FormatOnEntry)
                return ProcessCharResults.Ignored;

            var numericTextProperties = GetNumericTextProperties();
            var newText = numericTextProperties.LeftText + numberChar + numericTextProperties.RightText;

            var oldCurrencyPosition =
                newText.IndexOf(_setup.Culture.NumberFormat.CurrencySymbol, StringComparison.Ordinal);
            var oldSymbolCount = CountNumberSymbols(newText);

            var newValue = newText = StripNonNumericCharacters(newText);

            newText = ProcessNewText(newText);

            var newCurrencyPosition =
                newText.IndexOf(_setup.Culture.NumberFormat.CurrencySymbol, StringComparison.Ordinal);
            var newSymbolCount = CountNumberSymbols(newText);

            var newSelectionStart = Control.SelectionStart + 1;
            var selectionIncrement = newSymbolCount - oldSymbolCount;
            if (selectionIncrement > 0)
                newSelectionStart += selectionIncrement;

            if (newCurrencyPosition == 0 && oldCurrencyPosition == -1)
                newSelectionStart++;

            Control.Text = newText;
            Control.SelectionStart = newSelectionStart;
            Control.SelectionLength = 0;
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

            if ((wholeNumberText + decimalText).IsNullOrEmpty())
                return string.Empty;

            var wholeNumber = (decimal)0;
            if (!wholeNumberText.IsNullOrEmpty())
                wholeNumber = decimal.Parse(wholeNumberText);

            switch (_setup.EditFormatType)
            {
                case NumericEditFormatTypes.Number:
                    wholeNumberText = wholeNumber.ToString("N0", _setup.Culture);
                    newText = wholeNumberText + decimalText;
                    break;
                case NumericEditFormatTypes.Currency:
                    wholeNumberText = wholeNumber.ToString("C0", _setup.Culture);
                    var currencySymbolPosition = wholeNumberText.IndexOf(_setup.Culture.NumberFormat.CurrencySymbol,
                        StringComparison.Ordinal);
                    var currencySymbol = string.Empty;

                    if (currencySymbolPosition > 0)
                    {
                        currencySymbol = _setup.Culture.NumberFormat.CurrencySymbol;
                        wholeNumberText = wholeNumberText.LeftStr(currencySymbolPosition);
                        if (wholeNumberText.EndsWith(" "))
                        {
                            currencySymbol = $" {currencySymbol}";
                            wholeNumberText = wholeNumberText.Trim();
                        }
                    }

                    newText = wholeNumberText + decimalText;

                    if (currencySymbolPosition > 0)
                        newText += currencySymbol;
                    break;
                case NumericEditFormatTypes.Percent:
                    var percentNumber = decimal.Parse(newText);
                    newText = percentNumber.ToString($"P{_setup.Precision}", _setup.Culture.NumberFormat);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return newText;
        }

        private bool ValidateNumber(char numberChar)
        {
            var numericText = GetNumericTextProperties();
            var newText = StripNonNumericCharacters(numericText.LeftText + numberChar + numericText.RightText);

            if (newText == "00")
                return false;

            return ValidateNewText(newText);
        }

        public virtual ProcessCharResults ProcessDecimal(DataEntryNumericEditSetup setup)
        {
            _setup = setup;
            var decimalString = _setup.Culture.NumberFormat.CurrencyDecimalSeparator;

            if (!ValidateDecimal(decimalString))
                return ProcessCharResults.ValidationFailed;

            var numericProperties = GetNumericTextProperties();
            var newText = numericProperties.LeftText + decimalString + numericProperties.RightText;

            if (_setup.DataEntryMode == DataEntryModes.ValidateOnly)
            {
                var valSelectionStart = Control.SelectionStart;
                Control.Text = newText;
                Control.SelectionStart = valSelectionStart + 1;
                return ProcessCharResults.Processed;
            }

            var oldCurrencyPosition =
                newText.IndexOf(_setup.Culture.NumberFormat.CurrencySymbol, StringComparison.Ordinal);

            var oldSymbolCount = CountNumberSymbols(newText);

            var selectionIncrement = 0;
            var newValue = newText = StripNonNumericCharacters(newText);
            if (newValue == decimalString.ToString())
            {
                newValue = newText = $"0{decimalString}";
                selectionIncrement++;
            }

            newText = ProcessNewText(newText);

            var newCurrencyPosition =
                newText.IndexOf(_setup.Culture.NumberFormat.CurrencySymbol, StringComparison.Ordinal);
            var newSymbolCount = CountNumberSymbols(newText);

            var newSelectionStart = Control.SelectionStart + 1;
            selectionIncrement += newSymbolCount - oldSymbolCount;
            newSelectionStart += selectionIncrement;

            if (newCurrencyPosition == 0 && oldCurrencyPosition == -1)
                newSelectionStart++;

            Control.Text = newText;
            Control.SelectionStart = newSelectionStart;
            Control.SelectionLength = 0;
            OnValueChanged(newValue);

            return ProcessCharResults.Processed;
        }

        public virtual string StripNonNumericCharacters(string formattedText)
        {
            return formattedText.NumTextToString(_setup.Culture);
        }

        public virtual int CountNumberSymbols(string formattedText)
        {
            var searchString = _setup.Culture.NumberFormat.NumberGroupSeparator;
            if (_setup.EditFormatType == NumericEditFormatTypes.Currency)
                searchString = _setup.Culture.NumberFormat.CurrencyGroupSeparator;

            return formattedText.CountTextForChars(searchString);
        }

        private bool ValidateDecimal(string decimalString)
        {
            if (_setup.Precision <= 0)
                return false;

            var numericText = GetNumericTextProperties();
            var checkNewText = numericText.LeftText + numericText.RightText;

            if (checkNewText.Contains(decimalString))
                return false;

            var newText = numericText.LeftText + decimalString + numericText.RightText;
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
                if (_setup.MaximumValue != null && newValue > _setup.MaximumValue)
                    return false;

                if (_setup.MinimumValue != null && newValue < _setup.MinimumValue)
                    return false;
            }

            return true;
        }

        public virtual void OnBackspaceKeyDown(DataEntryNumericEditSetup setup)
        {
            _setup = setup;
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
                var oldSymbolCount = CountNumberSymbols(newText);

                var newValue = newText = StripNonNumericCharacters(newText);

                if (newText.IsNullOrEmpty())
                {
                    newValue = "0";
                }
                else
                {
                    newText = ProcessNewText(newText);
                }

                var newSymbolCount = CountNumberSymbols(newText);

                var selectionStart = Control.SelectionStart;
                selectionStart -= oldSymbolCount - newSymbolCount;
                if (selectionStart < 0)
                    selectionStart = 0;

                Control.Text = newText;
                Control.SelectionStart = selectionStart;
                Control.SelectionLength = 0;

                OnValueChanged(newValue);
            }
        }

        public virtual void OnDeleteKeyDown(DataEntryNumericEditSetup setup)
        {
            _setup = setup;
        }

        public virtual void OnValueChanged(string newValue)
        {
            decimal.TryParse(newValue, out var decimalValue);
            Value = decimalValue;

            ValueChanged?.Invoke(this, new ValueChangedArgs(newValue));
        }
    }
}
