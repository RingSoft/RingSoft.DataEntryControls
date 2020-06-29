﻿using System;
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
        public int SymbolIndex { get; set; } = -1;
        public NumberSymbolLocations SymbolLocation { get; set; }
        public string SymbolText { get; set; }
        public int GroupSeparatorCount { get; set; }
        public string NewWholeNumberText { get; set; }
        public string NewDecimalText { get; set; }
        public bool IsNegative { get; set; }
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
            var value = decimal.Parse(controlText, _setup.Culture.NumberFormat);
            var decimalText = value.ToString(CultureInfo.InvariantCulture);

            var numericTextProperties =
                GetNumericPropertiesForText(decimalText, decimalText.Length - 1, 0, string.Empty);

            var newText = GetFormattedText(new NumericTextProperties());
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
                RightText = controlText.GetRightText(selectionStart, selectionLength),
                GroupSeparatorCount = CountNumberGroupCharacter(controlText)
            };

            switch (_setup.EditFormatType)
            {
                case NumericEditFormatTypes.Currency:
                    result.SymbolText = _setup.CurrencyText;
                    result.SymbolIndex = controlText.IndexOf(result.SymbolText, StringComparison.Ordinal);
                    result.SymbolLocation = _setup.CurrencySymbolLocation;
                    break;
                case NumericEditFormatTypes.Number:
                    break;
                case NumericEditFormatTypes.Percent:
                    result.SymbolText = _setup.PercentText;
                    result.SymbolIndex = controlText.IndexOf(result.SymbolText, StringComparison.Ordinal);
                    result.SymbolLocation = _setup.PercentSymbolLocation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var selectedTextDecimalPosition = GetDecimalPosition(result.SelectedText);
            if (selectedTextDecimalPosition < 0)
                result.DecimalPosition = GetDecimalPosition(controlText);

            var newText = result.LeftText + charText + result.RightText;
            newText = StripNonNumericCharacters(newText);
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

            var newText = numericTextProperties.LeftText + numberChar + numericTextProperties.RightText;

            var oldCurrencyPosition =
                newText.IndexOf(_setup.Culture.NumberFormat.CurrencySymbol, StringComparison.Ordinal);
            var oldSymbolCount = CountNumberGroupCharacter(newText);

            var newValue = newText = StripNonNumericCharacters(newText);

            newText = GetFormattedText(numericTextProperties);

            var newCurrencyPosition =
                newText.IndexOf(_setup.Culture.NumberFormat.CurrencySymbol, StringComparison.Ordinal);
            var newSymbolCount = CountNumberGroupCharacter(newText);

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

        private string GetFormattedText(NumericTextProperties numericTextProperties)
        {
            var result = string.Empty;
            var wholeNumberText = numericTextProperties.NewWholeNumberText;
            var decimalText = numericTextProperties.NewDecimalText;

            if ((wholeNumberText + decimalText).IsNullOrEmpty())
                return result;

            var wholeNumber = (decimal)0;
            if (!wholeNumberText.IsNullOrEmpty())
                wholeNumber = decimal.Parse(wholeNumberText);

            switch (_setup.EditFormatType)
            {
                case NumericEditFormatTypes.Number:
                    wholeNumberText = wholeNumber.ToString("N0", _setup.Culture);
                    break;
                case NumericEditFormatTypes.Currency:
                    wholeNumberText = wholeNumber.ToString("C0", _setup.Culture);
                    break;
                case NumericEditFormatTypes.Percent:
                    var percentNumber = decimal.Parse(numericTextProperties.NewWholeNumberText);
                    percentNumber = percentNumber / 100;
                    result = percentNumber.ToString($"P{_setup.Precision}", _setup.Culture.NumberFormat);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var symbolPosition = wholeNumberText.IndexOf(numericTextProperties.SymbolText,
                StringComparison.Ordinal);

            if (symbolPosition > 0)
                wholeNumberText = wholeNumberText.LeftStr(symbolPosition);

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

            var oldSymbolCount = CountNumberGroupCharacter(newText);

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
            var newSymbolCount = CountNumberGroupCharacter(newText);

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

        public virtual int CountNumberGroupCharacter(string formattedText)
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
            if (numericTextProperties.NewDecimalText.Length > _setup.Precision)
                return false;

            var newText = numericTextProperties.NewWholeNumberText;
            if (numericTextProperties.NewDecimalText.Length > 0)
                newText = $"{newText}.{numericTextProperties.NewDecimalText}";

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
                var numericTextProperties = GetNumericTextProperties(string.Empty);
                var newText = numericTextProperties.LeftText + numericTextProperties.RightText;
                var oldSymbolCount = CountNumberGroupCharacter(newText);

                var newValue = newText = StripNonNumericCharacters(newText);

                if (newText.IsNullOrEmpty())
                {
                    newValue = "0";
                }
                else
                {
                    newText = GetFormattedText(numericTextProperties);
                }

                var newSymbolCount = CountNumberGroupCharacter(newText);

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
