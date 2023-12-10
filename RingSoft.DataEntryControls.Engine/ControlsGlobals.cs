﻿// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ControlsGlobals.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Class ControlsGlobals.
    /// </summary>
    public static class ControlsGlobals
    {
        /// <summary>
        /// The user interface
        /// </summary>
        private static IControlsUserInterface _userInterface;

        /// <summary>
        /// Gets or sets the user interface.
        /// </summary>
        /// <value>The user interface.</value>
        /// <exception cref="System.Exception">ControlsGlobals UserInterface not set.  Run WPFControlsGlobals.InitUI</exception>
        public static IControlsUserInterface UserInterface
        {
            get
            {
                if (_userInterface == null)
                    throw new Exception("ControlsGlobals UserInterface not set.  Run WPFControlsGlobals.InitUI");

                return _userInterface;
            }
            set => _userInterface = value;
        }

    }
}
