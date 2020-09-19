using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

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
        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register(nameof(DesignText), typeof(string), typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (ReadOnlyBox)obj;
            readOnlyBox.SetDesignText();
        }

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(TextAlignmentChangedCallback));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        private static void TextAlignmentChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (ReadOnlyBox)obj;
            if (readOnlyBox.TextBlock != null)
                readOnlyBox.TextBlock.TextAlignment = readOnlyBox.TextAlignment;
        }

        public TextBlock TextBlock { get; set; }

        private string _text;

        protected string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                    return;

                _text = value;
                if (TextBlock != null)
                {
                    if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty())
                        return;

                    TextBlock.Text = _text;
                }
            }
        }

        static ReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReadOnlyBox), new FrameworkPropertyMetadata(typeof(ReadOnlyBox)));
        }

        public override void OnApplyTemplate()
        {
            TextBlock = GetTemplateChild(nameof(TextBlock)) as TextBlock;

            base.OnApplyTemplate();

            SetDesignText();
        }

        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && TextBlock != null)
                TextBlock.Text = DesignText;
        }
    }
}
