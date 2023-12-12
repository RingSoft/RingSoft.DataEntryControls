// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-29-2022
// ***********************************************************************
// <copyright file="BaseWindow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;

// ReSharper disable InconsistentNaming

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Interface IReadOnlyControl
    /// </summary>
    public interface IReadOnlyControl
    {
        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        void SetReadOnlyMode(bool readOnlyValue);
    }
    /// <summary>
    /// Base window class.
    /// Implements the <see cref="Window" />
    /// </summary>
    /// <seealso cref="Window" />
    public class BaseWindow : Window
    {
        /// <summary>
        /// Gets the window long.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nIndex">Index of the n.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        /// <summary>
        /// Sets the window long.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nIndex">Index of the n.</param>
        /// <param name="dwNewLong">The dw new long.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// The GWL style
        /// </summary>
        private const int GWL_STYLE = -16;

        /// <summary>
        /// The ws both
        /// </summary>
        private const int WS_BOTH = 0x30000; //maximize and minimize buttons

        /// <summary>
        /// The snug width property
        /// </summary>
        public static readonly DependencyProperty SnugWidthProperty =
            DependencyProperty.Register(nameof(SnugWidthProperty), typeof(double), typeof(BaseWindow));

        /// <summary>
        /// Gets or sets the width of the snug.  This is a bind-able property.
        /// </summary>
        /// <value>The width of the snug.</value>
        public double SnugWidth
        {
            get { return (double)GetValue(SnugWidthProperty); }
            set { SetValue(SnugWidthProperty, value); }
        }

        /// <summary>
        /// The snug height property
        /// </summary>
        public static readonly DependencyProperty SnugHeightProperty =
            DependencyProperty.Register(nameof(SnugHeightProperty), typeof(double), typeof(BaseWindow));

        /// <summary>
        /// Gets or sets the height of the snug.
        /// </summary>
        /// <value>The height of the snug.</value>
        public double SnugHeight
        {
            get { return (double)GetValue(SnugHeightProperty); }
            set { SetValue(SnugHeightProperty, value); }
        }




        /// <summary>
        /// Gets or sets a value indicating whether to close window when the escape key is pressed.
        /// </summary>
        /// <value><c>true</c> if to close window when the escape key is pressed; otherwise, <c>false</c>.</value>
        public bool CloseOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets a value whether to set focus to the first editable control when the window first shows.
        /// </summary>
        /// <value><c>true</c> if [set focus to first control]; otherwise, <c>false</c>.</value>
        public bool SetFocusToFirstControl { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to hide the control box.
        /// </summary>
        /// <value><c>true</c> if [hide control box]; otherwise, <c>false</c>.</value>
        public bool HideControlBox { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enter to tab].
        /// </summary>
        /// <value><c>true</c> if [enter to tab]; otherwise, <c>false</c>.</value>
        public bool EnterToTab { get; set; }

        /// <summary>
        /// The read only tab control
        /// </summary>
        private TabControl _readOnlyTabControl;
        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;

        /// <summary>
        /// Initializes static members of the <see cref="BaseWindow"/> class.
        /// </summary>
        static BaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWindow"/> class.
        /// </summary>
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
                SnugWindow();

                if (SetFocusToFirstControl)
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            };
        }

        /// <summary>
        /// Snugs the window.
        /// </summary>
        public void SnugWindow()
        {
            if (SnugWidth == 0 && SnugHeight == 0)
            {
                return;
            }
            var horizontalBorderHeight = SystemParameters.ResizeFrameHorizontalBorderHeight;
            var verticalBorderWidth = SystemParameters.ResizeFrameVerticalBorderWidth;
            var captionHeight = SystemParameters.CaptionHeight;

            Width = SnugWidth + 2 * verticalBorderWidth;
            Height = SnugHeight + captionHeight + 2 * horizontalBorderHeight;

            CenterWindowOnScreen();

        }

        /// <summary>
        /// Centers the window on screen.
        /// </summary>
        public void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
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
    }
}
