﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="IDateEditControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
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
