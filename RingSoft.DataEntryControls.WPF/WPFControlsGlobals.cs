using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF
{
    public class DataEntryCustomContentTemplate : ObservableCollection<DataEntryCustomContentTemplateItem>
    {

    }

    public class DataEntryCustomContentTemplateItem
    {
        public int ItemId { get; set; }

        public DataTemplate DataTemplate { get; set; }

        public Key HotKey { get; set; }
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

        public static void SendKey(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(e);
                }
            }
        }
    }
}
