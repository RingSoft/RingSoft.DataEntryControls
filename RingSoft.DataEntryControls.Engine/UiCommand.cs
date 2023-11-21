using System;

namespace RingSoft.DataEntryControls.Engine
{
    public enum UiVisibilityTypes
    {
        Visible = 0,
        Hidden = 1,
        Collapsed = 2,
    }

    public class UiVisibilityArgs
    {
        public UiVisibilityTypes VisibilityType { get; internal set; }
    }

    public class UiEnabledArgs
    {
        public bool IsEnabled { get; internal set; }
    }

    public class UiReadOnlyArgs
    {
        public bool IsReadOnly { get; internal set; }
    }

    public class UiCaptionArgs
    {
        public string Caption { get; internal set; }
    }

    public class UiLostFocusArgs
    {
        public bool ContinueFocusChange { get; set; } = true;
    }

    public class UiCommand
    {
        private UiVisibilityTypes _uiVisibilityType = UiVisibilityTypes.Visible;

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

        private bool _isEnabled = true;

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

        private bool _isReadOnly;

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

        private string _caption;

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

        public bool IsFocused { get; private set; }

        public event EventHandler<UiVisibilityArgs> SetVisibility;

        public event EventHandler<UiEnabledArgs> SetEnabled;

        public event EventHandler<UiReadOnlyArgs> SetReadOnly;

        public event EventHandler<UiCaptionArgs> SetCaption;

        public event EventHandler OnSetFocus;

        public event EventHandler<UiLostFocusArgs> LostFocus;

        public event EventHandler GotFocus;

        public void SetFocus()
        {
            IsFocused = true;
            OnSetFocus?.Invoke(this, EventArgs.Empty);
        }


        public void FireLostFocusEvent(UiLostFocusArgs args)
        {
            IsFocused = false;
            LostFocus?.Invoke(this, args);
        }

        public void FireGotFocusEvent()
        {
            IsFocused = true;
            GotFocus?.Invoke(this, EventArgs.Empty);
        }
    }
}
