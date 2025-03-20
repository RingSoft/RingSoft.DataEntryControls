// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 01-14-2025
// ***********************************************************************
// <copyright file="IControlsUserInterface.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Threading.Tasks;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Enum WindowCursorTypes
    /// </summary>
    public enum WindowCursorTypes
    {
        /// <summary>
        /// The default
        /// </summary>
        Default = 0,
        /// <summary>
        /// The wait
        /// </summary>
        Wait = 1
    }

    /// <summary>
    /// Message Box icons.
    /// </summary>
    public enum RsMessageBoxIcons
    {
        /// <summary>
        /// Error
        /// </summary>
        Error = 0,
        /// <summary>
        /// Exclamation
        /// </summary>
        Exclamation = 1,
        /// <summary>
        /// Information
        /// </summary>
        Information = 2
    }

    /// <summary>
    /// Message Box result
    /// </summary>
    public enum MessageBoxButtonsResult
    {
        /// <summary>
        /// Yes
        /// </summary>
        Yes = 0,
        /// <summary>
        /// No
        /// </summary>
        No = 1,
        /// <summary>
        /// Cancel
        /// </summary>
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

        /// <summary>
        /// Gets the window cursor.
        /// </summary>
        /// <returns>WindowCursorTypes.</returns>
        WindowCursorTypes GetWindowCursor();

        /// <summary>
        /// Shows the message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>Task.</returns>
        Task ShowMessageBox(string text, string caption, RsMessageBoxIcons icon);

        /// <summary>
        /// Shows the yes no message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>Task&lt;MessageBoxButtonsResult&gt;.</returns>
        Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false);

        /// <summary>
        /// Shows the yes no cancel message box.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="playSound">if set to <c>true</c> [play sound].</param>
        /// <returns>Task&lt;MessageBoxButtonsResult&gt;.</returns>
        Task<MessageBoxButtonsResult> ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false);
    }
}
