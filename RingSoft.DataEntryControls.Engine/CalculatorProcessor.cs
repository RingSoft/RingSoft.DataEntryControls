using System;
using System.Globalization;

namespace RingSoft.DataEntryControls.Engine
{
    public class CalculatorProcessor
    {
        public ICalculatorControl Control { get; }

        public CalculatorProcessor(ICalculatorControl control)
        {
            Control = control;
        }

        public void SetValue(decimal? value)
        {
            var text = string.Empty;
            if (value != null)
            {
                var newValue = (decimal) value;
                var wholeNumberText = text = newValue.ToString(CultureInfo.CurrentCulture);
                var decimalIndex = text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                    StringComparison.Ordinal);
                var decimalText = string.Empty;
                if (decimalIndex >= 0)
                {
                    wholeNumberText = text.LeftStr(decimalIndex);
                    decimalText = text.GetRightText(decimalIndex, 1);
                }
                var wholeNumber = wholeNumberText.ToDecimal();
                if (!wholeNumberText.IsNullOrEmpty())
                    text = wholeNumber.ToString("N0", CultureInfo.CurrentCulture);
                if (!decimalText.IsNullOrEmpty())
                    text += $".{decimalText}";
            }

            Control.EntryText = text;
        }
    }
}
