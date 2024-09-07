// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DateSegment.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.Date.Segments
{
    /// <summary>
    /// A date segment.
    /// </summary>
    internal class DateSegment
    {
        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <value>The processor.</value>
        public DateEditProcessor Processor { get; }

        /// <summary>
        /// Gets the segment start.
        /// </summary>
        /// <value>The segment start.</value>
        public int SegmentStart { get; }

        /// <summary>
        /// Gets the segment end.
        /// </summary>
        /// <value>The segment end.</value>
        public int SegmentEnd { get; }

        /// <summary>
        /// Gets or sets the character being processed.
        /// </summary>
        /// <value>The character being processed.</value>
        public char CharBeingProcessed { get; internal set; }

        /// <summary>
        /// Gets the format character.
        /// </summary>
        /// <value>The format character.</value>
        public char FormatChar { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateSegment" /> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="segmentStart">The segment start.</param>
        /// <param name="segmentEnd">The segment end.</param>
        /// <param name="charBeingProcessed">The character being processed.</param>
        /// <param name="formatChar">The format character.</param>
        public DateSegment(DateEditProcessor processor, int segmentStart, int segmentEnd, char charBeingProcessed,
            char formatChar)
        {
            Processor = processor;
            SegmentStart = segmentStart;
            SegmentEnd = segmentEnd;
            CharBeingProcessed = charBeingProcessed;
            FormatChar = formatChar;
        }

        /// <summary>
        /// Segments the process character.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool SegmentProcessChar()
        {
            Processor.CheckDeleteAll();
            Processor.ReplaceDateCharAdvance(CharBeingProcessed, SegmentEnd);
            return true;
        }

        /// <summary>
        /// Validates the character.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool ValNumeric()
        {
            if (CharBeingProcessed < '0' || CharBeingProcessed > '9')
                return false;

            return true;
        }

        /// <summary>
        /// Gets the new segment text.
        /// </summary>
        /// <returns>System.String.</returns>
        protected string GetNewSegmentText()
        {
            var oldText = Processor.Control.Text.MidStr(SegmentStart, (SegmentEnd - SegmentStart) + 1);
            var charIndex = Processor.Control.SelectionStart - SegmentStart;
            var newValue = oldText.LeftStr(charIndex)
                             + CharBeingProcessed
                             + oldText.RightStr((oldText.Length - charIndex) - 1);
            return newValue;
        }
        /// <summary>
        /// Gets the current value.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetCurrentValue()
        {
            var value = "";
            if (SegmentStart >= 0 && SegmentEnd >= 0)
                value = Processor.Control.Text.MidStr(SegmentStart, (SegmentEnd - SegmentStart) + 1);
            return value;
        }

    }
}
