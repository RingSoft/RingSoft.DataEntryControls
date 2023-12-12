// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 05-05-2023
// ***********************************************************************
// <copyright file="DataEntryGridButton.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// A grid cell button column.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridControlColumn{RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridButton}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridControlColumn{RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridButton}" />
    public class DataEntryGridButtonColumn : DataEntryGridControlColumn<DataEntryGridButton>
    {
        /// <summary>
        /// Gets the designer data value.
        /// </summary>
        /// <value>The designer data value.</value>
        public override string DesignerDataValue
        {
            get
            {
                var cellStyle = new DataEntryGridButtonCellStyle {Content = DesignerContent};
                return new DataEntryGridDataValue().CreateDataValue(cellStyle, DesignerContent);
            }
        }

        /// <summary>
        /// The designer content
        /// </summary>
        private string _designerContent;

        /// <summary>
        /// Gets or sets the content of the designer.
        /// </summary>
        /// <value>The content of the designer.</value>
        public string DesignerContent
        {
            get => _designerContent;
            set
            {
                if (_designerContent == value)
                    return;

                _designerContent = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Processes the cell framework element factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory,
            string dataColumnName)
        {
            factory.SetBinding(DataEntryGridButton.DataValueProperty, new Binding(dataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
        }
    }

    /// <summary>
    /// A grid cell button.
    /// Implements the <see cref="Button" />
    /// </summary>
    /// <seealso cref="Button" />
    /// <font color="red">Badly formed XML comment.</font>
    public class DataEntryGridButton : Button
    {
        /// <summary>
        /// The data value property
        /// </summary>
        public static readonly DependencyProperty DataValueProperty =
            DependencyProperty.Register(nameof(DataValue), typeof(string), typeof(DataEntryGridButton),
                new FrameworkPropertyMetadata(DataValueChangedCallback));

        /// <summary>
        /// Gets or sets the data value.
        /// </summary>
        /// <value>The data value.</value>
        public string DataValue
        {
            get { return (string)GetValue(DataValueProperty); }
            set { SetValue(DataValueProperty, value); }
        }

        /// <summary>
        /// Datas the value changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DataValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryGridButton = (DataEntryGridButton)obj;
            dataEntryGridButton.SetDataValue();
        }

        /// <summary>
        /// The processor
        /// </summary>
        private DataEntryGridControlColumnProcessor _processor;

        /// <summary>
        /// Initializes static members of the <see cref="DataEntryGridButton"/> class.
        /// </summary>
        static DataEntryGridButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGridButton), new FrameworkPropertyMetadata(typeof(DataEntryGridButton)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridButton"/> class.
        /// </summary>
        public DataEntryGridButton()
        {
            _processor = new DataEntryGridControlColumnProcessor(this);

            _processor.ControlValueChanged += (sender, args) => Content = args.ControlValue;
        }

        /// <summary>
        /// Sets the data value.
        /// </summary>
        private void SetDataValue()
        {
            _processor.SetDataValue(DataValue);
        }
    }
}