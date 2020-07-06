﻿namespace RingSoft.DataEntryControls.Engine
{
    public interface ICalculatorControl
    {
        string EquationText { get; set; }
        string EntryText { get; set; }
        bool MemoryRecallEnabled { get; set; }
        bool MemoryClearEnabled { get; set; }
        bool MemoryStoreEnabled { get; set; }
        bool MemoryPlusEnabled { get; set; }
        bool MemoryMinusEnabled { get; set; }
        bool MemoryStatusVisible { get; set; }
        
        void OnValueChanged(decimal? oldValue, decimal? newValue);
    }
}
