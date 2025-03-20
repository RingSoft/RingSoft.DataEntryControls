// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-20-2024
//
// Last Modified By : petem
// Last Modified On : 11-20-2024
// ***********************************************************************
// <copyright file="EnhancedButton.cs" company="RingSoft">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class EnhancedButton.
    /// Implements the <see cref="Button" />
    /// </summary>
    /// <seealso cref="Button" />
    [TemplatePart(Name = "Image", Type = typeof(Image))]
    [TemplatePart(Name = "EnhancedToolTip", Type = typeof(EnhancedToolTip))]
    public class EnhancedButton : Button
    {
        /// <summary>
        /// The image source property
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
    DependencyProperty.RegisterAttached(
        nameof(ImageSource)
        , typeof(ImageSource)
        , typeof(EnhancedButton),
        new FrameworkPropertyMetadata(null, ImageSourceChangedCallback));

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        /// <summary>
        /// Images the source changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ImageSourceChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dbMaintenanceButton = (EnhancedButton)obj;
            dbMaintenanceButton.SetImage();
        }

        //private static void VisibilityChangedCallback(DependencyObject obj,
        //    DependencyPropertyChangedEventArgs args)
        //{
        //    var dbMaintenanceButton = (EnhancedButton)obj;
        //    var grid = dbMaintenanceButton.GetParentOfType<Grid>();
        //    if (grid != null)
        //    {
        //        if (grid.RowDefinitions.Count <= 1)
        //        {
        //            var columnIndex = Grid.GetColumn(dbMaintenanceButton);
        //            var columnDefinition = grid.ColumnDefinitions[columnIndex];
        //            if (columnDefinition != null)
        //            {
        //                switch (dbMaintenanceButton.Visibility)
        //                {
        //                    case Visibility.Visible:
        //                        columnDefinition.Width = new GridLength(1, GridUnitType.Star);
        //                        break;
        //                    case Visibility.Hidden:
        //                    case Visibility.Collapsed:
        //                        columnDefinition.Width = new GridLength(0);
        //                        break;
        //                    default:
        //                        throw new ArgumentOutOfRangeException();
        //                }
        //            }
        //        }

        //        if (grid.ColumnDefinitions.Count <= 1)
        //        {
        //            var rowIndex = Grid.GetRow(dbMaintenanceButton);
        //            var rowDefinition = grid.RowDefinitions[rowIndex];
        //            if (rowDefinition != null)
        //            {
        //                switch (dbMaintenanceButton.Visibility)
        //                {
        //                    case Visibility.Visible:
        //                        rowDefinition.Height = new GridLength(1, GridUnitType.Star);
        //                        break;
        //                    case Visibility.Hidden:
        //                    case Visibility.Collapsed:
        //                        rowDefinition.Height = new GridLength(0);
        //                        break;
        //                    default:
        //                        throw new ArgumentOutOfRangeException();
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Gets the tool tip.
        /// </summary>
        /// <value>The tool tip.</value>
        public new EnhancedToolTip ToolTip { get; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image { get; set; }
        /// <summary>
        /// Initializes static members of the <see cref="EnhancedButton"/> class.
        /// </summary>
        static EnhancedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(EnhancedButton)
                , new FrameworkPropertyMetadata(typeof(EnhancedButton)));

            //VisibilityProperty.OverrideMetadata(
            //    typeof(EnhancedButton)
            //    , new FrameworkPropertyMetadata(VisibilityChangedCallback));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedButton"/> class.
        /// </summary>
        public EnhancedButton()
        {
            base.ToolTip = ToolTip = new EnhancedToolTip();
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Image = GetTemplateChild(nameof(Image)) as Image;

            SetImage();

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
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
