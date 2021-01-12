using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RingSoft.DataEntryControls.WPF;

namespace TestDummyApp
{
    public class GridImageColumn : DataEntryGridControlColumn<GridImageControl>
    {
        public override string DesignerDataValue =>
            new DataEntryGridDataValue().CreateDataValue(new DataEntryGridControlCellStyle(),
                ((int)DesignerLineType).ToString());

        private AppGridLineTypes _designerLineType;

        public AppGridLineTypes DesignerLineType
        {
            get => _designerLineType;
            set
            {
                if (_designerLineType == value)
                    return;

                _designerLineType = value;
                OnPropertyChanged();
            }
        }

        protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName)
        {
            factory.SetBinding(GridImageControl.DataValueProperty, new Binding(dataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
        }
    }

    /// <summary>
    /// Interaction logic for GridImageControl.xaml
    /// </summary>
    public partial class GridImageControl : UserControl
    {
        public static readonly DependencyProperty DataValueProperty =
            DependencyProperty.Register(nameof(DataValue), typeof(string), typeof(GridImageControl),
                new FrameworkPropertyMetadata(DataValueChangedCallback));

        public string DataValue
        {
            get { return (string)GetValue(DataValueProperty); }
            set { SetValue(DataValueProperty, value); }
        }

        private static void DataValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var gridImageControl = (GridImageControl)obj;
            gridImageControl.SetDataValue();
        }

        private AppGridLineTypes _lineType;

        public AppGridLineTypes LineType
        {
            get => _lineType;
            private set
            {
                if (_lineType == value)
                    return;

                _lineType = value;

                SelectItem((int) LineType);
            }
        }

        private DataEntryGridControlColumnProcessor _processor;
        private bool _controlLoaded;

        public GridImageControl()
        {
            InitializeComponent();

            Loaded += (sender, args) => OnLoaded();

            _processor = new DataEntryGridControlColumnProcessor(this);

            _processor.ControlValueChanged += (sender, args) =>
            {
                LineType = (AppGridLineTypes)args.ControlValue.ToInt();
            };
        }

        protected virtual int GetCurrentId() => (int)LineType;

        protected virtual DataTemplate GetDataTemplateForId(int itemId) =>
            Globals.GetDataTemplateForLineType((AppGridLineTypes) itemId);

        private void OnLoaded()
        {
            _controlLoaded = true;

            SelectItem(GetCurrentId());
        }

        private void SetDataValue()
        {
            _processor.SetDataValue(DataValue);
        }

        protected void SelectItem(int itemId)
        {
            if (!_controlLoaded)
                return;

            ContentPresenter.ContentTemplate = GetDataTemplateForId(itemId);
        }
    }
}
