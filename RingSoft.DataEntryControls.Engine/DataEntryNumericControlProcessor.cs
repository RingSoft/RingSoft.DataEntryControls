namespace RingSoft.DataEntryControls.Engine
{
    public class DataEntryNumericControlProcessor
    {
        public DataEntryNumericEditSetup Setup { get; }

        public INumericControl Control { get; }

        public DataEntryNumericControlProcessor( INumericControl control, DataEntryNumericEditSetup setup)
        {
            Control = control;
            Setup = setup;
        }

        public bool ProcessChar(char keyChar)
        {
            return false;
        }
    }
}
