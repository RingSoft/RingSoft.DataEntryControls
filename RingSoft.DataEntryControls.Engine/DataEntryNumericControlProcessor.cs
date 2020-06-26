namespace RingSoft.DataEntryControls.Engine
{
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

        private DataEntryNumericEditSetup _setup;

        public DataEntryNumericControlProcessor(INumericControl control)
        {
            Control = control;
        }

        public bool ValidateChar(DataEntryNumericEditSetup setup, char keyChar)
        {
            _setup = setup;
            switch (keyChar)
            {
                case '\b':
                case '\u001b':  //Escape
                case '\t':
                case '\r':
                case '\n':
                    return true;
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
                    return ValidateNumber(keyChar);
                case '.':
                    return ValidateDecimal(keyChar);
            }

            Control.OnInvalidChar();
            return false;
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
        private bool ValidateNumber(char number)
        {
            var numericText = GetNumericText();
            
            var newText = numericText.LeftText + number + numericText.RightText;
            return ValidateNewText(newText);
        }

        private bool ValidateDecimal(char decimalChar)
        {
            if (_setup.Precision <= 0)
            {
                Control.OnInvalidChar();
                return false;
            }

            var numericText = GetNumericText();
            var checkNewText = numericText.LeftText + numericText.RightText;

            if (checkNewText.Contains(decimalChar.ToString()))
            {
                Control.OnInvalidChar();
                return false;
            }

            var newText = numericText.LeftText + decimalChar.ToString() + numericText.RightText;
            return ValidateNewText(newText);
        }

        private bool ValidateNewText(string newText)
        {
            newText = newText.NumTextToString();
            var decimalPosition = newText.IndexOf('.');
            if (decimalPosition >= 0)
            {
                var decimalText = newText.GetRightText(decimalPosition + 1, 0);
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
    }
}
