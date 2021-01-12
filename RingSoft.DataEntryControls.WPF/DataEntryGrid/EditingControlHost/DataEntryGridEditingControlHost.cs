using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public abstract class DataEntryGridEditingControlHost<TControl> : DataEntryGridEditingControlHostBase
    where TControl : Control
    {
        private TControl _control;

        public new TControl Control
        {
            get => _control;
            set => base.Control = _control = value;
        }

        private DataEntryGridEditingCellProps _cellProps;
        private DataEntryGridCellStyle _cellStyle;

        protected DataEntryGridEditingControlHost(DataEntryGrid grid) : base(grid)
        {
        }

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

        protected virtual void SetupFrameworkElementFactory(FrameworkElementFactory factory, DataEntryGridColumn column)
        {
        }

        protected abstract void OnControlLoaded(TControl control, DataEntryGridEditingCellProps cellProps, DataEntryGridCellStyle cellStyle);

        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TControl control)
            {
                Control = control;
                if (!SetSelection)
                    Grid.SelectedCells.Clear();

                control.Focus();

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

        protected virtual void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            Control.Background = dataGridCell.Background;
            Control.Foreground = dataGridCell.Foreground;

            if (Grid.CellEditingControlBorderThickness.Equals(new Thickness(0)))
                dataGridCell.BorderThickness = Grid.CellEditingControlBorderThickness;

            Control.BorderThickness = new Thickness(0);
        }

        protected DataEntryGridDisplayStyle GetCellDisplayStyle()
        {
            if (_cellStyle == null)
                throw new Exception("Control has not been initialized yet");

            if (_cellStyle.DisplayStyleId == 0)
                return new DataEntryGridDisplayStyle();

            return Grid.GetDisplayStyle(_cellStyle.DisplayStyleId, Row);
        }
    }
}
