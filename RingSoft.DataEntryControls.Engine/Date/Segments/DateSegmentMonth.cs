﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DateSegmentMonth.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    /// <summary>
    /// Class DateSegmentMonth.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.Date.Segments.DateSegment" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.Date.Segments.DateSegment" />
    internal class DateSegmentMonth : DateSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateSegmentMonth" /> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="segmentStart">The segment start.</param>
        /// <param name="segmentEnd">The segment end.</param>
        /// <param name="charBeingProcessed">The character being processed.</param>
        /// <param name="formatChar">The format character.</param>
        public DateSegmentMonth(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
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

        /// <summary>
        /// Gets the month value.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetMonthValue()
        {
            return GetCurrentValue().ToInt();
        }

    }
}
