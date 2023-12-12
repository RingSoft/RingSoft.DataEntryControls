// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="DataEntryGridCustomControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// A grid custom control column.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridControlColumn{RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridCustomControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridControlColumn{RingSoft.DataEntryControls.WPF.DataEntryGrid.DataEntryGridCustomControl}" />
    public class DataEntryGridCustomControlColumn : DataEntryGridControlColumn<DataEntryGridCustomControl>
    {
        /// <summary>
        /// The content template property
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataEntryCustomContentTemplate),
                typeof(DataEntryGridCustomControlColumn));

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        /// <value>The content template.</value>
        public DataEntryCustomContentTemplate ContentTemplate
        {
            get { return (DataEntryCustomContentTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets the designer data value.
        /// </summary>
        /// <value>The designer data value.</value>
        public override string DesignerDataValue =>
            new DataEntryGridDataValue().CreateDataValue(new DataEntryGridControlCellStyle(),
                DesignerSelectedId.ToString());

        /// <summary>
        /// The designer selected identifier
        /// </summary>
        private int _designerSelectedId;

        /// <summary>
        /// Gets or sets the designer selected identifier.
        /// </summary>
        /// <value>The designer selected identifier.</value>
        public int DesignerSelectedId
        {
            get => _designerSelectedId;
            set
            {
                if (_designerSelectedId == value)
                    return;

                _designerSelectedId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Processes the cell framework element factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="dataColumnName">Name of the data column.</param>
        /// <exception cref="System.Exception">The {nameof(ContentTemplate)} Property has not been set.</exception>
        protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName)
        {
            if (ContentTemplate == null)
                throw new Exception($"The {nameof(ContentTemplate)} Property has not been set.");

            factory.SetBinding(DataEntryGridCustomControl.DataValueProperty, new Binding(dataColumnName));
            factory.SetValue(CustomContentControl.ContentTemplateProperty, ContentTemplate);
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
        }
    }
    /// <summary>
    /// A grid cell custom content control.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.CustomContentControl" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.CustomContentControl" />
    public class DataEntryGridCustomControl : CustomContentControl
    {
        /// <summary>
        /// The data value property.
        /// </summary>
        public static readonly DependencyProperty DataValueProperty =
            DependencyProperty.Register(nameof(DataValue), typeof(string), typeof(DataEntryGridCustomControl),
                new FrameworkPropertyMetadata(DataValueChangedCallback));

        /// <summary>
        /// Gets or sets the data value.  This is a bind-able property.
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
            var customControl = (DataEntryGridCustomControl)obj;
            customControl.SetDataValue();
        }

        /// <summary>
        /// The processor
        /// </summary>
        private DataEntryGridControlColumnProcessor _processor;

        /// <summary>
        /// Initializes static members of the <see cref="DataEntryGridCustomControl"/> class.
        /// </summary>
        static DataEntryGridCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGridCustomControl), new FrameworkPropertyMetadata(typeof(DataEntryGridCustomControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridCustomControl"/> class.
        /// </summary>
        public DataEntryGridCustomControl()
        {
            Loaded += (sender, args) => OnLoaded();

            _processor = new DataEntryGridControlColumnProcessor(this);

            _processor.ControlValueChanged += (sender, args) =>
            {
                SelectedItemId = args.ControlValue.ToInt();
            };

        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        private void OnLoaded()
        {
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
