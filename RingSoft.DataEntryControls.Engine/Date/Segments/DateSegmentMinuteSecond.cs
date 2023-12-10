﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DateSegmentMinuteSecond.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    /// <summary>
    /// Class DateSegmentMinuteSecond.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.Date.Segments.DateSegment" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.Date.Segments.DateSegment" />
    internal class DateSegmentMinuteSecond : DateSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateSegmentMinuteSecond"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="segmentStart">The segment start.</param>
        /// <param name="segmentEnd">The segment end.</param>
        /// <param name="charBeingProcessed">The character being processed.</param>
        /// <param name="formatChar">The format character.</param>
        public DateSegmentMinuteSecond(DateEditProcessor processor, int segmentStart, int segmentEnd,
            char charBeingProcessed, char formatChar) : base(processor, segmentStart, segmentEnd, charBeingProcessed,
            formatChar)
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

            var newValue = GetNewSegmentText().ToInt();
            if (newValue > 59)
                return false;

            return base.SegmentProcessChar();
        }
    }
}
