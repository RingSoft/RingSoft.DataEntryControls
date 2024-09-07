// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="StringEditControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A control that edits string values.
    /// Implements the <see cref="TextBox" />
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    /// </summary>
    /// <seealso cref="TextBox" />
    /// <seealso cref="RingSoft.DataEntryControls.WPF.IReadOnlyControl" />
    public class StringEditControl : TextBox, IReadOnlyControl
    {
        /// <summary>
        /// The select all on got focus property
        /// </summary>
        public static readonly DependencyProperty SelectAllOnGotFocusProperty =
            DependencyProperty.Register(nameof(SelectAllOnGotFocus), typeof(bool), typeof(StringEditControl));

        /// <summary>
        /// Gets or sets a value indicating whether [select all on got focus].
        /// </summary>
        /// <value><c>true</c> if [select all on got focus]; otherwise, <c>false</c>.</value>
        /// This is a bind-able property.
        public bool SelectAllOnGotFocus
        {
            get { return (bool)GetValue(SelectAllOnGotFocusProperty); }
            set { SetValue(SelectAllOnGotFocusProperty, value); }
        }

        /// <summary>
        /// The UI command property
        /// </summary>
        public static readonly DependencyProperty UiCommandProperty =
            DependencyProperty.Register(nameof(UiCommand), typeof(UiCommand), typeof(StringEditControl),
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
            var stringEditControl = (StringEditControl)obj;
            if (stringEditControl._vmUiControl == null)
            {
                stringEditControl._vmUiControl = WPFControlsGlobals.VmUiFactory.CreateUiControl(
                    stringEditControl, stringEditControl.UiCommand);
                if (stringEditControl.UiLabel != null)
                {
                    stringEditControl._vmUiControl.SetLabel(stringEditControl.UiLabel);
                }
            }
        }

        /// <summary>
        /// The UI label property
        /// </summary>
        public static readonly DependencyProperty UiLabelProperty =
            DependencyProperty.Register(nameof(UiLabel), typeof(Label), typeof(StringEditControl),
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
            var stringEditControl = (StringEditControl)obj;
            if (stringEditControl._vmUiControl != null)
                stringEditControl._vmUiControl.SetLabel(stringEditControl.UiLabel);
        }


        /// <summary>
        /// The design text
        /// </summary>
        private string _designText;

        /// <summary>
        /// Gets or sets the design text.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get => _designText;
            set
            {
                _designText = value;
                SetDesignText();
            }
        }

        /// <summary>
        /// The override sel changed
        /// </summary>
        private bool _overrideSelChanged;
        /// <summary>
        /// The vm UI control
        /// </summary>
        private VmUiControl _vmUiControl;
        /// <summary>
        /// Initializes static members of the <see cref="StringEditControl" /> class.
        /// </summary>
        static StringEditControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StringEditControl),
                new FrameworkPropertyMetadata(typeof(StringEditControl)));
            SelectAllOnGotFocusProperty.OverrideMetadata(typeof(StringEditControl), new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringEditControl" /> class.
        /// </summary>
        public StringEditControl()
        {
            //SetResourceReference(StyleProperty, typeof(TextBox));
            ContextMenu = new ContextMenu();
            ContextMenu.AddTextBoxContextMenuItems();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.Control.MouseDoubleClick" /> routed event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            this.ScrollToTop();
        }

        /// <summary>
        /// Invoked whenever an unhandled <see cref="E:System.Windows.UIElement.GotFocus" /> event reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
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

        /// <summary>
        /// Sets the design text.
        /// </summary>
        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                Text = DesignText;
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.TextCompositionManager.PreviewTextInput" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.TextCompositionEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (MaxLength > 0 && Text.Length >= MaxLength && SelectionLength == 0)
            {
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }
            base.OnPreviewTextInput(e);
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
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
