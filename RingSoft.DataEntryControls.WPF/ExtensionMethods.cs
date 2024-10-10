// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="ExtensionMethods.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

// ReSharper disable InconsistentNaming

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Enum MapType
    /// </summary>
    public enum MapType : uint
    {
        /// <summary>
        /// The mapvk vk to VSC
        /// </summary>
        MAPVK_VK_TO_VSC = 0x0,
        /// <summary>
        /// The mapvk VSC to vk
        /// </summary>
        MAPVK_VSC_TO_VK = 0x1,
        /// <summary>
        /// The mapvk vk to character
        /// </summary>
        MAPVK_VK_TO_CHAR = 0x2,
        /// <summary>
        /// The mapvk VSC to vk ex
        /// </summary>
        MAPVK_VSC_TO_VK_EX = 0x3,
    }

    /// <summary>
    /// Class ExtensionMethods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts System.color to Media.color.
        /// </summary>
        /// <param name="drawingColor">Color of the drawing.</param>
        /// <returns>Color.</returns>
        public static Color GetMediaColor(this System.Drawing.Color drawingColor)
        {
            return Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
        }

        /// <summary>
        /// Gets the type of the parent of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The element.</param>
        /// <returns>T.</returns>
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

        /// <summary>
        /// Gets the logical parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child">The child.</param>
        /// <returns>T.</returns>
        public static T GetLogicalParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject parentObject = LogicalTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            
            return GetLogicalParent<T>(parentObject);
        }

        /// <summary>
        /// Gets the type of the parent of.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="type">The type.</param>
        /// <returns>DependencyObject.</returns>
        public static DependencyObject GetParentOfType(this DependencyObject element, Type type)
        {
            if (element == null) return null;
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            if (parent == null && ((FrameworkElement)element).Parent != null) parent = ((FrameworkElement)element).Parent;
            if (parent == null) return null;
            else if (parent.GetType() == type || parent.GetType().IsSubclassOf(type)) return parent;
            return GetParentOfType(parent, type);
        }

        /// <summary>
        /// Gets the visual child.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>T.</returns>
        public static T GetVisualChild<T>(this DependencyObject obj)
            where T : DependencyObject

        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                    return (T)child;
                else
                {
                    T childOfChild = GetVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the logical child.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>T.</returns>
        public static T GetLogicalChild<T>(this DependencyObject obj)
            where T : DependencyObject

        {
            var children = LogicalTreeHelper.GetChildren(obj);
            foreach (var child in children)
            {

                if (child is T)
                    return (T)child;
                else if (child is DependencyObject dependencyChild)
                {
                    T childOfChild = GetLogicalChild<T>(dependencyChild);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the logical children.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> GetLogicalChildren<T>(this DependencyObject obj)
            where T : DependencyObject

        {
            var result = new List<T>();
            var children = LogicalTreeHelper.GetChildren(obj);
            foreach (var child in children)
            {

                if (child is T)
                    result.Add((T)child);
                else if (child is DependencyObject dependencyChild)
                {
                    result.AddRange(GetLogicalChildren<T>(dependencyChild));
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the child controls.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="applyTemplates">if set to <c>true</c> [apply templates].</param>
        /// <returns>List&lt;Control&gt;.</returns>
        public static List<Control> GetChildControls(this DependencyObject parent, bool applyTemplates = false)
        {
            var result = new List<Control>();
            var list = parent.GetChildrenOfType<Control>();
            foreach (var control in list)
            {
                if (control is GroupBox groupBox)
                {
                    var groupControls = groupBox.GetChildControls(applyTemplates);
                    result.AddRange(groupControls);
                }
                else
                {
                    result.Add(control);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the type of the children of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <param name="applyTemplates">if set to <c>true</c> [apply templates].</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> GetChildrenOfType<T>(this DependencyObject parent, bool applyTemplates = false)
            where T : DependencyObject
        {
            var results = new List<T>();

            var descendants = new Queue<DependencyObject>();
            descendants.Enqueue(parent);

            while (descendants.Count > 0)
            {
                var currentDescendant = descendants.Dequeue();

                if (applyTemplates)
                    (currentDescendant as FrameworkElement)?.ApplyTemplate();

                var childCount = VisualTreeHelper.GetChildrenCount(currentDescendant);
                for (var i = 0; i < childCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(currentDescendant, i);
                    var addChildren = child is TabControl || child is TabItem;
                    if (child is T foundObject)
                    {
                        results.Add(foundObject);
                    }
                    else
                    {
                        addChildren = true;
                    }

                    if (addChildren)
                    {
                        if (child is Visual || child is Visual3D)
                            descendants.Enqueue(child);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Sets all child controls read only mode.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public static void SetAllChildControlsReadOnlyMode(this DependencyObject parent, bool readOnlyValue)
        {
            if (!(parent is BaseUserControl baseUserControl))
                baseUserControl = parent.GetParentOfType<BaseUserControl>();

            var children = parent.GetChildControls();
            foreach (var child in children)
            {
                baseUserControl?.SetControlReadOnlyMode(child, readOnlyValue);
            }
        }


        /// <summary>
        /// Converts to unicode.
        /// </summary>
        /// <param name="wVirtKey">The w virt key.</param>
        /// <param name="wScanCode">The w scan code.</param>
        /// <param name="lpKeyState">State of the lp key.</param>
        /// <param name="pwszBuff">The PWSZ buff.</param>
        /// <param name="cchBuff">The CCH buff.</param>
        /// <param name="wFlags">The w flags.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        /// <summary>
        /// Gets the state of the keyboard.
        /// </summary>
        /// <param name="lpKeyState">State of the lp key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// Maps the virtual key.
        /// </summary>
        /// <param name="uCode">The u code.</param>
        /// <param name="uMapType">Type of the u map.</param>
        /// <returns>System.UInt32.</returns>
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        /// <summary>
        /// Converts key value to char.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Char.</returns>
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

        /// <summary>
        /// Adds the text box context menu items.
        /// </summary>
        /// <param name="contextMenu">The context menu.</param>
        public static void AddTextBoxContextMenuItems(this ContextMenu contextMenu)
        {
            contextMenu.Items.Add(new MenuItem { Header = "Cu_t", Command = ApplicationCommands.Cut });
            contextMenu.Items.Add(new MenuItem { Header = "_Copy", Command = ApplicationCommands.Copy });
            contextMenu.Items.Add(new MenuItem { Header = "_Paste", Command = ApplicationCommands.Paste });
        }

        /// <summary>
        /// Gets the absolute placement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="relativeToScreen">if set to <c>true</c> [relative to screen].</param>
        /// <returns>Rect.</returns>
        public static Rect GetAbsolutePlacement(this FrameworkElement element, bool relativeToScreen = false)
        {
            return GetAbsolutePlacement(element, Application.Current.MainWindow, relativeToScreen);
        }
        /// <summary>
        /// Gets the absolute placement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="parentWindow">The parent window.</param>
        /// <param name="relativeToScreen">if set to <c>true</c> [relative to screen].</param>
        /// <returns>Rect.</returns>
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

        /// <summary>
        /// Converts to textalignment.
        /// </summary>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <returns>TextAlignment.</returns>
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

        /// <summary>
        /// Determines whether [is window closing] [the specified new focus].
        /// </summary>
        /// <param name="hostWindow">The host window.</param>
        /// <param name="newFocus">The new focus.</param>
        /// <returns><c>true</c> if [is window closing] [the specified new focus]; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether [is design mode] [the specified dependency object].
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns><c>true</c> if [is design mode] [the specified dependency object]; otherwise, <c>false</c>.</returns>
        public static bool IsDesignMode(this DependencyObject dependencyObject) => DesignerProperties.GetIsInDesignMode(dependencyObject);

        /// <summary>
        /// Determines whether [is user visible] [the specified container].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="container">The container.</param>
        /// <returns><c>true</c> if [is user visible] [the specified container]; otherwise, <c>false</c>.</returns>
        public static bool IsUserVisible(this FrameworkElement element, FrameworkElement container)
        {
            if (element == null)
                return false;

            if (!element.IsVisible)
                return false;
            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
        }

        /// <summary>
        /// Scrolls to top.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        public static void ScrollToTop(this TextBox textBox)
        {
            //Peter Ringering - 05/25/2023 01:48:19 PM - E-40
            var selectionStart = 0;
            var selectionLength = textBox.Text.Length;
            textBox.Select(selectionStart, selectionLength);
            var line = textBox.GetLineIndexFromCharacterIndex(textBox.SelectionStart);
            if (line >= 0)
            {
                textBox.ScrollToLine(line);
            }
        }
        /// <summary>
        /// Sets the tab focus to control.
        /// </summary>
        /// <param name="foundControl">The found control.</param>
        public static void SetTabFocusToControl(this Control foundControl)
        {
            var tabItem = foundControl.GetLogicalParent<TabItem>();
            if (tabItem != null)
            {
                var tabControl = foundControl.GetParentOfType<TabControl>();
                if (tabControl != null)
                {
                    tabControl.SelectedItem = tabItem;
                    tabControl.UpdateLayout();
                }
            }

            foundControl.Focus();
        }

        /// <summary>
        /// Gets the focused control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>IInputElement.</returns>
        public static IInputElement GetFocusedControl(this DependencyObject control)
        {
            return FocusManager.GetFocusedElement(control);
        }

        /// <summary>
        /// Determines whether [is child control] [the specified control].
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> if [is child control] [the specified control]; otherwise, <c>false</c>.</returns>
        public static bool IsChildControl(this DependencyObject parent, Control control)
        {
            var result = false;
            result = parent.GetChildControls().Contains(control);
            return result;
        }
    }
}
