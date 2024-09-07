// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="DropDownEditControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// the base class of all dropdown edit controls.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="IDropDownControl" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="IDropDownControl" />

    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "DropDownButton", Type = typeof(Button))]
    [TemplatePart(Name = "Popup", Type = typeof(Popup))]
    public abstract class DropDownEditControl : Control, IDropDownControl
    {
        /// <summary>
        /// The text alignment property
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(TextAlignmentChangedCallback));

        /// <summary>
        /// Gets or sets the text alignment.  This is a bind-able property.
        /// </summary>
        /// <value>The text alignment.</value>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Texts the alignment changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void TextAlignmentChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
                dropDownEditControl.TextBox.TextAlignment = dropDownEditControl.TextAlignment;
        }

        /// <summary>
        /// The design text property
        /// </summary>
        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register(nameof(DesignText), typeof(string), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        /// <summary>
        /// Gets or sets the design text.  This is a bind-able property.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        /// <summary>
        /// Designs the text changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            dropDownEditControl.SetDesignText();
        }

        /// <summary>
        /// The rs is tab stop property
        /// </summary>
        public static new readonly DependencyProperty RsIsTabStopProperty =
            DependencyProperty.Register(nameof(RsIsTabStop), typeof(bool), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(true, RsIsTabStopChangedCallback));

        /// <summary>
        /// Gets or sets a value indicating whether [rs is tab stop].  This is a bind-able property.
        /// </summary>
        /// <value><c>true</c> if [rs is tab stop]; otherwise, <c>false</c>.</value>
        public new bool RsIsTabStop
        {
            get { return (bool)GetValue(RsIsTabStopProperty); }
            set { SetValue(RsIsTabStopProperty, value); }
        }

        /// <summary>
        /// Rses the is tab stop changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void RsIsTabStopChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
            {
                dropDownEditControl.TextBox.IsTabStop = dropDownEditControl.RsIsTabStop;
            }
        }

        /// <summary>
        /// Borders the thickness changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void BorderThicknessChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
            {
                dropDownEditControl.TextBox.BorderThickness = dropDownEditControl.BorderThickness;
            }
        }

        /// <summary>
        /// Backgrounds the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
            {
                dropDownEditControl.TextBox.Background = dropDownEditControl.Background;
            }
        }

        /// <summary>
        /// Heights the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void HeightChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
            {
                var height = dropDownEditControl.Height;
                if (height > dropDownEditControl.ActualHeight)
                {
                    height = dropDownEditControl.ActualHeight;
                }
                dropDownEditControl.TextBox.Height = height;
                if (dropDownEditControl.DropDownButton != null)
                {
                    dropDownEditControl.DropDownButton.Height = height;
                }
            }
        }


        /// <summary>
        /// Foregrounds the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ForegroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl.TextBox != null)
            {
                dropDownEditControl.TextBox.Foreground = dropDownEditControl.Foreground;
            }
        }

        /// <summary>
        /// The selection brush property
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(SelectionBrushChangedCallback));

        /// <summary>
        /// Gets or sets the selection brush.
        /// </summary>
        /// <value>The selection brush.</value>
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        /// <summary>
        /// Selections the brush changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void SelectionBrushChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            dropDownEditControl.SetControlStyleProperties();
        }

        /// <summary>
        /// The UI command property
        /// </summary>
        public static readonly DependencyProperty UiCommandProperty =
            DependencyProperty.Register(nameof(UiCommand), typeof(UiCommand), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(UiCommandChangedCallback));

        /// <summary>
        /// Gets or sets the UI command.  This is a bind-able property.
        /// </summary>
        /// <value>The UI command.</value>
        public UiCommand UiCommand
        {
            get { return (UiCommand)GetValue(UiCommandProperty); }
            set { SetValue(UiCommandProperty, value); }
        }

        /// <summary>
        /// UIs the command changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void UiCommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl._vmUiControl == null)
            {
                dropDownEditControl._vmUiControl = WPFControlsGlobals.VmUiFactory.CreateUiControl(dropDownEditControl, dropDownEditControl.UiCommand);
                if (dropDownEditControl.UiLabel != null)
                {
                    dropDownEditControl._vmUiControl.SetLabel(dropDownEditControl.UiLabel);
                }
            }
        }

        /// <summary>
        /// The UI label property
        /// </summary>
        public static readonly DependencyProperty UiLabelProperty =
            DependencyProperty.Register(nameof(UiLabel), typeof(Label), typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(UiLabelChangedCallback));

        /// <summary>
        /// Gets or sets the UI label.  This is a bind-able property.
        /// </summary>
        /// <value>The UI label.</value>
        public Label UiLabel
        {
            get { return (Label)GetValue(UiLabelProperty); }
            set { SetValue(UiLabelProperty, value); }
        }

        /// <summary>
        /// UIs the label changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void UiLabelChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (DropDownEditControl)obj;
            if (dropDownEditControl._vmUiControl != null)
                dropDownEditControl._vmUiControl.SetLabel(dropDownEditControl.UiLabel);
        }


        /// <summary>
        /// The text box
        /// </summary>
        private TextBox _textBox;
        /// <summary>
        /// Gets or sets the text box.
        /// </summary>
        /// <value>The text box.</value>
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

        /// <summary>
        /// The drop down button
        /// </summary>
        private Button _dropDownButton;

        /// <summary>
        /// Gets or sets the drop down button.
        /// </summary>
        /// <value>The drop down button.</value>
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

        /// <summary>
        /// The popup
        /// </summary>
        private Popup _popup;

        /// <summary>
        /// Gets or sets the popup.
        /// </summary>
        /// <value>The popup.</value>
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

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
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

        /// <summary>
        /// Gets or sets the selection start.
        /// </summary>
        /// <value>The selection start.</value>
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

        /// <summary>
        /// Gets or sets the length of the selection.
        /// </summary>
        /// <value>The length of the selection.</value>
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

        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;

        /// <summary>
        /// Gets or sets a value indicating whether [read only mode].
        /// </summary>
        /// <value><c>true</c> if [read only mode]; otherwise, <c>false</c>.</value>
        public bool ReadOnlyMode
        {
            get => _readOnlyMode;
            set
            {
                _readOnlyMode = value;
                if (TextBox != null) TextBox.Focusable = !_readOnlyMode;
                //if (_readOnlyMode && DropDownButton != null)
                //{
                //    DropDownButton.Focus();
                //}
            }
        }


        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event EventHandler<ValueChangedArgs> ValueChanged;

        /// <summary>
        /// The processing key
        /// </summary>
        private bool _processingKey;
        /// <summary>
        /// The set focus
        /// </summary>
        private bool _setFocus;
        /// <summary>
        /// The vm UI control
        /// </summary>
        private VmUiControl _vmUiControl;

        /// <summary>
        /// Initializes static members of the <see cref="DropDownEditControl" /> class.
        /// </summary>
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

            HeightProperty.OverrideMetadata(typeof(DropDownEditControl),
                new FrameworkPropertyMetadata(HeightChangedCallback));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownEditControl" /> class.
        /// </summary>
        public DropDownEditControl()
        {
            LostFocus += (sender, args) =>
            {
                if (Popup != null && !IsKeyboardFocusWithin)
                    Popup.IsOpen = false;
            };
            MouseMove += (sender, args) =>
            {
                if (ReadOnlyMode)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                }
            };
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild("TextBox") as StringEditControl;
            DropDownButton = GetTemplateChild("DropDownButton") as Button;
            Popup = GetTemplateChild("Popup") as Popup;

            base.OnApplyTemplate();

            if (TextBox != null)
            {
                TextBox.TextAlignment = TextAlignment;
                ContextMenu = new ContextMenu();
                ContextMenu.AddTextBoxContextMenuItems();
                TextBox.ContextMenu = ContextMenu;
                //TextBox.SelectAllOnGotFocus = true;
            }

            SetDesignText();
            SetControlStyleProperties();
            if (_setFocus)
            {
                _setFocus = false;
                TextBox?.Focus();
            }
        }

        /// <summary>
        /// Sets the control style properties.
        /// </summary>
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

                TextBox.IsTabStop = RsIsTabStop;
            }
        }

        //Necessary
        /// <summary>
        /// Invoked whenever an unhandled <see cref="E:System.Windows.UIElement.GotFocus" /> event reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (TextBox == null)
            {
                _setFocus = true;
            }
            else
            {
                TextBox?.Focus();
            }

            base.OnGotFocus(e);
        }

        /// <summary>
        /// Focuses this instance.
        /// </summary>
        /// <returns><see langword="true" /> if keyboard focus and logical focus were set to this element; <see langword="false" /> if only logical focus was set to this element, or if the call to this method did not force the focus to change.</returns>
        public new bool Focus()
        {
            base.Focus();
            return IsKeyboardFocusWithin;
        }

        /// <summary>
        /// Sets the design text.
        /// </summary>
        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && TextBox != null)
                TextBox.Text = DesignText;
        }

        /// <summary>
        /// Handles the Click event of the _dropDownButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void _dropDownButton_Click(object sender, RoutedEventArgs e)
        {
            OnDropDownButtonClick();
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the _textBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Closed event of the _popup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _popup_Closed(object sender, EventArgs e)
        {
            if (TextBox != null)
                TextBox.Focus();
        }

        /// <summary>
        /// Called when [text box got focus].
        /// </summary>
        protected virtual void OnTextBoxGotFocus()
        {
            //_textBox.SelectionStart = 0;
            //_textBox.SelectionLength = _textBox.Text.Length;
        }
        /// <summary>
        /// Handles the GotFocus event of the _textBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void _textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            OnTextBoxGotFocus();
        }

        /// <summary>
        /// Handles the TextChanged event of the _textBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_processingKey && !DesignerProperties.GetIsInDesignMode(this))
                OnTextChanged(TextBox.Text);
        }

        /// <summary>
        /// Called when [text changed].
        /// </summary>
        /// <param name="newText">The new text.</param>
        protected virtual void OnTextChanged(string newText)
        {

        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Popup != null && Popup.IsOpen && e.Key == Key.Escape)
            {
                OnDropDownButtonClick();
                e.Handled = true;
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Called when [drop down button click].
        /// </summary>
        public virtual void OnDropDownButtonClick()
        {
            if (Popup != null)
            {
                Popup.IsOpen = !Popup.IsOpen;
                if (!Popup.IsOpen)
                {
                    if (ReadOnlyMode)
                    {
                        DropDownButton.Focus();
                    }
                    else
                    {
                        TextBox.Focus();
                    }
                }
            }
        }

        /// <summary>
        /// Processes the key character.
        /// </summary>
        /// <param name="keyChar">The key character.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ProcessKeyChar(char keyChar)
        {
            return false;
        }

        /// <summary>
        /// Processes the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ProcessKey(Key key)
        {
            return false;
        }

        /// <summary>
        /// Determines whether [is popup open].
        /// </summary>
        /// <returns><c>true</c> if [is popup open]; otherwise, <c>false</c>.</returns>
        public bool IsPopupOpen()
        {
            return Popup != null && Popup.IsOpen;
        }

        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public virtual void OnValueChanged(string newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedArgs(newValue));
        }

        /// <summary>
        /// Selects all.
        /// </summary>
        public void SelectAll()
        {
            TextBox?.SelectAll();
        }
    }
}
