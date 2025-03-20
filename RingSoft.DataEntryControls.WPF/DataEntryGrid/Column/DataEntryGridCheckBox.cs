// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridCheckBox.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// A grid checkbox column
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridControlColumn{RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridCheckBox}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridControlColumn{RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridCheckBox}" />
    public class DataEntryGridCheckBoxColumn : DataEntryGridControlColumn<DataEntryGridCheckBox>
    {
        /// <summary>
        /// Gets the designer data value.
        /// </summary>
        /// <value>The designer data value.</value>
        public override string DesignerDataValue
        {
            get
            {
                var controlCellStyle = new DataEntryGridControlCellStyle();
                return new DataEntryGridDataValue().CreateDataValue(controlCellStyle, DesignerValue.ToString());
            }
        }

        /// <summary>
        /// The designer value
        /// </summary>
        private bool _designerValue;

        /// <summary>
        /// Gets or sets a value indicating whether [designer value].
        /// </summary>
        /// <value><c>true</c> if [designer value]; otherwise, <c>false</c>.</value>
        public bool DesignerValue
        {
            get => _designerValue;
            set
            {
                if (_designerValue == value)
                    return;

                _designerValue = value;
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
            factory.SetBinding(DataEntryGridCheckBox.DataValueProperty, new Binding(dataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        }
    }

    /// <summary>
    /// A grid cell check box.
    /// Implements the <see cref="CheckBox" />
    /// </summary>
    /// <seealso cref="CheckBox" />
    public class DataEntryGridCheckBox : CheckBox
    {
        /// <summary>
        /// The data value property
        /// </summary>
        public static readonly DependencyProperty DataValueProperty =
            DependencyProperty.Register(nameof(DataValue), typeof(string), typeof(DataEntryGridCheckBox),
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
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void DataValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryGridCheckBox = (DataEntryGridCheckBox)obj;
            dataEntryGridCheckBox.SetDataValue();
        }

        /// <summary>
        /// The processor
        /// </summary>
        private DataEntryGridControlColumnProcessor _processor;

        /// <summary>
        /// Initializes static members of the <see cref="DataEntryGridCheckBox" /> class.
        /// </summary>
        static DataEntryGridCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGridCheckBox), new FrameworkPropertyMetadata(typeof(DataEntryGridCheckBox)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridCheckBox" /> class.
        /// </summary>
        public DataEntryGridCheckBox()
        {
            _processor = new DataEntryGridControlColumnProcessor(this);

            _processor.ControlValueChanged += (sender, args) => IsChecked = args.ControlValue.ToBool();
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
