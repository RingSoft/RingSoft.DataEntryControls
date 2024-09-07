// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DataEntryGridComboBoxCellProps.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    /// <summary>
    /// Enum defining how the combo box selected value change should trigger SetCellValue.
    /// </summary>
    public enum ComboBoxValueChangedTypes
    {
        /// <summary>
        /// On End edit
        /// </summary>
        EndEdit = 0,
        /// <summary>
        /// The selected item changed
        /// </summary>
        SelectedItemChanged = 1
    }

    /// <summary>
    /// Creates a combo box control in the data entry grid cell.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridEditingCellProps" />
    public abstract class DataEntryGridComboBoxCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// The change type.
        /// </summary>
        /// <value>The type of the change.</value>
        public ComboBoxValueChangedTypes ChangeType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridComboBoxCellProps" /> class and creates a combo box control in the data entry grid cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="changeType">Enum defining how the combo box selected value change should trigger SetCellValue.</param>
        protected DataEntryGridComboBoxCellProps(DataEntryGridRow row, int columnId,
            ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(row, columnId)
        {
            ChangeType = changeType;
        }
    }

    /// <summary>
    /// Creates a text combo box control in the data entry grid cell.
    /// Implements the <see cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridComboBoxCellProps" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.DataEntryGrid.DataEntryGridComboBoxCellProps" />
    public class DataEntryGridTextComboBoxCellProps : DataEntryGridComboBoxCellProps
    {
        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => DataEntryGridEditingCellProps.ComboBoxHostId;

        /// <summary>
        /// Gets the ComboBox setup.
        /// </summary>
        /// <value>The ComboBox setup.</value>
        public TextComboBoxControlSetup ComboBoxSetup { get; }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public TextComboBoxItem SelectedItem { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridTextComboBoxCellProps" /> class and creates a text combo box control in the data entry grid cell..
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="comboBoxSetup">The combo box setup.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <param name="changeType">Enum defining how the combo box selected value change should trigger SetCellValue.</param>
        public DataEntryGridTextComboBoxCellProps(DataEntryGridRow row, int columnId, TextComboBoxControlSetup comboBoxSetup,
            TextComboBoxItem selectedItem, ComboBoxValueChangedTypes changeType = ComboBoxValueChangedTypes.EndEdit) : base(
            row, columnId, changeType)
        {
            ComboBoxSetup = comboBoxSetup;
            SelectedItem = selectedItem;
        }

        /// <summary>
        /// Gets the control properties.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return SelectedItem?.TextValue;
        }
    }
}
