// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-26-2023
// ***********************************************************************
// <copyright file="IControlsUserInterface.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
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
    /// Enum RsMessageBoxIcons
    /// </summary>
    public enum RsMessageBoxIcons
    {
        /// <summary>
        /// The error
        /// </summary>
        Error = 0,
        /// <summary>
        /// The exclamation
        /// </summary>
        Exclamation = 1,
        /// <summary>
        /// The information
        /// </summary>
        Information = 2
    }

    /// <summary>
    /// Enum MessageBoxButtonsResult
    /// </summary>
    public enum MessageBoxButtonsResult
    {
        /// <summary>
        /// The yes
        /// </summary>
        Yes = 0,
        /// <summary>
        /// The no
        /// </summary>
        No = 1,
        /// <summary>
        /// The cancel
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
