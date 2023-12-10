// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridTextCellProps.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Enum TextCasing
    /// </summary>
    public enum TextCasing
    {
        /// <summary>
        /// The normal
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The upper
        /// </summary>
        Upper = 1,
        /// <summary>
        /// The lower
        /// </summary>
        Lower = 2
    }

    /// <summary>
    /// Class DataEntryGridTextCellProps.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    public class DataEntryGridTextCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => DataEntryGridEditingCellProps.TextBoxHostId;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the character casing.
        /// </summary>
        /// <value>The character casing.</value>
        public TextCasing CharacterCasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridTextCellProps"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        public DataEntryGridTextCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridTextCellProps"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="text">The text.</param>
        public DataEntryGridTextCellProps(DataEntryGridRow row, int columnId, string text) : base(row, columnId)
        {
            Text = text;
        }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return Text;
        }
    }
}
