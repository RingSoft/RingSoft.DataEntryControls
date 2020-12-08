using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
// ReSharper disable InconsistentNaming

namespace RingSoft.DataEntryControls.WPF
{
    public enum MapType : uint
    {
        MAPVK_VK_TO_VSC = 0x0,
        MAPVK_VSC_TO_VK = 0x1,
        MAPVK_VK_TO_CHAR = 0x2,
        MAPVK_VSC_TO_VK_EX = 0x3,
    }

    public static class ExtensionMethods
    {
        public static Color GetMediaColor(this System.Drawing.Color drawingColor)
        {
            return Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
        }

        public static T GetParentOfType<T>(this DependencyObject element) where T : DependencyObject
        {
            Type type = typeof(T);
            if (element == null) return null;
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            if (parent == null && ((FrameworkElement)element).Parent != null) parent = ((FrameworkElement)element).Parent;
            if (parent == null) return null;
            else if (parent.GetType() == type || parent.GetType().IsSubclassOf(type)) return parent as T;
            return GetParentOfType<T>(parent);
        }

        public static DependencyObject GetParentOfType(this DependencyObject element, Type type)
        {
            if (element == null) return null;
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            if (parent == null && ((FrameworkElement)element).Parent != null) parent = ((FrameworkElement)element).Parent;
            if (parent == null) return null;
            else if (parent.GetType() == type || parent.GetType().IsSubclassOf(type)) return parent;
            return GetParentOfType(parent, type);
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(this Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                {
                    ch = stringBuilder[0];
                    break;
                }
                default:
                {
                    ch = stringBuilder[0];
                    break;
                }
            }
            return ch;
        }

        public static void AddTextBoxContextMenuItems(this ContextMenu contextMenu)
        {
            contextMenu.Items.Add(new MenuItem { Header = "Cu_t", Command = ApplicationCommands.Cut });
            contextMenu.Items.Add(new MenuItem { Header = "_Copy", Command = ApplicationCommands.Copy });
            contextMenu.Items.Add(new MenuItem { Header = "_Paste", Command = ApplicationCommands.Paste });
        }

        public static Rect GetAbsolutePlacement(this FrameworkElement element, bool relativeToScreen = false)
        {
            return GetAbsolutePlacement(element, Application.Current.MainWindow, relativeToScreen);
        }
        public static Rect GetAbsolutePlacement(this FrameworkElement element, Window parentWindow, bool relativeToScreen = false)
        {
            var absolutePos = element.PointToScreen(new Point(0, 0));
            if (relativeToScreen)
            {
                return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
            }

            if (parentWindow != null)
            {
                var posPW = parentWindow.PointToScreen(new Point(0, 0));
                absolutePos = new Point(absolutePos.X - posPW.X, absolutePos.Y - posPW.Y);
            }

            return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
        }

        public static TextAlignment ToTextAlignment(
            this HorizontalAlignment horizontalAlignment)
        {
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return TextAlignment.Left;
                case HorizontalAlignment.Right:
                    return TextAlignment.Right;
                default:
                    return TextAlignment.Center;
            }
        }

        public static bool IsWindowClosing(this Window hostWindow, IInputElement newFocus)
        {
            var result = false;
            if (newFocus is DependencyObject dependencyObject)
            {
                if ((newFocus is Window))
                    result = true;
                else
                {
                    var newFocusWindow = Window.GetWindow(dependencyObject);
                    if (newFocusWindow != null)
                        result = !newFocusWindow.Equals(hostWindow);
                }
            }

            return result;
        }

        public static bool IsDesignMode(this DependencyObject dependencyObject) => DesignerProperties.GetIsInDesignMode(dependencyObject);
    }
}
