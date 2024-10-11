// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 09-18-2023
//
// Last Modified By : petem
// Last Modified On : 04-30-2024
// ***********************************************************************
// <copyright file="VmUiControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Maps UiCommand to control.
    /// </summary>
    public class VmUiControl
    {
        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public Control Control { get; }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <value>The element.</value>
        public FrameworkElement Element { get; }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        public Label Label { get; private set; }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public UiCommand Command { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VmUiControl" /> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="command">The command.</param>
        public VmUiControl(FrameworkElement element, UiCommand command)
        {
            Element = element;
            if (element is Control control)
            {
                Control = control;

                Control.PreviewLostKeyboardFocus += Control_PreviewLostKeyboardFocus;

                Control.GotFocus += Control_GotFocus;
            }
            Command = command;

            Command.SetVisibility += Command_SetVisibility;

            Command.SetEnabled += Command_SetEnabled;

            Command.SetReadOnly += Command_SetReadOnly;

            Command.SetCaption += Command_SetCaption;

            Command.OnSetFocus += Command_OnSetFocus;

            OnSetVisibility(Command.Visibility);
            OnSetReadOnly(command.IsReadOnly);
            OnSetEnabled(command.IsEnabled);
            if (command.Caption.IsNullOrEmpty())
            {
                if (Label != null)
                {
                    command.Caption = Label.Content.ToString();
                }
                else
                {
                    if (Control is Button button)
                    {
                        command.Caption = button.Content.ToString();
                    }
                }
            }
            else
            {
                if (Label != null)
                {
                    Label.Content = command.Caption;
                }
                else
                {
                    if (Control is Button button)
                    {
                        button.Content = command.Caption;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the Control control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            Command.FireGotFocusEvent();
        }

        /// <summary>
        /// Handles the PreviewLostKeyboardFocus event of the Control control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyboardFocusChangedEventArgs" /> instance containing the event data.</param>
        private void Control_PreviewLostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            var activeWindow = WPFControlsGlobals.ActiveWindow;
            var controlWindow = Window.GetWindow(Control);
            if (activeWindow == controlWindow)
            {
                var isChild = false;
                if (e.NewFocus is Control newControl)
                {
                    if (Control == newControl)
                    {
                        isChild = true;
                    }
                }

                if (!isChild)
                {
                    if (e.NewFocus is Control newControl1)
                    {
                        if (Control.IsChildControl(newControl1))
                        {
                            isChild = true;
                        }
                    }

                    if (!isChild)
                    {
                        var uiLostFocusArgs = new UiLostFocusArgs();
                        Command.FireLostFocusEvent(uiLostFocusArgs);
                        e.Handled = !uiLostFocusArgs.ContinueFocusChange;
                    }
                }
            }
        }

        /// <summary>
        /// Commands the set caption.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Command_SetCaption(object sender, UiCaptionArgs e)
        {
            if (Label != null)
            {
                Label.Content = e.Caption;
            }

            if (Control is Button button)
            {
                button.Content = e.Caption;
            }
        }

        /// <summary>
        /// Handles the OnSetFocus event of the Command control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Command_OnSetFocus(object sender, SetFocusArgs e)
        {
            OnSetFocus(e.IgnoreTabFocus);
        }

        /// <summary>
        /// Called when [set focus].
        /// </summary>
        protected virtual void OnSetFocus(bool ignoreTabFocus)
        {
            if (ignoreTabFocus)
            {
                Control.Focus();
                return;
            }
            Control.SetTabFocusToControl();
        }

        /// <summary>
        /// Commands the set read only.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Command_SetReadOnly(object sender, UiReadOnlyArgs e)
        {
            OnSetReadOnly(e.IsReadOnly);
        }

        /// <summary>
        /// Called when [set read only].
        /// </summary>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        protected void OnSetReadOnly(bool readOnly)
        {
            if (Control is TextBox textBox)
            {
                textBox.IsReadOnly = readOnly;
                textBox.IsReadOnlyCaretVisible = readOnly;
            }

            if (Control is IReadOnlyControl readOnlyControl)
            {
                Command.IsEnabled = true;
                readOnlyControl.SetReadOnlyMode(readOnly);
            }
        }

        /// <summary>
        /// Commands the set enabled.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Command_SetEnabled(object sender, UiEnabledArgs e)
        {
            OnSetEnabled(e.IsEnabled);
        }

        /// <summary>
        /// Called when [set enabled].
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        protected void OnSetEnabled(bool isEnabled)
        {
            Element.IsEnabled = isEnabled;
            if (Label != null)
            {
                Label.IsEnabled = isEnabled;
            }
        }

        /// <summary>
        /// Commands the set visibility.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Command_SetVisibility(object sender, UiVisibilityArgs e)
        {
            OnSetVisibility(e.VisibilityType);
        }

        /// <summary>
        /// Called when [set visibility].
        /// </summary>
        /// <param name="visibilityType">Type of the visibility.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected void OnSetVisibility(UiVisibilityTypes visibilityType)
        {
            switch (visibilityType)
            {
                case UiVisibilityTypes.Visible:
                    Element.Visibility = Visibility.Visible;
                    break;
                case UiVisibilityTypes.Hidden:
                    Element.Visibility = Visibility.Hidden;
                    break;
                case UiVisibilityTypes.Collapsed:
                    Element.Visibility = Visibility.Collapsed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Label != null)
            {
                Label.Visibility = Element.Visibility;
            }
        }

        /// <summary>
        /// Sets the label.
        /// </summary>
        /// <param name="label">The label.</param>
        public void SetLabel(Label label)
        {
            Label = label;
            if (Command != null)
            {
                if (Command.Caption.IsNullOrEmpty())
                {
                    if (Label != null)
                    {
                        Command.Caption = Label.Content.ToString();
                    }
                }
                else
                {
                    Label.Content = Command.Caption;
                }
            }

            Label.IsEnabled = Element.IsEnabled;
            Label.Visibility = Element.Visibility;
        }
    }
}
