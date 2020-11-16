using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public abstract class DataEntryGridControlHost<TControl> : DataEntryGridControlHostBase
    where TControl : Control
    {
        private TControl _control;

        public new TControl Control
        {
            get => _control;
            set => base.Control = _control = value;
        }

        private DataEntryGridCellProps _cellProps;
        private DataEntryGridCellStyle _cellStyle;

        protected DataEntryGridControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataTemplate GetEditingControlDataTemplate(DataEntryGridCellProps cellProps, DataEntryGridCellStyle cellStyle)
        {
            _cellProps = cellProps;
            _cellStyle = cellStyle;

            ColumnId = cellProps.ColumnId;

            var factory = new FrameworkElementFactory(typeof(TControl));
            factory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(Loaded));
            SetupFrameworkElementFactory(factory);

            var dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = factory;

            return dataTemplate;
        }

        protected virtual void SetupFrameworkElementFactory(FrameworkElementFactory factory)
        {
        }

        protected abstract void OnControlLoaded(TControl control, DataEntryGridCellProps cellProps, DataEntryGridCellStyle cellStyle);

        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TControl control)
            {
                Control = control;
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
    }
}
