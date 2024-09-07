// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="IDropDownCalendar.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DropDownEditControls
{
    /// <summary>
    /// Interface IDropDownCalendar
    /// </summary>
    public interface IDropDownCalendar
    {
        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        Control Control { get; }

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        /// <value>The selected date.</value>
        DateTime? SelectedDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum date.
        /// </summary>
        /// <value>The maximum date.</value>
        DateTime? MaximumDate { get; set; }

        /// <summary>
        /// Gets or sets the minimum date.
        /// </summary>
        /// <value>The minimum date.</value>
        DateTime? MinimumDate { get; set; }

        /// <summary>
        /// Occurs when [selected date changed].
        /// </summary>
        event EventHandler SelectedDateChanged;

        /// <summary>
        /// Occurs when [date picked].
        /// </summary>
        event EventHandler DatePicked;
    }
}
