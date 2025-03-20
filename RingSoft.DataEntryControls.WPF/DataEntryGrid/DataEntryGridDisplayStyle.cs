// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridDisplayStyle.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Media;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// Grid row/cell display style.
    /// </summary>
    public class DataEntryGridDisplayStyle
    {
        /// <summary>
        /// Gets or sets the display identifier.
        /// </summary>
        /// <value>The display identifier.</value>
        public int DisplayId { get; set; }

        /// <summary>
        /// Gets or sets the background brush.
        /// </summary>
        /// <value>The background brush.</value>
        public Brush BackgroundBrush { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush.
        /// </summary>
        /// <value>The foreground brush.</value>
        public Brush ForegroundBrush { get; set; }

        /// <summary>
        /// Gets or sets the selection brush.
        /// </summary>
        /// <value>The selection brush.</value>
        public Brush SelectionBrush { get; set; }
    }
}
