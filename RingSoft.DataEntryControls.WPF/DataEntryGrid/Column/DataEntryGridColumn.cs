using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    interface IDataEntryGridColumnControl
    {
        DataEntryGridCellStyle CellStyle { get; internal set; }

        void Initialize();
    }

    public abstract class DataEntryGridColumn : DataGridTemplateColumn, INotifyPropertyChanged
    {
        public new DataTemplate CellTemplate
        {
            get => base.CellTemplate;
            protected internal set => base.CellTemplate = value;
        }

        private object _defaultColumnHeader;
        public new object Header
        {
            get => base.Header;
            set
            {
                if (_defaultColumnHeader == null)
                    _defaultColumnHeader = base.Header;

                base.Header = value;
            }
        }

        public new DataTemplate CellEditingTemplate
        {
            get => base.CellEditingTemplate;
            internal set => base.CellEditingTemplate = value;
        }

        public int ColumnId { get; set; }

        private string _designText;

        public string DesignText
        {
            get => _designText;
            set
            {
                if (_designText == value)
                    return;

                _designText = value;
                OnPropertyChanged(nameof(DesignText));
            }
        }

        private TextAlignment _alignment;

        public TextAlignment Alignment
        {
            get => _alignment;
            set
            {
                if (_alignment == value)
                    return;

                _alignment = value;
                CellTemplate = CreateCellTemplate();
                OnPropertyChanged(nameof(Alignment));
            }
        }

        private string _dataColumnName;

        protected internal string DataColumnName
        {
            get => _dataColumnName;
            set
            {
                _dataColumnName = value;
                CellTemplate = CreateCellTemplate();
            }
        }

        protected abstract DataTemplate CreateCellTemplate();

        public void ResetColumnHeader()
        {
            if (_defaultColumnHeader == null)
                _defaultColumnHeader = base.Header;

            Header = _defaultColumnHeader;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class DataEntryGridColumn<TControl> : DataEntryGridColumn
        where TControl : FrameworkElement
    {
        protected abstract void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName);

        protected override DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TControl));
            factory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(Loaded));
            ProcessCellFrameworkElementFactory(factory, DataColumnName);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }

        protected virtual void Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TControl control)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (!(control is IDataEntryGridColumnControl gridColumnControl))
                    throw new Exception($"{control} must implement the {nameof(IDataEntryGridColumnControl)} interface.");

                var grid = control.GetParentOfType<DataEntryGrid>();
                if (grid != null)
                {
                    var row = control.GetParentOfType<DataGridRow>();
                    if (row != null)
                    {
                        gridColumnControl.CellStyle = grid.GetCellStyle(row, this);
                        gridColumnControl.Initialize();
                        //switch (cellStyle.State)
                        //{
                        //    case DataEntryGridCellStates.Enabled:
                        //        break;
                        //    case DataEntryGridCellStates.ReadOnly:
                        //    case DataEntryGridCellStates.Disabled:
                        //        control.IsEnabled = false;
                        //        break;
                        //}

                        //if (cellStyle is DataEntryGridControlCellStyle controlCellStyle)
                        //{
                        //    control.Visibility = controlCellStyle.ControlVisible ? Visibility.Visible : Visibility.Collapsed;
                        //}
                    }
                }
            }
        }
    }
}
