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
            
            Command.SetVisibility += (sender, args) =>
            {
                switch (args.VisibilityType)
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
            };

            Command.SetEnabled += (sender, args) =>
            {
                Control.IsEnabled = args.IsEnabled;
                if (Label != null)
                {
                    Label.IsEnabled = args.IsEnabled;
                }
            };

            Command.SetReadOnly += (sender, args) =>
            {
                if (Control is TextBox textBox)
                {
                    textBox.IsReadOnly = args.IsReadOnly;
                    textBox.IsReadOnlyCaretVisible = args.IsReadOnly;
                }

                if (Control is IReadOnlyControl readOnlyControl)
                {
                    Command.IsEnabled = true;
                    readOnlyControl.SetReadOnlyMode(args.IsReadOnly);
                }
            };

            Command.OnSetFocus += (sender, args) =>
            {
                Control.SetTabFocusToControl();            };
        }

        public void SetLabel(Label label)
        {
            Label = label;
            Label.IsEnabled = Control.IsEnabled;
            Label.Visibility = Control.Visibility;
        }
    }
}
