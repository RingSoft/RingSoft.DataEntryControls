using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using System.Windows;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class DataEntryGridCustomControlColumn : DataEntryGridControlColumn<DataEntryGridCustomControl>
    {
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataEntryCustomContentTemplate),
                typeof(DataEntryGridCustomControlColumn));

        public DataEntryCustomContentTemplate ContentTemplate
        {
            get { return (DataEntryCustomContentTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public override string DesignerDataValue =>
            new DataEntryGridDataValue().CreateDataValue(new DataEntryGridControlCellStyle(),
                DesignerSelectedId.ToString());

        private int _designerSelectedId;

        public int DesignerSelectedId
        {
            get => _designerSelectedId;
            set
            {
                if (_designerSelectedId == value)
                    return;

                _designerSelectedId = value;
                OnPropertyChanged();
            }
        }

        protected override void ProcessCellFrameworkElementFactory(FrameworkElementFactory factory, string dataColumnName)
        {
            if (ContentTemplate == null)
                throw new Exception($"The {nameof(ContentTemplate)} Property has not been set.");

            factory.SetBinding(DataEntryGridCustomControl.DataValueProperty, new Binding(dataColumnName));
            factory.SetValue(CustomContentControl.ContentTemplateProperty, ContentTemplate);
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
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
    ///     <MyNamespace:DataEntryGridCustomControl/>
    ///
    /// </summary>
    public class DataEntryGridCustomControl : CustomContentControl
    {
        public static readonly DependencyProperty DataValueProperty =
            DependencyProperty.Register(nameof(DataValue), typeof(string), typeof(DataEntryGridCustomControl),
                new FrameworkPropertyMetadata(DataValueChangedCallback));

        public string DataValue
        {
            get { return (string)GetValue(DataValueProperty); }
            set { SetValue(DataValueProperty, value); }
        }

        private static void DataValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customControl = (DataEntryGridCustomControl)obj;
            customControl.SetDataValue();
        }

        private DataEntryGridControlColumnProcessor _processor;

        static DataEntryGridCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGridCustomControl), new FrameworkPropertyMetadata(typeof(DataEntryGridCustomControl)));
        }

        public DataEntryGridCustomControl()
        {
            Loaded += (sender, args) => OnLoaded();

            _processor = new DataEntryGridControlColumnProcessor(this);

            _processor.ControlValueChanged += (sender, args) =>
            {
                SelectedItemId = args.ControlValue.ToInt();
            };

        }

        private void OnLoaded()
        {
        }

        private void SetDataValue()
        {
            _processor.SetDataValue(DataValue);
        }
    }
}
