using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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

    public class VmUiControlFactory
    {
        public virtual VmUiControl CreateUiControl(Control control, UiCommand uiCommand)
        {
            var result = new VmUiControl(control, uiCommand);
            return result;
        }
    }

    public class ControlsUserInterface : IControlsUserInterface
    {
        private static Window _activeWindow;

        public static void SetActiveWindow(Window activeWindow)
        {
            _activeWindow = activeWindow;
            _activeWindow.Closed += (sender, args) => _activeWindow = null;
        }
        public static Window GetActiveWindow()
        {
            try
            {
                var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                return activeWindow;
            }
            catch (Exception e)
            {
                return _activeWindow;
            }
            return null;
        }

        public async Task ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
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

            var activeWindow = GetActiveWindow();
            if (activeWindow == null)
            {
                MessageBox.Show(text, caption, MessageBoxButton.OK, messageBoxImage);
            }
            else
            {
                activeWindow.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(text, caption, MessageBoxButton.OK, messageBoxImage);
                });
            }
        }

        public async Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {

            var messageBoxResult = MessageBoxResult.Yes;
            var activeWindow = GetActiveWindow();
            if (activeWindow == null)
            {
                if (playSound)
                    SystemSounds.Exclamation.Play();

                messageBoxResult = MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            else
            {
                activeWindow.Dispatcher.Invoke(() =>
                {
                    if (playSound)
                        SystemSounds.Exclamation.Play();

                    messageBoxResult = MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
                });
            }

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    return MessageBoxButtonsResult.Yes;
                default:
                    return MessageBoxButtonsResult.No;
            }
        }

        public async Task<MessageBoxButtonsResult> ShowYesNoCancelMessageBox(string text, string caption,
            bool playSound = false)
        {
            var messageBoxResult = MessageBoxResult.Yes;
            var activeWindow = GetActiveWindow();
            if (activeWindow == null)
            {
                if (playSound)
                    SystemSounds.Exclamation.Play();

                messageBoxResult =
                    MessageBox.Show(text, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            }
            else
            {
                activeWindow.Dispatcher.Invoke(() =>
                {
                    if (playSound)
                        SystemSounds.Exclamation.Play();

                    messageBoxResult =
                        MessageBox.Show(text, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                });
            }

            switch (messageBoxResult)
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
            var activeWindow = GetActiveWindow();
            if (activeWindow != null)
            {
                activeWindow.Dispatcher.Invoke(() =>
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
                });
            }
        }
    }
    // ReSharper disable once InconsistentNaming
    public static class WPFControlsGlobals
    {
        public static Window ActiveWindow => ControlsUserInterface.GetActiveWindow();

        public static DataEntryGridHostFactory DataEntryGridHostFactory { get; set; } = new DataEntryGridHostFactory();

        public static VmUiControlFactory VmUiFactory { get; set; } = new VmUiControlFactory();

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

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(delegate { }));
        }
    }
}
