using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF
{
    public class BaseUserControl : UserControl
    {
        /// <summary>
        /// Gets or sets a value whether to set focus to the first editable control when the window first shows.
        /// </summary>
        /// <value><c>true</c> if [set focus to first control]; otherwise, <c>false</c>.</value>
        public bool SetFocusToFirstControl { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [enter to tab].
        /// </summary>
        /// <value><c>true</c> if [enter to tab]; otherwise, <c>false</c>.</value>
        public bool EnterToTab { get; set; }

    }
}
