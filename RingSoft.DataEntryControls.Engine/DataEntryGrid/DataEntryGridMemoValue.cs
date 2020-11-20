using System;
using System.Collections.Generic;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class GridMemoValueLine
    {
        public string Text { get; }

        public bool CrLf { get; }

        public GridMemoValueLine(string text, bool crLf)
        {
            Text = text;
            CrLf = crLf;
        }

        public override string ToString()
        {
            return Text;
        }
    }
    public class DataEntryGridMemoValue
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                SetText(_text);
            }
        }

        public int MaxCharactersPerLine { get; }

        public IReadOnlyList<GridMemoValueLine> Lines => _lines;

        private List<GridMemoValueLine> _lines = new List<GridMemoValueLine>();

        public DataEntryGridMemoValue(int maxCharsPerLine)
        {
            MaxCharactersPerLine = maxCharsPerLine;
        }

        private void SetText(string text)
        {
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

        public void Clear()
        {
            _lines.Clear();
            _text = string.Empty;
        }

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
