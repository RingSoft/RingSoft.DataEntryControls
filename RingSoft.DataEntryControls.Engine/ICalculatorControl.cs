namespace RingSoft.DataEntryControls.Engine
{
    public interface ICalculatorControl
    {
        string TapeText { get; set; }
        string EntryText { get; set; }
        bool MemoryRecallEnabled { get; set; }
        bool MemoryClearEnabled { get; set; }
        bool MemoryStatusVisible { get; set; }
        
        void OnValueChanged(decimal? oldValue, decimal? newValue);
    }
}
