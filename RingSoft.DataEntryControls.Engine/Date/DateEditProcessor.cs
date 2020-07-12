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
                case ' ':
                    return ProcessCharResults.Ignored;
            }

            var dateSegment = GetActiveSegment(keyChar);
            if (dateSegment == null)
                return ProcessCharResults.ValidationFailed;

            if (dateSegment.SegmentProcessChar())
            {
                if (DateTime.TryParse(Control.Text, out var newDate))
                {
                    OnValueChanged(newDate);
                }
                return ProcessCharResults.Processed;
            }

            return ProcessCharResults.ValidationFailed;
        }

        internal DateSegment GetActiveSegment(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            if (Control.SelectionStart >= entryFormat.Length)
                return null;

            char cSegmentChar = entryFormat[Control.SelectionStart];
            switch (cSegmentChar)
            {
                case 'M':
                    return GetDateSegmentMonth(cChar);
                case 'd':
                    return GetDateSegmentDay(cChar);
                case 'y':
                    return GetDateSegmentYear(cChar);
                case 'h':
                case 'H':
                    return GetDateSegmentHour(cChar);
                case 'm':
                case 's':
                    return GetDateSegmentMinuteSecond(cChar);
                case 't':
                    return GetDateSegmentAmPm(cChar);
            }
            return null;
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

        public bool CheckDeleteAll()
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

        internal DateSegmentMonth GetDateSegmentMonth()
        {
            return GetDateSegmentMonth('Z');
        }

        internal DateSegmentMonth GetDateSegmentMonth(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = 'M';

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentMonth(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        internal DateSegmentDay GetDateSegmentDay()
        {
            return GetDateSegmentDay('Z');
        }

        internal DateSegmentDay GetDateSegmentDay(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = 'd';

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentDay(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        internal DateSegmentYear GetDateSegmentYear()
        {
            return GetDateSegmentYear('Z');
        }

        internal DateSegmentYear GetDateSegmentYear(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = 'y';

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentYear(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        internal DateSegmentHour GetDateSegmentHour()
        {
            return GetDateSegmentHour('Z');
        }

        internal DateSegmentHour GetDateSegmentHour(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = entryFormat[Control.SelectionStart];

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentHour(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        internal DateSegmentMinuteSecond GetDateSegmentMinuteSecond()
        {
            return GetDateSegmentMinuteSecond('Z');
        }

        internal DateSegmentMinuteSecond GetDateSegmentMinuteSecond(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = entryFormat[Control.SelectionStart];

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentMinuteSecond(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        internal DateSegmentAmPm GetDateSegmentAmPm()
        {
            return GetDateSegmentAmPm('Z');
        }

        internal DateSegmentAmPm GetDateSegmentAmPm(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = 't';

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentAmPm(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        private void OnValueChanged(DateTime? newValue)
        {
            Value = newValue;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
