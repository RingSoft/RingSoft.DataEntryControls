// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridCellProps.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Enum how the cell was lost focus
    /// </summary>
    public enum CellLostFocusTypes
    {
        /// <summary>
        /// The cell lost focus through other means.
        /// </summary>
        LostFocus = 0,
        /// <summary>
        /// The user tabbed left
        /// </summary>
        TabLeft = 1,
        /// <summary>
        /// The user tabbed right
        /// </summary>
        TabRight = 2,
        /// <summary>
        /// Lost focus via keyboard navigation
        /// </summary>
        KeyboardNavigation = 3,
        /// <summary>
        /// Lost focus when validating grid
        /// </summary>
        ValidatingGrid = 4
    }

    /// <summary>
    /// Enum Cel state.
    /// </summary>
    public enum CellPropsTypes
    {
        /// <summary>
        /// Cell is editable
        /// </summary>
        Editable = 0,
        /// <summary>
        /// Cell is read only
        /// </summary>
        ReadOnly = 1
    }

    /// <summary>
    /// Class DataEntryGridCellProps.
    /// </summary>
    public abstract class DataEntryGridCellProps
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public virtual CellPropsTypes Type { get; internal set; } = CellPropsTypes.ReadOnly;

        /// <summary>
        /// Gets the column identifier.
        /// </summary>
        /// <value>The column identifier.</value>
        public int ColumnId { get; }

        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <value>The row.</value>
        public DataEntryGridRow Row { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [control mode].
        /// </summary>
        /// <value><c>true</c> if [control mode]; otherwise, <c>false</c>.</value>
        public bool ControlMode { get; set; }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <value>The data value.</value>
        public string DataValue => GetDataValue(Row, ColumnId, ControlMode);

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridCellProps"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        public DataEntryGridCellProps(DataEntryGridRow row, int columnId)
        {
            Row = row;
            ColumnId = columnId;
        }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        protected abstract string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode);
    }

    /// <summary>
    /// Class DataEntryGridEditingCellProps.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridCellProps" />
    public abstract class DataEntryGridEditingCellProps : DataEntryGridCellProps
    {
        /// <summary>
        /// The text box host identifier
        /// </summary>
        public const int TextBoxHostId = 0;
        /// <summary>
        /// The ComboBox host identifier
        /// </summary>
        public const int ComboBoxHostId = 1;
        /// <summary>
        /// The CheckBox host identifier
        /// </summary>
        public const int CheckBoxHostId = 2;
        /// <summary>
        /// The button host identifier
        /// </summary>
        public const int ButtonHostId = 3;
        /// <summary>
        /// The decimal edit host identifier
        /// </summary>
        public const int DecimalEditHostId = 4;
        /// <summary>
        /// The date edit host identifier
        /// </summary>
        public const int DateEditHostId = 5;
        /// <summary>
        /// The integer edit host identifier
        /// </summary>
        public const int IntegerEditHostId = 6;
        /// <summary>
        /// The content control host identifier
        /// </summary>
        public const int ContentControlHostId = 7;
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public abstract int EditingControlId { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public override CellPropsTypes Type { get; internal set; } = CellPropsTypes.Editable;

        /// <summary>
        /// Gets or sets a value indicating whether to override cell movement.
        /// </summary>
        /// <value><c>true</c> if override cell movement; otherwise, <c>false</c>.</value>
        public bool OverrideCellMovement { get; set; }

        /// <summary>
        /// Gets or sets the type of the cell lost focus.
        /// </summary>
        /// <value>The type of the cell lost focus.</value>
        public CellLostFocusTypes CellLostFocusType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridEditingCellProps"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        protected DataEntryGridEditingCellProps(DataEntryGridRow row, int columnId) : base(row, columnId)
        {
        }
    }
}
