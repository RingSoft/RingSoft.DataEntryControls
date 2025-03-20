// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DateSegmentDay.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    /// <summary>
    /// Class DateSegmentDay.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.Date.Segments.DateSegment" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.Date.Segments.DateSegment" />
    internal class DateSegmentDay : DateSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateSegmentDay" /> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="segmentStart">The segment start.</param>
        /// <param name="segmentEnd">The segment end.</param>
        /// <param name="charBeingProcessed">The character being processed.</param>
        /// <param name="formatChar">The format character.</param>
        public DateSegmentDay(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed, formatChar)
        {
        }

        /// <summary>
        /// Segments the process character.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Gets the last day.
        /// </summary>
        /// <param name="nMonth">The month.</param>
        /// <param name="nYear">The n year.</param>
        /// <returns>System.Int32.</returns>
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
