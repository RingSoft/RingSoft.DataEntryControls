// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine
{
    public interface IDateEditControl
    {
        string Text { get; set; }

        int SelectionStart { get; set; }

        int SelectionLength { get; set; }
    }
}
