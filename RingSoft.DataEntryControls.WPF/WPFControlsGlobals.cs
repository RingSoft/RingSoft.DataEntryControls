using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;

namespace RingSoft.DataEntryControls.WPF
{
    public class CustomContentItem
    {
        public int Id { get; set; }

        public DataTemplate DataTemplate { get; set; }
    }

    public class ControlsUserInterface : IControlsUserInterface
    {
        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            var messageBoxImage = MessageBoxImage.Error;
            switch (icon)
            {
                case RsMessageBoxIcons.Error:
                    break;
                case RsMessageBoxIcons.Exclamation:
                    messageBoxImage = MessageBoxImage.Exclamation;
                    break;
                case RsMessageBoxIcons.Information:
                    messageBoxImage = MessageBoxImage.Information;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
            }

            MessageBox.Show(text, caption, MessageBoxButton.OK, messageBoxImage);
        }

        public MessageBoxButtonsResult ShowYesNoMessageBox(string text, string caption)
        {
            switch (MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes:
                    return MessageBoxButtonsResult.Yes;
                default:
                    return MessageBoxButtonsResult.No;
            }
        }

        public MessageBoxButtonsResult ShowYesNoCancelMessageBox(string text, string caption)
        {
            switch (MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes:
                    return MessageBoxButtonsResult.Yes;
                case MessageBoxResult.No:
                    return MessageBoxButtonsResult.No;
                default:
                    return MessageBoxButtonsResult.Cancel;
            }
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            switch (cursor)
            {
                case WindowCursorTypes.Default:
                    Mouse.OverrideCursor = null;
                    break;
                case WindowCursorTypes.Wait:
                    Mouse.OverrideCursor = Cursors.Wait;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cursor), cursor, null);
            }
        }
    }
    // ReSharper disable once InconsistentNaming
    public static class WPFControlsGlobals
    {
        public static DataEntryGridHostFactory DataEntryGridHostFactory { get; set; } = new DataEntryGridHostFactory();

        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        public static void InitUi()
        {
            ControlsGlobals.UserInterface = _userInterface;
        }
    }
}
