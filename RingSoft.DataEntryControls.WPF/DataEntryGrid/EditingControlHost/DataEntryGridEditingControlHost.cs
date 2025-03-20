// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridEditingControlHost.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    /// <summary>
    /// Grid cell's base control host.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHostBase" />
    /// </summary>
    /// <typeparam name="TControl">The type of the t control.</typeparam>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHostBase" />
    public abstract class DataEntryGridEditingControlHost<TControl> : DataEntryGridEditingControlHostBase
    where TControl : Control
    {
        /// <summary>
        /// The control
        /// </summary>
        private TControl _control;

        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>The control.</value>
        public new TControl Control
        {
            get => _control;
            set => base.Control = _control = value;
        }

        /// <summary>
        /// The cell props
        /// </summary>
        private DataEntryGridEditingCellProps _cellProps;
        /// <summary>
        /// The cell style
        /// </summary>
        private DataEntryGridCellStyle _cellStyle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridEditingControlHost{TControl}" /> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        protected DataEntryGridEditingControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the editing control data template.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <param name="column">The column.</param>
        /// <returns>DataTemplate.</returns>
        public sealed override DataTemplate GetEditingControlDataTemplate(DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle, DataEntryGridColumn column)
        {
            _cellProps = cellProps;
            _cellStyle = cellStyle;

            ColumnId = cellProps.ColumnId;

            var factory = new FrameworkElementFactory(typeof(TControl));
            factory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(Loaded));
            SetupFrameworkElementFactory(factory, column);

            var dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = factory;

            return dataTemplate;
        }

        /// <summary>
        /// Setups the framework element factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="column">The column.</param>
        protected virtual void SetupFrameworkElementFactory(FrameworkElementFactory factory, DataEntryGridColumn column)
        {
        }

        /// <summary>
        /// Called when [control loaded].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected abstract void OnControlLoaded(TControl control, DataEntryGridEditingCellProps cellProps, DataEntryGridCellStyle cellStyle);

        /// <summary>
        /// Loadeds the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TControl control)
            {
                Control = control;
                if (!SetSelection)
                    Grid.SelectedCells.Clear();

                var thisWindow = Window.GetWindow(Control);
                var activeWindow = WPFControlsGlobals.ActiveWindow;
                if (thisWindow == activeWindow)
                {
                    control.Focus();
                }

                var contextMenu = control.ContextMenu;
                Separator separator = null;
                if (contextMenu == null)
                {
                    contextMenu = new ContextMenu();
                }
                else
                {
                    separator = new Separator();
                    contextMenu.Items.Add(separator);
                }

                var contextMenuItemCount = contextMenu.Items.Count;
                Grid.AddGridContextMenuItems(contextMenu);
                if (separator != null && contextMenuItemCount == contextMenu.Items.Count)
                    contextMenu.Items.Remove(separator);
                control.ContextMenu = contextMenu;

                Control.Width = double.NaN;
                
                OnControlLoaded(control, _cellProps, _cellStyle);

                var dataGridCell = Grid.GetCurrentCell();
                if (dataGridCell != null)
                    ImportDataGridCellProperties(dataGridCell);
            }
        }

        /// <summary>
        /// Imports the data grid cell properties.
        /// </summary>
        /// <param name="dataGridCell">The data grid cell.</param>
        protected virtual void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            Control.Background = dataGridCell.Background;
            Control.Foreground = dataGridCell.Foreground;

            if (Grid.CellEditingControlBorderThickness.Equals(new Thickness(0)))
                dataGridCell.BorderThickness = Grid.CellEditingControlBorderThickness;

            Control.BorderThickness = new Thickness(0);
            Control.VerticalAlignment = VerticalAlignment.Center;
            Control.Height = 17;
            Control.MinHeight = dataGridCell.MinHeight;
            Control.MinWidth = dataGridCell.MinWidth;
        }

        /// <summary>
        /// Gets the cell display style.
        /// </summary>
        /// <returns>DataEntryGridDisplayStyle.</returns>
        /// <exception cref="System.Exception">Control has not been initialized yet</exception>
        protected DataEntryGridDisplayStyle GetCellDisplayStyle()
        {
            if (_cellStyle == null)
                throw new Exception("Control has not been initialized yet");

            var displayStyleId = _cellStyle.DisplayStyleId;

            if (displayStyleId == 0)
                displayStyleId = Row.DisplayStyleId;

            DataEntryGridDisplayStyle displayStyle;
            if (displayStyleId == 0)
                displayStyle = new DataEntryGridDisplayStyle();
            else 
                displayStyle = Grid.GetDisplayStyle(displayStyleId, Row);

            if (Grid.DefaultSelectionBrush != null && displayStyle.SelectionBrush == null)
            {
                displayStyle.SelectionBrush = Grid.DefaultSelectionBrush;
            }

            return displayStyle;
        }
    }
}
