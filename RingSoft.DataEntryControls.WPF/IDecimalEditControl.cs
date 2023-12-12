// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="IDecimalEditControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Interface IDecimalEditControl
    /// Extends the <see cref="INumericControl" />
    /// </summary>
    /// <seealso cref="INumericControl" />
    public interface IDecimalEditControl : INumericControl
    {
        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <value>The edit control.</value>
        Control EditControl { get; }

        /// <summary>
        /// Gets or sets the numeric setup.
        /// </summary>
        /// <value>The numeric setup.</value>
        DecimalEditControlSetup NumericSetup { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        double? Value { get; set; }
    }
}
