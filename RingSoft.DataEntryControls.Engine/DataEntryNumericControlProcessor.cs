// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="DataEntryNumericControlProcessor.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Enum ProcessCharResults
    /// </summary>
    public enum ProcessCharResults
    {
        /// <summary>
        /// The processed
        /// </summary>
        Processed = 0,
        /// <summary>
        /// The ignored
        /// </summary>
        Ignored = 1,
        /// <summary>
        /// The validation failed
        /// </summary>
        ValidationFailed = 2
    }

    /// <summary>
    /// Class ValueChangedArgs.
    /// </summary>
    public class ValueChangedArgs
    {
        /// <summary>
        /// Creates new value.
        /// </summary>
        /// <value>The new value.</value>
        public string NewValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedArgs"/> class.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public ValueChangedArgs(string newValue)
        {
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Class DataEntryNumericControlProcessor.
    /// </summary>
    public class DataEntryNumericControlProcessor
    {
        /// <summary>
        /// Class NumericTextProperties.
        /// </summary>
        private class NumericTextProperties
        {
            /// <summary>
            /// Gets or sets the character being processed.
            /// </summary>
            /// <value>The character being processed.</value>
            public string CharacterBeingProcessed { get; set; }
            /// <summary>
            /// Gets or sets the left text.
            /// </summary>
            /// <value>The left text.</value>
            public string LeftText { get; set; }
            /// <summary>
            /// Gets or sets the selected text.
            /// </summary>
            /// <value>The selected text.</value>
            public string SelectedText { get; set; }
            /// <summary>
            /// Gets or sets the right text.
            /// </summary>
            /// <value>The right text.</value>
            public string RightText { get; set; }
            /// <summary>
            /// Gets or sets the index of the decimal.
            /// </summary>
            /// <value>The index of the decimal.</value>
            public int DecimalIndex { get; set; } = -1;
            /// <summary>
            /// Gets or sets the symbol properties.
            /// </summary>
            /// <value>The symbol properties.</value>
            public SymbolProperties SymbolProperties { get; set; }
            /// <summary>
            /// Gets or sets the group separator count.
            /// </summary>
            /// <value>The group separator count.</value>
            public int GroupSeparatorCount { get; set; }
            /// <summary>
            /// Creates new wholenumbertext.
            /// </summary>
            /// <value>The new whole number text.</value>
            public string NewWholeNumberText { get; set; }
            /// <summary>
            /// Creates new decimaltext.
            /// </summary>
            /// <value>The new decimal text.</value>
            public string NewDecimalText { get; set; }
            /// <summary>
            /// Gets or sets the index of the negative sign.
            /// </summary>
            /// <value>The index of the negative sign.</value>
            public int NegativeSignIndex { get; set; } = -1;
            /// <summary>
            /// Gets or sets the index of the symbol.
            /// </summary>
            /// <value>The index of the symbol.</value>
            public int SymbolIndex { get; set; }
            /// <summary>
            /// Gets or sets the first index of the digit.
            /// </summary>
            /// <value>The first index of the digit.</value>
            public int FirstDigitIndex { get; set; }
            /// <summary>
            /// Gets or sets the end index.
            /// </summary>
            /// <value>The end index.</value>
            public int EndIndex { get; set; }
        }

        /// <summary>
        /// Class SymbolProperties.
        /// </summary>
        private class SymbolProperties
        {
            /// <summary>
            /// Gets or sets the symbol location.
            /// </summary>
            /// <value>The symbol location.</value>
            public NumberSymbolLocations SymbolLocation { get; set; }
            /// <summary>
            /// Gets or sets the symbol text.
            /// </summary>
            /// <value>The symbol text.</value>
            public string SymbolText { get; set; }
        }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public INumericControl Control { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public double? Value { get; private set; }

        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event EventHandler<ValueChangedArgs> ValueChanged;

        /// <summary>
        /// The setup
        /// </summary>
        private DecimalEditControlSetup _setup;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryNumericControlProcessor"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public DataEntryNumericControlProcessor(INumericControl control)
        {
            Control = control;
        }

        /// <summary>
        /// Processes the character.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="keyChar">The key character.</param>
        /// <returns>ProcessCharResults.</returns>
        public ProcessCharResults ProcessChar(DecimalEditControlSetup setup, char keyChar)
        {
            var stringChar = keyChar.ToString();
            _setup = setup;
            switch (keyChar)
            {
                case '\b':
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
                case '-':
                    return ProcessNegativeChar();
                case ' ':
                    return ProcessCharResults.Ignored;
            }

            if (stringChar == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                return ProcessDecimal(setup);

            return ProcessCharResults.ValidationFailed;
        }

        /// <summary>
        /// Formats the text for entry.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="controlText">The control text.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public string FormatTextForEntry(DecimalEditControlSetup setup, string controlText)
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

            if (_setup.FormatType == DecimalEditFormatTypes.Percent)
            {
                value *= 100;
                value = Math.Round(value, _setup.Precision);
            }

            var decimalText = value.ToString(_setup.Culture);
            var numericTextProperties =
                GetNumericPropertiesForText(decimalText, decimalText.Length, 0, string.Empty);

            var newText = GetFormattedText(numericTextProperties);
            return newText;
        }

        /// <summary>
        /// Gets the symbol properties.
        /// </summary>
        /// <returns>SymbolProperties.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private SymbolProperties GetSymbolProperties()
        {
            var result = new SymbolProperties();

            switch (_setup.FormatType)
            {
                case DecimalEditFormatTypes.Currency:
                    result.SymbolText = _setup.CurrencyText;
                    result.SymbolLocation = _setup.CurrencySymbolLocation;
                    break;
                case DecimalEditFormatTypes.Number:
                    result.SymbolText = string.Empty;
                    break;
                case DecimalEditFormatTypes.Percent:
                    result.SymbolText = _setup.PercentText;
                    result.SymbolLocation = _setup.PercentSymbolLocation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        /// <summary>
        /// Gets the numeric text properties.
        /// </summary>
        /// <param name="charText">The character text.</param>
        /// <returns>NumericTextProperties.</returns>
        private NumericTextProperties GetNumericTextProperties(string charText)
        {
            return GetNumericPropertiesForText(Control.Text, Control.SelectionStart, Control.SelectionLength, charText);
        }

        /// <summary>
        /// Gets the numeric properties for text.
        /// </summary>
        /// <param name="controlText">The control text.</param>
        /// <param name="selectionStart">The selection start.</param>
        /// <param name="selectionLength">Length of the selection.</param>
        /// <param name="charText">The character text.</param>
        /// <returns>NumericTextProperties.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private NumericTextProperties GetNumericPropertiesForText(string controlText, int selectionStart,
            int selectionLength, string charText)
        {
            var result = new NumericTextProperties()
            {
                CharacterBeingProcessed = charText,
                LeftText = controlText.LeftStr(selectionStart),
                SelectedText = controlText.MidStr(selectionStart, selectionLength),
                RightText = controlText.GetRightText(selectionStart, selectionLength),
                SymbolProperties = GetSymbolProperties()
            };

            result.SymbolIndex = -1;
            if (!result.SymbolProperties.SymbolText.IsNullOrEmpty())
                result.SymbolIndex = controlText.IndexOf(result.SymbolProperties.SymbolText, StringComparison.Ordinal);

            if (result.SymbolIndex >= 0)
            {
                switch (result.SymbolProperties.SymbolLocation)
                {
                    case NumberSymbolLocations.Prefix:
                        result.FirstDigitIndex = result.SymbolIndex + result.SymbolProperties.SymbolText.Length;
                        result.EndIndex = controlText.Length;
                        break;
                    case NumberSymbolLocations.Suffix:
                        result.FirstDigitIndex = 0;
                        result.EndIndex = result.SymbolIndex;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                result.FirstDigitIndex = 0;
                result.EndIndex = controlText.Length;
            }


            var newText = result.LeftText + charText + result.RightText;
            var selectedTextDecimalPosition = GetDecimalPosition(result.SelectedText);
            if (selectedTextDecimalPosition < 0)
                result.DecimalIndex = GetDecimalPosition(controlText);

            result.GroupSeparatorCount = CountNumberGroupSeparators(newText);

            newText = StripNonNumericCharacters(newText);
            result.NegativeSignIndex = newText.IndexOf('-');

            if (result.NegativeSignIndex >= 0 && result.FirstDigitIndex == 0)
                result.FirstDigitIndex = result.NegativeSignIndex + 1;

            result.NewWholeNumberText = newText;

            var newDecimalPosition = GetDecimalPosition(newText);
            if (newDecimalPosition >= 0)
            {
                result.NewWholeNumberText = newText.LeftStr(newDecimalPosition);
                result.NewDecimalText = newText.GetRightText(newDecimalPosition, 1);
            }

            if (result.NewWholeNumberText == "-")
                result.NewWholeNumberText = "0";

            return result;
        }

        /// <summary>
        /// Gets the decimal position.
        /// </summary>
        /// <param name="numberText">The number text.</param>
        /// <returns>System.Int32.</returns>
        private int GetDecimalPosition(string numberText)
        {
            if (numberText.IsNullOrEmpty())
                return -1;

            return numberText.IndexOf(GetDecimalPointString(), StringComparison.Ordinal);
        }

        /// <summary>
        /// Processes the number digit.
        /// </summary>
        /// <param name="numberChar">The number character.</param>
        /// <returns>ProcessCharResults.</returns>
        protected virtual ProcessCharResults ProcessNumberDigit(char numberChar)
        {
            var numericTextProperties = GetNumericTextProperties(numberChar.ToString());

            if (_setup.DataEntryMode == DataEntryModes.FormatOnEntry)
            {
                ScrubDataEntrySelectionStart(numericTextProperties, numberChar.ToString());
                numericTextProperties = GetNumericTextProperties(numberChar.ToString());
            }

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

            newSelectionStart = UpdateSelectionStart(newText, numericTextProperties, newSelectionStart);

            Control.Text = newText;
            Control.SelectionStart = newSelectionStart;
            Control.SelectionLength = 0;
            OnValueChanged(GetNewValue(numericTextProperties));

            return ProcessCharResults.Processed;
        }

        /// <summary>
        /// Scrubs the data entry selection start.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <param name="numberChar">The number character.</param>
        private void ScrubDataEntrySelectionStart(NumericTextProperties numericTextProperties, string numberChar)
        {
            ScrubSelectionProperties(numericTextProperties);

            if (Control.SelectionLength == 0)
            {
                if (Control.SelectionStart < numericTextProperties.FirstDigitIndex)
                {
                    if (numberChar == "-")
                    {
                        if (Control.SelectionStart > numericTextProperties.SymbolIndex)
                            Control.SelectionStart = numericTextProperties.FirstDigitIndex;
                    }
                    else
                    {
                        if (Control.SelectionStart >= numericTextProperties.SymbolIndex)
                            Control.SelectionStart = numericTextProperties.FirstDigitIndex;
                        else if (Control.SelectionStart == 0)
                            Control.SelectionStart = numericTextProperties.FirstDigitIndex;
                    }
                }

                if (Control.SelectionStart > numericTextProperties.EndIndex)
                    Control.SelectionStart = numericTextProperties.EndIndex;
            }
        }

        /// <summary>
        /// Updates the selection start.
        /// </summary>
        /// <param name="newText">The new text.</param>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <param name="newSelectionStart">The new selection start.</param>
        /// <returns>System.Int32.</returns>
        private int UpdateSelectionStart(string newText, NumericTextProperties numericTextProperties, int newSelectionStart)
        {
            if ((numericTextProperties.LeftText + numericTextProperties.RightText).IsNullOrEmpty())
            {
                var newTextProperties = GetNumericPropertiesForText(newText, newSelectionStart, 0, string.Empty);
                newSelectionStart = newTextProperties.EndIndex;
            }

            return newSelectionStart;
        }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <returns>System.String.</returns>
        private string GetNewValue(NumericTextProperties numericTextProperties)
        {
            var result = numericTextProperties.NewWholeNumberText;
            if (!numericTextProperties.NewDecimalText.IsNullOrEmpty())
            {
                var decimalPoint = GetDecimalPointString();
                result = $"{result}{decimalPoint}{numericTextProperties.NewDecimalText}";
            }

            return result;
        }

        /// <summary>
        /// Gets the decimal point string.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private string GetDecimalPointString()
        {
            var decimalPoint = _setup.Culture.NumberFormat.CurrencyDecimalSeparator;
            switch (_setup.FormatType)
            {
                case DecimalEditFormatTypes.Currency:
                    break;
                case DecimalEditFormatTypes.Number:
                case DecimalEditFormatTypes.Percent:
                    decimalPoint = _setup.Culture.NumberFormat.NumberDecimalSeparator;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return decimalPoint;
        }

        /// <summary>
        /// Gets the formatted text.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

            switch (_setup.FormatType)
            {
                case DecimalEditFormatTypes.Number:
                    wholeNumberText = wholeNumber.ToString("N0", _setup.Culture);
                    if (!decimalText.IsNullOrEmpty())
                        decimalText = $"{_setup.Culture.NumberFormat.NumberDecimalSeparator}{decimalText}";
                    break;
                case DecimalEditFormatTypes.Currency:
                    wholeNumberText = wholeNumber.ToString("C0", _setup.Culture);
                    if (!decimalText.IsNullOrEmpty())
                        decimalText = $"{_setup.Culture.NumberFormat.CurrencyDecimalSeparator}{decimalText}";
                    break;
                case DecimalEditFormatTypes.Percent:
                    var percentNumber = wholeNumber / 100;
                    wholeNumberText = percentNumber.ToString("P0", _setup.Culture.NumberFormat);
                    if (!decimalText.IsNullOrEmpty())
                        decimalText = $"{_setup.Culture.NumberFormat.NumberDecimalSeparator}{decimalText}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var symbolPosition = -1;
            if (!numericTextProperties.SymbolProperties.SymbolText.IsNullOrEmpty())
                symbolPosition = wholeNumberText.IndexOf(numericTextProperties.SymbolProperties.SymbolText,
                    StringComparison.Ordinal);

            if (symbolPosition > 0)
                wholeNumberText = wholeNumberText.LeftStr(symbolPosition);

            if (isNegative)
                wholeNumberText = $"-{wholeNumberText}";

            if (numericTextProperties.CharacterBeingProcessed == GetDecimalPointString() && decimalText.IsNullOrEmpty())
                result = wholeNumberText + numericTextProperties.CharacterBeingProcessed + decimalText;
            else
                result = wholeNumberText + decimalText;

            if (symbolPosition > 0)
                result += numericTextProperties.SymbolProperties.SymbolText;

            return result;
        }

        /// <summary>
        /// Validates the number.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateNumber(NumericTextProperties numericTextProperties)
        {
            if (numericTextProperties.NewWholeNumberText == "00")
                return false;

            return ValidateNewText(numericTextProperties);
        }

        /// <summary>
        /// Processes the decimal.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <returns>ProcessCharResults.</returns>
        private ProcessCharResults ProcessDecimal(DecimalEditControlSetup setup)
        {
            _setup = setup;
            var decimalString = GetDecimalPointString();
            if (Control.Text.IndexOf(decimalString, StringComparison.Ordinal) == Control.SelectionStart &&
                Control.SelectionLength < 2)
            {
                Control.SelectionStart++;
                return ProcessCharResults.Processed;
            }
            var numericTextProperties = GetNumericTextProperties(decimalString);

            if (!ValidateDecimal(numericTextProperties))
                return ProcessCharResults.ValidationFailed;

            var newText = numericTextProperties.LeftText + decimalString + numericTextProperties.RightText;

            if (_setup.DataEntryMode == DataEntryModes.ValidateOnly)
            {
                var valSelectionStart = Control.SelectionStart;
                Control.Text = newText;
                Control.SelectionStart = valSelectionStart + 1;
                return ProcessCharResults.Processed;
            }

            var selectionIncrement = 0;
            if (newText == decimalString)
            {
                numericTextProperties.NewWholeNumberText = "0";
                selectionIncrement++;
            }

            ScrubDataEntrySelectionStart(numericTextProperties, decimalString);
            numericTextProperties = GetNumericTextProperties(decimalString);

            newText = GetFormattedText(numericTextProperties);

            var newSymbolCount = CountNumberGroupSeparators(newText);

            var newSelectionStart = Control.SelectionStart + 1;
            selectionIncrement += newSymbolCount - numericTextProperties.GroupSeparatorCount;
            newSelectionStart += selectionIncrement;

            newSelectionStart = UpdateSelectionStart(newText, numericTextProperties, newSelectionStart);

            Control.Text = newText;
            Control.SelectionStart = newSelectionStart;
            Control.SelectionLength = 0;
            OnValueChanged(GetNewValue(numericTextProperties));

            return ProcessCharResults.Processed;
        }

        /// <summary>
        /// Strips the non numeric characters.
        /// </summary>
        /// <param name="formattedText">The formatted text.</param>
        /// <returns>System.String.</returns>
        public virtual string StripNonNumericCharacters(string formattedText)
        {
            var result = formattedText.Replace(_setup.CurrencyText, "");
            result = result.Replace(_setup.PercentText, "");
            return result.NumTextToString(_setup.Culture);
        }

        /// <summary>
        /// Counts the number group separators.
        /// </summary>
        /// <param name="formattedText">The formatted text.</param>
        /// <returns>System.Int32.</returns>
        public virtual int CountNumberGroupSeparators(string formattedText)
        {
            var searchString = GetGroupSeparatorString();

            return formattedText.CountTextForChars(searchString);
        }

        /// <summary>
        /// Gets the group separator string.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetGroupSeparatorString()
        {
            if (_setup.FormatType == DecimalEditFormatTypes.Currency)
                return _setup.Culture.NumberFormat.CurrencyGroupSeparator;

            return _setup.Culture.NumberFormat.NumberGroupSeparator;
        }

        /// <summary>
        /// Validates the decimal.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateDecimal(NumericTextProperties numericTextProperties)
        {
            if (_setup.Precision <= 0)
                return false;

            if (numericTextProperties.DecimalIndex >= 0)
                return false;

            return ValidateNewText(numericTextProperties);
        }

        /// <summary>
        /// Validates the new text.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

            //if (_setup.MinimumValue != null && newValue < _setup.MinimumValue)
            //    return false;

            return true;
        }

        /// <summary>
        /// Called when [backspace key down].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <returns>ProcessCharResults.</returns>
        public virtual ProcessCharResults OnBackspaceKeyDown(DecimalEditControlSetup setup)
        {
            if (Control.SelectionStart == 0 && Control.SelectionLength == 0)
                return ProcessCharResults.ValidationFailed;

            _setup = setup;
            var numericTextProperties = GetNumericTextProperties(string.Empty);

            if (!ScrubBackspace(numericTextProperties))
                return ProcessCharResults.Processed;

            ScrubSelectionProperties(numericTextProperties);

            if (!ScrubBackspace(numericTextProperties))
                return ProcessCharResults.Processed;

            var groupSeparator = GetGroupSeparatorString();
            var groupSeparatorAtLeft = false;
            if (Control.SelectionLength == 0)
            {
                if (Control.SelectionStart > 0)
                {
                    
                    var leftText = Control.Text.MidStr(Control.SelectionStart - groupSeparator.Length,
                        groupSeparator.Length);
                    if (leftText == groupSeparator)
                    {
                        Control.SelectionStart -= groupSeparator.Length + 1;
                        Control.SelectionLength = groupSeparator.Length + 1;
                        groupSeparatorAtLeft = true;
                    }
                }
                if (Control.SelectionLength == 0)
                {
                    Control.SelectionStart--;
                    Control.SelectionLength = 1;
                }
            }

            DeleteSelectedText();

            if (groupSeparatorAtLeft)
            {
                var newTextProperties = GetNumericTextProperties(string.Empty);
                if (Control.SelectionStart > newTextProperties.FirstDigitIndex)
                {
                    if (newTextProperties.NegativeSignIndex < 0)
                    {
                        Control.SelectionStart -= groupSeparator.Length;
                    }
                    else
                    {
                        if (Control.SelectionStart != newTextProperties.NegativeSignIndex + 1)
                            Control.SelectionStart -= groupSeparator.Length;
                    }
                }
            }
            return ProcessCharResults.Processed;
        }

        /// <summary>
        /// Scrubs the backspace.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ScrubBackspace(NumericTextProperties numericTextProperties)
        {
            if (Control.SelectionLength == 0)
            {
                if (Control.SelectionStart == numericTextProperties.FirstDigitIndex)
                {
                    if (numericTextProperties.NegativeSignIndex < 0)
                        Control.SelectionStart = 0;
                    else
                    {
                        if (Control.SelectionStart == 1)
                            return true;

                        Control.SelectionStart = numericTextProperties.NegativeSignIndex + 1;
                    }

                    return false;
                }
                else if (Control.SelectionStart > numericTextProperties.EndIndex)
                {
                    Control.SelectionStart = numericTextProperties.EndIndex;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Deletes the selected text.
        /// </summary>
        private void DeleteSelectedText()
        {
            var numericTextProperties = GetNumericTextProperties(string.Empty);
            if (Control.SelectionLength > 0)
            {
                var newText = numericTextProperties.LeftText + numericTextProperties.RightText;
                var oldGroupSeparatorCount = CountNumberGroupSeparators(newText);

                var newValue = newText = StripNonNumericCharacters(newText);

                if (newText.IsNullOrEmpty())
                {
                    if (_setup.AllowNullValue)
                        newValue = string.Empty;
                    else
                        newValue = "0";
                }
                else
                {
                    if (numericTextProperties.DecimalIndex >= 0 &&
                        Control.SelectionStart >= numericTextProperties.DecimalIndex + 1)
                    {
                        newText = numericTextProperties.LeftText + numericTextProperties.RightText;
                    }
                    else
                    {
                        newText = GetFormattedText(numericTextProperties);
                        if (numericTextProperties.NewWholeNumberText == "0" &&
                            numericTextProperties.NewDecimalText.IsNullOrEmpty())
                            newText = string.Empty;
                    }
                }

                var newGroupSeparatorCount = CountNumberGroupSeparators(newText);

                var selectionStart = Control.SelectionStart;
                if (selectionStart > numericTextProperties.FirstDigitIndex)
                    selectionStart -= oldGroupSeparatorCount - newGroupSeparatorCount;
                if (selectionStart < 0)
                    selectionStart = 0;

                Control.Text = newText;
                Control.SelectionStart = selectionStart;
                Control.SelectionLength = 0;

                OnValueChanged(newValue);
            }
        }

        /// <summary>
        /// Scrubs the selection properties.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        private void ScrubSelectionProperties(NumericTextProperties numericTextProperties)
        {
            if (Control.SelectionLength > 0)
            {
                if (Control.SelectionLength == Control.Text.Length)
                    return;

                if (Control.SelectionStart < numericTextProperties.FirstDigitIndex)
                {
                    if (Control.SelectionStart + Control.SelectionLength > numericTextProperties.FirstDigitIndex)
                    {
                        //User selects digits next to symbol.  Reset selection start to be the first digit.
                        //Select text that was selected beyond the first digit.
                        var newSelectionLength =
                            (Control.SelectionStart + Control.SelectionLength) - numericTextProperties.FirstDigitIndex;
                        Control.SelectionLength = newSelectionLength;
                    }
                    else
                    {
                        Control.SelectionLength = 0;
                    }
                    Control.SelectionStart = numericTextProperties.FirstDigitIndex;
                }

                if (Control.SelectionStart + Control.SelectionLength > numericTextProperties.EndIndex)
                {
                    //User selects digits next to symbol.  Select text that was selected before symbol.
                    var newSelectionLength = numericTextProperties.EndIndex - Control.SelectionStart;
                    if (newSelectionLength < 0)
                        newSelectionLength = 0;
                    Control.SelectionLength = newSelectionLength;
                }
            }
        }

        /// <summary>
        /// Called when [delete key down].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <returns>ProcessCharResults.</returns>
        public virtual ProcessCharResults OnDeleteKeyDown(DecimalEditControlSetup setup)
        {
            _setup = setup;
            var numericTextProperties = GetNumericTextProperties(string.Empty);

            if (Control.SelectionStart == numericTextProperties.EndIndex)
                return ProcessCharResults.ValidationFailed;


            if (!ScrubDelete(numericTextProperties))
                return ProcessCharResults.Processed;

            ScrubSelectionProperties(numericTextProperties);

            if (!ScrubDelete(numericTextProperties))
                return ProcessCharResults.Processed;


            var groupSeparator = GetGroupSeparatorString();
            if (Control.SelectionLength == 0)
            {
                if (Control.SelectionStart < numericTextProperties.EndIndex)
                {

                    var rightText = Control.Text.MidStr(Control.SelectionStart,
                        groupSeparator.Length);
                    if (rightText == groupSeparator)
                    {
                        Control.SelectionLength = groupSeparator.Length + 1;
                    }
                }
                if (Control.SelectionLength == 0)
                {
                    Control.SelectionLength = 1;
                    if (Control.SelectionStart + Control.SelectionLength == numericTextProperties.DecimalIndex)
                        Control.SelectionLength += GetDecimalPointString().Length;
                }
            }

            DeleteSelectedText();

            var newTextProperties = GetNumericTextProperties(string.Empty);
            if (Control.SelectionStart < newTextProperties.FirstDigitIndex)
                Control.SelectionStart = newTextProperties.FirstDigitIndex;

            return ProcessCharResults.Processed;
        }

        /// <summary>
        /// Scrubs the delete.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ScrubDelete(NumericTextProperties numericTextProperties)
        {
            if (Control.SelectionStart + Control.SelectionLength <= numericTextProperties.FirstDigitIndex)
            {
                if (Control.SelectionStart < numericTextProperties.FirstDigitIndex)
                {
                    if (numericTextProperties.NegativeSignIndex < 0)
                    {
                        Control.SelectionStart = numericTextProperties.FirstDigitIndex;
                        Control.SelectionLength = 0;
                        return false;
                    }

                    //Negative
                    if (Control.SelectionStart > 0)
                    {
                        Control.SelectionStart = numericTextProperties.FirstDigitIndex;
                        Control.SelectionLength = 0;
                        return false;
                    }
                }
                else if (Control.SelectionLength == 0 && Control.SelectionStart > numericTextProperties.EndIndex)
                {
                    Control.SelectionStart = numericTextProperties.EndIndex;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Processes the negative character.
        /// </summary>
        /// <returns>ProcessCharResults.</returns>
        private ProcessCharResults ProcessNegativeChar()
        {
            if (_setup.MinimumValue != null && _setup.MinimumValue >= 0)
                return ProcessCharResults.ValidationFailed;

            var numericTextProperties = GetNumericTextProperties("");
            if (numericTextProperties.NegativeSignIndex >= 0)
                return ProcessCharResults.ValidationFailed;

            if (Control.SelectionStart > numericTextProperties.FirstDigitIndex)
                return ProcessCharResults.ValidationFailed;

            if ((numericTextProperties.LeftText + numericTextProperties.RightText).IsNullOrEmpty())
            {
                SetEmptyNegativeText(numericTextProperties);
            }
            else
            {
                return ProcessNumberDigit('-');
            }

            return ProcessCharResults.Processed;
        }

        /// <summary>
        /// Sets the empty negative text.
        /// </summary>
        /// <param name="numericTextProperties">The numeric text properties.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void SetEmptyNegativeText(NumericTextProperties numericTextProperties)
        {
            Control.SelectionLength = 0;
            if (!numericTextProperties.SymbolProperties.SymbolText.IsNullOrEmpty())
            {
                Control.Text = $"-{numericTextProperties.SymbolProperties.SymbolText}";
                switch (numericTextProperties.SymbolProperties.SymbolLocation)
                {
                    case NumberSymbolLocations.Prefix:
                        Control.SelectionStart = Control.Text.Length;
                        break;
                    case NumberSymbolLocations.Suffix:
                        Control.SelectionStart = 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                Control.Text = "-";
                Control.SelectionStart = 1;
            }
        }

        /// <summary>
        /// Pastes the text.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="newText">The new text.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool PasteText(DecimalEditControlSetup setup, string newText)
        {
            _setup = setup;

            if (newText.IsNullOrEmpty())
            {
                Control.Text = string.Empty;
                Control.SelectionStart = 0;
                Control.SelectionLength = 0;
                OnValueChanged(string.Empty);
                return true;
            }

            bool validationPassed = newText.TryParseDecimal(out var result, _setup.Culture);

            if (validationPassed)
            {
                var numericTextProperties = GetNumericPropertiesForText(newText, 0, 0, string.Empty);
                validationPassed = ValidateNewText(numericTextProperties);
            }

            if (!validationPassed)
            {
                newText = "0";
                Control.Text = FormatTextForEntry(setup, newText);
                Control.SelectionStart = 0;
                Control.SelectionLength = Control.Text.Length;
                OnValueChanged(newText);
                return false;
            }

            if (_setup.DataEntryMode == DataEntryModes.FormatOnEntry)
            {
                Control.Text = FormatTextForEntry(setup, newText);
                Control.SelectionStart = 0;
                Control.SelectionLength = Control.Text.Length;
            }

            newText = result.ToString(_setup.Culture);
            OnValueChanged(newText);
            return true;
        }

        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public virtual void OnValueChanged(string newValue)
        {
            var decimalValue = newValue.ToDecimal(_setup.Culture);

            if (_setup.FormatType == DecimalEditFormatTypes.Percent)
            {
                decimalValue /= 100;
                newValue = decimalValue.ToString(_setup.Culture);
            }

            if (newValue.IsNullOrEmpty())
                Value = null;
            else 
                Value = decimalValue;

            ValueChanged?.Invoke(this, new ValueChangedArgs(newValue));
        }
    }
}
