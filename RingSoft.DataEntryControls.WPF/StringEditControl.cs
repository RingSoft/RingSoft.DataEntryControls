using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;

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
    public class StringEditControl : TextBox, IReadOnlyControl
    {
        public static readonly DependencyProperty SelectAllOnGotFocusProperty =
            DependencyProperty.Register(nameof(SelectAllOnGotFocus), typeof(bool), typeof(StringEditControl));

        public bool SelectAllOnGotFocus
        {
            get { return (bool)GetValue(SelectAllOnGotFocusProperty); }
            set { SetValue(SelectAllOnGotFocusProperty, value); }
        }

        public static readonly DependencyProperty UiCommandProperty =
            DependencyProperty.Register(nameof(UiCommand), typeof(UiCommand), typeof(StringEditControl),
                new FrameworkPropertyMetadata(UiCommandChangedCallback));

        public UiCommand UiCommand
        {
            get { return (UiCommand)GetValue(UiCommandProperty); }
            set { SetValue(UiCommandProperty, value); }
        }

        private static void UiCommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var stringEditControl = (StringEditControl)obj;
            if (stringEditControl._vmUiControl == null)
            {
                stringEditControl._vmUiControl = new VmUiControl(stringEditControl, stringEditControl.UiCommand);
                if (stringEditControl.UiLabel != null)
                {
                    stringEditControl._vmUiControl.SetLabel(stringEditControl.UiLabel);
                }
            }
        }

        public static readonly DependencyProperty UiLabelProperty =
            DependencyProperty.Register(nameof(UiLabel), typeof(Label), typeof(StringEditControl),
                new FrameworkPropertyMetadata(UiLabelChangedCallback));

        public Label UiLabel
        {
            get { return (Label)GetValue(UiLabelProperty); }
            set { SetValue(UiLabelProperty, value); }
        }

        private static void UiLabelChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var stringEditControl = (StringEditControl)obj;
            if (stringEditControl._vmUiControl != null)
                stringEditControl._vmUiControl.SetLabel(stringEditControl.UiLabel);
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
        private VmUiControl _vmUiControl;
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

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            this.ScrollToTop();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            var mouseClick = Mouse.LeftButton == MouseButtonState.Pressed ||
                             Mouse.RightButton == MouseButtonState.Pressed;
            if (SelectAllOnGotFocus && !mouseClick)
            {
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

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            if (UiLabel != null)
            {
                UiLabel.IsEnabled = !readOnlyValue;
            }
            IsEnabled = !readOnlyValue;
        }
    }
}
