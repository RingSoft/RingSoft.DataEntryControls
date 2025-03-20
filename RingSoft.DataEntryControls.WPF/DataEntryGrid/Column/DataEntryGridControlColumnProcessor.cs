﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridControlColumnProcessor.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// Class ControlValueChangedArgs.
    /// </summary>
    public class ControlValueChangedArgs
    {
        /// <summary>
        /// Gets the control value.
        /// </summary>
        /// <value>The control value.</value>
        public string ControlValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlValueChangedArgs" /> class.
        /// </summary>
        /// <param name="controlValue">The control value.</param>
        public ControlValueChangedArgs(string controlValue)
        {
            ControlValue = controlValue;
        }
    }

    /// <summary>
    /// Controls how a grid cell control's display behavior works.
    /// </summary>
    public class DataEntryGridControlColumnProcessor
    {
        /// <summary>
        /// Gets the display style.
        /// </summary>
        /// <value>The display style.</value>
        public DataEntryGridDisplayStyle DisplayStyle { get; private set; } = new DataEntryGridDisplayStyle();

        /// <summary>
        /// Occurs when [control value changed].
        /// </summary>
        public event EventHandler<ControlValueChangedArgs> ControlValueChanged;

        /// <summary>
        /// The control
        /// </summary>
        private Control _control;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridControlColumnProcessor" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public DataEntryGridControlColumnProcessor(Control control)
        {
            _control = control;

            //This is to avoid random showing when grid is being scrolled and DataEntryControlCellValue sets IsVisible to false.
            _control.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Sets the data value.
        /// </summary>
        /// <param name="dataValue">The data value.</param>
        /// <exception cref="System.Exception"></exception>
        public void SetDataValue(string dataValue)
        {
            if (!dataValue.IsNullOrEmpty() && dataValue.Length < DataEntryGridDataValue.CheckDataValue.Length)
            {
                var message = $"{nameof(SetDataValue)} Runtime Error:  Invalid {nameof(dataValue)} input ({dataValue}). Make sure the {nameof(dataValue)} parameter is generated by the {nameof(DataEntryGridDataValue.CreateDataValue)} method in the {nameof(DataEntryGridCellProps.DataValue)} property getter.";

                throw new Exception(message);
            }

            var dataValueObj = new DataEntryGridDataValue();
            dataValueObj.ProcessDataValueInput(dataValue);
            
            _control.Visibility = dataValueObj.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            _control.IsEnabled = dataValueObj.IsEnabled;

            if (dataValueObj.DisplayStyleId > 0)
            {
                var dataEntryGrid = _control.GetParentOfType<DataEntryGrid>();
                if (dataEntryGrid != null)
                    DisplayStyle = dataEntryGrid.GetDisplayStyle(dataValueObj.DisplayStyleId);
            }

            if (!dataValueObj.ControlValue.IsNullOrEmpty())
                ControlValueChanged?.Invoke(this, new ControlValueChangedArgs(dataValueObj.ControlValue));
        }
    }
}
