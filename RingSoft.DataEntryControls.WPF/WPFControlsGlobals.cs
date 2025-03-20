﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 01-14-2025
// ***********************************************************************
// <copyright file="WPFControlsGlobals.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class that displays custom content to user.
    /// Implements the <see cref="System.Collections.ObjectModel.ObservableCollection{RingSoft.DataEntryControls.WPF.DataEntryCustomContentTemplateItem}" />
    /// </summary>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{RingSoft.DataEntryControls.WPF.DataEntryCustomContentTemplateItem}" />
    public class DataEntryCustomContentTemplate : ObservableCollection<DataEntryCustomContentTemplateItem>
    {

    }

    /// <summary>
    /// A custom control content item.
    /// </summary>
    public class DataEntryCustomContentTemplateItem
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>The item identifier.</value>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the data template.
        /// </summary>
        /// <value>The data template.</value>
        public DataTemplate DataTemplate { get; set; }

        /// <summary>
        /// Gets or sets the hot key.
        /// </summary>
        /// <value>The hot key.</value>
        public Key HotKey { get; set; }
    }

    /// <summary>
    /// Creates a vmUiControl based on ctrol.
    /// </summary>
    public class VmUiControlFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VmUiControlFactory" /> class.
        /// </summary>
        public VmUiControlFactory()
        {
            WPFControlsGlobals.VmUiFactory = this;
        }
        /// <summary>
        /// Creates the UI control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="uiCommand">The UI command.</param>
        /// <returns>VmUiControl.</returns>
        public virtual VmUiControl CreateUiControl(Control control, UiCommand uiCommand)
        {
            var result = new VmUiControl(control, uiCommand);
            return result;
        }
    }

    /// <summary>
    /// The user interface.
    /// Implements the <see cref="IControlsUserInterface" />
    /// </summary>
    /// <seealso cref="IControlsUserInterface" />
    public class ControlsUserInterface : IControlsUserInterface
    {
        /// <summary>
        /// The active window
        /// </summary>
        private static Window _activeWindow;
        /// <summary>
        /// The cursor type
        /// </summary>
        private static WindowCursorTypes _cursorType = WindowCursorTypes.Default;

        /// <summary>
        /// Sets the active window.
        /// </summary>
        /// <param name="activeWindow">The active window.</param>
        public static void SetActiveWindow(Window activeWindow)
        {
            _activeWindow = activeWindow;
            _activeWindow.Closed += (sender, args) => _activeWindow = null;
        }
        /// <summary>
        /// Gets the active window.
        /// </summary>
        /// <returns>Window.</returns>
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

        /// <summary>
        /// Shows the message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">icon - null</exception>
        public async Task ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            var activeCursor = GetWindowCursor(); 
            SetWindowCursor(WindowCursorTypes.Default);

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
            SetWindowCursor(activeCursor);
        }

        /// <summary>
        /// Shows the yes no message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>Task&lt;MessageBoxButtonsResult&gt;.</returns>
        public async Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            var activeCursor = GetWindowCursor();
            SetWindowCursor(WindowCursorTypes.Default);

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

            SetWindowCursor(activeCursor);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    return MessageBoxButtonsResult.Yes;
                default:
                    return MessageBoxButtonsResult.No;
            }
        }

        /// <summary>
        /// Shows the yes no cancel message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>Task&lt;MessageBoxButtonsResult&gt;.</returns>
        public async Task<MessageBoxButtonsResult> ShowYesNoCancelMessageBox(string text, string caption,
            bool playSound = false)
        {
            var activeCursor = GetWindowCursor();
            SetWindowCursor(WindowCursorTypes.Default);

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

            SetWindowCursor(activeCursor);

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

        /// <summary>
        /// Sets the window cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            var activeWindow = GetActiveWindow();
            if (activeWindow != null)
            {
                activeWindow.Dispatcher.Invoke(() =>
                {
                    _cursorType = cursor;
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

        //Peter Ringering - 01/14/2025 02:05:25 PM - E-109
        /// <summary>
        /// Gets the window cursor.
        /// </summary>
        /// <returns>WindowCursorTypes.</returns>
        public WindowCursorTypes GetWindowCursor()
        {
            return _cursorType;
        }
    }
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Class WPFControlsGlobals.
    /// </summary>
    public static class WPFControlsGlobals
    {
        /// <summary>
        /// Gets the active window.
        /// </summary>
        /// <value>The active window.</value>
        public static Window ActiveWindow => ControlsUserInterface.GetActiveWindow();

        /// <summary>
        /// Gets the data entry grid host factory.
        /// </summary>
        /// <value>The data entry grid host factory.</value>
        public static DataEntryGridHostFactory DataEntryGridHostFactory { get; internal set; } = new DataEntryGridHostFactory();

        /// <summary>
        /// Gets the vm UI factory.
        /// </summary>
        /// <value>The vm UI factory.</value>
        public static VmUiControlFactory VmUiFactory { get; internal set; } = new VmUiControlFactory();

        /// <summary>
        /// The user interface
        /// </summary>
        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        /// <summary>
        /// Initializes static members of the <see cref="WPFControlsGlobals" /> class.
        /// </summary>
        static WPFControlsGlobals()
        {
            InitUi();
        }
        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private static void InitUi()
        {
            ControlsGlobals.UserInterface = _userInterface;
        }

        /// <summary>
        /// Sends the key.
        /// </summary>
        /// <param name="key">The key.</param>
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

        /// <summary>
        /// Does the events.
        /// </summary>
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(delegate { }));
        }
    }
}
