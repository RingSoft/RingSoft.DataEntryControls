namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    public class DateSegmentMonth : DateSegment
    {
        public DateSegmentMonth(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed, formatChar)
        {
        }

        public override bool SegmentProcessChar()
        {
            if (!ValNumeric())
                return false;

            if (Processor.Control.SelectionStart == SegmentStart)
            {
                if (CharBeingProcessed < '0' || CharBeingProcessed > '1')
                    return false;
            }
            else
            {
                var newValue = GetNewSegmentText().ToInt();
                if (newValue > 12)
                    return false;
            }

            return base.SegmentProcessChar();
        }

        public int GetMonthValue()
        {
            return GetCurrentValue().ToInt();
        }

    }
}
