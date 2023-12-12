// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="DateEditProcessor.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.Date.Segments;
using System;

namespace RingSoft.DataEntryControls.Engine.Date
{
    /// <summary>
    /// Class DateEditProcessor.
    /// </summary>
    public class DateEditProcessor
    {
        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public IDateEditControl Control { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public DateTime? Value { get; private set; }

        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// The setup
        /// </summary>
        private DateEditControlSetup _setup;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateEditProcessor"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public DateEditProcessor(IDateEditControl control)
        {
            Control = control;
        }

        /// <summary>
        /// Called when [set focus].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="value">The value.</param>
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
            Control.SetSelectAll();
        }

        /// <summary>
        /// Gets the null date pattern.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetNullDatePattern()
        {
            var entryFormat = _setup.GetEntryFormat();
            var nullDate = entryFormat;

            var segments = "Hhms";

            foreach (char segmentChar in segments)
                nullDate = nullDate.Replace(segmentChar, '0');

            if (nullDate.Contains("tt"))
                nullDate = nullDate.Replace("tt", _setup.Culture.DateTimeFormat.AMDesignator);

            nullDate = nullDate.Replace("MM", "mm");

            return nullDate;
        }

        /// <summary>
        /// Called when [lost focus].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
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

        /// <summary>
        /// Validates the date.
        /// </summary>
        /// <param name="dateValue">The date value.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
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

        /// <summary>
        /// Processes the character.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="keyChar">The key character.</param>
        /// <returns>ProcessCharResults.</returns>
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

            if (Control.SelectionStart >= Control.Text.Length)
            {
                return ProcessCharResults.ValidationFailed;
            }
            if (IsCharAtIndexDateSeparator(Control.SelectionStart))
                Control.SelectionStart++;

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

        /// <summary>
        /// Sets the new date.
        /// </summary>
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

        /// <summary>
        /// Gets the active segment.
        /// </summary>
        /// <param name="cChar">The c character.</param>
        /// <param name="index">The index.</param>
        /// <returns>DateSegment.</returns>
        internal DateSegment GetActiveSegment(char cChar, int index = -1)
        {
            var entryFormat = _setup.GetEntryFormat();
            if (Control.SelectionStart >= entryFormat.Length)
                return null;

            if (index < 0)
                index = Control.SelectionStart;

            char cSegmentChar = entryFormat[index];

            //Screws up backspace past segment divider
            //var segmentString = cSegmentChar.ToString();

            //if (segmentString == _setup.Culture.DateTimeFormat.DateSeparator ||
            //    segmentString == _setup.Culture.DateTimeFormat.TimeSeparator)
            //{
            //    cSegmentChar = entryFormat[index + 1];
            //    if (index == Control.SelectionStart)
            //        Control.SelectionStart++;
            //}

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

        /// <summary>
        /// Called when [backspace key down].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <returns>ProcessCharResults.</returns>
        public ProcessCharResults OnBackspaceKeyDown(DateEditControlSetup setup)
        {
            _setup = setup;

            if (CheckDeleteAll())
                return ProcessCharResults.Processed;

            if (Control.SelectionStart > 0)
            {
                if (IsCharAtIndexDateSeparator(Control.SelectionStart - 1))
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
                Control.SelectionStart =
                    selectionStart - 1; //MS always resets SelectionStart when you change Text property.

                if (IsCharAtIndexDateSeparator(Control.SelectionStart - 1))
                    Control.SelectionStart--;
                
                SetNewDate();
            }

            return ProcessCharResults.Processed;
        }

        /// <summary>
        /// Determines whether [is character at index date separator] [the specified character index].
        /// </summary>
        /// <param name="charIndex">Index of the character.</param>
        /// <returns><c>true</c> if [is character at index date separator] [the specified character index]; otherwise, <c>false</c>.</returns>
        private bool IsCharAtIndexDateSeparator(int charIndex)
        {
            if (charIndex > 0)
            {

                if (Control.Text[charIndex] == ' ')
                    return true;

                var segmentString = Control.Text[charIndex].ToString();
                if (segmentString == _setup.Culture.DateTimeFormat.DateSeparator ||
                    segmentString == _setup.Culture.DateTimeFormat.TimeSeparator)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Called when [space key].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <returns>ProcessCharResults.</returns>
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
        /// <summary>
        /// Called when [delete key down].
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <returns>ProcessCharResults.</returns>
        public ProcessCharResults OnDeleteKeyDown(DateEditControlSetup setup)
        {
            _setup = setup;

            if (CheckDeleteAll())
                return ProcessCharResults.Processed;

            return ProcessCharResults.ValidationFailed;
        }

        /// <summary>
        /// Checks the delete all.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Pastes the text.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="newText">The new text.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Replaces the date character advance.
        /// </summary>
        /// <param name="newChar">The new character.</param>
        /// <param name="segmentEnd">The segment end.</param>
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

        /// <summary>
        /// Gets the date segment month.
        /// </summary>
        /// <returns>DateSegmentMonth.</returns>
        internal DateSegmentMonth GetDateSegmentMonth()
        {
            return GetDateSegmentMonth('Z');
        }

        /// <summary>
        /// Gets the date segment month.
        /// </summary>
        /// <param name="cChar">The c character.</param>
        /// <returns>DateSegmentMonth.</returns>
        internal DateSegmentMonth GetDateSegmentMonth(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = 'M';

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentMonth(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        /// <summary>
        /// Gets the date segment day.
        /// </summary>
        /// <returns>DateSegmentDay.</returns>
        internal DateSegmentDay GetDateSegmentDay()
        {
            return GetDateSegmentDay('Z');
        }

        /// <summary>
        /// Gets the date segment day.
        /// </summary>
        /// <param name="cChar">The c character.</param>
        /// <returns>DateSegmentDay.</returns>
        internal DateSegmentDay GetDateSegmentDay(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = 'd';

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentDay(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        /// <summary>
        /// Gets the date segment year.
        /// </summary>
        /// <returns>DateSegmentYear.</returns>
        internal DateSegmentYear GetDateSegmentYear()
        {
            return GetDateSegmentYear('Z');
        }

        /// <summary>
        /// Gets the date segment year.
        /// </summary>
        /// <param name="cChar">The c character.</param>
        /// <returns>DateSegmentYear.</returns>
        internal DateSegmentYear GetDateSegmentYear(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = 'y';

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentYear(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        /// <summary>
        /// Gets the date segment hour.
        /// </summary>
        /// <returns>DateSegmentHour.</returns>
        internal DateSegmentHour GetDateSegmentHour()
        {
            return GetDateSegmentHour('Z');
        }

        /// <summary>
        /// Gets the date segment hour.
        /// </summary>
        /// <param name="cChar">The c character.</param>
        /// <returns>DateSegmentHour.</returns>
        internal DateSegmentHour GetDateSegmentHour(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = entryFormat[Control.SelectionStart];

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentHour(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        /// <summary>
        /// Gets the date segment minute second.
        /// </summary>
        /// <returns>DateSegmentMinuteSecond.</returns>
        internal DateSegmentMinuteSecond GetDateSegmentMinuteSecond()
        {
            return GetDateSegmentMinuteSecond('Z');
        }

        /// <summary>
        /// Gets the date segment minute second.
        /// </summary>
        /// <param name="cChar">The c character.</param>
        /// <returns>DateSegmentMinuteSecond.</returns>
        internal DateSegmentMinuteSecond GetDateSegmentMinuteSecond(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = entryFormat[Control.SelectionStart];

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentMinuteSecond(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        /// <summary>
        /// Gets the date segment am pm.
        /// </summary>
        /// <returns>DateSegmentAmPm.</returns>
        internal DateSegmentAmPm GetDateSegmentAmPm()
        {
            return GetDateSegmentAmPm('Z');
        }

        /// <summary>
        /// Gets the date segment am pm.
        /// </summary>
        /// <param name="cChar">The c character.</param>
        /// <returns>DateSegmentAmPm.</returns>
        internal DateSegmentAmPm GetDateSegmentAmPm(char cChar)
        {
            var entryFormat = _setup.GetEntryFormat();
            char segmentFormatChar = 't';

            DateEditControlSetup.GetSegmentFirstLastPosition(entryFormat, segmentFormatChar, out var firstSegIndex,
                out var lastSegIndex);
            return new DateSegmentAmPm(this, firstSegIndex, lastSegIndex, cChar, segmentFormatChar);
        }

        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="newValue">The new value.</param>
        private void OnValueChanged(DateTime? newValue)
        {
            if (_setup.AllowNullValue || newValue != null)
                Value = newValue;
            else
                Value = _setup.MinimumDate ?? DateTime.Today;

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
