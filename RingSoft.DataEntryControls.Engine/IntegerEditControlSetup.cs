using System;

namespace RingSoft.DataEntryControls.Engine
{
    public class IntegerEditControlSetup : NumericEditControlSetup
    {
        public int? MaximumValue { get; set; }

        public int? MinimumValue { get; set; }

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
