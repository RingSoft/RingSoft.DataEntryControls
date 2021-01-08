using RingSoft.DataEntryControls.Engine;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridCheckBoxColumn : DataEntryGridControlColumn<DataEntryGridCheckBox>
    {
        public override string DesignerDataValue
        {
            get
            {
                var controlCellStyle = new DataEntryGridControlCellStyle();
                return new DataEntryGridDataValue().CreateDataValue(controlCellStyle, DesignerValue.ToString());
            }
        }

        private bool _designerValue;

        public bool DesignerValue
        {
            get => _designerValue;
            set
            {
                if (_designerValue == value)
                    return;

                _designerValue = value;
                OnPropertyChanged();
            }
        }

        protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory,
            string dataColumnName)
        {
            factory.SetBinding(DataEntryGridCheckBox.DataValueProperty, new Binding(dataColumnName));
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        }
    }

    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DataEntryGrid.Column"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DataEntryGrid.Column;assembly=RingSoft.DataEntryControls.WPF.DataEntryGrid.Column"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DataEntryGridCheckBox/>
    ///
    /// </summary>
    public class DataEntryGridCheckBox : CheckBox
    {
        public static readonly DependencyProperty DataValueProperty =
            DependencyProperty.Register(nameof(DataValue), typeof(string), typeof(DataEntryGridCheckBox),
                new FrameworkPropertyMetadata(DataValueChangedCallback));

        public string DataValue
        {
            get { return (string)GetValue(DataValueProperty); }
            set { SetValue(DataValueProperty, value); }
        }

        private static void DataValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dataEntryGridCheckBox = (DataEntryGridCheckBox)obj;
            dataEntryGridCheckBox.SetDataValue();
        }

        private DataEntryGridControlColumnProcessor _processor;

        static DataEntryGridCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGridCheckBox), new FrameworkPropertyMetadata(typeof(DataEntryGridCheckBox)));
        }

        public DataEntryGridCheckBox()
        {
            _processor = new DataEntryGridControlColumnProcessor(this);

            _processor.ControlValueChanged += (sender, args) => IsChecked = args.ControlValue.ToBool();
        }

        private void SetDataValue()
        {
            _processor.SetDataValue(DataValue);
        }
    }
}
