// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="ICalculatorControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Interface ICalculatorControl
    /// </summary>
    public interface ICalculatorControl
    {
        /// <summary>
        /// Gets or sets the equation text.
        /// </summary>
        /// <value>The equation text.</value>
        string EquationText { get; set; }
        /// <summary>
        /// Gets or sets the entry text.
        /// </summary>
        /// <value>The entry text.</value>
        string EntryText { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [memory recall enabled].
        /// </summary>
        /// <value><c>true</c> if [memory recall enabled]; otherwise, <c>false</c>.</value>
        bool MemoryRecallEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [memory clear enabled].
        /// </summary>
        /// <value><c>true</c> if [memory clear enabled]; otherwise, <c>false</c>.</value>
        bool MemoryClearEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [memory store enabled].
        /// </summary>
        /// <value><c>true</c> if [memory store enabled]; otherwise, <c>false</c>.</value>
        bool MemoryStoreEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [memory plus enabled].
        /// </summary>
        /// <value><c>true</c> if [memory plus enabled]; otherwise, <c>false</c>.</value>
        bool MemoryPlusEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [memory minus enabled].
        /// </summary>
        /// <value><c>true</c> if [memory minus enabled]; otherwise, <c>false</c>.</value>
        bool MemoryMinusEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [memory status visible].
        /// </summary>
        /// <value><c>true</c> if [memory status visible]; otherwise, <c>false</c>.</value>
        bool MemoryStatusVisible { get; set; }

        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        void OnValueChanged(double? oldValue, double? newValue);
    }
}
