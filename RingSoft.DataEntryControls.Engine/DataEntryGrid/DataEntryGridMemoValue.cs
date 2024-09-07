// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DataEntryGridMemoValue.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// A grid control memo line.
    /// </summary>
    public class GridMemoValueLine
    {
        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; }

        /// <summary>
        /// Gets a value indicating whether [cr lf].
        /// </summary>
        /// <value><c>true</c> if [cr lf]; otherwise, <c>false</c>.</value>
        public bool CrLf { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMemoValueLine" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="crLf">if set to <c>true</c> [cr lf].</param>
        public GridMemoValueLine(string text, bool crLf)
        {
            Text = text;
            CrLf = crLf;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Text;
        }
    }
    /// <summary>
    /// A grid memo value.
    /// </summary>
    public class DataEntryGridMemoValue
    {
        /// <summary>
        /// The text
        /// </summary>
        private string _text;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                SetText(_text);
            }
        }

        /// <summary>
        /// Gets the maximum characters per line.
        /// </summary>
        /// <value>The maximum characters per line.</value>
        public int MaxCharactersPerLine { get; }

        /// <summary>
        /// Gets the lines.
        /// </summary>
        /// <value>The lines.</value>
        public IReadOnlyList<GridMemoValueLine> Lines => _lines;

        /// <summary>
        /// The lines
        /// </summary>
        private List<GridMemoValueLine> _lines = new List<GridMemoValueLine>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridMemoValue" /> class.
        /// </summary>
        /// <param name="maxCharsPerLine">The maximum chars per line.</param>
        public DataEntryGridMemoValue(int maxCharsPerLine)
        {
            MaxCharactersPerLine = maxCharsPerLine;
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="text">The text.</param>
        private void SetText(string text)
        {
            if (MaxCharactersPerLine == 0)
                return;

            _lines.Clear();

            var remainder = text;
            while (!string.IsNullOrEmpty(remainder))
            {
                string newLine;
                if (remainder.Length < MaxCharactersPerLine)
                    newLine = remainder;
                else
                    newLine = remainder.LeftStr(MaxCharactersPerLine);

                var crLfPos = newLine.IndexOf("\r\n", StringComparison.Ordinal);
                if (crLfPos >= 0)
                {
                    newLine = newLine.LeftStr(crLfPos + 2);
                    _lines.Add(new GridMemoValueLine(newLine.Trim(), true));
                }
                else
                {
                    if (remainder.Length < MaxCharactersPerLine)
                    {
                        _lines.Add(new GridMemoValueLine(remainder.Trim(), false));
                        remainder = string.Empty;
                    }
                    else
                    {
                        var spacePos = newLine.LastIndexOf(' ');
                        if (spacePos > 0)
                        {
                            newLine = newLine.LeftStr(spacePos);
                        }

                        _lines.Add(new GridMemoValueLine(newLine.Trim(), false));
                    }
                }
                if (!remainder.IsNullOrEmpty())
                    remainder = remainder.RightStr(remainder.Length - newLine.Length);
            }
        }

        /// <summary>
        /// Clears the lines and text.
        /// </summary>
        public void Clear()
        {
            _lines.Clear();
            _text = string.Empty;
        }

        /// <summary>
        /// Adds a memo line.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="crLf">if set to <c>true</c> [cr lf].</param>
        public void AddLine(string text, bool crLf)
        {
            _lines.Add(new GridMemoValueLine(text, crLf));
            _text += text;
            if (crLf)
                _text += "\r\n";
            else
                _text += ' ';
        }
    }
}
