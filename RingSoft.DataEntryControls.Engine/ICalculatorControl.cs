namespace RingSoft.DataEntryControls.Engine
{
    public interface ICalculatorControl
    {
        string TapeText { get; set; }
        string EntryText { get; set; }

        int Precision { get; }
    }
}
