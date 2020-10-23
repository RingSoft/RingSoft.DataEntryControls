using System;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.Engine
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _handler;
        private bool _isEnabled = true;

        public RelayCommand(Action<object> handler)
        {
            _handler = handler;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _handler(parameter);
        }
    }
}