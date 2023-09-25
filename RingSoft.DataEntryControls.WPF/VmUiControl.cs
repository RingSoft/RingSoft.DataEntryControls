using System;
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

            Command.SetVisibility += Command_SetVisibility;

            Command.SetEnabled += Command_SetEnabled;

            Command.SetReadOnly += Command_SetReadOnly;

            Command.OnSetFocus += Command_OnSetFocus;

            OnSetVisibility(Command.Visibility);
            OnSetReadOnly(command.IsReadOnly);
            OnSetEnabled(command.IsEnabled);
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
            Label.IsEnabled = Control.IsEnabled;
            Label.Visibility = Control.Visibility;
        }
    }
}
