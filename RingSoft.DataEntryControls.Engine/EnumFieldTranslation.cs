// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="EnumFieldTranslation.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// A list of enum numeric values and their corresponding description values.
    /// </summary>
    public class EnumFieldTranslation
    {
        /// <summary>
        /// The type translations
        /// </summary>
        private readonly List<TypeTranslation> _typeTranslations = new List<TypeTranslation>();

        /// <summary>
        /// Gets the type translations.
        /// </summary>
        /// <value>The type translations.</value>
        public IReadOnlyList<TypeTranslation> TypeTranslations => _typeTranslations;

        /// <summary>
        /// Loads from enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void LoadFromEnum<T>() where T : Enum
        {
            LoadFromEnum(typeof(T));
        }

        /// <summary>
        /// Loads from enum.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        public void LoadFromEnum(Type enumType)
        {
            var enumValues = Enum.GetValues(enumType);

            foreach (var enumValue in enumValues)
            {
                var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var textValue = attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();

                var typeTranslation = new TypeTranslation
                {
                    NumericValue = (int)enumValue,
                    TextValue = textValue
                };
                _typeTranslations.Add(typeTranslation);
            }
        }

        /// <summary>
        /// Loads from boolean.
        /// </summary>
        /// <param name="trueText">The true text.</param>
        /// <param name="falseText">The false text.</param>
        public void LoadFromBoolean(string trueText, string falseText)
        {
            _typeTranslations.Add(new TypeTranslation
            {
                NumericValue = 1,
                TextValue = trueText
            });

            _typeTranslations.Add(new TypeTranslation
            {
                NumericValue = 0,
                TextValue = falseText
            });
        }
    }

    /// <summary>
    /// A database field's numeric value and its corresponding text value.
    /// </summary>
    public class TypeTranslation
    {
        /// <summary>
        /// Gets the numeric value.
        /// </summary>
        /// <value>The numeric value.</value>
        public int NumericValue { get; internal set; }

        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>The text value.</value>
        public string TextValue { get; internal set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return TextValue;
        }
    }
}
