// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="TextComboBoxControlSetup.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// An item inside a combo box.
    /// </summary>
    public class TextComboBoxItem
    {
        /// <summary>
        /// Gets or sets the text value.
        /// </summary>
        /// <value>The text value.</value>
        public string TextValue { get; set; }

        /// <summary>
        /// Gets or sets the numeric value.
        /// </summary>
        /// <value>The numeric value.</value>
        public int NumericValue { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return TextValue;
        }
    }

    /// <summary>
    /// All the properties necessary to set up a combo box.
    /// </summary>
    public class TextComboBoxControlSetup
    {
        /// <summary>
        /// Gets the combo box items.
        /// </summary>
        /// <value>The combo box items.</value>
        public List<TextComboBoxItem> Items { get; } = new List<TextComboBoxItem>();

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
            var typeTranslation = new EnumFieldTranslation();
            typeTranslation.LoadFromEnum(enumType);
            LoadFromEnum(typeTranslation);
        }

        /// <summary>
        /// Loads from enum.
        /// </summary>
        /// <param name="enumField">The enum field.</param>
        public void LoadFromEnum(EnumFieldTranslation enumField)
        {
            Items.Clear();
            foreach (var enumFieldTypeTranslation in enumField.TypeTranslations)
            {
                var comboItem = new TextComboBoxItem
                {
                    NumericValue = (int)enumFieldTypeTranslation.NumericValue,
                    TextValue = enumFieldTypeTranslation.TextValue
                };
                Items.Add(comboItem);

            }
        }


        /// <summary>
        /// Gets the combo box item of the associated numeric value.
        /// </summary>
        /// <param name="numericValue">The numeric value.</param>
        /// <returns>The combo box item of the numeric value.</returns>
        /// <exception cref="System.ArgumentException">No Combo Box Item found for numeric value: {numericValue}</exception>
        public TextComboBoxItem GetItem(int numericValue)
        {
            var item = Items.FirstOrDefault(f => f.NumericValue == numericValue);
            if (item == null)
                throw new ArgumentException($"No Combo Box Item found for numeric value: {numericValue}");

            return item;
        }
    }
}
