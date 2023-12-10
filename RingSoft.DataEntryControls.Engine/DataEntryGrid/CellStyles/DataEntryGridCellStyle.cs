// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridCellStyle.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


// ReSharper disable once CheckNamespace

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Enum DataEntryGridCellStates
    /// </summary>
    public enum DataEntryGridCellStates
    {
        /// <summary>
        /// The enabled
        /// </summary>
        Enabled = 0,
        /// <summary>
        /// The read only
        /// </summary>
        ReadOnly = 1,
        /// <summary>
        /// The disabled
        /// </summary>
        Disabled = 2
    }

    /// <summary>
    /// Class DataEntryGridCellStyle.
    /// </summary>
    public class DataEntryGridCellStyle
    {
        /// <summary>
        /// Gets or sets the display style identifier.
        /// </summary>
        /// <value>The display style identifier.</value>
        public int DisplayStyleId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public DataEntryGridCellStates State { get; set; }

        /// <summary>
        /// Gets or sets the column header.
        /// </summary>
        /// <value>The column header.</value>
        public string ColumnHeader { get; set; }
    }

    /// <summary>
    /// Class DataEntryGridControlCellStyle.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridCellStyle" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridCellStyle" />
    public class DataEntryGridControlCellStyle : DataEntryGridCellStyle
    {
        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get
            {
                switch (State)
                {
                    case DataEntryGridCellStates.Disabled:
                    case DataEntryGridCellStates.ReadOnly:
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible { get; set; } = true;
    }

    /// <summary>
    /// Class DataEntryGridButtonCellStyle.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridControlCellStyle" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridControlCellStyle" />
    public class DataEntryGridButtonCellStyle : DataEntryGridControlCellStyle
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }
    }
}
