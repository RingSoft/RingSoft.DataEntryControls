using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
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
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DropDownEditControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DropDownEditControls;assembly=RingSoft.DataEntryControls.WPF.DropDownEditControls"
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
    ///     <MyNamespace:DropDownEditControl/>
    ///
    /// </summary>

    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "DropDownButton", Type = typeof(Button))]
    [TemplatePart(Name = "Popup", Type = typeof(Popup))]
    public abstract class DropDownEditControl : Control, IDropDownControl
    {
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(TextAlignmentChangedCallback));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        private static void TextAlignmentChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
                dropDownEditControl.TextBox.TextAlignment = dropDownEditControl.TextAlignment;
        }

        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register(nameof(DesignText), typeof(string), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            dropDownEditControl.SetDesignText();
        }

        private static void BorderThicknessChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
            {
                dropDownEditControl.TextBox.BorderThickness = dropDownEditControl.BorderThickness;
            }
        }

        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
            {
                dropDownEditControl.TextBox.Background = dropDownEditControl.Background;
            }
        }

        private static void ForegroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
            {
                dropDownEditControl.TextBox.Foreground = dropDownEditControl.Foreground;
            }
        }

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(SelectionBrushChangedCallback));

        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        private static void SelectionBrushChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            dropDownEditControl.SetControlStyleProperties();
        }

        private TextBox _textBox;
        public TextBox TextBox
        {
            get => _textBox;
            set
            {
                if (_textBox != null)
                {
                    //_textBox.PreviewTextInput -= _textBox_PreviewTextInput;
                    _textBox.PreviewKeyDown -= _textBox_PreviewKeyDown;
                    _textBox.GotFocus -= _textBox_GotFocus;
                    _textBox.TextChanged -= _textBox_TextChanged;
                }

                _textBox = value;

                if (_textBox != null)
                {
                    //_textBox.PreviewTextInput += _textBox_PreviewTextInput;
                    _textBox.PreviewKeyDown += _textBox_PreviewKeyDown;
                    _textBox.GotFocus += _textBox_GotFocus;
                    _textBox.TextChanged += _textBox_TextChanged;
                }
            }
        }

        private Button _dropDownButton;

        public Button DropDownButton
        {
            get => _dropDownButton;
            set
            {
                if (_dropDownButton != null)
                    _dropDownButton.Click -= _dropDownButton_Click;

                _dropDownButton = value;

                if (_dropDownButton != null)
                {
                    _dropDownButton.IsTabStop = false;
                    _dropDownButton.Click += _dropDownButton_Click;
                }
            }
        }

        private Popup _popup;

        public Popup Popup
        {
            get => _popup;
            set
            {
                if (_popup != null)
                    _popup.Closed -= _popup_Closed;

                _popup = value;
                
                if (_popup != null)
                    _popup.Closed += _popup_Closed;
            }
        }

        public string Text
        {
            get
            {
                if (TextBox == null)
                    return string.Empty;

                return TextBox.Text;
            }
            set
            {
                if (TextBox != null)
                    TextBox.Text = value;
            }
        }

        public int SelectionStart
        {
            get
            {
                if (TextBox == null)
                    return 0;

                return TextBox.SelectionStart;
            }
            set
            {
                if (TextBox != null)
                    TextBox.SelectionStart = value;
            }
        }

        public int SelectionLength
        {
            get
            {
                if (TextBox == null)
                    return 0;

                return TextBox.SelectionLength;
            }
            set
            {
                if (TextBox != null)
                    TextBox.SelectionLength = value;
            }
        }

        public event EventHandler<ValueChangedArgs> ValueChanged;

        private bool _processingKey;

        static DropDownEditControl()
        {
            IsTabStopProperty.OverrideMetadata(typeof(DropDownEditControl), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));

            BorderThicknessProperty.OverrideMetadata(typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(new Thickness(1), BorderThicknessChangedCallback));

            BackgroundProperty.OverrideMetadata(typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(BackgroundChangedCallback));

            ForegroundProperty.OverrideMetadata(typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(ForegroundChangedCallback));
        }

        public DropDownEditControl()
        {
            LostFocus += (sender, args) =>
            {
                if (Popup != null && !IsKeyboardFocusWithin)
                    Popup.IsOpen = false;
            };
        }

        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild("TextBox") as TextBox;
            DropDownButton = GetTemplateChild("DropDownButton") as Button;
            Popup = GetTemplateChild("Popup") as Popup;

            base.OnApplyTemplate();

            if (TextBox != null)
            {
                TextBox.TextAlignment = TextAlignment;
                ContextMenu = new ContextMenu();
                ContextMenu.AddTextBoxContextMenuItems();
                TextBox.ContextMenu = ContextMenu;
            }

            SetDesignText();
            SetControlStyleProperties();
        }

        private void SetControlStyleProperties()
        {
            if (TextBox != null)
            {
                if (Foreground != null)
                    TextBox.Foreground = Foreground;

                if (Background != null)
                    TextBox.Background = Background;

                if (SelectionBrush != null)
                    TextBox.SelectionBrush = SelectionBrush;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            TextBox?.Focus();

            base.OnGotFocus(e);
        }

        public new bool Focus()
        {
            base.Focus();
            return IsKeyboardFocusWithin;
        }

        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && TextBox != null)
                TextBox.Text = DesignText;
        }

        private void _dropDownButton_Click(object sender, RoutedEventArgs e)
        {
            OnDropDownButtonClick();
        }

        private void _textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            _processingKey = true;

            if (ProcessKey(e.Key))
                e.Handled = true;
            else
            {
                if (!(Keyboard.IsKeyDown(Key.LeftAlt)
                      || Keyboard.IsKeyDown(Key.RightAlt)
                      || Keyboard.IsKeyDown(Key.LeftCtrl)
                      || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    switch (e.Key)
                    {
                        case Key.Left:
                        case Key.Right:
                        case Key.Home:
                        case Key.End:
                            break;
                        default:
                            if (ProcessKeyChar(e.Key.GetCharFromKey()))
                                e.Handled = true;
                            break;
                    }
                }
            }

            _processingKey = false;
        }

        private void _popup_Closed(object sender, EventArgs e)
        {
            if (TextBox != null)
                TextBox.Focus();
        }

        protected virtual void OnTextBoxGotFocus()
        {
            _textBox.SelectionStart = 0;
            _textBox.SelectionLength = _textBox.Text.Length;
        }
        private void _textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            OnTextBoxGotFocus();
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_processingKey && !DesignerProperties.GetIsInDesignMode(this))
                OnTextChanged(TextBox.Text);
        }

        protected virtual void OnTextChanged(string newText)
        {

        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Popup != null && Popup.IsOpen && e.Key == Key.Escape)
            {
                OnDropDownButtonClick();
                e.Handled = true;
            }

            base.OnPreviewKeyDown(e);
        }

        protected virtual void OnDropDownButtonClick()
        {
            if (Popup != null)
            {
                Popup.IsOpen = !Popup.IsOpen;
                if (!Popup.IsOpen)
                    TextBox.Focus();
            }
        }

        protected virtual bool ProcessKeyChar(char keyChar)
        {
            return false;
        }

        protected virtual bool ProcessKey(Key key)
        {
            return false;
        }

        public bool IsPopupOpen()
        {
            return Popup != null && Popup.IsOpen;
        }

        public virtual void OnValueChanged(string newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedArgs(newValue));
        }

        public void SelectAll()
        {
            TextBox?.SelectAll();
        }
    }
}
