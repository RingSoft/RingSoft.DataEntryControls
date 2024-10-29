using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF
{
    public class DataEntryTabControl : TabControl
    {
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    return;
                }
            }
            base.OnKeyDown(e);
        }
    }
}
