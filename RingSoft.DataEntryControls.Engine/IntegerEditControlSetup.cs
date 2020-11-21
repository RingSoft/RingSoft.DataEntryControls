﻿using System;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// All the properties necessary to set up an IntegerEditControl.
    /// </summary>
    public class IntegerEditControlSetup : NumericEditControlSetup<int?>
    {
        public void InitializeFromType<T>()
        {
            InitializeFromType(typeof(T));
        }

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

        public override string GetNumberFormatString()
        {
            var result = base.GetNumberFormatString();
            if (result.IsNullOrEmpty())
                result = "N0";

            return result;
        }
    }
}
