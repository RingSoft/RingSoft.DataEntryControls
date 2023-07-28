using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels
{
    public class MauiDemoViewModel : INotifyPropertyChanged
    {
        private DateTime _dateOnly;

        public DateTime DateOnly
        {
            get => _dateOnly;
            set
            {
                if (_dateOnly == value)
                {
                    return;
                }
                _dateOnly = value;
                OnPropertyChanged();
            }
        }

        public MauiDemoViewModel()
        {
            DateOnly = DateTime.Today;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
