using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
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

        protected DataEntryGridControlHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataTemplate GetEditingControlDataTemplate(DataEntryGridCellProps cellProps)
        {
            CellProps = cellProps;

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

        protected abstract void OnControlLoaded(TControl control, DataEntryGridCellProps cellProps);

        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TControl control)
            {
                Control = control;
                control.Focus();
                OnControlLoaded(control, CellProps);
            }
        }
    }
}
