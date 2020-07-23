using RingSoft.DataEntryControls.Engine.Date.Segments;
using System;

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
                Control.Text = GetNullDatePattern();
            }
            else
            {
                var newValue = (DateTime) value;
                Control.Text = newValue.ToString(entryFormat, _setup.Culture);
            }
        }

        public string GetNullDatePattern()
        {
            var entryFormat = _setup.GetEntryFormat();
            var nullDate = entryFormat;
            var segments = "Hhms";

            foreach (char segmentChar in segments)
                nullDate = nullDate.Replace(segmentChar, '0');

            if (nullDate.Contains("tt"))
                nullDate = nullDate.Replace("tt", _setup.Culture.DateTimeFormat.AMDesignator);

            return nullDate;
        }

        public DateTime? OnLostFocus(DateEditControlSetup setup, DateTime? value)
        {
            _setup = setup;
            DateTime? result = null;

            if (value == null)
            {
                Control.Text = string.Empty;
            }
            else
            {
                var dateTimeValue = (DateTime) value;
                result = ValidateDate(dateTimeValue);

                if (result != null)
                    dateTimeValue = (DateTime) result;

                Control.Text = dateTimeValue.ToString(_setup.GetDisplayFormat(), _setup.Culture);
            }

            return result;
        }

        private DateTime? ValidateDate(DateTime dateValue)
        {
            DateTime? result = null;
            if (_setup.MaximumDate != null)
            {
                if (dateValue > _setup.MaximumDate)
                {
                    result = _setup.MaximumDate;
                }
            }

            if (_setup.MinimumDate != null)
            {
                if (dateValue < _setup.MinimumDate)
                {
                    result = _setup.MinimumDate;
                }
            }

            return result;
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
                SetNewDate();
                return ProcessCharResults.Processed;
            }

            return ProcessCharResults.ValidationFailed;
        }

        private void SetNewDate()
        {
            if (Control.Text.TryParseDateTime(out var newDate, _setup.Culture))
            {
                OnValueChanged(newDate);
            }
            else
            {
                OnValueChanged(null);
            }
        }

        internal DateSegment GetActiveSegment(char cChar, int index = -1)
        {
            var entryFormat = _setup.GetEntryFormat();
            if (Control.SelectionStart >= entryFormat.Length)
                return null;

            if (index < 0)
                index = Control.SelectionStart;

            char cSegmentChar = entryFormat[index];
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

            if (Control.SelectionStart > 0)
            {
                if (GetActiveSegment('Z', Control.SelectionStart - 1) == null)
                    //We're backspacing in the divider area.  Set selection to go back 1 now.
                    Control.SelectionStart--;

                var left = "";
                if (Control.SelectionStart > 1)
                    left = Control.Text.LeftStr(Control.SelectionStart - 1);

                var nullPattern = GetNullDatePattern();
                var nullChar = nullPattern[Control.SelectionStart - 1];
                var newText = left
                                + nullChar
                                + Control.Text.RightStr(Control.Text.Length - Control.SelectionStart);

                var selectionStart = Control.SelectionStart;
                Control.Text = newText;
                Control.SelectionStart = selectionStart - 1;  //MS always resets SelectionStart when you change Text property.
                SetNewDate();
            }
            
            return ProcessCharResults.Processed;
        }

        public ProcessCharResults OnSpaceKey(DateEditControlSetup setup)
        {
            _setup = setup;

            if (CheckDeleteAll())
                return ProcessCharResults.Processed;

            var nullDatePattern = GetNullDatePattern();
            var entryFormat = _setup.GetEntryFormat();
            if (Control.SelectionStart >= entryFormat.Length)
                return ProcessCharResults.ValidationFailed;

            char segmentChar = entryFormat[Control.SelectionStart];
            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentChar, out _, out var lastSegIndex);
            ReplaceDateCharAdvance(nullDatePattern[Control.SelectionStart], lastSegIndex);

            SetNewDate();
            return ProcessCharResults.Processed;
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
                Control.Text = GetNullDatePattern();
                Control.SelectionStart = Control.SelectionLength = 0;
                OnValueChanged(null);
                return true;
            }

            return false;
        }

        public bool PasteText(DateEditControlSetup setup, string newText)
        {
            _setup = setup;
            if (newText.TryParseDateTime(out var result, _setup.Culture))
            {
                Control.Text = result.ToString(_setup.GetEntryFormat(), _setup.Culture);
                Control.SelectionStart = Control.Text.Length - 1;
                Control.SelectionLength = 0;
                OnValueChanged(result);
                return true;
            }

            if (Value == null)
            {
                Control.Text = GetNullDatePattern();
            }
            else
            {
                var currentValue = (DateTime) Value;
                Control.Text = currentValue.ToString(_setup.GetEntryFormat(), _setup.Culture);
            }
            Control.SelectionStart = Control.Text.Length - 1;
            Control.SelectionLength = 0;
            OnValueChanged(Value);

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
