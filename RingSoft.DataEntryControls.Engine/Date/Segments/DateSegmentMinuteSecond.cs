namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    public class DateSegmentMinuteSecond : DateSegment
    {
        public DateSegmentMinuteSecond(DateEditProcessor processor, int segmentStart, int segmentEnd,
            char charBeingProcessed, char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed,
            formatChar)
        {
        }
    }
}
