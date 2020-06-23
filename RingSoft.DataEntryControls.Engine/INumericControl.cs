namespace RingSoft.DataEntryControls.Engine
{
    public interface INumericControl
    {
        string Text { get; set; }

        int SelectionStart { get; set; }

        int SelectionLength { get; set; }

        void OnInvalidChar();
    }
}
