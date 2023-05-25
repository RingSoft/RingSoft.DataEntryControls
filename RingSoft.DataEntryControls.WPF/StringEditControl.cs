using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
    ///     <MyNamespace:StringEditControl/>
    ///
    /// </summary>
    public class StringEditControl : TextBox
    {
        public static readonly DependencyProperty SelectAllOnGotFocusProperty =
            DependencyProperty.Register(nameof(SelectAllOnGotFocus), typeof(bool), typeof(StringEditControl));

        public bool SelectAllOnGotFocus
        {
            get { return (bool)GetValue(SelectAllOnGotFocusProperty); }
            set { SetValue(SelectAllOnGotFocusProperty, value); }
        }

        private string _designText;

        public string DesignText
        {
            get => _designText;
            set
            {
                _designText = value;
                SetDesignText();
            }
        }

        private bool _overrideSelChanged;
        static StringEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StringEditControl),
                new FrameworkPropertyMetadata(typeof(StringEditControl)));
            SelectAllOnGotFocusProperty.OverrideMetadata(typeof(StringEditControl), new FrameworkPropertyMetadata(true));
        }

        public StringEditControl()
        {
            //SetResourceReference(StyleProperty, typeof(TextBox));
            ContextMenu = new ContextMenu();
            ContextMenu.AddTextBoxContextMenuItems();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (SelectAllOnGotFocus)
            {
                this.ScrollToTop();
                _overrideSelChanged = true;
                SelectionChanged += StringEditControl_SelectionChanged;
            }

            base.OnGotFocus(e);
        }

        private void StringEditControl_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (SelectionLength == 0 && _overrideSelChanged)
            {
                SelectionChanged -= StringEditControl_SelectionChanged;
                _overrideSelChanged = false;
                this.ScrollToTop();
            }
        }

        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                Text = DesignText;
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (MaxLength > 0 && Text.Length >= MaxLength && SelectionLength == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }
            base.OnPreviewTextInput(e);
        }
    }
}
