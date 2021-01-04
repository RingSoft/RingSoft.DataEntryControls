using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridTextColumn : DataGridTemplateColumn, INotifyPropertyChanged
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

        private string _dataColumnName;

        public string DataColumnName
        {
            get => _dataColumnName;
            internal set
            {
                _dataColumnName = value;
                var dataTemplate = CreateCellTemplate();

                CellTemplate = dataTemplate;
            }
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

        protected virtual DataTemplate CreateCellTemplate()
        {
            var dataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, new Binding(_dataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(TextBlock.TextAlignmentProperty, Alignment);
            dataTemplate.VisualTree = factory;
            return dataTemplate;
        }

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
}
