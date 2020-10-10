﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Color = System.Drawing.Color;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF;assembly=RingSoft.DataEntryControls.WPF"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DataEntryGrid/>
    ///
    /// </summary>
    public class DataEntryGrid : DataGrid, IDataEntryGrid
    {
        private class NextTabFocusCell
        {
            public int RowIndex { get; set; }

            public int ColumnIndex { get; set; }
        }

        public static readonly DependencyProperty ManagerProperty =
            DependencyProperty.Register(nameof(Manager), typeof(DataEntryGridManager), typeof(DataEntryGrid),
                new FrameworkPropertyMetadata(ManagerChangedCallback));

        public DataEntryGridManager Manager
        {
            get { return (DataEntryGridManager)GetValue(ManagerProperty); }
            set { SetValue(ManagerProperty, value); }
        }

        private static void ManagerChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryGrid = (DataEntryGrid)obj;
            if (dataEntryGrid._controlLoaded)
                dataEntryGrid.SetManager();
        }

        public static readonly DependencyProperty ControlHostFactoryProperty =
            DependencyProperty.Register(nameof(ControlHostFactory), typeof(DataEntryGridHostFactory), typeof(DataEntryGrid));

        public DataEntryGridHostFactory ControlHostFactory
        {
            get { return (DataEntryGridHostFactory)GetValue(ControlHostFactoryProperty); }
            set { SetValue(ControlHostFactoryProperty, value); }
        }

        public static readonly DependencyProperty EnterToTabProperty =
            DependencyProperty.Register(nameof(EnterToTab), typeof(bool), typeof(DataEntryGrid));

        public bool EnterToTab
        {
            get { return (bool)GetValue(EnterToTabProperty); }
            set { SetValue(EnterToTabProperty, value); }
        }

        public static readonly DependencyProperty CancelEditOnEscapeProperty =
            DependencyProperty.Register(nameof(CancelEditOnEscape), typeof(bool), typeof(DataEntryGrid));

        public bool CancelEditOnEscape
        {
            get { return (bool)GetValue(CancelEditOnEscapeProperty); }
            set { SetValue(CancelEditOnEscapeProperty, value); }
        }


        public new ObservableCollection<DataEntryGridColumn> Columns { get; set; } = new ObservableCollection<DataEntryGridColumn>();

        public new bool CanUserAddRows { get; set; } = true;

        public new bool AutoGenerateColumns
        {
            get => base.AutoGenerateColumns;
            private set => base.AutoGenerateColumns = value;
        }

        public new DataGridSelectionUnit SelectionUnit
        {
            get => base.SelectionUnit;
            private set => base.SelectionUnit = value;
        }

        public new double RowHeight
        {
            get => base.RowHeight;
            set
            {
                base.RowHeight = value;
                DesignerFillGrid();
            }
        }

        public DataEntryGridControlHostBase EditingControlHost { get; private set; }

        private DataTable _dataSourceTable = new DataTable("DataSource");
        private bool _controlLoaded;
        private bool _gridHasFocus;
        private bool _cancellingEdit;
        private NextTabFocusCell _nextTabFocusCell;
        private bool _tabbingRight;
        private bool _undoEdit;

        static DataEntryGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGrid), new FrameworkPropertyMetadata(typeof(DataEntryGrid)));
        }

        public DataEntryGrid()
        {
            base.CanUserAddRows = false;
            AutoGenerateColumns = false;
            SelectionUnit = DataGridSelectionUnit.Cell;

            ItemsSource = _dataSourceTable.DefaultView;

            Columns.CollectionChanged += Columns_CollectionChanged;
            Loaded += (sender, args) => OnLoad();
            GotFocus += DataEntryGrid_GotFocus;
            LostFocus += DataEntryGrid_LostFocus;
        }

        private void DataEntryGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                //For some reason, when a new row is added at the bottom when the editing control is dirty, it prevents OnCellEditEnding from being run and the current row index to be set to -1 when grid looses focus via mouse click.  This hack covers that situation.
                if (EditingControlHost != null)
                    CancelEdit();

                _gridHasFocus = false;
            }

            SelectedCells.Clear();
        }

        private void DataEntryGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            var beginEdit = true;

            if (EditingControlHost == null)
            {
                if (Keyboard.IsKeyDown(Key.Tab))
                {
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        var lastRowIndex = Items.Count - 1;
                        var lastColumnIndex = Columns.Count - 1;
                        if (!_gridHasFocus)
                        {
                            //Shift+Tab from outside the grid.  Set active cell to next editable cell from cell on Last Row, Last Column.
                            _gridHasFocus = true; //Set to avoid double tab.
                            TabLeft(lastRowIndex, lastColumnIndex + 1);
                            beginEdit = false;
                        }
                    }
                    else
                    {
                        if (!_gridHasFocus)
                        {
                            //Tab from outside the grid.  Set active cell to next editable cell from cell on First Row, First Column.
                            _gridHasFocus = true; //Set to avoid double tab.
                            TabRight(0, -1);
                            beginEdit = false;
                        }
                    }
                }
            }

            if (beginEdit && !_cancellingEdit)
            {
                BeginEdit();
            }

            _gridHasFocus = true;
        }

        public void Refocus()
        { 
            BeginEdit();
        }

        protected override void OnSelectedCellsChanged(SelectedCellsChangedEventArgs e)
        {
            if (!IsKeyboardFocusWithin && SelectedCells.Any())
                SelectedCells.Clear();

            base.OnSelectedCellsChanged(e);
        }

        private void OnLoad()
        {
            if (IsVisible)
            {
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    if (Manager != null && !_controlLoaded)
                    {
                        SetManager();
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

            var scrollViewer = FindVisualChild<ScrollViewer>(this);
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
                if (column is DataEntryGridCheckBoxColumn)
                {
                    var boolColumn = new DataColumn(columnName, typeof(bool));
                    _dataSourceTable.Columns.Add(boolColumn);
                }
                else
                {
                    _dataSourceTable.Columns.Add(columnName);
                }
                column.DataColumnName = columnName;

                index++;
            }
            DesignerFillGrid();
        }

        private void Column_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DesignerFillGrid();
        }

        private int GetColumnIndexOfColumnId(int columnId)
        {
            var column = Columns.FirstOrDefault(f => f.ColumnId == columnId);
            return Columns.IndexOf(column);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            DesignerFillGrid();

            base.OnRenderSizeChanged(sizeInfo);
        }

        private void DesignerFillGrid()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
                return;

            _dataSourceTable.Rows.Clear();

            UpdateLayout();
            
            if (ActualHeight < 15)
                return;

            if (double.IsNaN(ColumnHeaderHeight) && Columns.Any())
            {
                if (ActualHeight < 30)
                    return;
            }
            else if (!double.IsNaN(ColumnHeaderHeight))
            {
                if (ActualHeight < ColumnHeaderHeight + 20)
                    return;
            }

            AddDesignerRow();
            var lastRowIndex = _dataSourceTable.Rows.Count - 1;

            UpdateLayout();
            var dataGridRow = ItemContainerGenerator.ContainerFromItem(Items[lastRowIndex]) as DataGridRow;

            while (dataGridRow != null)
            {
                AddDesignerRow();
                lastRowIndex++;
                UpdateLayout();

                dataGridRow = ItemContainerGenerator.ContainerFromItem(Items[lastRowIndex]) as DataGridRow;
            }
        }

        private void AddDesignerRow()
        {
            var designerRow = _dataSourceTable.NewRow();

            foreach (var column in Columns)
            {
                if (column is DataEntryGridCheckBoxColumn)
                {
                    designerRow[column.DataColumnName] = false;
                }
                else
                {
                    designerRow[column.DataColumnName] = column.DesignText;
                }
            }

            _dataSourceTable.Rows.Add(designerRow);

        }

        protected override void OnCurrentCellChanged(EventArgs e)
        {
            var rowIndex = GetCurrentRowIndex();
            if (rowIndex >= 0)
            {
                var gridRow = Manager.Rows[rowIndex];
                foreach (var column in Columns)
                {
                    column.ResetColumnHeader();
                    var cellStyle = gridRow.GetCellStyle(column.ColumnId);
                    if (!string.IsNullOrEmpty(cellStyle.ColumnHeader))
                        column.Header = cellStyle.ColumnHeader;
                }
            }

            base.OnCurrentCellChanged(e);
        }

        private void SetManager()
        {
            Manager.RowsChanged += GridRows_CollectionChanged;

            if (Manager.Rows.Any())
                RefreshDataSource();

            Manager.SetupGrid(this);
        }

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

                    if (e.NewItems.Count > 0)
                        RefreshGridView();

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var unused in e.OldItems)
                    {
                        _dataSourceTable.Rows.RemoveAt(e.OldStartingIndex);
                    }
                    RefreshGridView();
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

        public void RefreshDataSource()
        {
            _dataSourceTable.Rows.Clear();
            foreach (var gridRow in Manager.Rows)
            {
                AddRow(gridRow);
            }
        }

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

        public void UpdateRow(DataEntryGridRow gridRow)
        {
            var rowIndex = Manager.Rows.IndexOf(gridRow);
            if (rowIndex < 0)
                throw new Exception(
                    "This row must be added to the Rows collection before it can be updated.");

            UpdateRow(gridRow, rowIndex);
        }

        public void UpdateRow(DataEntryGridRow gridRow, int rowIndex)
        {
            UpdateRow(gridRow, _dataSourceTable.Rows[rowIndex], rowIndex);
        }

        private void SetNextTabFocusToCell(DataEntryGridRow row, int columnId)
        {
            if (!_tabbingRight)
                return;

            var nextFocusTabCell = new NextTabFocusCell
            {
                RowIndex = Manager.Rows.IndexOf(row),
                ColumnIndex = GetColumnIndexOfColumnId(columnId)
            };

            if (nextFocusTabCell.RowIndex < 0)
                throw new ArgumentException("The Row has not been added to the grid.");

            if (nextFocusTabCell.ColumnIndex < 0)
                throw new ArgumentException($"Column ID: {columnId} does not exist in the Columns collection");

            _nextTabFocusCell = nextFocusTabCell;
        }

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
                if (column is DataEntryGridCheckBoxColumn)
                {
                    if (cellProps is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                        dataRow[dataTableColumn] = checkBoxCellProps.Value;
                }
                else
                {
                    dataRow[dataTableColumn] = cellProps.Text;
                }

                UpdateCellColors(gridRow, column);
            }
        }

        private void RefreshGridView()
        {
            UpdateLayout();

            var rowIndex = 0;
            foreach (var gridRow in Manager.Rows)
            {
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

        private void UpdateRowColors(DataGridRow dataGridRow, DataEntryGridRow gridRow)
        {
            dataGridRow.ClearValue(BackgroundProperty);
            dataGridRow.ClearValue(ForegroundProperty);

            if (!gridRow.BackgroundColor.IsEmpty)
                dataGridRow.Background = new SolidColorBrush(gridRow.BackgroundColor.GetMediaColor());
            if (!gridRow.ForegroundColor.IsEmpty)
                dataGridRow.Foreground = new SolidColorBrush(gridRow.ForegroundColor.GetMediaColor());
        }

        private void UpdateCellColors(DataEntryGridRow gridRow, DataEntryGridColumn column)
        {
            var rowIndex = Manager.Rows.IndexOf(gridRow);
            if (ItemContainerGenerator.ContainerFromItem(Items[rowIndex]) is DataGridRow dataGridRow)
            {
                UpdateCellColors(column, dataGridRow, gridRow);
            }
        }

        private void UpdateCellColors(DataEntryGridColumn column, DataGridRow dataGridRow, DataEntryGridRow gridRow)
        {
            var cellContent = column.GetCellContent(dataGridRow);
            if (cellContent != null)
            {
                if (cellContent.Parent is DataGridCell dataGridCell)
                {
                    var cellStyle = gridRow.GetCellStyle(column.ColumnId);

                    var backgroundColor = cellStyle.BackgroundColor;
                    var foregroundColor = cellStyle.ForegroundColor;

                    switch (cellStyle.CellStyle)
                    {
                        case DataEntryGridCellStyles.Enabled:
                        case DataEntryGridCellStyles.ReadOnly:
                            break;
                        case DataEntryGridCellStyles.Disabled:
                            if (backgroundColor.IsEmpty)
                                backgroundColor = Color.DarkGray;
                            if (foregroundColor.IsEmpty)
                                foregroundColor = Color.Black;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    dataGridCell.ClearValue(BackgroundProperty);
                    dataGridCell.ClearValue(ForegroundProperty);
                    dataGridCell.ClearValue(BorderBrushProperty);

                    if (!backgroundColor.IsEmpty)
                    {
                        dataGridCell.Background = new SolidColorBrush(backgroundColor.GetMediaColor());
                        if (GridLinesVisibility == DataGridGridLinesVisibility.None)
                            dataGridCell.BorderBrush = new SolidColorBrush(backgroundColor.GetMediaColor());
                    }

                    if (!foregroundColor.IsEmpty)
                    {
                        dataGridCell.Foreground = new SolidColorBrush(foregroundColor.GetMediaColor());
                    }
                }
            }
        }


        protected override void OnBeginningEdit(DataGridBeginningEditEventArgs e)
        {
            if (e.Column is DataEntryGridColumn dataEntryGridColumn)
            {
                var dataEntryGridRow = Manager.Rows[e.Row.GetIndex()];
                var cellProps = dataEntryGridRow.GetCellProps(dataEntryGridColumn.ColumnId);
                var cellStyle = dataEntryGridRow.GetCellStyle(dataEntryGridColumn.ColumnId);

                switch (cellStyle.CellStyle)
                {
                    case DataEntryGridCellStyles.Enabled:
                        break;
                    case DataEntryGridCellStyles.ReadOnly:
                    case DataEntryGridCellStyles.Disabled:
                        e.Cancel = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!e.Cancel)
                {
                    if (ControlHostFactory == null)
                        ControlHostFactory = new DataEntryGridHostFactory();
                    EditingControlHost = ControlHostFactory.GetControlHost(this, cellProps.EditingControlId);

                    dataEntryGridColumn.CellEditingTemplate = EditingControlHost.GetEditingControlDataTemplate(cellProps);
                    EditingControlHost.ControlDirty += EditingControl_ControlDirty;
                    EditingControlHost.UpdateSource += EditingControlHost_UpdateSource;
                }
            }

            base.OnBeginningEdit(e);
        }

        protected override void OnPreparingCellForEdit(DataGridPreparingCellForEditEventArgs e)
        {
            var element = e.EditingElement;

            element.Dispatcher.BeginInvoke(
                DispatcherPriority.Input,
                new Action(() => element.MoveFocus(
                    new TraversalRequest(FocusNavigationDirection.First))));

            base.OnPreparingCellForEdit(e);
        }

        private void EditingControlHost_UpdateSource(object sender, DataEntryGridCellProps e)
        {
            var rowIndex = Items.IndexOf(CurrentCell.Item);

            if (!EditingControlHost.HasDataChanged())
                return;

            var dataEntryGridRow = Manager.Rows[rowIndex];
            dataEntryGridRow.SetCellValue(e);
        }

        private void EditingControl_ControlDirty(object sender, EventArgs e)
        {
            var currentRowIndex = Items.IndexOf(CurrentCell.Item);
            if (currentRowIndex >= Items.Count - 1)
            {
                if (CanUserAddRows)
                    Manager.InsertNewRow();
            }
            Manager.RaiseDirtyFlag();
        }

        protected override void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
        {
            if (_undoEdit)
            {
                EditingControlHost = null;
                base.OnCellEditEnding(e);
                SelectedCells.Clear();
                return;
            }
            if (e.Column is DataEntryGridColumn && EditingControlHost != null &&
                EditingControlHost.Control != null)
            {
                var rowIndex = e.Row.GetIndex();
                if (!EditingControlHost.HasDataChanged())
                {
                    EditingControlHost = null;
                    base.OnCellEditEnding(e);
                    if (!IsKeyboardFocusWithin)
                        SelectedCells.Clear();
                    return;
                }

                var skipValidation = !IsKeyboardFocusWithin;
                var cellValue = EditingControlHost.GetCellValue();
                cellValue.SkipValidation = skipValidation;
                var dataEntryGridRow = Manager.Rows[rowIndex];
                dataEntryGridRow.SetCellValue(cellValue);

                if (!cellValue.ValidationResult && !skipValidation)
                {
                    EditingControlHost.ProcessValidationFail(cellValue);
                    e.Cancel = true;
                    base.OnCellEditEnding(e);
                    return;
                }

                if (_tabbingRight && cellValue.NextTabFocusRow != null && cellValue.NextTabFocusColumnId >=  0)
                    SetNextTabFocusToCell(cellValue.NextTabFocusRow, cellValue.NextTabFocusColumnId);

                EditingControlHost = null;
                if (skipValidation)
                    SelectedCells.Clear();
            }
            base.OnCellEditEnding(e);
        }

        public new bool CancelEdit()
        {
            if (EditingControlHost == null)
                return true;

            _cancellingEdit = true;
            var result = base.CancelEdit();
            _cancellingEdit = false;
            return result;
        }

        public new bool CancelEdit(DataGridEditingUnit editingUnit)
        {
            if (EditingControlHost == null)
                return true;

            _cancellingEdit = true;
            var result = base.CancelEdit(editingUnit);
            _cancellingEdit = false;
            return result;
        }

        public bool CancelEdit(bool undoEdit)
        {
            _undoEdit = undoEdit;
            var result = CancelEdit();
            _undoEdit = false;
            return result;
        }

        public void ResetGridFocus()
        {
            if (IsKeyboardFocusWithin)
                TabRight(0, -1);
        }

        public void GotoCell(DataEntryGridRow row, int columnId)
        {
            var rowIndex = Manager.Rows.IndexOf(row);
            var columnIndex = GetColumnIndexOfColumnId(columnId);
            SetFocusToCell(rowIndex, columnIndex);
        }

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
                    if (!CancelEdit())
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
                        Manager.RaiseDirtyFlag();
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
                        if (CancelEditOnEscape)
                        {
                            if (CancelEdit(true))
                                //Send Escape key again so window can close on Escape after cell edit mode has ended.
                                SendKey(Key.Escape);
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }

                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EditingControlHost != null && EditingControlHost.Control != null)
            {
                if (!EditingControlHost.CanGridProcessKey(e.Key))
                {
                    //This is so calculator on decimal edit control can process enter key and prevents grid from cancelling edit on Enter.
                    //We don't set handled because otherwise the calculator won't update the edit control when the user presses Enter to
                    //get the results of a calculation.
                    return;
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            var currentRowIndex = GetCurrentRowIndex();
            var currentColumnIndex = GetCurrentColumnIndex();

            HitTestResult hitTestResult =
                VisualTreeHelper.HitTest(this, e.GetPosition(this));

            if (hitTestResult != null)
            {
                var cell = hitTestResult.VisualHit.GetParentOfType<DataGridCell>();
                var row = hitTestResult.VisualHit.GetParentOfType<DataGridRow>();
                if (row != null && cell != null)
                {
                    var mouseRowIndex = Items.IndexOf(row.Item);
                    var mouseColumnIndex = base.Columns.IndexOf(cell.Column);

                    if (!(mouseRowIndex == currentRowIndex && mouseColumnIndex == currentColumnIndex))
                    {
                        if (!(EditingControlHost != null && EditingControlHost.IsDropDownOpen))
                        {
                            if (!CancelEdit())
                                e.Handled = true;
                        }
                    }
                }
            }

            base.OnPreviewMouseDown(e);
        }

        private void ProcessTab()
        {
            var currentRowIndex = GetCurrentRowIndex();
            var currentColumnIndex = GetCurrentColumnIndex();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                TabLeft(currentRowIndex, currentColumnIndex);
            else
                TabRight(currentRowIndex, currentColumnIndex);
        }

        private int GetCurrentRowIndex()
        {
            return Items.IndexOf(CurrentCell.Item);
        }

        private int GetCurrentColumnIndex()
        {
            return base.Columns.IndexOf(CurrentCell.Column);
        }

        private void TabRight(int startRowIndex, int startColumnIndex)
        {
            _tabbingRight = true;

            if (CancelEdit())
            {
                if (_nextTabFocusCell != null)
                {
                    SetFocusToCell(_nextTabFocusCell.RowIndex, _nextTabFocusCell.ColumnIndex);
                    _tabbingRight = false;
                    _nextTabFocusCell = null;
                    return;
                }
            }
            else
            {
                _tabbingRight = false;
                _nextTabFocusCell = null;
                return;
            }

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
                _tabbingRight = false;
                SetFocusToCell(lastRowIndex, lastColumnIndex, false);
                SendKey(Key.Tab);
                return;
            }

            var gridRow = Manager.Rows[startRowIndex];
            var gridColumn = Columns[startColumnIndex];
            var cellStyle = gridRow.GetCellStyle(gridColumn.ColumnId);

            if (cellStyle.CellStyle == DataEntryGridCellStyles.Enabled)
                SetFocusToCell(startRowIndex, startColumnIndex);
            else
                TabRight(startRowIndex, startColumnIndex);
        }

        private void TabLeft(int startRowIndex, int startColumnIndex)
        {
            if (!CancelEdit())
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
                SendKey(Key.LeftShift);
                SendKey(Key.Tab);
                return;
            }

            var gridRow = Manager.Rows[startRowIndex];
            var gridColumn = Columns[startColumnIndex];
            var cellStyle = gridRow.GetCellStyle(gridColumn.ColumnId);

            if (cellStyle.CellStyle == DataEntryGridCellStyles.Enabled)
                SetFocusToCell(startRowIndex, startColumnIndex);
            else
                TabLeft(startRowIndex, startColumnIndex);
        }
        private void DeleteCurrentRow()
        {
            if (CanUserDeleteRows)
            {
                var rowIndex = Items.IndexOf(CurrentCell.Item);
                var columnIndex = base.Columns.IndexOf(CurrentCell.Column);
                
                CancelEdit(true);
                
                Manager.RemoveRow(rowIndex);
                SetFocusToCell(rowIndex, columnIndex);
            }
        }

        private void InsertRow()
        {
            if (CanUserAddRows)
            {
                var rowIndex = Items.IndexOf(CurrentCell.Item);
                var columnIndex = base.Columns.IndexOf(CurrentCell.Column);

                if (!CancelEdit())
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

        private void SetFocusToCell(int rowIndex, int columnIndex, bool beginEdit = true)
        {
           
            if (CancelEdit(DataGridEditingUnit.Cell))
            {
                rowIndex = ScrubRowIndex(rowIndex);
                columnIndex = ScrubColumnIndex(columnIndex);

                CurrentCell = new DataGridCellInfo(Items[rowIndex], Columns[columnIndex]);
                SelectedCells.Clear();
                ScrollIntoView(CurrentCell.Item, CurrentCell.Column);

                if (beginEdit)
                    BeginEdit();
            }

            _tabbingRight = false;
            _nextTabFocusCell = null;
        }

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

        private int ScrubColumnIndex(int columnIndex)
        {
            var lastColumnIndex = Columns.Count - 1;

            if (columnIndex > lastColumnIndex)
                columnIndex = lastColumnIndex;

            if (columnIndex < 0)
                columnIndex = 0;

            return columnIndex;
        }

        public static void SendKey(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(e);
                }
            }
        }

        private TChildItem FindVisualChild<TChildItem>(DependencyObject obj)
            where TChildItem : DependencyObject

        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is TChildItem)
                    return (TChildItem)child;
                else
                {
                    TChildItem childOfChild = FindVisualChild<TChildItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
