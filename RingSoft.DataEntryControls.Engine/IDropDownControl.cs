namespace RingSoft.DataEntryControls.Engine
{
    public interface IDropDownControl
    {
        string Text { get; set; }

        int SelectionStart { get; set; }

        int SelectionLength { get; set; }
    }
}
