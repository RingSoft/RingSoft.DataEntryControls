// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-20-2024
//
// Last Modified By : petem
// Last Modified On : 11-20-2024
// ***********************************************************************
// <copyright file="EnhancedToolTip.cs" company="RingSoft">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class EnhancedToolTip.
    /// Implements the <see cref="ToolTip" />
    /// </summary>
    /// <seealso cref="ToolTip" />
    [TemplatePart(Name = "HeaderTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "DescriptionTextBlock", Type = typeof(TextBlock))]
    public class EnhancedToolTip : ToolTip
    {
        /// <summary>
        /// The header text property
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.RegisterAttached(nameof(HeaderText), typeof(string), typeof(EnhancedToolTip),
                new FrameworkPropertyMetadata(null, HeaderTextChangedCallback));

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>The header text.</value>
        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        /// <summary>
        /// Headers the text changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void HeaderTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var enhancedToolTip = (EnhancedToolTip)obj;
            enhancedToolTip.SetHeaderText();
        }

        /// <summary>
        /// The description text property
        /// </summary>
        public static readonly DependencyProperty DescriptionTextProperty =
            DependencyProperty.RegisterAttached(nameof(DescriptionText), typeof(string)
                , typeof(EnhancedToolTip),
                new FrameworkPropertyMetadata(null, DescriptionTextChangedCallback));

        /// <summary>
        /// Gets or sets the description text.
        /// </summary>
        /// <value>The description text.</value>
        public string DescriptionText
        {
            get => (string)GetValue(DescriptionTextProperty);
            set => SetValue(DescriptionTextProperty, value);
        }

        /// <summary>
        /// Descriptions the text changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DescriptionTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var enhancedToolTip = (EnhancedToolTip)obj;
            enhancedToolTip.SetDescriptionText();
        }

        /// <summary>
        /// Gets or sets the header text block.
        /// </summary>
        /// <value>The header text block.</value>
        public TextBlock HeaderTextBlock { get; set; }
        /// <summary>
        /// Gets or sets the description text block.
        /// </summary>
        /// <value>The description text block.</value>
        public TextBlock DescriptionTextBlock { get; set; }
        /// <summary>
        /// Initializes static members of the <see cref="EnhancedToolTip"/> class.
        /// </summary>
        static EnhancedToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(EnhancedToolTip)
                , new FrameworkPropertyMetadata(typeof(EnhancedToolTip)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            HeaderTextBlock = GetTemplateChild(nameof(HeaderTextBlock)) as TextBlock;
            DescriptionTextBlock = GetTemplateChild(nameof(DescriptionTextBlock)) as TextBlock;

            SetHeaderText();
            SetDescriptionText();
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Sets the header text.
        /// </summary>
        private void SetHeaderText()
        {
            if (HeaderTextBlock != null && !HeaderText.IsNullOrEmpty())
                HeaderTextBlock.Text = HeaderText;
        }

        /// <summary>
        /// Sets the description text.
        /// </summary>
        private void SetDescriptionText()
        {
            if (DescriptionTextBlock != null && !DescriptionText.IsNullOrEmpty())
                DescriptionTextBlock.Text = DescriptionText;
        }
    }
}
