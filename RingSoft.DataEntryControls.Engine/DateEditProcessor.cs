using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public class DateEditProcessor
    {
        public IDateEditControl Control { get; }

        public DateTime? Value { get; private set; }

        public event EventHandler ValueChanged;

        private DateEditControlSetup _setup;

        public DateEditProcessor(IDateEditControl control)
        {
            Control = control;
        }

        public ProcessCharResults ProcessChar(DateEditControlSetup setup, char keyChar)
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
                case ' ':
                    return ProcessCharResults.Ignored;
            }

            return ProcessCharResults.ValidationFailed;
        }

        private ProcessCharResults ProcessNumberDigit(char keyChar)
        {
            return ProcessCharResults.ValidationFailed;
        }

        public ProcessCharResults OnBackspaceKeyDown(DateEditControlSetup setup)
        {
            _setup = setup;
            return ProcessCharResults.ValidationFailed;
        }

        public ProcessCharResults OnDeleteKeyDown(DateEditControlSetup setup)
        {
            _setup = setup;
            return ProcessCharResults.ValidationFailed;
        }

        public bool PasteText(DateEditControlSetup setup, string newText)
        {
            _setup = setup;
            return false;
        }

        private void OnValueChanged(DateTime newValue)
        {
            Value = newValue;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
