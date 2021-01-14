using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
// ReSharper disable InconsistentNaming

namespace RingSoft.DataEntryControls.WPF
{
    public interface IReadOnlyControl
    {
        void SetReadOnlyMode(bool readOnlyValue);
    }
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
    ///     <MyNamespace:BaseWindow/>
    ///
    /// </summary>
    public class BaseWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;

        private const int WS_BOTH = 0x30000; //maximize and minimize buttons

        /// <summary>
        /// Gets or sets a value indicating whether to close window when the escape key is pressed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to close window when the escape key is pressed; otherwise, <c>false</c>.
        /// </value>
        public bool CloseOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets a value whether to set focus to the first editable control when the window first shows.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [set focus to first control]; otherwise, <c>false</c>.
        /// </value>
        public bool SetFocusToFirstControl { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to hide the control box.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [hide control box]; otherwise, <c>false</c>.
        /// </value>
        public bool HideControlBox { get; set; }

        public bool EnterToTab { get; set; }

        private TabControl _readOnlyTabControl;
        private bool _readOnlyMode;

        static BaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        }

        public BaseWindow()
        {
            if (Application.Current.MainWindow != this)
                if (Application.Current.MainWindow != null)
                    Icon = Application.Current.MainWindow.Icon;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            KeyDown += (sender, args) =>
            {
                switch (args.Key)
                {
                    case Key.Escape:
                        if (CloseOnEscape)
                        {
                            Close();
                            args.Handled = true;
                        }

                        break;
                    case Key.Enter:
                        if (EnterToTab)
                        {
                            WPFControlsGlobals.SendKey(Key.Tab);
                            args.Handled = true;
                        }
                        break;
                }
            };

            SourceInitialized += (sender, args) =>
            {
                if (HideControlBox)
                {
                    var hwnd = new WindowInteropHelper((Window)sender ?? throw new InvalidOperationException()).Handle;
                    GetWindowLong(hwnd, GWL_STYLE);
                    SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_BOTH);
                }
            };

            Loaded += (sender, args) =>
            {
                if (SetFocusToFirstControl)
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            };
        }

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
                if (!focusedElement.IsEnabled)
                {
                    WPFControlsGlobals.SendKey(Key.Tab);
                }
            }

            OnReadOnlyModeSet(readOnlyValue);
        }

        protected virtual void OnReadOnlyModeSet(bool readOnlyValue)
        {
        }

        public virtual void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (control is IReadOnlyControl readOnlyControl)
                readOnlyControl.SetReadOnlyMode(readOnlyValue);
            else if (!(control is TabControl) && !(control is TabItem) && !(control is Label))
                control.IsEnabled = !readOnlyValue;
        }

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
    }
}
