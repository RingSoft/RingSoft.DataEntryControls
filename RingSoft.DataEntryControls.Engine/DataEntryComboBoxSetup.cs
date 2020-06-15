using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DataEntryControls.Engine
{
    public class ComboBoxItem
    {
        public string TextValue { get; set; }

        public int NumericValue { get; set; }

        public override string ToString()
        {
            return TextValue;
        }
    }
    public class DataEntryComboBoxSetup
    {
        public List<ComboBoxItem> Items { get; } = new List<ComboBoxItem>();

        public void LoadFromEnum<T>() where T : Enum
        {
            LoadFromEnum(typeof(T));
        }

        public void LoadFromEnum(Type enumType)
        {
            Items.Clear();
            var enumValues = Enum.GetValues(enumType);

            foreach (var enumValue in enumValues)
            {
                var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var textValue = attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();

                var comboItem = new ComboBoxItem
                {
                    NumericValue = (int)enumValue,
                    TextValue = textValue
                };
                Items.Add(comboItem);
            }
        }

        public ComboBoxItem GetItem(int numericValue)
        {
            var item = Items.FirstOrDefault(f => f.NumericValue == numericValue);
            if (item == null)
                throw new ArgumentException($"No Combo Box Item found for numeric value: {numericValue}");

            return item;
        }
    }
}
