using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    public class VmUiControl
    {
        public Control Control { get; }

        public Label Label { get; private set; }

        public UiCommand Command { get; }

        public VmUiControl(Control control, UiCommand command)
        {
            Control = control;
            Command = command;

            Control.PreviewLostKeyboardFocus += Control_PreviewLostKeyboardFocus;

            Control.GotFocus += Control_GotFocus;

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

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            Command.FireGotFocusEvent();
        }

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

        private void Command_OnSetFocus(object sender, EventArgs e)
        {
            OnSetFocus();
        }

        protected virtual void OnSetFocus()
        {
            Control.SetTabFocusToControl();
        }

        private void Command_SetReadOnly(object sender, UiReadOnlyArgs e)
        {
            OnSetReadOnly(e.IsReadOnly);
        }

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

        private void Command_SetEnabled(object sender, UiEnabledArgs e)
        {
            OnSetEnabled(e.IsEnabled);
        }

        protected void OnSetEnabled(bool isEnabled)
        {
            Control.IsEnabled = isEnabled;
            if (Label != null)
            {
                Label.IsEnabled = isEnabled;
            }
        }

        private void Command_SetVisibility(object sender, UiVisibilityArgs e)
        {
            OnSetVisibility(e.VisibilityType);
        }

        protected void OnSetVisibility(UiVisibilityTypes visibilityType)
        {
            switch (visibilityType)
            {
                case UiVisibilityTypes.Visible:
                    Control.Visibility = Visibility.Visible;
                    break;
                case UiVisibilityTypes.Hidden:
                    Control.Visibility = Visibility.Hidden;
                    break;
                case UiVisibilityTypes.Collapsed:
                    Control.Visibility = Visibility.Collapsed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Label != null)
            {
                Label.Visibility = Control.Visibility;
            }
        }

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

            Label.IsEnabled = Control.IsEnabled;
            Label.Visibility = Control.Visibility;
        }
    }
}
