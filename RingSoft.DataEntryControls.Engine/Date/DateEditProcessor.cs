using System;
using RingSoft.DataEntryControls.Engine.Date.Segments;

namespace RingSoft.DataEntryControls.Engine.Date
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

        public void OnSetFocus(DateEditControlSetup setup, DateTime? value)
        {
            _setup = setup;

            var entryFormat = _setup.GetEntryFormat();
            if (value == null)
            {
                Control.Text = NullDate();
            }
            else
            {
                var newValue = (DateTime) value;
                Control.Text = newValue.ToString(entryFormat);
            }
        }

        private string NullDate()
        {
            var entryFormat = _setup.GetEntryFormat();
            var nullDate = entryFormat;
            var segments = "MdyHhms";
            foreach (char segmentChar in segments)
                nullDate = nullDate.Replace(segmentChar, '0');
            return nullDate;
        }

        public void OnLostFocus(DateEditControlSetup setup, DateTime? value)
        {
            _setup = setup;
            if (value == null)
            {
                Control.Text = string.Empty;
            }
            else
            {
                var dateTimeValue = (DateTime) value;
                Control.Text = dateTimeValue.ToString(_setup.GetDisplayFormat());
            }
        }

        public ProcessCharResults ProcessChar(DateEditControlSetup setup, char keyChar)
        {
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
            CheckDeleteAll();

            return ProcessCharResults.ValidationFailed;
        }

        public ProcessCharResults OnBackspaceKeyDown(DateEditControlSetup setup)
        {
            _setup = setup;

            if (CheckDeleteAll())
                return ProcessCharResults.Processed;

            return ProcessCharResults.ValidationFailed;
        }

        public ProcessCharResults OnDeleteKeyDown(DateEditControlSetup setup)
        {
            _setup = setup;

            if (CheckDeleteAll())
                return ProcessCharResults.Processed;

            return ProcessCharResults.ValidationFailed;
        }

        private bool CheckDeleteAll()
        {
            if (Control.SelectionLength == Control.Text.Length)
            {
                Control.Text = NullDate();
                Control.SelectionStart = Control.SelectionLength = 0;
                OnValueChanged(null);
                return true;
            }

            return false;
        }

        public bool PasteText(DateEditControlSetup setup, string newText)
        {
            _setup = setup;
            return false;
        }

        internal void ReplaceDateCharAdvance(char newChar, int segmentEnd)
        {
            var newText = Control.Text.LeftStr(Control.SelectionStart)
                            + newChar
                            + Control.Text.RightStr((Control.Text.Length - Control.SelectionStart) - 1);

            var selStart = Control.SelectionStart;
            Control.Text = newText;

            Control.SelectionStart = selStart + 1;
            if (Control.SelectionStart > segmentEnd)
                if (Control.SelectionStart < Control.Text.Length - 1)
                    Control.SelectionStart++;
        }

        //internal DateSegmentMonth GetDateSegmentMonth()

        private void OnValueChanged(DateTime? newValue)
        {
            Value = newValue;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
