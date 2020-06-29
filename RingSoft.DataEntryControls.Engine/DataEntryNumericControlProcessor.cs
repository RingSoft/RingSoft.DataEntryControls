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
        public bool SymbolExists { get; set; }
        public NumberSymbolLocations SymbolLocation { get; set; }
        public string SymbolText { get; set; }
        public int GroupSeparatorCount { get; set; }
        public string NewWholeNumberText { get; set; }
        public string NewDecimalText { get; set; }
        public int NegativeSignIndex { get; set; } = -1;
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

        public string FormatTextForEntry(DataEntryNumericEditSetup setup, string controlText)
        {
            _setup = setup;
            var value = controlText.ToDecimal(_setup.Culture);
            
            switch (_setup.DataEntryMode)
            {
                case DataEntryModes.FormatOnEntry:
                    break;
                case DataEntryModes.ValidateOnly:
                case DataEntryModes.RawEntry:
                    return value.ToString(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var decimalText = value.ToString(_setup.Culture);
            var numericTextProperties =
                GetNumericPropertiesForText(decimalText, decimalText.Length, 0, string.Empty);

            var newText = GetFormattedText(numericTextProperties);
            return newText;
        }

        protected NumericTextProperties GetNumericTextProperties(string charText)
        {
            return GetNumericPropertiesForText(Control.Text, Control.SelectionStart, Control.SelectionLength, charText);
        }

        private NumericTextProperties GetNumericPropertiesForText(string controlText, int selectionStart,
            int selectionLength, string charText)
        {
            var result = new NumericTextProperties()
            {
                LeftText = controlText.LeftStr(selectionStart),
                SelectedText = controlText.MidStr(selectionStart, selectionLength),
                RightText = controlText.GetRightText(selectionStart, selectionLength)
            };

            var newText = result.LeftText + charText + result.RightText;
            switch (_setup.EditFormatType)
            {
                case NumericEditFormatTypes.Currency:
                    result.SymbolText = _setup.CurrencyText;
                    result.SymbolLocation = _setup.CurrencySymbolLocation;
                    result.SymbolExists = newText.IndexOf(result.SymbolText, StringComparison.Ordinal) >= 0;
                    break;
                case NumericEditFormatTypes.Number:
                    break;
                case NumericEditFormatTypes.Percent:
                    result.SymbolText = _setup.PercentText;
                    result.SymbolLocation = _setup.PercentSymbolLocation;
                    result.SymbolExists = newText.IndexOf(result.SymbolText, StringComparison.Ordinal) >= 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var selectedTextDecimalPosition = GetDecimalPosition(result.SelectedText);
            if (selectedTextDecimalPosition < 0)
                result.DecimalPosition = GetDecimalPosition(controlText);

            result.GroupSeparatorCount = CountNumberGroupSeparators(newText);

            newText = StripNonNumericCharacters(newText);
            result.NegativeSignIndex = newText.IndexOf('-');

            result.NewWholeNumberText = newText;

            var newDecimalPosition = GetDecimalPosition(newText);
            if (newDecimalPosition >= 0)
            {
                result.NewWholeNumberText = newText.LeftStr(newDecimalPosition);
                result.NewDecimalText = newText.GetRightText(newDecimalPosition, 1);
            }
            return result;
        }

        private int GetDecimalPosition(string numberText)
        {
            switch (_setup.EditFormatType)
            {
                case NumericEditFormatTypes.Currency:
                    return numberText.IndexOf(_setup.Culture.NumberFormat.CurrencyDecimalSeparator,
                        StringComparison.Ordinal);
                case NumericEditFormatTypes.Number:
                case NumericEditFormatTypes.Percent:
                    return numberText.IndexOf(_setup.Culture.NumberFormat.NumberDecimalSeparator,
                        StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual ProcessCharResults ProcessNonNumericChar(char keyChar)
        {
            return ProcessCharResults.ValidationFailed;
        }

        protected virtual ProcessCharResults ProcessNumberDigit(char numberChar)
        {
            var numericTextProperties = GetNumericTextProperties(numberChar.ToString());
            if (!ValidateNumber(numericTextProperties))
                return ProcessCharResults.ValidationFailed;

            if (_setup.DataEntryMode != DataEntryModes.FormatOnEntry)
                return ProcessCharResults.Ignored;

            var newText = GetFormattedText(numericTextProperties);

            var newGroupSeparatorCount = CountNumberGroupSeparators(newText);

            var newSelectionStart = Control.SelectionStart + 1;
            var selectionIncrement = newGroupSeparatorCount - numericTextProperties.GroupSeparatorCount;
            if (selectionIncrement > 0)
                newSelectionStart += selectionIncrement;

            switch (numericTextProperties.SymbolLocation)
            {
                case NumberSymbolLocations.Prefix:
                    if (!numericTextProperties.SymbolExists && !numericTextProperties.SymbolText.IsNullOrEmpty())
                        newSelectionStart += numericTextProperties.SymbolText.Length;
                    break;
                case NumberSymbolLocations.Suffix:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Control.Text = newText;
            Control.SelectionStart = newSelectionStart;
            Control.SelectionLength = 0;
            OnValueChanged(GetNewValue(numericTextProperties));

            return ProcessCharResults.Processed;
        }

        public string GetNewValue(NumericTextProperties numericTextProperties)
        {
            var result = numericTextProperties.NewWholeNumberText;
            if (!numericTextProperties.NewDecimalText.IsNullOrEmpty())
            {
                var decimalPoint = _setup.Culture.NumberFormat.CurrencyDecimalSeparator;
                switch (_setup.EditFormatType)
                {
                    case NumericEditFormatTypes.Currency:
                        break;
                    case NumericEditFormatTypes.Number:
                    case NumericEditFormatTypes.Percent:
                        decimalPoint = _setup.Culture.NumberFormat.NumberDecimalSeparator;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                result = $"{result}{decimalPoint}{numericTextProperties.NewDecimalText}";
            }

            return result;
        }

        private string GetFormattedText(NumericTextProperties numericTextProperties)
        {
            var result = string.Empty;
            var wholeNumberText = numericTextProperties.NewWholeNumberText;
            var decimalText = numericTextProperties.NewDecimalText;

            if ((wholeNumberText + decimalText).IsNullOrEmpty())
                return result;

            var wholeNumber = wholeNumberText.ToDecimal(_setup.Culture);
            var isNegative = wholeNumber < 0;
            if (isNegative)
                wholeNumber *= -1;

            switch (_setup.EditFormatType)
            {
                case NumericEditFormatTypes.Number:
                    wholeNumberText = wholeNumber.ToString("N0", _setup.Culture);
                    if (!decimalText.IsNullOrEmpty())
                        decimalText = $"{_setup.Culture.NumberFormat.NumberDecimalSeparator}{decimalText}";
                    break;
                case NumericEditFormatTypes.Currency:
                    wholeNumberText = wholeNumber.ToString("C0", _setup.Culture);
                    if (!decimalText.IsNullOrEmpty())
                        decimalText = $"{_setup.Culture.NumberFormat.CurrencyDecimalSeparator}{decimalText}";
                    break;
                case NumericEditFormatTypes.Percent:
                    var percentNumber = decimal.Parse(numericTextProperties.NewWholeNumberText);
                    percentNumber = percentNumber / 100;
                    wholeNumberText = percentNumber.ToString("P0", _setup.Culture.NumberFormat);
                    if (!decimalText.IsNullOrEmpty())
                        decimalText = $"{_setup.Culture.NumberFormat.NumberDecimalSeparator}{decimalText}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var symbolPosition = -1;
            if (!numericTextProperties.SymbolText.IsNullOrEmpty())
                symbolPosition = wholeNumberText.IndexOf(numericTextProperties.SymbolText,
                    StringComparison.Ordinal);

            if (symbolPosition > 0)
                wholeNumberText = wholeNumberText.LeftStr(symbolPosition);

            if (isNegative)
                wholeNumberText = $"-{wholeNumberText}";

            result = wholeNumberText + decimalText;

            if (symbolPosition > 0)
                result += numericTextProperties.SymbolText;

            return result;
        }

        private bool ValidateNumber(NumericTextProperties numericTextProperties)
        {
            if (numericTextProperties.NewWholeNumberText == "00")
                return false;

            return ValidateNewText(numericTextProperties);
        }

        public virtual ProcessCharResults ProcessDecimal(DataEntryNumericEditSetup setup)
        {
            _setup = setup;
            var decimalString = _setup.Culture.NumberFormat.CurrencyDecimalSeparator;
            var numericProperties = GetNumericTextProperties(decimalString);

            if (!ValidateDecimal(numericProperties))
                return ProcessCharResults.ValidationFailed;

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

            var oldSymbolCount = CountNumberGroupSeparators(newText);

            var selectionIncrement = 0;
            var newValue = newText = StripNonNumericCharacters(newText);
            if (newValue == decimalString.ToString())
            {
                newValue = newText = $"0{decimalString}";
                selectionIncrement++;
            }

            newText = GetFormattedText(numericProperties);

            var newCurrencyPosition =
                newText.IndexOf(_setup.Culture.NumberFormat.CurrencySymbol, StringComparison.Ordinal);
            var newSymbolCount = CountNumberGroupSeparators(newText);

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
            var result = formattedText.Replace(_setup.CurrencyText, "");
            result = result.Replace(_setup.PercentText, "");
            return result.NumTextToString(_setup.Culture);
        }

        public virtual int CountNumberGroupSeparators(string formattedText)
        {
            var searchString = _setup.Culture.NumberFormat.NumberGroupSeparator;
            if (_setup.EditFormatType == NumericEditFormatTypes.Currency)
                searchString = _setup.Culture.NumberFormat.CurrencyGroupSeparator;

            return formattedText.CountTextForChars(searchString);
        }

        private bool ValidateDecimal(NumericTextProperties numericTextProperties)
        {
            //TODO:ValidateDecimal
            //if (_setup.Precision <= 0)
            //    return false;

            //var numericText = GetNumericTextProperties();
            //var checkNewText = numericText.LeftText + numericText.RightText;

            //if (checkNewText.Contains(decimalString))
            //    return false;

            //var newText = numericText.LeftText + decimalString + numericText.RightText;
            //return ValidateNewText(newText);
            return true;
        }

        private bool ValidateNewText(NumericTextProperties numericTextProperties)
        {
            if (!numericTextProperties.NewDecimalText.IsNullOrEmpty())
            {
                if (numericTextProperties.NewDecimalText.Length > _setup.Precision)
                    return false;
            }

            var newText = GetNewValue(numericTextProperties);
            var newValue = newText.ToDecimal(_setup.Culture);

            if (_setup.MaximumValue != null && newValue > _setup.MaximumValue)
                return false;

            if (_setup.MinimumValue != null && newValue < _setup.MinimumValue)
                return false;

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
                var numericTextProperties = GetNumericTextProperties(string.Empty);
                var newText = numericTextProperties.LeftText + numericTextProperties.RightText;
                var oldSymbolCount = CountNumberGroupSeparators(newText);

                var newValue = newText = StripNonNumericCharacters(newText);

                if (newText.IsNullOrEmpty())
                {
                    newValue = "0";
                }
                else
                {
                    newText = GetFormattedText(numericTextProperties);
                }

                var newSymbolCount = CountNumberGroupSeparators(newText);

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
