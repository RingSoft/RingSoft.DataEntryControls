using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;

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

                InventoryPanel.Visibility = NonInventoryPanel.Visibility = CommentPanel.Visibility = Visibility.Collapsed;

                switch (LineType)
                {
                    case AppGridLineTypes.Inventory:
                        InventoryPanel.Visibility = Visibility.Visible;
                        break;
                    case AppGridLineTypes.NonInventory:
                        NonInventoryPanel.Visibility = Visibility.Visible;
                        break;
                    case AppGridLineTypes.Comment:
                        CommentPanel.Visibility = Visibility.Visible;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        private DataEntryGridControlColumnProcessor _processor;

        public GridImageControl()
        {
            InitializeComponent();

            _processor = new DataEntryGridControlColumnProcessor(this);

            NonInventoryPanel.Visibility = CommentPanel.Visibility = Visibility.Collapsed;

            _processor.ControlValueChanged += (sender, args) => LineType = (AppGridLineTypes) args.ControlValue.ToInt();
        }

        private void SetDataValue()
        {
            _processor.SetDataValue(DataValue);
        }
    }
}
