using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// An item inside a combo box.
    /// </summary>
    public class ComboBoxItem
    {
        /// <summary>
        /// Gets or sets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        public string TextValue { get; set; }

        /// <summary>
        /// Gets or sets the numeric value.
        /// </summary>
        /// <value>
        /// The numeric value.
        /// </value>
        public int NumericValue { get; set; }

        public override string ToString()
        {
            return TextValue;
        }
    }

    /// <summary>
    /// All the properties necessary to set up a combo box.
    /// </summary>
    public class ComboBoxControlSetup
    {
        /// <summary>
        /// Gets the combo box items.
        /// </summary>
        /// <value>
        /// The combo box items.
        /// </value>
        public List<ComboBoxItem> Items { get; } = new List<ComboBoxItem>();

        /// <summary>
        /// Loads from an enum.  Uses each enum item's Description attribute as the combo box item text.
        /// </summary>
        /// <typeparam name="T">An enum</typeparam>
        public void LoadFromEnum<T>() where T : Enum
        {
            LoadFromEnum(typeof(T));
        }

        /// <summary>
        /// Loads from an enum.  Uses each enum item's Description attribute as the combo box item text.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
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

        /// <summary>
        /// Gets the combo box item of the associated numeric value.
        /// </summary>
        /// <param name="numericValue">The numeric value.</param>
        /// <returns>The combo box item of the numeric value.</returns>
        /// <exception cref="ArgumentException">No Combo Box Item found for numeric value: {numericValue}</exception>
        public ComboBoxItem GetItem(int numericValue)
        {
            var item = Items.FirstOrDefault(f => f.NumericValue == numericValue);
            if (item == null)
                throw new ArgumentException($"No Combo Box Item found for numeric value: {numericValue}");

            return item;
        }
    }
}
