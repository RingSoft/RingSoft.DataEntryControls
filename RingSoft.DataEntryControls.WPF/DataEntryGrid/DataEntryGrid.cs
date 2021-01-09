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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
    public class DataEntryGrid : DataGrid, IDataEntryGrid, IReadOnlyControl
    {
        private class CellSnapshot
        {
            public int RowIndex { get; set; }
            public int ColumnIndex { get; set; }

            public int BottomVisibleRowIndex { get; set; }

            public int RightVisibleColumnIndex { get; set; }
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

        public static readonly DependencyProperty EnterToTabProperty =
            DependencyProperty.Register(nameof(EnterToTab), typeof(bool), typeof(DataEntryGrid));

        public bool EnterToTab
        {
            get { return (bool)GetValue(EnterToTabProperty); }
            set { SetValue(EnterToTabProperty, value); }
        }

        public static readonly DependencyProperty CloseWindowOnEscapeProperty =
            DependencyProperty.Register(nameof(CloseWindowOnEscape), typeof(bool), typeof(DataEntryGrid));

        public bool CloseWindowOnEscape
        {
            get { return (bool)GetValue(CloseWindowOnEscapeProperty); }
            set { SetValue(CloseWindowOnEscapeProperty, value); }
        }

        public static readonly DependencyProperty CellEditingControlBorderThicknessProperty =
            DependencyProperty.Register(nameof(CellEditingControlBorderThickness), typeof(Thickness), typeof(DataEntryGrid));

        public Thickness CellEditingControlBorderThickness
        {
            get { return (Thickness)GetValue(CellEditingControlBorderThicknessProperty); }
            set { SetValue(CellEditingControlBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty ReadOnlyModeProperty =
            DependencyProperty.Register(nameof(ReadOnlyMode), typeof(bool), typeof(DataEntryGrid),
                new FrameworkPropertyMetadata(ReadOnlyModeChangedCallback));

        public bool ReadOnlyMode
        {
            get { return (bool)GetValue(ReadOnlyModeProperty); }
            set { SetValue(ReadOnlyModeProperty, value); }
        }

        private static void ReadOnlyModeChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryGrid = (DataEntryGrid)obj;
            dataEntryGrid.SetReadOnlyMode(dataEntryGrid.ReadOnlyMode);
        }


        public static readonly DependencyProperty SetTabFocusToSelectedCellProperty =
            DependencyProperty.Register(nameof(SetTabFocusToSelectedCell), typeof(bool), typeof(DataEntryGrid));

        public bool SetTabFocusToSelectedCell
        {
            get { return (bool)GetValue(SetTabFocusToSelectedCellProperty); }
            set { SetValue(SetTabFocusToSelectedCellProperty, value); }
        }

        public static readonly DependencyProperty StoreCurrentCellOnLoadGridProperty =
            DependencyProperty.Register(nameof(StoreCurrentCellOnLoadGrid), typeof(bool), typeof(DataEntryGrid));

        public bool StoreCurrentCellOnLoadGrid
        {
            get { return (bool)GetValue(StoreCurrentCellOnLoadGridProperty); }
            set { SetValue(StoreCurrentCellOnLoadGridProperty, value); }
        }

        public static readonly DependencyProperty StoreCurrentCellOnLostFocusProperty =
            DependencyProperty.Register(nameof(StoreCurrentCellOnLostFocus), typeof(bool), typeof(DataEntryGrid));

        public bool StoreCurrentCellOnLostFocus
        {
            get { return (bool)GetValue(StoreCurrentCellOnLostFocusProperty); }
            set { SetValue(StoreCurrentCellOnLostFocusProperty, value); }
        }

        public static readonly DependencyProperty DisabledCellDisplayStyleProperty =
            DependencyProperty.Register(nameof(DisabledCellDisplayStyle), typeof(DataEntryGridDisplayStyle), typeof(DataEntryGrid));

        public DataEntryGridDisplayStyle DisabledCellDisplayStyle
        {
            get { return (DataEntryGridDisplayStyle)GetValue(DisabledCellDisplayStyleProperty); }
            set { SetValue(DisabledCellDisplayStyleProperty, value); }
        }

        public new ObservableCollection<DataEntryGridColumn> Columns { get; } = new ObservableCollection<DataEntryGridColumn>();

        public ObservableCollection<DataEntryGridDisplayStyle> DisplayStyles { get; } = new ObservableCollection<DataEntryGridDisplayStyle>();

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
                DesignerFillGrid(nameof(RowHeight));
            }
        }

        public DataEntryGridEditingControlHostBase EditingControlHost { get; private set; }

        private DataTable _dataSourceTable = new DataTable("DataSource");
        private bool _controlLoaded;
        private bool _gridHasFocus;
        private bool _bulkInsertMode;
        private bool _designerFillingGrid;
        private bool _buttonClick;
        private CellSnapshot _initCell;
        private bool _readOnlyMode;
        private CellSnapshot _cellSnapshot;

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

        private void DataEntryGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                _gridHasFocus = false;
                EditingControlHost = null; //This is done when DbMaintenance window resets focus on New Record operation.

                if (!StoreCurrentCellOnLostFocus)
                    SelectedCells.Clear();
            }
        }

        protected override void OnSelectedCellsChanged(SelectedCellsChangedEventArgs e)
        {
            if (SelectedCells.Any() && EditingControlHost != null && EditingControlHost.Control != null) 
                //This needs to run when user clicks on the lookup button.
                SelectedCells.Clear();
            
            base.OnSelectedCellsChanged(e);
        }

        public new bool Focus()
        {
            base.Focus();
            ResetGridFocus();
            return IsKeyboardFocusWithin;
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

        private bool SetFocusToSelectedCell()
        {
            if (!SetTabFocusToSelectedCell || !SelectedCells.Any())
                return false;

            SetFocusToCell(GetSelectedRowIndex(), GetSelectedColumnIndex());
            return true;
        }

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
            DesignerFillGrid(nameof(Columns_CollectionChanged));
        }

        private void Column_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DesignerFillGrid(nameof(Column_PropertyChanged));
        }

        private int GetColumnIndexOfColumnId(int columnId)
        {
            var column = Columns.FirstOrDefault(f => f.ColumnId == columnId);
            return Columns.IndexOf(column);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            DesignerFillGrid(nameof(OnRenderSizeChanged));

            base.OnRenderSizeChanged(sizeInfo);
        }

        // ReSharper disable once UnusedParameter.Local
        private void DesignerFillGrid(string trace)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
                return;

            if (_designerFillingGrid)
                return;

            _designerFillingGrid = true;

            _dataSourceTable.Rows.Clear();

            UpdateLayout();

            if (ActualHeight < 15)
            {
                _designerFillingGrid = false;
                return;
            }

            var columnHeaderHeight = double.NaN;

            var headersPresenter = this.GetVisualChild<DataGridColumnHeadersPresenter>();

            if (headersPresenter != null)
                columnHeaderHeight = headersPresenter.ActualHeight;

            if (double.IsNaN(columnHeaderHeight) && Columns.Any())
            {
                if (ActualHeight < 30)
                {
                    _designerFillingGrid = false;
                    return;
                }
            }
            else if (!double.IsNaN(columnHeaderHeight))
            {
                if (ActualHeight < columnHeaderHeight + 10)
                {
                    _designerFillingGrid = false;
                    return;
                }
            }

            //MessageBox.Show($"RowHeight={RowHeight}\r\nColumn Header Height={columnHeaderHeight}\r\nActualHeight={ActualHeight}", trace);

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

            _designerFillingGrid = false;
        }

        private void AddDesignerRow()
        {
            var designerRow = _dataSourceTable.NewRow();

            foreach (var column in Columns)
            {
                //if (column is DataEntryGridCheckBoxColumn)
                //{
                //    designerRow[column.DataColumnName] = false;
                //}
                //else
                //{
                //    designerRow[column.DataColumnName] = column.DesignText;
                //}
                column.ValidateDesignerDataValue();
                designerRow[column.DataColumnName] = column.DesignerDataValue;
            }

            _dataSourceTable.Rows.Add(designerRow);

        }

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

        private void UpdateColumnHeaders()
        {
            var rowIndex = GetCurrentRowIndex();
            if (rowIndex >= 0)
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

                    if (e.NewItems.Count > 0 && !_bulkInsertMode)
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

        public void SetBulkInsertMode(bool value = true)
        {
            _bulkInsertMode = value;
            if (!_bulkInsertMode)
                RefreshGridView();
        }

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

        //This overload should only be run outside by the interface.  Otherwise it will update column headers for each row and slow down performance.
        public void UpdateRow(DataEntryGridRow gridRow)
        {
            var rowIndex = Manager.Rows.IndexOf(gridRow);
            if (rowIndex < 0)
                throw new Exception(
                    "This row must be added to the Rows collection before it can be updated.");

            UpdateRow(gridRow, rowIndex);
            UpdateColumnHeaders();
        }

        public void UpdateRow(DataEntryGridRow gridRow, int rowIndex)
        {
            if (rowIndex < _dataSourceTable.Rows.Count)
                UpdateRow(gridRow, _dataSourceTable.Rows[rowIndex], rowIndex);
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

            if (gridRow.DisplayStyleId > 0)
            {
                var displayStyle = GetDisplayStyle(gridRow.DisplayStyleId, gridRow);

                if (displayStyle.BackgroundBrush != null)
                    dataGridRow.Background = displayStyle.BackgroundBrush;
                if (displayStyle.ForegroundBrush != null)
                    dataGridRow.Foreground = displayStyle.ForegroundBrush;
            }
        }

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
                    var cellStyle = GetCellStyle(gridRow, column.ColumnId);

                    DataEntryGridDisplayStyle displayStyle = null;
                    
                    switch (cellStyle.State)
                    {
                        case DataEntryGridCellStates.Enabled:
                        case DataEntryGridCellStates.ReadOnly:
                            break;
                        case DataEntryGridCellStates.Disabled:
                            displayStyle = DisabledCellDisplayStyle;
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
                        case DataEntryGridCellStates.ReadOnly:
                        case DataEntryGridCellStates.Disabled:
                            e.Cancel = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    e.Cancel = true;
                }

                var cellProps = dataEntryGridRow.GetCellProps(dataEntryGridColumn.ColumnId) as DataEntryGridEditingCellProps;

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
                        if (EditingControlHost.SetSelection && !SelectedCells.Any())
                            SelectedCells.Add(CurrentCell);

                        dataEntryGridColumn.CellEditingTemplate =
                            EditingControlHost.GetEditingControlDataTemplate(cellProps, cellStyle);
                        EditingControlHost.ControlDirty += EditingControl_ControlDirty;
                        EditingControlHost.UpdateSource += EditingControlHost_UpdateSource;

                        if (_buttonClick)
                        {
                            dataEntryGridRow.SetCellValue(cellProps);
                            _buttonClick = false;
                        }
                    }
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

        public DataGridCell GetCurrentCell()
        {
            var cellContent = CurrentCell.Column.GetCellContent(CurrentCell.Item);
            if (cellContent != null && cellContent.Parent is DataGridCell dataGridCell)
            {
                return dataGridCell;
            }

            return null;
        }

        private void EditingControlHost_UpdateSource(object sender, DataEntryGridEditingCellProps e)
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
            EditingControlHost = null;
            base.OnCellEditEnding(e);
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (EditingControlHost != null && EditingControlHost.Control != null &&
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

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (EditingControlHost != null && EditingControlHost.Control != null &&
                e.OldFocus == null && e.NewFocus.GetType() == typeof(DataGridCell))
            {
                SetFocusToCell(GetCurrentRowIndex(), GetCurrentColumnIndex());
            }
            else if (EditingControlHost == null && e.NewFocus is Button)
            {
                _buttonClick = true;
            }
            base.OnPreviewGotKeyboardFocus(e);
        }

        public bool CommitCellEdit()
        {
            return CommitCellEdit(CellLostFocusTypes.LostFocus, false);
        }
        public bool CommitCellEdit(CellLostFocusTypes cellLostFocusType, bool cancelEdit = true)
        {
            if (EditingControlHost != null && EditingControlHost.Control != null)
            {
                var currentRow = Manager.Rows[GetCurrentRowIndex()];
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

        //public new bool CancelEdit()
        //{
        //    if (EditingControlHost == null)
        //        return true;

        //    _cancellingEdit = true;
        //    var result = base.CancelEdit();
        //    _cancellingEdit = false;
        //    return result;
        //}

        //public new bool CancelEdit(DataGridEditingUnit editingUnit)
        //{
        //    if (EditingControlHost == null)
        //        return true;

        //    _cancellingEdit = true;
        //    var result = base.CancelEdit(editingUnit);
        //    _cancellingEdit = false;
        //    return result;
        //}

        //public bool CancelEdit(bool undoEdit)
        //{
        //    _undoEdit = undoEdit;
        //    var result = CancelEdit();
        //    _undoEdit = false;
        //    return result;
        //}

        public void ResetGridFocus()
        {
            if (IsKeyboardFocusWithin)
            {
                if (_readOnlyMode)
                    SetFocusToCell(0,0);
                else 
                    TabRight(0, -1);
            }
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
                            var window = Window.GetWindow(this);
                            window?.Close();
                        }
                        e.Handled = true;
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

        private void ProcessTab()
        {
            var currentRowIndex = GetCurrentRowIndex();
            var currentColumnIndex = GetCurrentColumnIndex();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                TabLeft(currentRowIndex, currentColumnIndex);
            else
                TabRight(currentRowIndex, currentColumnIndex);
        }

        private int GetSelectedRowIndex()
        {
            if (SelectedCells.Any())
                return GetCurrentRowIndex(SelectedCells[0]);

            return GetCurrentRowIndex();
        }

        private int GetCurrentRowIndex() => GetCurrentRowIndex(CurrentCell);

        private int GetCurrentRowIndex(DataGridCellInfo cellInfo)
        {
            return Items.IndexOf(cellInfo.Item);
        }

        private int GetSelectedColumnIndex()
        {
            if (SelectedCells.Any())
                return GetCurrentColumnIndex(SelectedCells[0]);

            return GetCurrentColumnIndex();
        }

        private int GetCurrentColumnIndex() => GetCurrentColumnIndex(CurrentCell);

        private int GetCurrentColumnIndex(DataGridCellInfo cellInfo)
        {
            return base.Columns.IndexOf(cellInfo.Column);
        }

        public DataEntryGridRow GetCurrentRow()
        {
            var currentRowIndex = GetCurrentRowIndex();
            if (currentRowIndex < 0)
                return null;

            return Manager.Rows[currentRowIndex];
        }

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
                SendKey(Key.Tab);
                if (_readOnlyMode)
                    SetFocusToCell(0,0);
                return;
            }

            if (CanCellGetTabFocus(startRowIndex, startColumnIndex))
                SetFocusToCell(startRowIndex, startColumnIndex);
            else
                TabRight(startRowIndex, startColumnIndex);
        }

        private bool CanCellGetTabFocus(int rowIndex, int columnIndex)
        {
            var gridRow = Manager.Rows[rowIndex];
            var gridColumn = Columns[columnIndex];
            var cellStyle = GetCellStyle(gridRow, gridColumn.ColumnId);

            var setFocus = cellStyle.State == DataEntryGridCellStates.Enabled;
            if (_readOnlyMode)
                setFocus = cellStyle.State == DataEntryGridCellStates.ReadOnly;

            if (setFocus)
                setFocus = gridColumn.Visibility == Visibility.Visible;

            if (setFocus)
            {
                var cellProps = gridRow.GetCellProps(gridColumn.ColumnId);
                setFocus = cellProps.Type == CellPropsTypes.Editable;
            }

            return setFocus;
        }

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
                SendKey(Key.LeftShift);
                SendKey(Key.Tab);
                return;
            }

            if (CanCellGetTabFocus(startRowIndex, startColumnIndex))
                SetFocusToCell(startRowIndex, startColumnIndex);
            else
                TabLeft(startRowIndex, startColumnIndex);
        }
        private void DeleteCurrentRow()
        {
            if (IsDeleteOk())
            {
                var rowIndex = Items.IndexOf(CurrentCell.Item);
                var columnIndex = base.Columns.IndexOf(CurrentCell.Column);
                CancelEdit();
                EditingControlHost = null;

                var row = Manager.Rows[rowIndex];
                Manager.RemoveRow(rowIndex);

                if (!row.IsNew)
                    Manager.RaiseDirtyFlag();

                SetFocusToCell(rowIndex, columnIndex);
            }
            else
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private bool IsDeleteOk()
        {
            var rowIndex = Items.IndexOf(CurrentCell.Item);
            var deleteOk = CanUserDeleteRows && rowIndex < Items.Count - 1;
            if (deleteOk && rowIndex >= 0)
            {
                var row = Manager.Rows[rowIndex];
                deleteOk = row.AllowUserDelete;
            }

            return deleteOk;
        }

        private void InsertRow()
        {
            if (CanUserAddRows)
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

            CurrentCell = new DataGridCellInfo(Items[rowIndex], Columns[columnIndex]);

            ScrollIntoView(CurrentCell.Item, CurrentCell.Column);

            if (beginEdit)
            {
                BeginEdit();
                if (EditingControlHost != null && EditingControlHost.SetSelection && !SelectedCells.Any())
                    SelectedCells.Add(CurrentCell);
            }
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

        //private TChildItem FindVisualChild<TChildItem>(DependencyObject obj)
        //    where TChildItem : DependencyObject

        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        //        if (child is TChildItem)
        //            return (TChildItem)child;
        //        else
        //        {
        //            TChildItem childOfChild = FindVisualChild<TChildItem>(child);
        //            if (childOfChild != null)
        //                return childOfChild;
        //        }
        //    }
        //    return null;
        //}

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

        internal void AddGridContextMenuItems(ContextMenu contextMenu)
        {
            if (CanUserAddRows)
                contextMenu.Items.Add(new MenuItem
                {
                    Header = "_Insert Row", 
                    Command = new RelayCommand(InsertRow)
                });

            if (CanUserDeleteRows)
                contextMenu.Items.Add(new MenuItem
                {
                    Header = "_Delete Row", 
                    Command = new RelayCommand(DeleteCurrentRow){IsEnabled = IsDeleteOk()}
                });

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

        private DataEntryGridCellStyle GetCellStyle(DataEntryGridRow row, int columnId)
        {
            var cellStyle = row.GetCellStyle(columnId);
            if (_readOnlyMode && cellStyle.State == DataEntryGridCellStates.Enabled)
                cellStyle.State = DataEntryGridCellStates.ReadOnly;

            return cellStyle;
        }

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

        public DataEntryGridCellProps GetCellProps(DataGridRow dataGridRow, DataEntryGridColumn dataEntryGridColumn)
        {
            if (this.IsDesignMode())
                return null;

            var dataEntryGridRow = Manager.Rows[dataGridRow.GetIndex()];
            return dataEntryGridRow.GetCellProps(dataEntryGridColumn.ColumnId);
        }
    }
}
