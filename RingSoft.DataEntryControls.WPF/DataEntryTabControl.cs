// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 10-29-2024
//
// Last Modified By : petem
// Last Modified On : 10-31-2024
// ***********************************************************************
// <copyright file="DataEntryTabControl.cs" company="RingSoft">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class DataEntryTabControl.
    /// Implements the <see cref="TabControl" />
    /// </summary>
    /// <seealso cref="TabControl" />
    public class DataEntryTabControl : TabControl
    {
        /// <summary>
        /// Handles the <see cref="E:KeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
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
