namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    internal class DateSegmentDay : DateSegment
    {
        public DateSegmentDay(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed, formatChar)
        {
        }

        public override bool SegmentProcessChar()
        {
            if (!ValNumeric())
                return false;

            var month = Processor.GetDateSegmentMonth();
            var monthValue = month.GetMonthValue();
            var year = Processor.GetDateSegmentYear();
            var yearValue = year.GetYearValue();

            if (Processor.Control.SelectionStart == SegmentStart)
            {
                if (CharBeingProcessed < '0' || CharBeingProcessed > '3')
                    return false;
                if (monthValue == 2 && CharBeingProcessed == '3')
                    return false;
            }

            var newValue = GetNewSegmentText().ToInt();
            if (newValue > 31)
                return false;

            if (monthValue > 0)
                if (newValue > GetLastDay(monthValue, yearValue))
                    return false;

            return base.SegmentProcessChar();
        }

        internal static int GetLastDay(int nMonth, int nYear)
        {
            //int nToday = 
            int nLastDay = 0;
            switch (nMonth)
            {
                case 1:	//January
                case 3:	//March
                case 5:	//May
                case 7:	//July
                case 8:	//August
                case 10:	//October
                case 12:	//December
                    nLastDay = 31;
                    break;
                case 4: //April
                case 6: //June
                case 9: //September
                case 11: //November
                    nLastDay = 30;
                    break;
                case 2: //February (Who came up with this kind of a wacky month???)
                    if (nYear % 4 == 0 && nYear % 100 != 0 || nYear % 400 == 0)
                        //Leap Year
                        nLastDay = 29;
                    else
                        nLastDay = 28;
                    break;
            }
            return nLastDay;
        }
    }
}
