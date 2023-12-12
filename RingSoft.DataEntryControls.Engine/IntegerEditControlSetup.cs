// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="IntegerEditControlSetup.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// All the properties necessary to set up an IntegerEditControl.
    /// </summary>
    public class IntegerEditControlSetup : NumericEditControlSetup<int?>
    {
        /// <summary>
        /// Initializes from type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void InitializeFromType<T>()
        {
            InitializeFromType(typeof(T));
        }

        /// <summary>
        /// Initializes from type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void InitializeFromType(Type type)
        {
            if (type == typeof(int)
                     || type == typeof(int?))
            {
                MaximumValue = int.MaxValue;
                MinimumValue = int.MinValue;
            }
            else if (type == typeof(byte)
                     || type == typeof(byte?))
            {
                MaximumValue = byte.MaxValue;
                MinimumValue = byte.MinValue;
            }
            else if (type == typeof(short)
                     || type == typeof(short?))
            {
                MaximumValue = short.MaxValue;
                MinimumValue = short.MinValue;
            }
        }

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetNumberFormatString()
        {
            var result = base.GetNumberFormatString();
            if (result.IsNullOrEmpty())
                result = "N0";

            return result;
        }
    }
}
