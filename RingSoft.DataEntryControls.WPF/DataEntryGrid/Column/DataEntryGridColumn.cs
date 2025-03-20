// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridColumn.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{

    /// <summary>
    /// Enum DataEntryGridColumnTypes
    /// </summary>
    public enum DataEntryGridColumnTypes
    {
        /// <summary>
        /// Text column
        /// </summary>
        Text = 0,
        /// <summary>
        /// Control column
        /// </summary>
        Control = 1
    }

    /// <summary>
    /// Class DataEntryGridColumn.
    /// Implements the <see cref="DataGridTemplateColumn" />
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="DataGridTemplateColumn" />
    /// <seealso cref="INotifyPropertyChanged" />
    public abstract class DataEntryGridColumn : DataGridTemplateColumn, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the cell template.
        /// </summary>
        /// <value>The cell template.</value>
        public new DataTemplate CellTemplate
        {
            get => base.CellTemplate;
            protected internal set => base.CellTemplate = value;
        }

        /// <summary>
        /// The default column header
        /// </summary>
        private object _defaultColumnHeader;
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public new object Header
        {
            get => base.Header;
            set
            {
                if (_defaultColumnHeader == null)
                    _defaultColumnHeader = base.Header;

                base.Header = value;
            }
        }

        /// <summary>
        /// Gets the cell editing template.
        /// </summary>
        /// <value>The cell editing template.</value>
        public new DataTemplate CellEditingTemplate
        {
            get => base.CellEditingTemplate;
            internal set => base.CellEditingTemplate = value;
        }

        /// <summary>
        /// Gets or sets the column identifier.
        /// </summary>
        /// <value>The column identifier.</value>
        public int ColumnId { get; set; }

        /// <summary>
        /// The alignment
        /// </summary>
        private TextAlignment _alignment;

        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        /// <value>The alignment.</value>
        public TextAlignment Alignment
        {
            get => _alignment;
            set
            {
                if (_alignment == value)
                    return;

                _alignment = value;
                CellTemplate = CreateCellTemplate();
                OnPropertyChanged(nameof(Alignment));
            }
        }

        /// <summary>
        /// The data column name
        /// </summary>
        private string _dataColumnName;

        /// <summary>
        /// Gets or sets the name of the data column.
        /// </summary>
        /// <value>The name of the data column.</value>
        protected internal string DataColumnName
        {
            get => _dataColumnName;
            set
            {
                _dataColumnName = value;
                CellTemplate = CreateCellTemplate();
            }
        }

        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public abstract DataEntryGridColumnTypes ColumnType { get; }

        /// <summary>
        /// Gets the designer data value.
        /// </summary>
        /// <value>The designer data value.</value>
        public abstract string DesignerDataValue { get; }

        /// <summary>
        /// Creates the cell template.
        /// </summary>
        /// <returns>DataTemplate.</returns>
        protected internal abstract DataTemplate CreateCellTemplate();

        /// <summary>
        /// Validates the designer data value.
        /// </summary>
        internal abstract void ValidateDesignerDataValue();

        /// <summary>
        /// Resets the column header.
        /// </summary>
        public void ResetColumnHeader()
        {
            if (_defaultColumnHeader == null)
                _defaultColumnHeader = base.Header;

            Header = _defaultColumnHeader;
        }
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Class DataEntryGridControlColumn.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridColumn" />
    /// </summary>
    /// <typeparam name="TControl">The type of the t control.</typeparam>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridColumn" />
    public abstract class DataEntryGridControlColumn<TControl> : DataEntryGridColumn
        where TControl : Control
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public override DataEntryGridColumnTypes ColumnType => DataEntryGridColumnTypes.Control;

        /// <summary>
        /// Processes the cell framework element factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        protected abstract void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName);

        /// <summary>
        /// Creates the cell template.
        /// </summary>
        /// <returns>DataTemplate.</returns>
        protected internal override DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TControl));

            ProcessCellFrameworkElementFactory(factory, DataColumnName);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }

        /// <summary>
        /// Validates the designer data value.
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        internal override void ValidateDesignerDataValue()
        {
            if (DesignerDataValue.Length < DataEntryGridDataValue.CheckDataValue.Length)
            {
                var message =
                    $"{this} Designer Error: Invalid {nameof(DesignerDataValue)} input ({DesignerDataValue}). Make sure the {nameof(DesignerDataValue)} parameter is generated by the {nameof(DataEntryGridDataValue)}.{nameof(DataEntryGridDataValue.CreateDataValue)} method in the {this}.{nameof(DesignerDataValue)} property getter.";

                throw new Exception(message);
            }
        }
    }
}
