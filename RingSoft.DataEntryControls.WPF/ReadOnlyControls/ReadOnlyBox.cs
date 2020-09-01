﻿using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.ReadOnlyControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.ReadOnlyControls;assembly=RingSoft.DataEntryControls.WPF.ReadOnlyControls"
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
    ///     <MyNamespace:ReadOnlyBox/>
    ///
    /// </summary>
    [TemplatePart(Name = "TextBlock", Type = typeof(TextBlock))]
    public class ReadOnlyBox : Control
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(TextChangedCallback));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void TextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (ReadOnlyBox)obj;

            if (readOnlyBox.TextBlock != null)
            {
                readOnlyBox.TextBlock.Text = readOnlyBox.Text;
            }
        }

        public TextBlock TextBlock { get; set; }

        static ReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReadOnlyBox), new FrameworkPropertyMetadata(typeof(ReadOnlyBox)));
        }

        public override void OnApplyTemplate()
        {
            TextBlock = GetTemplateChild(nameof(TextBlock)) as TextBlock;

            base.OnApplyTemplate();
        }
    }
}
