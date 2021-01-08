using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{

    public enum DataEntryGridColumnTypes
    {
        Text = 0,
        Control = 1
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

        public abstract DataEntryGridColumnTypes ColumnType { get; }

        protected internal abstract DataTemplate CreateCellTemplate();

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

    public abstract class DataEntryGridControlColumn<TControl> : DataEntryGridColumn
        where TControl : Control
    {
        public override DataEntryGridColumnTypes ColumnType => DataEntryGridColumnTypes.Control;

        protected abstract void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName);

        protected internal override DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TControl));

            ProcessCellFrameworkElementFactory(factory, DataColumnName);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }
    }
}
