﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="IDropDownCalculator.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DropDownEditControls
{
    /// <summary>
    /// Interface IDropDownCalculator
    /// </summary>
    public interface IDropDownCalculator
    {
        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        Control Control { get; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        double? Value { get; set; }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        /// <value>The precision.</value>
        int Precision { get; set; }

        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        event RoutedPropertyChangedEventHandler<object> ValueChanged;
    }
}
