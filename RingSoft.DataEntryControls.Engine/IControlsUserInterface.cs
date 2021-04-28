namespace RingSoft.DataEntryControls.Engine
{
    public enum WindowCursorTypes
    {
        Default = 0,
        Wait = 1
    }

    public enum RsMessageBoxIcons
    {
        Error = 0,
        Exclamation = 1,
        Information = 2
    }

    public enum MessageBoxButtonsResult
    {
        Yes = 0,
        No = 1,
        Cancel = 2
    }

    /// <summary>
    /// Implement this to so DbLookup classes can interact with the user interface.
    /// </summary>
    public interface IControlsUserInterface
    {
        /// <summary>
        /// Sets the window cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        void SetWindowCursor(WindowCursorTypes cursor);

        void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon);

        MessageBoxButtonsResult ShowYesNoMessageBox(string text, string caption, bool playSound = false);

        MessageBoxButtonsResult ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false);
    }
}
