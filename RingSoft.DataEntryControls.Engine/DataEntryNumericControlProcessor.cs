using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public class ValueChangedArgs
    {
        public string NewValue { get; }

        public ValueChangedArgs(string newValue)
        {
            NewValue = newValue;
        }
    }

    public class DataEntryNumericControlProcessor
    {
        private class NumericText
        {
            public string LeftText { get; set; }
            public string SelectedText { get; set; }
            public string RightText { get; set; }
            public int DecimalPosition { get; set; } = -1;
        }

        public INumericControl Control { get; }

        public event EventHandler<ValueChangedArgs> ValueChanged;

        private DataEntryNumericEditSetup _setup;

        public DataEntryNumericControlProcessor(INumericControl control)
        {
            Control = control;
        }

        public bool ProcessChar(DataEntryNumericEditSetup setup, char keyChar)
        {
            _setup = setup;
            switch (keyChar)
            {
                case '\b':
                    ProcessBackspace(setup);
                    return true;
                case '\u001b':  //Escape
                case '\t':
                case '\r':
                case '\n':
                    return false;
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
                    ProcessNumber(keyChar);
                    return true;
                case '.':
                    ProcessDecimal(keyChar);
                    return true;
            }

            Control.OnInvalidChar();
            return true;
        }

        private NumericText GetNumericText()
        {
            return new NumericText()
            {
                LeftText = Control.Text.LeftStr(Control.SelectionStart),
                SelectedText = Control.Text.MidStr(Control.SelectionStart, Control.SelectionLength),
                RightText = Control.Text.GetRightText(Control.SelectionStart, Control.SelectionLength),
                DecimalPosition = Control.Text.IndexOf('.')
            };
        }
        private void ProcessNumber(char number)
        {
            var numericText = GetNumericText();
            
            var newText = numericText.LeftText + number + numericText.RightText;
            ValidateAndSetText(newText, 1);
        }

        private void ProcessDecimal(char decimalChar)
        {
            if (_setup.Precision <= 0)
            {
                Control.OnInvalidChar();
                return;
            }

            ProcessNumber(decimalChar);
        }

        private void ValidateAndSetText(string newText, int selectionStartChange)
        {
            var oldSymbolCount = newText.CountTextForNumSymbols();
            newText = newText.NumTextToString();
            var decimalPosition = newText.IndexOf('.');
            var decimalText = string.Empty;

            if (!ValidateNewText(newText, decimalPosition, ref decimalText))
                return;

            var displayText = newText;
            var newSelectionStart = Control.SelectionStart + selectionStartChange;
            if (!_setup.NumberFormatString.IsNullOrEmpty())
            {
                displayText = string.Empty;
                var startIndex = decimalPosition < 0 ? newText.Length - 1 : decimalPosition - 1;
                var groupIndex = 0;
                for (int i = startIndex; i >= 0; i--)
                {
                    if (groupIndex == 3)
                    {
                        displayText = NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator + displayText;
                        groupIndex = 0;
                    }

                    var cChar = newText[i];
                    displayText = cChar + displayText;
                    groupIndex++;
                }

                if (decimalPosition >= 0)
                    displayText += "." + decimalText;

                var newSymbolCount = displayText.CountTextForNumSymbols();
                if (newSymbolCount > oldSymbolCount)
                    newSelectionStart += newSymbolCount - oldSymbolCount;
            }

            Control.Text = displayText;
            Control.SelectionStart = newSelectionStart;
            Control.SelectionLength = 0;
            OnValueChanged(newText);
        }

        private bool ValidateNewText(string newText, int decimalPosition, ref string decimalText)
        {
            if (decimalPosition >= 0)
            {
                decimalText = newText.GetRightText(decimalPosition + 1, 0);
                if (decimalText.Length > _setup.Precision)
                {
                    Control.OnInvalidChar();
                    return false;
                }
            }

            if (decimal.TryParse(newText, out var newValue))
            {
                if (_setup.MaximumValue > 0 && newValue > _setup.MaximumValue)
                {
                    Control.OnInvalidChar();
                    return false;
                }

                if (_setup.MinimumValue > 0 && newValue < _setup.MinimumValue)
                {
                    Control.OnInvalidChar();
                    return false;
                }
            }

            return true;
        }

        public void ProcessBackspace(DataEntryNumericEditSetup setup)
        {
            _setup = setup;
        }

        public void ProcessDelete(DataEntryNumericEditSetup setup)
        {
            _setup = setup;
        }

        private void OnValueChanged(string newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedArgs(newValue));
        }
    }
}
