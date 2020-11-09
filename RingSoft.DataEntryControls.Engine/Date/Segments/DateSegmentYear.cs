namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    internal class DateSegmentYear : DateSegment
    {
        public DateSegmentYear(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed, formatChar)
        {
        }

        public override bool SegmentProcessChar()
        {
            if (!ValNumeric())
                return false;

            return base.SegmentProcessChar();
        }

        public int GetYearValue()
        {
            return GetCurrentValue().ToInt();
        }
    }
}
