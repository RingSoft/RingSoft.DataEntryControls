// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DateSegmentHour.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    /// <summary>
    /// Class DateSegmentHour.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.Date.Segments.DateSegment" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.Date.Segments.DateSegment" />
    internal class DateSegmentHour : DateSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateSegmentHour" /> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="segmentStart">The segment start.</param>
        /// <param name="segmentEnd">The segment end.</param>
        /// <param name="charBeingProcessed">The character being processed.</param>
        /// <param name="formatChar">The format character.</param>
        public DateSegmentHour(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
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
