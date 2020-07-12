namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    public class DateSegment
    {
        public DateEditProcessor Processor { get; }

        public int SegmentStart { get; }

        public int SegmentEnd { get; }

        public char CharBeingProcessed { get; internal set; }

        public char FormatChar { get; }

        public DateSegment(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar)
        {
            Processor = processor;
            SegmentStart = segmentStart;
            SegmentEnd = segmentEnd;
            CharBeingProcessed = charBeingProcessed;
            FormatChar = formatChar;
        }

        public virtual bool SegmentProcessChar()
        {
            Processor.CheckDeleteAll();
            Processor.ReplaceDateCharAdvance(CharBeingProcessed, SegmentEnd);
            return true;
        }

        protected bool ValNumeric()
        {
            if (CharBeingProcessed < '0' || CharBeingProcessed > '9')
                return false;

            return true;
        }

        protected string GetNewSegmentText()
        {
            var oldText = Processor.Control.Text.MidStr(SegmentStart, (SegmentEnd - SegmentStart) + 1);
            var charIndex = Processor.Control.SelectionStart - SegmentStart;
            var newValue = oldText.LeftStr(charIndex)
                             + CharBeingProcessed
                             + oldText.RightStr((oldText.Length - charIndex) - 1);
            return newValue;
        }
        public string GetCurrentValue()
        {
            var value = "";
            if (SegmentStart >= 0 && SegmentEnd >= 0)
                value = Processor.Control.Text.MidStr(SegmentStart, (SegmentEnd - SegmentStart) + 1);
            return value;
        }

    }
}
