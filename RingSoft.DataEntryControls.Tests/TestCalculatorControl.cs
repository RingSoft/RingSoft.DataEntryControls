using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Tests
{
    public class TestCalculatorControl : ICalculatorControl
    {
        public string EquationText { get; set; }
        public string EntryText { get; set; }
        public bool MemoryRecallEnabled { get; set; }
        public bool MemoryClearEnabled { get; set; }
        public bool MemoryStoreEnabled { get; set; }
        public bool MemoryPlusEnabled { get; set; }
        public bool MemoryMinusEnabled { get; set; }
        public bool MemoryStatusVisible { get; set; }

        public void OnValueChanged(decimal? oldValue, decimal? newValue)
        {
            
        }
    }
}
