// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridContextMenuItem.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Input;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// The grid context menu.
    /// </summary>
    public class DataEntryGridContextMenuItem
    {
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header { get; }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command { get; }

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        /// <value>The command parameter.</value>
        public object CommandParameter { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public object Icon { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridContextMenuItem" /> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="command">The command.</param>
        public DataEntryGridContextMenuItem(string header, ICommand command)
        {
            Header = header;
            Command = command;
        }
    }
}
