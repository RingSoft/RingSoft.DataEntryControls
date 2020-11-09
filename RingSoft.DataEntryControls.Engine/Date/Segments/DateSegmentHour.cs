namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    internal class DateSegmentHour : DateSegment
    {
        public DateSegmentHour(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed, formatChar)
        {
        }

        public override bool SegmentProcessChar()
        {
            if (!ValNumeric())
                return false;

            var newHour = GetNewSegmentText().ToInt();
            if (FormatChar == 'h')
            {
                if (Processor.Control.SelectionStart == SegmentStart)
                {
                    if (CharBeingProcessed < '0' || CharBeingProcessed > '1')
                        return false;
                }
                else
                if (newHour > 12)
                    return false;
            }
            else
            {
                if (Processor.Control.SelectionStart == SegmentStart)
                {
                    if (CharBeingProcessed < '0' || CharBeingProcessed > '2')
                        return false;
                }
                else
                if (newHour > 23)
                    return false;
            }
            return base.SegmentProcessChar();
        }

    }
}
