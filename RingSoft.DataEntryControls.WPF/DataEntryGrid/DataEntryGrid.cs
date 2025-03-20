// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 01-02-2025
// ***********************************************************************
// <copyright file="DataEntryGrid.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// A control that enters data in a grid.
    /// Implements the <see cref="DataGrid" />
    /// Implements the <see cref="IDataEntryGrid" />
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    /// </summary>
    /// <seealso cref="DataGrid" />
    /// <seealso cref="IDataEntryGrid" />
    /// <seealso cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    public class DataEntryGrid : DataGrid, IDataEntryGrid, IReadOnlyControl
    {
        /// <summary>
        /// Class CellSnapshot.
        /// </summary>
        private class CellSnapshot
        {
            /// <summary>
            /// Gets or sets the index of the row.
            /// </summary>
            /// <value>The index of the row.</value>
            public int RowIndex { get; set; }
            /// <summary>
            /// Gets or sets the index of the column.
            /// </summary>
            /// <value>The index of the column.</value>
            public int ColumnIndex { get; set; }

            /// <summary>
            /// Gets or sets the index of the bottom visible row.
            /// </summary>
            /// <value>The index of the bottom visible row.</value>
            public int BottomVisibleRowIndex { get; set; }

            /// <summary>
            /// Gets or sets the index of the right visible column.
            /// </summary>
            /// <value>The index of the right visible column.</value>
            public int RightVisibleColumnIndex { get; set; }
        }

        /// <summary>
        /// The manager property
        /// </summary>
        public static readonly DependencyProperty ManagerProperty =
            DependencyProperty.Register(nameof(Manager), typeof(DataEntryGridManager), typeof(DataEntryGrid),
                new FrameworkPropertyMetadata(ManagerChangedCallback));

        /// <summary>
        /// Gets or sets the manager.  This is a bind-able property.
        /// </summary>
        /// <value>The manager.</value>
        public DataEntryGridManager Manager
        {
            get { return (DataEntryGridManager)GetValue(ManagerProperty); }
            set { SetValue(ManagerProperty, value); }
        }

        /// <summary>
        /// Managers the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ManagerChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryGrid = (DataEntryGrid)obj;
            if (dataEntryGrid._controlLoaded)
                dataEntryGrid.SetManager();
        }

        /// <summary>
        /// The enter to tab property
        /// </summary>
        public static readonly DependencyProperty EnterToTabProperty =
            DependencyProperty.Register(nameof(EnterToTab), typeof(bool), typeof(DataEntryGrid));

        /// <summary>
        /// Gets or sets a value indicating whether to convert ENTER key to Tab key.  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [enter to tab]; otherwise, <c>false</c>.</value>
        public bool EnterToTab
        {
            get { return (bool)GetValue(EnterToTabProperty); }
            set { SetValue(EnterToTabProperty, value); }
        }

        /// <summary>
        /// The close window on escape property
        /// </summary>
        public static readonly DependencyProperty CloseWindowOnEscapeProperty =
            DependencyProperty.Register(nameof(CloseWindowOnEscape), typeof(bool), typeof(DataEntryGrid));

        /// <summary>
        /// Gets or sets a value indicating whether [close window on escape].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [close window on escape]; otherwise, <c>false</c>.</value>
        public bool CloseWindowOnEscape
        {
            get { return (bool)GetValue(CloseWindowOnEscapeProperty); }
            set { SetValue(CloseWindowOnEscapeProperty, value); }
        }

        /// <summary>
        /// The cell editing control border thickness property.
        /// </summary>
        public static readonly DependencyProperty CellEditingControlBorderThicknessProperty =
            DependencyProperty.Register(nameof(CellEditingControlBorderThickness), typeof(Thickness),
                typeof(DataEntryGrid));

        /// <summary>
        /// Gets or sets the cell editing control border thickness.  This is a bind-able property.
        /// </summary>
        /// <value>The cell editing control border thickness.</value>
        public Thickness CellEditingControlBorderThickness
        {
            get { return (Thickness)GetValue(CellEditingControlBorderThicknessProperty); }
            set { SetValue(CellEditingControlBorderThicknessProperty, value); }
        }

        /// <summary>
        /// The read only mode property
        /// </summary>
        public static readonly DependencyProperty ReadOnlyModeProperty =
            DependencyProperty.Register(nameof(ReadOnlyMode), typeof(bool), typeof(DataEntryGrid),
                new FrameworkPropertyMetadata(ReadOnlyModeChangedCallback));

        /// <summary>
        /// Gets or sets a value indicating whether [read only mode].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [read only mode]; otherwise, <c>false</c>.</value>
        public bool ReadOnlyMode
        {
            get { return (bool)GetValue(ReadOnlyModeProperty); }
            set { SetValue(ReadOnlyModeProperty, value); }
        }

        /// <summary>
        /// Reads the only mode changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ReadOnlyModeChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryGrid = (DataEntryGrid)obj;
            dataEntryGrid.SetReadOnlyMode(dataEntryGrid.ReadOnlyMode);
        }


        /// <summary>
        /// The set tab focus to selected cell property
        /// </summary>
        public static readonly DependencyProperty SetTabFocusToSelectedCellProperty =
            DependencyProperty.Register(nameof(SetTabFocusToSelectedCell), typeof(bool), typeof(DataEntryGrid));

        /// <summary>
        /// Gets or sets a value indicating whether [set tab focus to selected cell].This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [set tab focus to selected cell]; otherwise, <c>false</c>.</value>
        public bool SetTabFocusToSelectedCell
        {
            get { return (bool)GetValue(SetTabFocusToSelectedCellProperty); }
            set { SetValue(SetTabFocusToSelectedCellProperty, value); }
        }

        /// <summary>
        /// The store current cell on load grid property
        /// </summary>
        public static readonly DependencyProperty StoreCurrentCellOnLoadGridProperty =
            DependencyProperty.Register(nameof(StoreCurrentCellOnLoadGrid), typeof(bool), typeof(DataEntryGrid));

        /// <summary>
        /// Gets or sets a value indicating whether [store current cell on load grid].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [store current cell on load grid]; otherwise, <c>false</c>.</value>
        public bool StoreCurrentCellOnLoadGrid
        {
            get { return (bool)GetValue(StoreCurrentCellOnLoadGridProperty); }
            set { SetValue(StoreCurrentCellOnLoadGridProperty, value); }
        }

        /// <summary>
        /// The store current cell on lost focus property
        /// </summary>
        public static readonly DependencyProperty StoreCurrentCellOnLostFocusProperty =
            DependencyProperty.Register(nameof(StoreCurrentCellOnLostFocus), typeof(bool), typeof(DataEntryGrid));

        /// <summary>
        /// Gets or sets a value indicating whether [store current cell on lost focus].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [store current cell on lost focus]; otherwise, <c>false</c>.</value>
        public bool StoreCurrentCellOnLostFocus
        {
            get { return (bool)GetValue(StoreCurrentCellOnLostFocusProperty); }
            set { SetValue(StoreCurrentCellOnLostFocusProperty, value); }
        }

        /// <summary>
        /// The disabled cell display style property
        /// </summary>
        public static readonly DependencyProperty DisabledCellDisplayStyleProperty =
            DependencyProperty.Register(nameof(DisabledCellDisplayStyle), typeof(DataEntryGridDisplayStyle),
                typeof(DataEntryGrid));

        /// <summary>
        /// Gets or sets the disabled cell display style.  This is a bind-able property.
        /// </summary>
        /// <value>The disabled cell display style.</value>
        public DataEntryGridDisplayStyle DisabledCellDisplayStyle
        {
            get { return (DataEntryGridDisplayStyle)GetValue(DisabledCellDisplayStyleProperty); }
            set { SetValue(DisabledCellDisplayStyleProperty, value); }
        }

        /// <summary>
        /// The default selection brush property
        /// </summary>
        public static readonly DependencyProperty DefaultSelectionBrushProperty =
            DependencyProperty.Register(nameof(DefaultSelectionBrush), typeof(Brush), typeof(DataEntryGrid));

        /// <summary>
        /// Gets or sets the default selection brush.  This is a bind-able property.
        /// </summary>
        /// <value>The default selection brush.</value>
        public Brush DefaultSelectionBrush
        {
            get { return (Brush)GetValue(DefaultSelectionBrushProperty); }
            set { SetValue(DefaultSelectionBrushProperty, value); }
        }

        /// <summary>
        /// Coerces the can user add rows property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns>System.Object.</returns>
        private static object CoerceCanUserAddRowsProperty(DependencyObject obj, object baseValue)
        {
            return false;
        }


        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public new ObservableCollection<DataEntryGridColumn> Columns { get; } =
            new ObservableCollection<DataEntryGridColumn>();

        /// <summary>
        /// Gets the display styles.
        /// </summary>
        /// <value>The display styles.</value>
        public ObservableCollection<DataEntryGridDisplayStyle> DisplayStyles { get; } =
            new ObservableCollection<DataEntryGridDisplayStyle>();

        /// <summary>
        /// Gets or sets a value indicating whether [data entry can user add rows].
        /// </summary>
        /// <value><c>true</c> if [data entry can user add rows]; otherwise, <c>false</c>.</value>
        public bool DataEntryCanUserAddRows { get; set; } = true;
        /// <summary>
        /// Gets the current row.
        /// </summary>
        /// <value>The current row.</value>
        public DataEntryGridRow CurrentRow => GetCurrentRow();
        /// <summary>
        /// Gets the index of the current row.
        /// </summary>
        /// <value>The index of the current row.</value>
        public int CurrentRowIndex => GetCurrentRowIndex();
        /// <summary>
        /// Gets the current column identifier.
        /// </summary>
        /// <value>The current column identifier.</value>
        public int CurrentColumnId => GetCurrentColumnId();

        /// <summary>
        /// Gets a value indicating whether [automatic generate columns].
        /// </summary>
        /// <value><c>true</c> if [automatic generate columns]; otherwise, <c>false</c>.</value>
        public new bool AutoGenerateColumns
        {
            get => base.AutoGenerateColumns;
            private set => base.AutoGenerateColumns = value;
        }

        /// <summary>
        /// Gets the selection unit.
        /// </summary>
        /// <value>The selection unit.</value>
        public new DataGridSelectionUnit SelectionUnit
        {
            get => base.SelectionUnit;
            private set => base.SelectionUnit = value;
        }

        /// <summary>
        /// Gets or sets the height of the row.
        /// </summary>
        /// <value>The height of the row.</value>
        public new double RowHeight
        {
            get => base.RowHeight;
            set
            {
                base.RowHeight = value;
                //DesignerFillGrid(nameof(RowHeight));
            }
        }

        /// <summary>
        /// Gets the active cell editing control host.
        /// </summary>
        /// <value>The editing control host.</value>
        public DataEntryGridEditingControlHostBase EditingControlHost { get; private set; }

        /// <summary>
        /// The data source table
        /// </summary>
        private DataTable _dataSourceTable = new DataTable("DataSource");
        /// <summary>
        /// The control loaded
        /// </summary>
        private bool _controlLoaded;
        /// <summary>
        /// The grid has focus
        /// </summary>
        private bool _gridHasFocus;
        /// <summary>
        /// The bulk insert mode
        /// </summary>
        private bool _bulkInsertMode;
        /// <summary>
        /// The designer filling grid
        /// </summary>
        private bool _designerFillingGrid;
        /// <summary>
        /// The button click
        /// </summary>
        private bool _buttonClick;
        /// <summary>
        /// The initialize cell
        /// </summary>
        private CellSnapshot _initCell;
        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;
        /// <summary>
        /// The cell snapshot
        /// </summary>
        private CellSnapshot _cellSnapshot;

        /// <summary>
        /// Initializes static members of the <see cref="DataEntryGrid" /> class.
        /// </summary>
        static DataEntryGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGrid),
                new FrameworkPropertyMetadata(typeof(DataEntryGrid)));

            CellEditingControlBorderThicknessProperty.OverrideMetadata(typeof(DataEntryGrid),
                new FrameworkPropertyMetadata(new Thickness(0)));

            var disabledCellDisplayStyle = new DataEntryGridDisplayStyle
            {
                BackgroundBrush = new SolidColorBrush(Colors.DarkGray),
                ForegroundBrush = new SolidColorBrush(Colors.Black)
            };

            DisabledCellDisplayStyleProperty.OverrideMetadata(typeof(DataEntryGrid),
                new FrameworkPropertyMetadata(disabledCellDisplayStyle));

            CanUserAddRowsProperty.OverrideMetadata(typeof(DataEntryGrid),
                new FrameworkPropertyMetadata(null, CoerceCanUserAddRowsProperty));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGrid" /> class.
        /// </summary>
        public DataEntryGrid()
        {
            AutoGenerateColumns = false;
            SelectionUnit = DataGridSelectionUnit.Cell;

            ItemsSource = _dataSourceTable.DefaultView;

            Columns.CollectionChanged += Columns_CollectionChanged;
            Loaded += (sender, args) => OnLoad();
            GotFocus += DataEntryGrid_GotFocus;
            LostFocus += DataEntryGrid_LostFocus;

            Loaded += (sender, args) =>
            {
                if (_initCell != null)
                {
                    base.Focus();
                    SetFocusToCell(_initCell.RowIndex, _initCell.ColumnIndex);
                    _initCell = null;
                }
            };
        }

        /// <summary>
        /// Handles the LostFocus event of the DataEntryGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DataEntryGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                _gridHasFocus = false;
                EditingControlHost =
                    null; //This is done when DbMaintenance window resets focus on New Record operation.

                if (!StoreCurrentCellOnLostFocus)
                    SelectedCells.Clear();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.DataGrid.SelectedCellsChanged" /> event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnSelectedCellsChanged(SelectedCellsChangedEventArgs e)
        {
            if (SelectedCells.Any() && EditingControlHost != null && EditingControlHost.Control != null)
                //This needs to run when user clicks on the lookup button.
                SelectedCells.Clear();

            base.OnSelectedCellsChanged(e);
        }

        /// <summary>
        /// Focuses this instance.
        /// </summary>
        /// <returns><see langword="true" /> if keyboard focus and logical focus were set to this element; <see langword="false" /> if only logical focus was set to this element, or if the call to this method did not force the focus to change.</returns>
        public new bool Focus()
        {
            base.Focus();
            ResetGridFocus();
            return IsKeyboardFocusWithin;
        }

        /// <summary>
        /// Handles the GotFocus event of the DataEntryGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DataEntryGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            var beginEdit = true;

            if (EditingControlHost == null)
            {
                if (Keyboard.IsKeyDown(Key.Tab) || Keyboard.IsKeyDown(Key.Enter))
                {
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        var lastRowIndex = Items.Count - 1;
                        var lastColumnIndex = Columns.Count - 1;
                        if (!_gridHasFocus)
                        {
                            //Shift+Tab from outside the grid.  Set active cell to next editable cell from cell on Last Row, Last Column.
                            beginEdit = false;
                            _gridHasFocus = true; //Set to avoid double tab.
                            if (!SetFocusToSelectedCell())
                                TabLeft(lastRowIndex, lastColumnIndex + 1);
                        }
                    }
                    else
                    {
                        if (!_gridHasFocus)
                        {
                            //Tab from outside the grid.  Set active cell to next editable cell from cell on First Row, First Column.
                            beginEdit = false;
                            _gridHasFocus = true; //Set to avoid double tab.
                            if (!SetFocusToSelectedCell())
                                TabRight(0, -1);
                        }
                    }
                }
            }

            if (beginEdit)
            {
                BeginEdit();
            }

            _gridHasFocus = true;
        }

        /// <summary>
        /// Sets the focus to selected cell.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SetFocusToSelectedCell()
        {
            if (!SetTabFocusToSelectedCell || !SelectedCells.Any())
                return false;

            SetFocusToCell(GetSelectedRowIndex(), GetSelectedColumnIndex());
            return true;
        }

        /// <summary>
        /// Refocuses this instance.
        /// </summary>
        public void Refocus()
        {
            BeginEdit();
        }

        //protected override void OnSelectedCellsChanged(SelectedCellsChangedEventArgs e)
        //{
        //    if (!StoreCurrentCellOnLostFocus && SelectedCells.Any())
        //        SelectedCells.Clear();

        //    base.OnSelectedCellsChanged(e);
        //}

        /// <summary>
        /// Called when [load].
        /// </summary>
        private void OnLoad()
        {
            if (IsVisible)
            {
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    if (Manager != null)
                    {
                        if (!_controlLoaded)
                            SetManager();

                        RefreshGridView();
                    }

                    _controlLoaded = true;

                }
            }

            //if ((Items.Count > 0) &&
            //    (Columns.Count > 0))
            //{
            //    //Select the first column of the first item.
            //    SetFocusToCell(0, 1, false);
            //    //CurrentCell = new DataGridCellInfo(Items[0], Columns[0]);
            //    //SelectedCells.Add(CurrentCell);
            //}

            var scrollViewer = this.GetVisualChild<ScrollViewer>();
            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += (sender, args) =>
                {
                    if (args.VerticalChange > 0 || args.VerticalChange < 0)
                    {
                        RefreshGridView();
                    }
                };
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Columns control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.Exception"></exception>
        private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var startIndex = -1;
                    if (e.NewStartingIndex < base.Columns.Count)
                        startIndex = e.NewStartingIndex;

                    foreach (DataEntryGridColumn column in e.NewItems)
                    {
                        //var columnIdCount = Columns.Count(w => w.ColumnId == column.ColumnId);

                        //if (columnIdCount > 1)
                        //{
                        //    var message = $"There are {columnIdCount} columns with Column ID '{column.ColumnId}'";
                        //    MessageBox.Show(message);
                        //    throw new Exception(message);
                        //}

                        if (startIndex < 0)
                            base.Columns.Add(column);
                        else
                            base.Columns.Insert(startIndex, column);

                        column.PropertyChanged += Column_PropertyChanged;
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var unused in e.OldItems)
                    {
                        base.Columns.RemoveAt(e.OldStartingIndex);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _dataSourceTable.Columns.Clear();
            var index = 0;

            foreach (var column in Columns)
            {
                var columnName = $"Column{index}";
                //if (column is DataEntryGridCheckBoxColumn)
                //{
                //    var boolColumn = new DataColumn(columnName, typeof(bool));
                //    _dataSourceTable.Columns.Add(boolColumn);
                //}
                //else
                //{
                //    _dataSourceTable.Columns.Add(columnName);
                //}
                _dataSourceTable.Columns.Add(columnName);
                column.DataColumnName = columnName;
                index++;
            }
            //DesignerFillGrid(nameof(Columns_CollectionChanged));
        }

        /// <summary>
        /// Handles the PropertyChanged event of the Column control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void Column_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //DesignerFillGrid(nameof(Column_PropertyChanged));
        }

        /// <summary>
        /// Gets the column index of column identifier.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>System.Int32.</returns>
        private int GetColumnIndexOfColumnId(int columnId)
        {
            var column = Columns.FirstOrDefault(f => f.ColumnId == columnId);
            return Columns.IndexOf(column);
        }


        /// <summary>
        /// Adds the designer row.
        /// </summary>
        private void AddDesignerRow()
        {
            var designerRow = _dataSourceTable.NewRow();

            foreach (var column in Columns)
            {
                column.ValidateDesignerDataValue();
                designerRow[column.DataColumnName] = column.DesignerDataValue;
            }

            _dataSourceTable.Rows.Add(designerRow);

        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.DataGrid.CurrentCellChanged" /> event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnCurrentCellChanged(EventArgs e)
        {
            UpdateColumnHeaders();

            if (CurrentCell.Column == null || CurrentCell.Item == null)
                return;

            var cellContent = CurrentCell.Column.GetCellContent(CurrentCell.Item);
            if (cellContent != null && cellContent.Parent is DataGridCell dataGridCell)
            {
                var contextMenu = new ContextMenu();
                AddGridContextMenuItems(contextMenu);
                dataGridCell.ContextMenu = contextMenu;
                dataGridCell.ContextMenu.Placement = PlacementMode.Bottom;
            }

            base.OnCurrentCellChanged(e);
        }

        /// <summary>
        /// Updates the column headers.
        /// </summary>
        private void UpdateColumnHeaders()
        {
            var rowIndex = GetCurrentRowIndex();
            if (rowIndex >= 0 && rowIndex < Manager.Rows.Count)
            {
                var gridRow = Manager.Rows[rowIndex];
                foreach (var column in Columns)
                {
                    column.ResetColumnHeader();
                    var cellStyle = GetCellStyle(gridRow, column.ColumnId);
                    if (!string.IsNullOrEmpty(cellStyle.ColumnHeader))
                        column.Header = cellStyle.ColumnHeader;
                }
            }
        }

        /// <summary>
        /// Sets the manager.
        /// </summary>
        private void SetManager()
        {
            Manager.RowsChanged += GridRows_CollectionChanged;

            if (Manager.Rows.Any())
                RefreshDataSource();

            Manager.SetupGrid(this);
        }

        /// <summary>
        /// Handles the CollectionChanged event of the GridRows control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void GridRows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var startIndex = -1;
                    if (e.NewStartingIndex < _dataSourceTable.Rows.Count)
                        startIndex = e.NewStartingIndex;
                    foreach (DataEntryGridRow row in e.NewItems)
                    {
                        AddRow(row, startIndex);
                    }

                    if (e.NewItems.Count > 0 && !_bulkInsertMode)
                        RefreshGridView();

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var unused in e.OldItems)
                    {
                        _dataSourceTable.Rows.RemoveAt(e.OldStartingIndex);
                    }

                    //RefreshGridView();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _dataSourceTable.Rows.Clear();
                    RefreshGridView();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Sets the bulk insert mode.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void SetBulkInsertMode(bool value = true)
        {
            _bulkInsertMode = value;
            if (!_bulkInsertMode)
                RefreshGridView();
        }

        /// <summary>
        /// Takes the cell snapshot.
        /// </summary>
        /// <param name="doOnlyWhenGridHasFocus">if set to <c>true</c> [do only when grid has focus].</param>
        public void TakeCellSnapshot(bool doOnlyWhenGridHasFocus = true)
        {
            bool takeSnapshot = !(doOnlyWhenGridHasFocus && !IsKeyboardFocusWithin);
            if (takeSnapshot)
                takeSnapshot = StoreCurrentCellOnLoadGrid;

            if (takeSnapshot)
            {
                _cellSnapshot = new CellSnapshot
                {
                    RowIndex = GetSelectedRowIndex(),
                    ColumnIndex = GetSelectedColumnIndex(),
                    BottomVisibleRowIndex = GetBottomVisibleRowIndex(),
                    RightVisibleColumnIndex = GetRightVisibleColumnIndex()
                };
            }
        }

        /// <summary>
        /// Gets the index of the bottom visible row.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetBottomVisibleRowIndex()
        {
            var result = Manager.Rows.Count - 1;
            var currentColumIndex = GetSelectedColumnIndex();
            for (int i = Manager.Rows.Count - 1; i >= 0; i--)
            {
                var cell = GetDataGridCell(i, currentColumIndex);
                if (cell.IsUserVisible(this))
                {
                    result = i;
                    break;
                }
            }

            if (result < Manager.Rows.Count - 1)
                result--;
            else
            {
                var topRowCell = GetDataGridCell(0, currentColumIndex);
                if (!topRowCell.IsUserVisible(this))
                    //We're at the second to bottom row and the first row is not visible.
                    result--;
            }

            return result;
        }

        /// <summary>
        /// Gets the index of the right visible column.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetRightVisibleColumnIndex()
        {
            var result = Columns.Count - 1;
            var currentRowIndex = GetSelectedRowIndex();
            for (int i = Columns.Count - 1; i >= 0; i--)
            {
                var cell = GetDataGridCell(currentRowIndex, i);
                if (cell.IsUserVisible(this))
                {
                    result = i;
                    break;
                }
            }

            if (result < Columns.Count - 1)
                result--;
            else
            {
                var leftColumnCell = GetDataGridCell(currentRowIndex, 0);
                if (!leftColumnCell.IsUserVisible(this))
                    //We're at the second to right column and first column is not visible.
                    result--;
            }

            return result;
        }

        /// <summary>
        /// Gets the data grid cell.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>DataGridCell.</returns>
        private DataGridCell GetDataGridCell(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0)
                return null;

            if (ItemContainerGenerator.ContainerFromItem(Items[rowIndex]) is DataGridRow dataGridRow)
            {
                if (columnIndex < 0)
                    return null;

                var column = Columns[columnIndex];
                var cellContent = column.GetCellContent(dataGridRow);
                if (cellContent != null)
                {
                    if (cellContent.Parent is DataGridCell dataGridCell)
                        return dataGridCell;
                }
            }

            return null;
        }

        /// <summary>
        /// Restores the cell snapshot.
        /// </summary>
        /// <param name="doOnlyWhenGridHasFocus">if set to <c>true</c> [do only when grid has focus].</param>
        public void RestoreCellSnapshot(bool doOnlyWhenGridHasFocus = true)
        {
            bool restoreSnapshot = !(doOnlyWhenGridHasFocus && !IsKeyboardFocusWithin);
            if (restoreSnapshot)
            {
                if (_cellSnapshot == null)
                    ResetGridFocus();
                else if (StoreCurrentCellOnLoadGrid)
                {
                    SetFocusToCell(_cellSnapshot.BottomVisibleRowIndex, _cellSnapshot.RightVisibleColumnIndex, false);
                    SetFocusToCell(_cellSnapshot.RowIndex, _cellSnapshot.ColumnIndex, IsKeyboardFocusWithin);
                    _cellSnapshot = null;
                }
            }
        }

        /// <summary>
        /// Refreshes the data source.
        /// </summary>
        public void RefreshDataSource()
        {
            var refreshEdit = false;
            DataEntryGridRow currentRow = null;
            int currentColumnId = 0;
            if (EditingControlHost != null)
            {
                refreshEdit = true;
                currentRow = Manager.Rows[GetCurrentRowIndex()];
                currentColumnId = Columns[GetCurrentColumnIndex()].ColumnId;
                CancelEdit();
            }

            _dataSourceTable.Rows.Clear();
            foreach (var gridRow in Manager.Rows)
            {
                AddRow(gridRow);
            }

            if (refreshEdit)
                GotoCell(currentRow, currentColumnId);
        }

        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="gridRow">The grid row.</param>
        /// <param name="index">The index.</param>
        private void AddRow(DataEntryGridRow gridRow, int index = -1)
        {
            var dataRow = _dataSourceTable.NewRow();

            if (index < 0)
            {
                _dataSourceTable.Rows.Add(dataRow);
                index = _dataSourceTable.Rows.IndexOf(dataRow);
            }
            else
                _dataSourceTable.Rows.InsertAt(dataRow, index);

            UpdateLayout();
            UpdateRow(gridRow, dataRow, index);
        }

        //This overload should only be run outside by the interface.  Otherwise it will update column headers for each row and slow down performance.
        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="gridRow">The grid row.</param>
        /// <exception cref="System.Exception">This row must be added to the Rows collection before it can be updated.</exception>
        public void UpdateRow(DataEntryGridRow gridRow)
        {
            var rowIndex = Manager.Rows.IndexOf(gridRow);
            if (rowIndex < 0)
                throw new Exception(
                    "This row must be added to the Rows collection before it can be updated.");

            UpdateRow(gridRow, rowIndex);
            UpdateColumnHeaders();
        }

        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="gridRow">The grid row.</param>
        /// <param name="rowIndex">Index of the row.</param>
        public void UpdateRow(DataEntryGridRow gridRow, int rowIndex)
        {
            if (rowIndex < _dataSourceTable.Rows.Count)
                UpdateRow(gridRow, _dataSourceTable.Rows[rowIndex], rowIndex);
        }



        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="gridRow">The grid row.</param>
        /// <param name="dataRow">The data row.</param>
        /// <param name="rowIndex">Index of the row.</param>
        private void UpdateRow(DataEntryGridRow gridRow, DataRow dataRow, int rowIndex)
        {
            if (ItemContainerGenerator.ContainerFromItem(Items[rowIndex]) is DataGridRow dataGridRow)
            {
                UpdateRowColors(dataGridRow, gridRow);
            }

            foreach (var column in Columns)
            {
                var dataTableColumn = _dataSourceTable.Columns[column.DataColumnName];
                var cellProps = gridRow.GetCellProps(column.ColumnId);
                if (column.ColumnType == DataEntryGridColumnTypes.Control)
                    cellProps.ControlMode = true;

                //if (column is DataEntryGridCheckBoxColumn)
                //{
                //    if (cellProps is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                //        dataRow[dataTableColumn] = checkBoxCellProps.Value;
                //    else
                //        dataRow[dataTableColumn] = false;
                //}
                //else
                //{
                //    dataRow[dataTableColumn] = cellProps.DataValue;
                //}
                dataRow[dataTableColumn] = cellProps.DataValue;

                UpdateCellColors(gridRow, column);
            }
        }

        /// <summary>
        /// Refreshes the grid view.
        /// </summary>
        public void RefreshGridView()
        {
            UpdateLayout();

            var rowIndex = 0;
            foreach (var gridRow in Manager.Rows)
            {
                UpdateRow(gridRow);
                if (ItemContainerGenerator.ContainerFromItem(Items[rowIndex]) is DataGridRow dataGridRow)
                {
                    UpdateRowColors(dataGridRow, gridRow);

                    foreach (var column in Columns)
                    {
                        UpdateCellColors(column, dataGridRow, gridRow);
                    }
                }

                rowIndex++;
            }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <returns>List&lt;ColumnMap&gt;.</returns>
        public List<ColumnMap> GetColumns()
        {
            var result = new List<ColumnMap>();
            foreach (var column in Columns)
            {
                var header = column.Header as string;
                result.Add(new ColumnMap(column.ColumnId, header));
            }
            return result;
        }

        /// <summary>
        /// Updates the row colors.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="gridRow">The grid row.</param>
        private void UpdateRowColors(DataGridRow dataGridRow, DataEntryGridRow gridRow)
        {
            dataGridRow.ClearValue(BackgroundProperty);
            dataGridRow.ClearValue(ForegroundProperty);

            if (gridRow.DisplayStyleId > 0)
            {
                var displayStyle = GetDisplayStyle(gridRow.DisplayStyleId, gridRow);

                if (displayStyle.BackgroundBrush != null)
                    dataGridRow.Background = displayStyle.BackgroundBrush;
                if (displayStyle.ForegroundBrush != null)
                    dataGridRow.Foreground = displayStyle.ForegroundBrush;
            }
        }

        /// <summary>
        /// Gets the display style.
        /// </summary>
        /// <param name="displayStyleId">The display style identifier.</param>
        /// <param name="dataEntryGridRow">The data entry grid row.</param>
        /// <returns>DataEntryGridDisplayStyle.</returns>
        /// <exception cref="System.Exception"></exception>
        internal DataEntryGridDisplayStyle GetDisplayStyle(int displayStyleId, DataEntryGridRow dataEntryGridRow = null)
        {
            var displayStyle = DisplayStyles.FirstOrDefault(f => f.DisplayId == displayStyleId);
            if (displayStyle == null)
            {
                var message = $"DisplayStyle not found for DisplayStyleId {displayStyleId}";
                if (dataEntryGridRow != null)
                    message += $" of Row {dataEntryGridRow}";

                throw new Exception(message);
            }

            return displayStyle;
        }

        /// <summary>
        /// Updates the cell colors.
        /// </summary>
        /// <param name="gridRow">The grid row.</param>
        /// <param name="column">The column.</param>
        private void UpdateCellColors(DataEntryGridRow gridRow, DataEntryGridColumn column)
        {
            var rowIndex = Manager.Rows.IndexOf(gridRow);
            if (ItemContainerGenerator.ContainerFromItem(Items[rowIndex]) is DataGridRow dataGridRow)
            {
                UpdateCellColors(column, dataGridRow, gridRow);
            }
        }

        /// <summary>
        /// Updates the cell colors.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="gridRow">The grid row.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void UpdateCellColors(DataEntryGridColumn column, DataGridRow dataGridRow, DataEntryGridRow gridRow)
        {
            var cellContent = column.GetCellContent(dataGridRow);
            if (cellContent != null)
            {
                if (cellContent.Parent is DataGridCell dataGridCell)
                {
                    var cellStyle = GetCellStyle(gridRow, column.ColumnId);

                    DataEntryGridDisplayStyle displayStyle = null;

                    switch (cellStyle.State)
                    {
                        case DataEntryGridCellStates.Enabled:
                            break;
                        case DataEntryGridCellStates.ReadOnly:
                            break;
                        case DataEntryGridCellStates.Disabled:
                            displayStyle = new DataEntryGridDisplayStyle
                            {
                                DisplayId = 0,
                                BackgroundBrush = DisabledCellDisplayStyle.BackgroundBrush,
                                ForegroundBrush = DisabledCellDisplayStyle.ForegroundBrush,
                                SelectionBrush = DisabledCellDisplayStyle.SelectionBrush
                            };
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (displayStyle == null)
                        displayStyle = new DataEntryGridDisplayStyle();

                    if (cellStyle.DisplayStyleId > 0)
                    {
                        var cellDisplayStyle = GetDisplayStyle(cellStyle.DisplayStyleId, gridRow);

                        if (cellDisplayStyle.BackgroundBrush != null)
                            displayStyle.BackgroundBrush = cellDisplayStyle.BackgroundBrush;

                        if (cellDisplayStyle.ForegroundBrush != null)
                            displayStyle.ForegroundBrush = cellDisplayStyle.ForegroundBrush;
                    }

                    dataGridCell.ClearValue(BackgroundProperty);
                    dataGridCell.ClearValue(ForegroundProperty);
                    dataGridCell.ClearValue(BorderBrushProperty);

                    if (displayStyle.BackgroundBrush != null)
                    {
                        dataGridCell.Background = displayStyle.BackgroundBrush;
                        if (GridLinesVisibility == DataGridGridLinesVisibility.None)
                            dataGridCell.BorderBrush = dataGridCell.Background;
                    }

                    if (displayStyle.ForegroundBrush != null)
                    {
                        dataGridCell.Foreground = displayStyle.ForegroundBrush;
                    }
                }
            }
        }


        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.DataGrid.BeginningEdit" /> event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void OnBeginningEdit(DataGridBeginningEditEventArgs e)
        {
            if (e.Column is DataEntryGridColumn dataEntryGridColumn)
            {
                var dataEntryGridRow = Manager.Rows[e.Row.GetIndex()];
                var cellStyle = GetCellStyle(dataEntryGridRow, dataEntryGridColumn.ColumnId);

                if (dataEntryGridColumn.Visibility == Visibility.Visible)
                {
                    switch (cellStyle.State)
                    {
                        case DataEntryGridCellStates.Enabled:
                            break;
                        case DataEntryGridCellStates.Disabled:
                        case DataEntryGridCellStates.ReadOnly:
                            var readOnlyCellProps =
                                dataEntryGridRow.GetCellProps(dataEntryGridColumn.ColumnId) as
                                    DataEntryGridEditingCellProps;

                            if (readOnlyCellProps == null)
                                e.Cancel = true;

                            var host =
                                WPFControlsGlobals.DataEntryGridHostFactory.GetControlHost(this,
                                    readOnlyCellProps.EditingControlId);
                            if (!host.AllowReadOnlyEdit)
                            {
                                e.Cancel = true;
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    e.Cancel = true;
                }

                var cellProps =
                    dataEntryGridRow.GetCellProps(dataEntryGridColumn.ColumnId) as DataEntryGridEditingCellProps;

                if (cellProps == null)
                    e.Cancel = true;

                if (e.Cancel)
                {
                    if (!SelectedCells.Any())
                        SelectedCells.Add(CurrentCell);
                }
                else
                {
                    if (cellProps != null)
                    {
                        cellProps.ControlMode = true;
                        EditingControlHost =
                            WPFControlsGlobals.DataEntryGridHostFactory.GetControlHost(this,
                                cellProps.EditingControlId);

                        if (!EditingControlHost.SetReadOnlyMode(_readOnlyMode))
                        {
                            EditingControlHost = null;
                            cellProps.ControlMode = false;
                            e.Cancel = true;
                        }
                        else
                        {
                            if (EditingControlHost.SetSelection && !SelectedCells.Any())
                                SelectedCells.Add(CurrentCell);

                            dataEntryGridColumn.CellEditingTemplate =
                                EditingControlHost.GetEditingControlDataTemplate(cellProps, cellStyle,
                                    dataEntryGridColumn);
                            EditingControlHost.ControlDirty += EditingControl_ControlDirty;
                            EditingControlHost.UpdateSource += EditingControlHost_UpdateSource;

                            if (_buttonClick)
                            {
                                if (!ReadOnlyMode)
                                {
                                    dataEntryGridRow.SetCellValue(cellProps);
                                }

                                _buttonClick = false;
                            }
                        }
                    }
                }
            }

            base.OnBeginningEdit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.DataGrid.PreparingCellForEdit" /> event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnPreparingCellForEdit(DataGridPreparingCellForEditEventArgs e)
        {
            var element = e.EditingElement;

            var thisWindow = Window.GetWindow(this);
            var activeWindow = WPFControlsGlobals.ActiveWindow;
            if (thisWindow == activeWindow)
            {
                element.Dispatcher.BeginInvoke(
                    DispatcherPriority.Input,
                    new Action(() => element.MoveFocus(
                        new TraversalRequest(FocusNavigationDirection.First))));
            }

            base.OnPreparingCellForEdit(e);
        }

        /// <summary>
        /// Gets the current cell.
        /// </summary>
        /// <returns>DataGridCell.</returns>
        public DataGridCell GetCurrentCell()
        {
            var cellContent = CurrentCell.Column.GetCellContent(CurrentCell.Item);
            if (cellContent != null && cellContent.Parent is DataGridCell dataGridCell)
            {
                return dataGridCell;
            }

            return null;
        }

        /// <summary>
        /// Editings the control host update source.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void EditingControlHost_UpdateSource(object sender, DataEntryGridEditingCellProps e)
        {
            var rowIndex = Items.IndexOf(CurrentCell.Item);

            if (!EditingControlHost.HasDataChanged())
                return;

            var dataEntryGridRow = Manager.Rows[rowIndex];
            dataEntryGridRow.SetCellValue(e);
        }

        /// <summary>
        /// Handles the ControlDirty event of the EditingControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void EditingControl_ControlDirty(object sender, EventArgs e)
        {
            var currentRowIndex = Items.IndexOf(CurrentCell.Item);
            if (currentRowIndex >= Items.Count - 1)
            {
                if (DataEntryCanUserAddRows)
                    Manager.InsertNewRow();
            }

            Manager.RaiseDirtyFlag();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.DataGrid.CellEditEnding" /> event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
        {
            EditingControlHost = null;
            base.OnCellEditEnding(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyboardFocusChangedEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus != null && EditingControlHost != null && EditingControlHost.Control != null &&
                !e.NewFocus.Equals(EditingControlHost.Control))
            {
                if (e.NewFocus is DependencyObject newFocus)
                {
                    if (!(newFocus is ContextMenu))
                    {
                        var parent = newFocus.GetParentOfType(EditingControlHost.Control.GetType());
                        if (!EditingControlHost.Control.Equals(parent))
                        {
                            if (!Window.GetWindow(this).IsWindowClosing(e.NewFocus))
                            {
                                if (!CommitCellEdit(CellLostFocusTypes.LostFocus))
                                {
                                    e.Handled = true;
                                    if (!StoreCurrentCellOnLostFocus)
                                        SelectedCells.Clear();
                                }
                                else if (!e.NewFocus.Equals(this))
                                    CancelEdit();
                            }
                        }
                    }
                }
            }

            base.OnPreviewLostKeyboardFocus(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewGotKeyboardFocus" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyboardFocusChangedEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (EditingControlHost != null && EditingControlHost.Control != null &&
                e.OldFocus == null && e.NewFocus.GetType() == typeof(DataGridCell))
            {
                SetFocusToCell(GetCurrentRowIndex(), GetCurrentColumnIndex());
                _buttonClick = false;
            }
            else if ((EditingControlHost == null
                     || EditingControlHost is DataEntryGridButtonHost) && e.NewFocus is Button)
            {
                _buttonClick = true;
            }
            else
            {
                _buttonClick = false;
            }

            var thisWindow = Window.GetWindow(this);
            var activeWindow = WPFControlsGlobals.ActiveWindow;
            if (thisWindow != activeWindow)
            {
                e.Handled = true;
            }

            base.OnPreviewGotKeyboardFocus(e);
        }

        /// <summary>
        /// Commits the cell edit.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CommitCellEdit()
        {
            return CommitCellEdit(CellLostFocusTypes.LostFocus, false);
        }

        /// <summary>
        /// Commits the cell edit.
        /// </summary>
        /// <param name="cellLostFocusType">Type of the cell lost focus.</param>
        /// <param name="cancelEdit">if set to <c>true</c> [cancel edit].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CommitCellEdit(CellLostFocusTypes cellLostFocusType, bool cancelEdit = true)
        {
            if (EditingControlHost != null && EditingControlHost.Control != null)
            {
                var currentRowIndex = GetCurrentRowIndex();
                if (currentRowIndex < 0)
                {
                    CancelEdit();
                    return true;
                }

                var currentRow = Manager.Rows[currentRowIndex];
                var cellValue = EditingControlHost.GetCellValue();
                cellValue.CellLostFocusType = cellLostFocusType;

                if (EditingControlHost.HasDataChanged())
                {
                    var currentEditHost = EditingControlHost;
                    currentRow.SetCellValue(cellValue);

                    if (EditingControlHost == currentEditHost)
                    {
                        var newCellProps = currentRow.GetCellProps(cellValue.ColumnId);
                        EditingControlHost.UpdateFromCellProps(newCellProps);
                    }
                    else
                    {
                        //DataEntryGridRow SetCellValue changed cell focus.
                        return false;
                    }

                    if (cellValue.OverrideCellMovement)
                        return false;
                }
                else
                {
                    if (!currentRow.AllowEndEdit(cellValue))
                        return false;
                }
            }

            if (cancelEdit)
                CancelEdit();

            if (StoreCurrentCellOnLostFocus && !SelectedCells.Any())
                SelectedCells.Add(CurrentCell);

            return true;
        }

        /// <summary>
        /// Datas the entry grid cancel edit.
        /// </summary>
        public void DataEntryGridCancelEdit()
        {
            CancelEdit();

            EditingControlHost = null;
            _gridHasFocus = false;
        }

        /// <summary>
        /// Resets the grid focus.
        /// </summary>
        public void ResetGridFocus()
        {
            if (IsKeyboardFocusWithin)
            {
                if (_readOnlyMode)
                    SetFocusToCell(0, 0);
                else
                    TabRight(0, -1);
            }
        }

        /// <summary>
        /// Goes to the cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        public void GotoCell(DataEntryGridRow row, int columnId)
        {
            if (!IsKeyboardFocusWithin)
            {
                this.SetTabFocusToControl();
            }
            var rowIndex = Manager.Rows.IndexOf(row);
            var columnIndex = GetColumnIndexOfColumnId(columnId);
            SetFocusToCell(rowIndex, columnIndex);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var canProcessKey = true;
            if (EditingControlHost != null)
            {
                canProcessKey = EditingControlHost.CanGridProcessKey(e.Key);
                var currentRowIndex = Items.IndexOf(CurrentCell.Item);
                var currentColumnIndex = base.Columns.IndexOf(CurrentCell.Column);

                var atTopEdge = currentRowIndex <= 0;
                var atBottomEdge = currentRowIndex >= Items.Count - 1;
                var atLeftEdge = currentColumnIndex <= 0;
                var atRightEdge = currentColumnIndex >= base.Columns.Count - 1;

                var checkKey = false;
                switch (e.Key)
                {
                    case Key.Left:
                        if (atLeftEdge && canProcessKey)
                        {
                            e.Handled = true;
                            return;
                        }

                        checkKey = true;
                        break;
                    case Key.Right:
                        if (atRightEdge && canProcessKey)
                        {
                            e.Handled = true;
                            return;
                        }

                        checkKey = true;
                        break;
                    case Key.Up:
                        if (atTopEdge && canProcessKey)
                        {
                            e.Handled = true;
                            return;
                        }

                        checkKey = true;
                        break;
                    case Key.Down:
                        if (atBottomEdge && canProcessKey)
                        {
                            e.Handled = true;
                            return;
                        }

                        checkKey = true;
                        break;
                    case Key.PageUp:
                        if (atTopEdge && canProcessKey)
                        {
                            e.Handled = true;
                            return;
                        }

                        checkKey = true;
                        break;
                    case Key.PageDown:
                        if (atBottomEdge && canProcessKey)
                        {
                            e.Handled = true;
                            return;
                        }

                        checkKey = true;
                        break;
                }

                if (checkKey && canProcessKey)
                {
                    if (!CommitCellEdit(CellLostFocusTypes.KeyboardNavigation))
                        e.Handled = true;
                }
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Delete:
                        DeleteCurrentRow();
                        Manager.RaiseDirtyFlag();
                        e.Handled = true;
                        break;
                    case Key.Insert:
                        InsertRow();
                        e.Handled = true;
                        break;
                }
            }

            switch (e.Key)
            {
                case Key.Tab:
                    ProcessTab();
                    e.Handled = true;
                    break;
                case Key.Enter:
                    if (canProcessKey && EnterToTab)
                    {
                        ProcessTab();
                        e.Handled = true;
                    }

                    break;
                case Key.Escape:
                    if (canProcessKey)
                    {
                        if (CloseWindowOnEscape)
                        {
                            //Peter Ringering - 12/11/2024 01:30:34 PM - E-80
                            var window = Window.GetWindow(this);
                            var userControl = this.GetParentOfType<UserControl>();
                            if (userControl == null)
                            {
                                window?.Close();
                            }
                            else
                            {
                                var tabControl = userControl.GetParentOfType<TabControl>();
                                if (tabControl == null)
                                {
                                    window?.Close();
                                }
                            }
                        }

                        e.Handled = true;
                    }

                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Gets the header tab.
        /// </summary>
        /// <returns>TabItem.</returns>
        private TabItem GetHeaderTab()
        {
            var userControl = this.GetParentOfType<UserControl>();
            if (userControl == null)
            {
                return null;
            }
            else
            {
                var tabControl = userControl.GetParentOfType<TabControl>();
                if (tabControl == null)
                {
                    return null;
                }

                if (tabControl.SelectedItem is TabItem tabItem)
                {
                    return tabItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Invoked when the <see cref="E:System.Windows.UIElement.KeyDown" /> event is received.
        /// </summary>
        /// <param name="e">The Information about the event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EditingControlHost != null && EditingControlHost.Control != null)
            {
                if (!EditingControlHost.CanGridProcessKey(e.Key))
                {
                    //This is so calculator on double edit control can process enter key and prevents grid from cancelling edit on Enter.
                    //We don't set handled because otherwise the calculator won't update the edit control when the user presses Enter to
                    //get the results of a calculation.
                    return;
                }
                else
                {
                    if (e.Key == Key.Tab)
                    {
                        TabRight();
                        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                        {
                            return;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
            else
            {
                var keyChar = e.Key.GetCharFromKey();
                switch (keyChar)
                {
                    case ' ':
                    case '\t':
                        break;
                    default:
                        SystemSounds.Exclamation.Play();
                        break;
                }
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Processes the tab.
        /// </summary>
        private void ProcessTab()
        {
            var currentRowIndex = GetCurrentRowIndex();
            var currentColumnIndex = GetCurrentColumnIndex();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                TabLeft(currentRowIndex, currentColumnIndex);
            else
                TabRight(currentRowIndex, currentColumnIndex);
        }

        /// <summary>
        /// Gets the index of the selected row.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetSelectedRowIndex()
        {
            if (SelectedCells.Any())
                return GetCurrentRowIndex(SelectedCells[0]);

            return GetCurrentRowIndex();
        }

        /// <summary>
        /// Gets the index of the current row.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetCurrentRowIndex() => GetCurrentRowIndex(CurrentCell);

        /// <summary>
        /// Gets the index of the current row.
        /// </summary>
        /// <param name="cellInfo">The cell information.</param>
        /// <returns>System.Int32.</returns>
        private int GetCurrentRowIndex(DataGridCellInfo cellInfo)
        {
            return Items.IndexOf(cellInfo.Item);
        }

        /// <summary>
        /// Gets the index of the selected column.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetSelectedColumnIndex()
        {
            if (SelectedCells.Any())
                return GetCurrentColumnIndex(SelectedCells[0]);

            return GetCurrentColumnIndex();
        }

        /// <summary>
        /// Gets the index of the current column.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetCurrentColumnIndex() => GetCurrentColumnIndex(CurrentCell);

        /// <summary>
        /// Gets the index of the current column.
        /// </summary>
        /// <param name="cellInfo">The cell information.</param>
        /// <returns>System.Int32.</returns>
        private int GetCurrentColumnIndex(DataGridCellInfo cellInfo)
        {
            return base.Columns.IndexOf(cellInfo.Column);
        }

        /// <summary>
        /// Gets the current column identifier.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetCurrentColumnId()
        {
            var currentColumnIndex = GetCurrentColumnIndex();
            if (currentColumnIndex < 0)
                return -1;

            return Columns[currentColumnIndex].ColumnId;
        }

        /// <summary>
        /// Gets the current row.
        /// </summary>
        /// <returns>DataEntryGridRow.</returns>
        public DataEntryGridRow GetCurrentRow()
        {
            var currentRowIndex = GetCurrentRowIndex();
            if (currentRowIndex < 0)
                return null;

            return Manager.Rows[currentRowIndex];
        }

        /// <summary>
        /// Tabs the right.
        /// </summary>
        public void TabRight()
        {
            TabRight(GetCurrentRowIndex(), GetCurrentColumnIndex());
        }

        /// <summary>
        /// Tabs the right.
        /// </summary>
        /// <param name="startRowIndex">Start index of the row.</param>
        /// <param name="startColumnIndex">Start index of the column.</param>
        private void TabRight(int startRowIndex, int startColumnIndex)
        {
            if (!CommitCellEdit(CellLostFocusTypes.TabRight))
                return;

            if (startRowIndex < 0)
                startRowIndex = 0;

            var lastColumnIndex = Columns.Count - 1;
            var lastRowIndex = Items.Count - 1;
            startColumnIndex++;

            if (startColumnIndex > lastColumnIndex)
            {
                startRowIndex++;
                startColumnIndex = 0;
            }

            if (startRowIndex > lastRowIndex)
            {
                //Tab Out
                SetFocusToCell(lastRowIndex, lastColumnIndex, false);
                WPFControlsGlobals.SendKey(Key.Tab);
                if (_readOnlyMode)
                    SetFocusToCell(0,0);
                return;
            }

            if (CanCellGetTabFocus(startRowIndex, startColumnIndex))
                SetFocusToCell(startRowIndex, startColumnIndex);
            else
                TabRight(startRowIndex, startColumnIndex);
        }

        /// <summary>
        /// Determines whether this instance [can cell get tab focus] the specified row index.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns><c>true</c> if this instance [can cell get tab focus] the specified row index; otherwise, <c>false</c>.</returns>
        private bool CanCellGetTabFocus(int rowIndex, int columnIndex)
        {
            var gridRow = Manager.Rows[rowIndex];
            var gridColumn = Columns[columnIndex];
            var cellStyle = GetCellStyle(gridRow, gridColumn.ColumnId);

            var setFocus = cellStyle.State == DataEntryGridCellStates.Enabled;
            //if (_readOnlyMode)
            //    setFocus = cellStyle.State == DataEntryGridCellStates.ReadOnly;

            if (setFocus)
                setFocus = gridColumn.Visibility == Visibility.Visible;

            if (setFocus)
            {
                var cellProps = gridRow.GetCellProps(gridColumn.ColumnId);
                setFocus = cellProps.Type == CellPropsTypes.Editable;
            }

            return setFocus;
        }

        /// <summary>
        /// Tabs the left.
        /// </summary>
        /// <param name="startRowIndex">Start index of the row.</param>
        /// <param name="startColumnIndex">Start index of the column.</param>
        private void TabLeft(int startRowIndex, int startColumnIndex)
        {
            if (!CommitCellEdit(CellLostFocusTypes.TabLeft))
                return;

            var lastColumnIndex = Columns.Count - 1;
            startColumnIndex--;

            if (startColumnIndex < 0)
            {
                startRowIndex--;
                startColumnIndex = lastColumnIndex;
            }

            if (startRowIndex < 0)
            {
                //Tab Out
                SetFocusToCell(0, 0, false);
                WPFControlsGlobals.SendKey(Key.LeftShift);
                WPFControlsGlobals.SendKey(Key.Tab);
                return;
            }

            if (CanCellGetTabFocus(startRowIndex, startColumnIndex))
                SetFocusToCell(startRowIndex, startColumnIndex);
            else
                TabLeft(startRowIndex, startColumnIndex);
        }
        /// <summary>
        /// Deletes the current row.
        /// </summary>
        private void DeleteCurrentRow()
        {
            //Peter Ringering - 11/22/2024 08:51:47 PM - E-78
            var rowIndex = Items.IndexOf(CurrentCell.Item);
            if (Manager.OnDeletingRow(rowIndex) && IsDeleteOk())
            {
                var columnIndex = base.Columns.IndexOf(CurrentCell.Column);
                CancelEdit();
                EditingControlHost = null;

                var row = Manager.Rows[rowIndex];
                Manager.RemoveRow(rowIndex);

                if (!row.IsNew)
                    Manager.RaiseDirtyFlag();

                RefreshGridView();
                SetFocusToCell(rowIndex, columnIndex);
            }
            else
            {
                SystemSounds.Exclamation.Play();
            }
        }

        /// <summary>
        /// Determines whether [is delete ok].
        /// </summary>
        /// <returns><c>true</c> if [is delete ok]; otherwise, <c>false</c>.</returns>
        private bool IsDeleteOk()
        {
            var rowIndex = Items.IndexOf(CurrentCell.Item);
            var deleteOk = CanUserDeleteRows;
            if (deleteOk)
            {
                if (DataEntryCanUserAddRows)
                {

                    deleteOk = Manager.IsDeleteOk(rowIndex);
                }
            }

            if (deleteOk)
            {
                var row = Manager.Rows[rowIndex];
                deleteOk = row.AllowUserDelete;
            }

            return deleteOk;
        }

        /// <summary>
        /// Inserts the row.
        /// </summary>
        private void InsertRow()
        {
            if (DataEntryCanUserAddRows)
            {
                var rowIndex = Items.IndexOf(CurrentCell.Item);
                var columnIndex = base.Columns.IndexOf(CurrentCell.Column);

                if (!CommitCellEdit(CellLostFocusTypes.KeyboardNavigation))
                    return;

                var gridRow = Manager.Rows[rowIndex];
                var oldestAncestor = gridRow.GetOldestAncestorRow();
                if (oldestAncestor != null)
                {
                    rowIndex = Manager.Rows.IndexOf(oldestAncestor);
                }

                Manager.InsertNewRow(rowIndex);
                RefreshGridView();
                SetFocusToCell(rowIndex, columnIndex);
            }
        }

        /// <summary>
        /// Sets the focus to cell.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="beginEdit">if set to <c>true</c> [begin edit].</param>
        private void SetFocusToCell(int rowIndex, int columnIndex, bool beginEdit = true)
        {
            if (Items.Count == 0)
            {
                _initCell = new CellSnapshot
                {
                    RowIndex = rowIndex,
                    ColumnIndex = columnIndex
                };
                return;
            }

            CancelEdit();
            rowIndex = ScrubRowIndex(rowIndex);
            columnIndex = ScrubColumnIndex(columnIndex);

            if (_readOnlyMode)
                SelectedCells.Clear();

            CurrentCell = new DataGridCellInfo(Items[rowIndex], Columns[columnIndex]);

            ScrollIntoView(CurrentCell.Item, CurrentCell.Column);

            if (beginEdit)
            {
                BeginEdit();
                if (EditingControlHost != null && EditingControlHost.SetSelection && !SelectedCells.Any() || _readOnlyMode)
                    if (!SelectedCells.Contains(CurrentCell))
                        SelectedCells.Add(CurrentCell);
            }
        }

        /// <summary>
        /// Scrubs the index of the row.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>System.Int32.</returns>
        private int ScrubRowIndex(int rowIndex)
        {
            var lastRowIndex = Items.Count - 1;

            if (rowIndex > lastRowIndex)
                rowIndex = lastRowIndex;

            if (rowIndex < 0)
                rowIndex = 0
                    ;
            return rowIndex;
        }

        /// <summary>
        /// Scrubs the index of the column.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>System.Int32.</returns>
        private int ScrubColumnIndex(int columnIndex)
        {
            var lastColumnIndex = Columns.Count - 1;

            if (columnIndex > lastColumnIndex)
                columnIndex = lastColumnIndex;

            if (columnIndex < 0)
                columnIndex = 0;

            return columnIndex;
        }

        /// <summary>
        /// Gets the first visual child.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj">The dep object.</param>
        /// <returns>T.</returns>
        public static T GetFirstVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T)
                    {
                        return (T)child;
                    }

                    T childItem = GetFirstVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds the grid context menu items.
        /// </summary>
        /// <param name="contextMenu">The context menu.</param>
        internal void AddGridContextMenuItems(ContextMenu contextMenu)
        {
            if (DataEntryCanUserAddRows)
                contextMenu.Items.Add(new MenuItem
                {
                    Header = "_Insert Row", 
                    Command = new RelayCommand(InsertRow)
                });

            if (CanUserDeleteRows)
            {
                contextMenu.Items.Add(new MenuItem
                {
                    Header = "_Delete Row",
                    Command = new RelayCommand(DeleteCurrentRow) {IsEnabled = IsDeleteOk()}
                });

                contextMenu.Items.Add(new MenuItem
                {
                    Header = "C_lear Grid",
                    Command = new RelayCommand(ClearGrid)
                });
            }

            var row = Manager.Rows[GetCurrentRowIndex()];
            var column = Columns[GetCurrentColumnIndex()];
            var contextMenuItems = new List<DataEntryGridContextMenuItem>();
            row.AddContextMenuItems(contextMenuItems, column.ColumnId);

            if (contextMenuItems.Any())
            {
                if (contextMenu.Items.Count > 0)
                {
                    contextMenu.Items.Add(new Separator());
                }

                foreach (var contextMenuItem in contextMenuItems)
                {
                    contextMenu.Items.Add(new MenuItem
                    {
                        Header = contextMenuItem.Header,
                        Command = contextMenuItem.Command,
                        CommandParameter = contextMenuItem.CommandParameter,
                        Icon = contextMenuItem.Icon
                    });
                }
            }
        }

        /// <summary>
        /// Clears the grid.
        /// </summary>
        private void ClearGrid()
        {
            if (!CommitCellEdit())
                return;
            Manager.RaiseDirtyFlag();
            Manager.SetupForNewRecord();
            

            if (!DataEntryCanUserAddRows)
            {
                CancelEdit();
                EditingControlHost = null;
                _gridHasFocus = false;
            }
        }

        /// <summary>
        /// Gets the cell style.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellStyle.</returns>
        private DataEntryGridCellStyle GetCellStyle(DataEntryGridRow row, int columnId)
        {
            var cellStyle = row.GetCellStyle(columnId);
            //if (_readOnlyMode && cellStyle.State == DataEntryGridCellStates.Enabled)
            //    cellStyle.State = DataEntryGridCellStates.ReadOnly;

            return cellStyle;
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            if (!readOnlyValue && ReadOnlyMode)
                readOnlyValue = ReadOnlyMode;

            if (readOnlyValue == _readOnlyMode)
                return;
            
            _readOnlyMode = readOnlyValue;

            if (_controlLoaded)
            {
                RefreshGridView();
                if (_gridHasFocus)
                    Refocus();
                else
                    SetFocusToCell(0, 0);
            }
        }

        /// <summary>
        /// Gets the cell props.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="dataEntryGridColumn">The data entry grid column.</param>
        /// <returns>DataEntryGridCellProps.</returns>
        public DataEntryGridCellProps GetCellProps(DataGridRow dataGridRow, DataEntryGridColumn dataEntryGridColumn)
        {
            if (this.IsDesignMode())
                return null;

            var dataEntryGridRow = Manager.Rows[dataGridRow.GetIndex()];
            return dataEntryGridRow.GetCellProps(dataEntryGridColumn.ColumnId);
        }
    }
}
