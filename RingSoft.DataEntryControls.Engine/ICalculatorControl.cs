namespace RingSoft.DataEntryControls.Engine
{
    public interface ICalculatorControl
    {
        string TapeText { get; set; }
        string EntryText { get; set; }
        void OnValueChanged(decimal? oldValue, decimal? newValue);
    }
}
