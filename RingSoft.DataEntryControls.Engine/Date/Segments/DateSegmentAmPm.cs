namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    internal class DateSegmentAmPm : DateSegment
    {
        public DateSegmentAmPm(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed, formatChar)
        {
        }

        public override bool SegmentProcessChar()
        {
            string charString = "";
            charString += CharBeingProcessed;
            charString = charString.ToUpper();
            CharBeingProcessed = charString[0];

            //if (Processor.Control.SelectionStart == SegmentStart)
            //{
            //    if (!(CharBeingProcessed == 'A' || CharBeingProcessed == 'P'))
            //        return false;
            //}
            //else
            //if (CharBeingProcessed != 'M')
            //    return false;

            return base.SegmentProcessChar();
        }
    }
}
