using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

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
            factory.SetValue(DataEntryGridCustomControl.ContentTemplateProperty, ContentTemplate);
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
    [TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter))]
    public class DataEntryGridCustomControl : Control
    {
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataEntryCustomContentTemplate), typeof(DataEntryGridCustomControl),
                new FrameworkPropertyMetadata(ContentTemplateChangedCallback));

        public DataEntryCustomContentTemplate ContentTemplate
        {
            get { return (DataEntryCustomContentTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        private static void ContentTemplateChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customControl = (DataEntryGridCustomControl)obj;
            customControl.SelectItem(customControl.SelectedItemId);
        }

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

        private int _selectedItemId;

        public int SelectedItemId
        {
            get => _selectedItemId;
            private set
            {
                if (_selectedItemId == value)
                    return;

                _selectedItemId = value;

                SelectItem(SelectedItemId);
            }
        }


        public ContentPresenter ContentPresenter { get; set; }

        private bool _controlLoaded;
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

        public override void OnApplyTemplate()
        {
            ContentPresenter = GetTemplateChild(nameof(ContentPresenter)) as ContentPresenter;

            _controlLoaded = true;

            SelectItem(SelectedItemId);

            base.OnApplyTemplate();
        }

        private void OnLoaded()
        {
        }

        private void SetDataValue()
        {
            _processor.SetDataValue(DataValue);
        }

        protected void SelectItem(int itemId)
        {
            if (!_controlLoaded || ContentTemplate == null)
                return;

            var contentItem = ContentTemplate.FirstOrDefault(f => f.ItemId == itemId);
            if (contentItem != null)
                ContentPresenter.ContentTemplate = contentItem.DataTemplate;
        }
    }
}
