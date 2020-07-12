namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    public class DateSegmentMinuteSecond : DateSegment
    {
        public DateSegmentMinuteSecond(DateEditProcessor processor, int segmentStart, int segmentEnd,
            char charBeingProcessed, char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed,
            formatChar)
        {
        }

        public override bool SegmentProcessChar()
        {
            if (!ValNumeric())
                return false;

            var newValue = GetNewSegmentText().ToInt();
            if (newValue > 59)
                return false;

            return base.SegmentProcessChar();
        }
    }
}
