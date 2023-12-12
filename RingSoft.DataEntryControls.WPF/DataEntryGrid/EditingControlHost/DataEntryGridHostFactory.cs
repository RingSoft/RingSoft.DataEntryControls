// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridHostFactory.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Class DataEntryGridHostFactory.
    /// </summary>
    public class DataEntryGridHostFactory
    {
        /// <summary>
        /// Gets the control host.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="editingControlHostId">The editing control host identifier.</param>
        /// <returns>DataEntryGridEditingControlHostBase.</returns>
        /// <exception cref="System.ArgumentException">Data Entry Grid Control Host not found for ID: {editingControlHostId}</exception>
        public virtual DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {

            if (editingControlHostId == DataEntryGridEditingCellProps.TextBoxHostId)
                return new DataEntryGridTextBoxHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.ComboBoxHostId)
                return new DataEntryGridTextComboBoxHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.CheckBoxHostId)
                return new DataEntryGridCheckBoxHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.ButtonHostId)
                return new DataEntryGridButtonHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.DecimalEditHostId)
                return new DataEntryGridDecimalControlHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.DateEditHostId)
                return new DataEntryGridDateHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.IntegerEditHostId)
                return new DataEntryGridIntegerControlHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.ContentControlHostId)
                return new DataEntryGridContentComboBoxControlHost(grid);

            throw new ArgumentException($"Data Entry Grid Control Host not found for ID: {editingControlHostId}");
        }
    }
}
