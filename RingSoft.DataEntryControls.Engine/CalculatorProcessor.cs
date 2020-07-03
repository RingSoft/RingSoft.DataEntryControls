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
                text = newValue.ToString("N");
            }

            Control.EntryText = text;
        }
    }
}
