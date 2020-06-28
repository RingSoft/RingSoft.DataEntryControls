using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Tests
{
    public class TestNumericControl : INumericControl
    {
        public string Text { get; set; }
        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }
    }
}
