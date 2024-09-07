// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 09-18-2023
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="UiCommand.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Visibility Types
    /// </summary>
    public enum UiVisibilityTypes
    {
        /// <summary>
        /// Visible
        /// </summary>
        Visible = 0,
        /// <summary>
        /// Hidden
        /// </summary>
        Hidden = 1,
        /// <summary>
        /// Collapsed
        /// </summary>
        Collapsed = 2,
    }

    /// <summary>
    /// Class UiVisibilityArgs.
    /// </summary>
    public class UiVisibilityArgs
    {
        /// <summary>
        /// Gets the type of the visibility.
        /// </summary>
        /// <value>The type of the visibility.</value>
        public UiVisibilityTypes VisibilityType { get; internal set; }
    }

    /// <summary>
    /// Class UiEnabledArgs.
    /// </summary>
    public class UiEnabledArgs
    {
        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled { get; internal set; }
    }

    /// <summary>
    /// Class UiReadOnlyArgs.
    /// </summary>
    public class UiReadOnlyArgs
    {
        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly { get; internal set; }
    }

    /// <summary>
    /// Class UiCaptionArgs.
    /// </summary>
    public class UiCaptionArgs
    {
        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption { get; internal set; }
    }

    /// <summary>
    /// Class UiLostFocusArgs.
    /// </summary>
    public class UiLostFocusArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether [continue focus change].
        /// </summary>
        /// <value><c>true</c> if [continue focus change]; otherwise, <c>false</c>.</value>
        public bool ContinueFocusChange { get; set; } = true;
    }

    /// <summary>
    /// Class UiCommand.
    /// </summary>
    public class UiCommand
    {
        /// <summary>
        /// The UI visibility type
        /// </summary>
        private UiVisibilityTypes _uiVisibilityType = UiVisibilityTypes.Visible;

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        /// <value>The visibility.</value>
        public UiVisibilityTypes Visibility
        {
            get => _uiVisibilityType;
            set
            {
                _uiVisibilityType = value;
                SetVisibility?.Invoke(this, new UiVisibilityArgs
                {
                    VisibilityType = _uiVisibilityType
                });
            }
        }

        /// <summary>
        /// The is enabled
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                SetEnabled?.Invoke(this, new UiEnabledArgs
                {
                    IsEnabled = _isEnabled
                });
            }
        }

        /// <summary>
        /// The is read only
        /// </summary>
        private bool _isReadOnly;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                SetReadOnly?.Invoke(this, new UiReadOnlyArgs
                {
                    IsReadOnly = _isReadOnly
                });
            }
        }

        /// <summary>
        /// The caption
        /// </summary>
        private string _caption;

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                SetCaption?.Invoke(this, new UiCaptionArgs
                {
                    Caption = _caption
                });
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is focused.
        /// </summary>
        /// <value><c>true</c> if this instance is focused; otherwise, <c>false</c>.</value>
        public bool IsFocused { get; private set; }

        /// <summary>
        /// Occurs when [set visibility].
        /// </summary>
        public event EventHandler<UiVisibilityArgs> SetVisibility;

        /// <summary>
        /// Occurs when [set enabled].
        /// </summary>
        public event EventHandler<UiEnabledArgs> SetEnabled;

        /// <summary>
        /// Occurs when [set read only].
        /// </summary>
        public event EventHandler<UiReadOnlyArgs> SetReadOnly;

        /// <summary>
        /// Occurs when [set caption].
        /// </summary>
        public event EventHandler<UiCaptionArgs> SetCaption;

        /// <summary>
        /// Occurs when [on set focus].
        /// </summary>
        public event EventHandler OnSetFocus;

        /// <summary>
        /// Occurs when [lost focus].
        /// </summary>
        public event EventHandler<UiLostFocusArgs> LostFocus;

        /// <summary>
        /// Occurs when [got focus].
        /// </summary>
        public event EventHandler GotFocus;

        /// <summary>
        /// Sets the focus.
        /// </summary>
        public void SetFocus()
        {
            IsFocused = true;
            OnSetFocus?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Fires the lost focus event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void FireLostFocusEvent(UiLostFocusArgs args)
        {
            IsFocused = false;
            LostFocus?.Invoke(this, args);
        }

        /// <summary>
        /// Fires the got focus event.
        /// </summary>
        public void FireGotFocusEvent()
        {
            IsFocused = true;
            GotFocus?.Invoke(this, EventArgs.Empty);
        }
    }
}
