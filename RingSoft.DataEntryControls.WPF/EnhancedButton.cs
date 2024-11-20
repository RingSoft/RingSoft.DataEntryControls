using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF;assembly=RingSoft.DataEntryControls.WPF"
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
    ///     <MyNamespace:EnhancedButton/>
    ///
    /// </summary>
    [TemplatePart(Name = "Image", Type = typeof(Image))]
    [TemplatePart(Name = "EnhancedToolTip", Type = typeof(EnhancedToolTip))]
    public class EnhancedButton : Button
    {
        public static readonly DependencyProperty ImageSourceProperty =
    DependencyProperty.RegisterAttached(
        nameof(ImageSource)
        , typeof(ImageSource)
        , typeof(EnhancedButton),
        new FrameworkPropertyMetadata(null, ImageSourceChangedCallback));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        private static void ImageSourceChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dbMaintenanceButton = (EnhancedButton)obj;
            dbMaintenanceButton.SetImage();
        }

        private static void VisibilityChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dbMaintenanceButton = (EnhancedButton)obj;
            var grid = dbMaintenanceButton.GetParentOfType<Grid>();
            if (grid != null)
            {
                if (grid.RowDefinitions.Count <= 1)
                {
                    var columnIndex = Grid.GetColumn(dbMaintenanceButton);
                    var columnDefinition = grid.ColumnDefinitions[columnIndex];
                    if (columnDefinition != null)
                    {
                        switch (dbMaintenanceButton.Visibility)
                        {
                            case Visibility.Visible:
                                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                                break;
                            case Visibility.Hidden:
                            case Visibility.Collapsed:
                                columnDefinition.Width = new GridLength(0);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                if (grid.ColumnDefinitions.Count <= 1)
                {
                    var rowIndex = Grid.GetRow(dbMaintenanceButton);
                    var rowDefinition = grid.RowDefinitions[rowIndex];
                    if (rowDefinition != null)
                    {
                        switch (dbMaintenanceButton.Visibility)
                        {
                            case Visibility.Visible:
                                rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                                break;
                            case Visibility.Hidden:
                            case Visibility.Collapsed:
                                rowDefinition.Height = new GridLength(0);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }

        public new EnhancedToolTip ToolTip { get; }

        public Image Image { get; set; }
        static EnhancedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(EnhancedButton)
                , new FrameworkPropertyMetadata(typeof(EnhancedButton)));

            VisibilityProperty.OverrideMetadata(
                typeof(EnhancedButton)
                , new FrameworkPropertyMetadata(VisibilityChangedCallback));
        }

        public EnhancedButton()
        {
            base.ToolTip = ToolTip = new EnhancedToolTip();
        }

        public override void OnApplyTemplate()
        {
            Image = GetTemplateChild(nameof(Image)) as Image;

            SetImage();

            base.OnApplyTemplate();
        }

        private void SetImage()
        {
            if (Image != null)
            {
                if (ImageSource == null)
                {
                    Image.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Image.Source = ImageSource;
                    Image.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
