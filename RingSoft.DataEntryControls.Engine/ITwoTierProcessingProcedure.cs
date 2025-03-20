// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 07-11-2024
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="ITwoTierProcessingProcedure.cs" company="Peter Ringering">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Interface ITwoTierProcessingProcedure
    /// </summary>
    public interface ITwoTierProcessingProcedure
    {
        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="topMax">The top maximum.</param>
        /// <param name="topValue">The top value.</param>
        /// <param name="topText">The top text.</param>
        /// <param name="bottomMax">The bottom maximum.</param>
        /// <param name="bottomValue">The bottom value.</param>
        /// <param name="bottomText">The bottom text.</param>
        public void SetProgress(int topMax = 0
            , int topValue = 0
            , string topText = ""
            , int bottomMax = 0
            , int bottomValue = 0
            , string bottomText = "");
    }
}
