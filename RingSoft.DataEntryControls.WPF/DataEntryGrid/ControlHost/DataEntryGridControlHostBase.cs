using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public abstract class DataEntryGridControlHostBase : INotifyPropertyChanged
    {
        public DataEntryGrid Grid { get; }

        public Control Control { get; internal set; }

        //public DataEntryGridCellProps CellProps { get; internal set; }
        public DataEntryGridRow Row => Grid.GetCurrentRow();

        public int ColumnId { get; internal set; }

        public abstract bool IsDropDownOpen { get; }

        public event EventHandler ControlDirty;

        public event EventHandler<DataEntryGridCellProps> UpdateSource;

        protected internal DataEntryGridControlHostBase(DataEntryGrid grid)
        {
            Grid = grid;
        }

        protected virtual void OnControlDirty()
        {
            ControlDirty?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnUpdateSource(DataEntryGridCellProps e)
        {
            UpdateSource?.Invoke(this, e);
        }

        public abstract DataTemplate GetEditingControlDataTemplate(DataEntryGridCellProps cellProps,
            DataEntryGridCellStyle cellStyle);

        public abstract DataEntryGridCellProps GetCellValue();

        public abstract bool HasDataChanged();

        public virtual bool CanGridProcessKey(Key key)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
