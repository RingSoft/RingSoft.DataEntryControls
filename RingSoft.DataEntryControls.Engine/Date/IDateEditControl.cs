// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="IDateEditControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Interface IDateEditControl
    /// Extends the <see cref="RingSoft.DataEntryControls.Engine.IDropDownControl" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.Engine.IDropDownControl" />
    public interface IDateEditControl : IDropDownControl
    {
        /// <summary>
        /// Sets the select all.
        /// </summary>
        void SetSelectAll();
    }
}
