// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DataEntryGridEditingControlHostBase.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Class DataEntryGridEditingControlHostBase.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public abstract class DataEntryGridEditingControlHostBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the grid.
        /// </summary>
        /// <value>The grid.</value>
        public DataEntryGrid Grid { get; }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public Control Control { get; internal set; }

        //public DataEntryGridCellProps CellProps { get; internal set; }
        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <value>The row.</value>
        public DataEntryGridRow Row => Grid.GetCurrentRow();

        /// <summary>
        /// Gets the column identifier.
        /// </summary>
        /// <value>The column identifier.</value>
        public int ColumnId { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public abstract bool IsDropDownOpen { get; }

        /// <summary>
        /// Gets a value indicating whether [set selection].
        /// </summary>
        /// <value><c>true</c> if [set selection]; otherwise, <c>false</c>.</value>
        public virtual bool SetSelection { get; } = false;

        /// <summary>
        /// Gets a value indicating whether [allow read only edit].
        /// </summary>
        /// <value><c>true</c> if [allow read only edit]; otherwise, <c>false</c>.</value>
        public virtual bool AllowReadOnlyEdit { get; } = false;

        /// <summary>
        /// Occurs when [control dirty].
        /// </summary>
        public event EventHandler ControlDirty;

        /// <summary>
        /// Occurs when [update source].
        /// </summary>
        public event EventHandler<DataEntryGridEditingCellProps> UpdateSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridEditingControlHostBase" /> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        protected internal DataEntryGridEditingControlHostBase(DataEntryGrid grid)
        {
            Grid = grid;
        }

        /// <summary>
        /// Called when [control dirty].
        /// </summary>
        protected virtual void OnControlDirty()
        {
            ControlDirty?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when [update source].
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void OnUpdateSource(DataEntryGridEditingCellProps e)
        {
            UpdateSource?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the editing control data template.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <param name="column">The column.</param>
        /// <returns>DataTemplate.</returns>
        public abstract DataTemplate GetEditingControlDataTemplate(DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle, DataEntryGridColumn column);

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public abstract DataEntryGridEditingCellProps GetCellValue();

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public abstract bool HasDataChanged();

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public abstract void UpdateFromCellProps(DataEntryGridCellProps cellProps);

        /// <summary>
        /// Determines whether this instance [can grid process key] the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if this instance [can grid process key] the specified key; otherwise, <c>false</c>.</returns>
        public virtual bool CanGridProcessKey(Key key)
        {
            return true;
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyMode">if set to <c>true</c> [read only mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool SetReadOnlyMode(bool readOnlyMode)
        {
            return !readOnlyMode;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
