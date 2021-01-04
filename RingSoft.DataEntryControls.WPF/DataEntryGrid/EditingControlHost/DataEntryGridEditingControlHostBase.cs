using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public abstract class DataEntryGridEditingControlHostBase : INotifyPropertyChanged
    {
        public DataEntryGrid Grid { get; }

        public Control Control { get; internal set; }

        //public DataEntryGridCellProps CellProps { get; internal set; }
        public DataEntryGridRow Row => Grid.GetCurrentRow();

        public int ColumnId { get; internal set; }

        public abstract bool IsDropDownOpen { get; }

        public virtual bool SetSelection { get; } = false;

        public event EventHandler ControlDirty;

        public event EventHandler<DataEntryGridEditingCellProps> UpdateSource;

        protected internal DataEntryGridEditingControlHostBase(DataEntryGrid grid)
        {
            Grid = grid;
        }

        protected virtual void OnControlDirty()
        {
            ControlDirty?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnUpdateSource(DataEntryGridEditingCellProps e)
        {
            UpdateSource?.Invoke(this, e);
        }

        public abstract DataTemplate GetEditingControlDataTemplate(DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle);

        public abstract DataEntryGridEditingCellProps GetCellValue();

        public abstract bool HasDataChanged();

        public abstract void UpdateFromCellProps(DataEntryGridCellProps cellProps);

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
