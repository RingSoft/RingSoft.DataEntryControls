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



        public event EventHandler<UiVisibilityArgs> SetVisibility;

        public event EventHandler<UiEnabledArgs> SetEnabled;

        public event EventHandler<UiReadOnlyArgs> SetReadOnly;

        public event EventHandler OnSetFocus;

        public void SetFocus()
        {
            OnSetFocus?.Invoke(this, EventArgs.Empty);
        }
    }
}
