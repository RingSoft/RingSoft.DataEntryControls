// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridTextColumn.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// A grid text column.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridColumn" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridColumn" />
    public class DataEntryGridTextColumn : DataEntryGridColumn
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public override DataEntryGridColumnTypes ColumnType => DataEntryGridColumnTypes.Text;
        /// <summary>
        /// Gets the designer data value.
        /// </summary>
        /// <value>The designer data value.</value>
        public override string DesignerDataValue => DesignText;

        /// <summary>
        /// The design text
        /// </summary>
        private string _designText;

        /// <summary>
        /// Gets or sets the design text.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get => _designText;
            set
            {
                if (_designText == value)
                    return;

                _designText = value;
                OnPropertyChanged(nameof(DesignText));
            }
        }

        /// <summary>
        /// Creates the cell template.
        /// </summary>
        /// <returns>DataTemplate.</returns>
        protected internal override DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));

            ProcessCellFrameworkElementFactory(factory, DataColumnName);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }

        /// <summary>
        /// Validates the designer data value.
        /// </summary>
        internal override void ValidateDesignerDataValue()
        {
            
        }

        /// <summary>
        /// Processes the cell framework element factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        protected void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory,
            string dataColumnName)
        {
            factory.SetBinding(TextBlock.TextProperty, new Binding(dataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(TextBlock.TextAlignmentProperty, Alignment);
        }
    }
}
