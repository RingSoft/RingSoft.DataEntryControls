namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    public class DateSegmentAmPm : DateSegment
    {
        public DateSegmentAmPm(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed, formatChar)
        {
        }
    }
}
