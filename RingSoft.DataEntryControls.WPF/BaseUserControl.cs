// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 10-08-2024
//
// Last Modified By : petem
// Last Modified On : 10-12-2024
// ***********************************************************************
// <copyright file="BaseUserControl.cs" company="RingSoft">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class BaseUserControl.
    /// Implements the <see cref="UserControl" />
    /// </summary>
    /// <seealso cref="UserControl" />
    public class BaseUserControl : UserControl
    {
        /// <summary>
        /// Gets or sets a value whether to set focus to the first editable control when the window first shows.
        /// </summary>
        /// <value><c>true</c> if [set focus to first control]; otherwise, <c>false</c>.</value>
        public bool SetFocusToFirstControl { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [enter to tab].
        /// </summary>
        /// <value><c>true</c> if [enter to tab]; otherwise, <c>false</c>.</value>
        public bool EnterToTab { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore tab].
        /// </summary>
        /// <value><c>true</c> if [ignore tab]; otherwise, <c>false</c>.</value>
        public bool IgnoreTab { get; set; }

        /// <summary>
        /// Gets the owner window.
        /// </summary>
        /// <value>The owner window.</value>
        public Window OwnerWindow { get; private set; }

        /// <summary>
        /// The read only tab control
        /// </summary>
        private TabControl _readOnlyTabControl;
        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserControl"/> class.
        /// </summary>
        public BaseUserControl()
        {
            Loaded += (sender, args) =>
            {
                OwnerWindow = Window.GetWindow(this);
            };
            KeyDown += (sender, args) =>
            {
                if (args.Key == Key.Tab)
                {
                    if (IgnoreTab)
                    {
                        args.Handled = true;
                        IgnoreTab = false;
                        return;
                    }
                }

                switch (args.Key)
                {
                    case Key.Enter:
                        if (EnterToTab)
                        {
                            WPFControlsGlobals.SendKey(Key.Tab);
                            args.Handled = true;
                        }
                        break;
                }
            };
        }

        /// <summary>
        /// Called when [tab selection changed].
        /// </summary>
        private void OnTabSelectionChanged()
        {
            if (_readOnlyTabControl.SelectedContent is DependencyObject rootDependencyObject)
            {
                if (rootDependencyObject is Control rootControl)
                    SetControlReadOnlyMode(rootControl, _readOnlyMode);
                else
                    rootDependencyObject.SetAllChildControlsReadOnlyMode(_readOnlyMode);
            }
        }

        /// <summary>
        /// Sets the control read only mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public virtual void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (control is IReadOnlyControl readOnlyControl)
                readOnlyControl.SetReadOnlyMode(readOnlyValue);
            else if (!(control is TabControl) && !(control is TabItem) && !(control is Label))
                control.IsEnabled = !readOnlyValue;
        }


        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            _readOnlyMode = readOnlyValue;
            if (_readOnlyTabControl == null)
            {
                _readOnlyTabControl = this.GetVisualChild<TabControl>();
                if (_readOnlyTabControl != null)
                    _readOnlyTabControl.SelectionChanged += (sender, args) => OnTabSelectionChanged();
            }

            var focusedElement = FocusManager.GetFocusedElement(this);

            this.SetAllChildControlsReadOnlyMode(readOnlyValue);

            if (readOnlyValue)
            {
                if (focusedElement != null && !focusedElement.IsEnabled)
                {
                    WPFControlsGlobals.SendKey(Key.Tab);
                }
            }

            OnReadOnlyModeSet(readOnlyValue);
        }

        /// <summary>
        /// Called when [read only mode set].
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        protected virtual void OnReadOnlyModeSet(bool readOnlyValue)
        {
        }
    }
}
