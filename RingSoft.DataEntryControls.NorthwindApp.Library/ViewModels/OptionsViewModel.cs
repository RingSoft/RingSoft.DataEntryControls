using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels
{
    public class OptionsViewModel : INotifyPropertyChanged
    {
        private CultureTypes _numberCultureType;

        public CultureTypes NumberCultureType
        {
            get => _numberCultureType;
            set
            {
                if (_numberCultureType == value)
                    return;

                _numberCultureType = value;

                if (!_initializing)
                    NumberCultureId = RegistrySettings.GetCultureId(NumberCultureType, CustomCultureId);

                OnPropertyChanged(nameof(NumberCultureType));
            }
        }

        private string _numberCultureId;

        public string NumberCultureId
        {
            get => _numberCultureId;
            set
            {
                if (_numberCultureId == value)
                    return;

                _numberCultureId = value;
                OnPropertyChanged(nameof(NumberCultureId));
            }
        }

        private string _customCultureId;

        public string CustomCultureId
        {
            get => _customCultureId;
            set
            {
                if (_customCultureId == value)
                    return;

                _customCultureId = value;
                OnPropertyChanged(nameof(CustomCultureId));
            }
        }

        private string _dateEntryFormat;

        public string DateEntryFormat
        {
            get => _dateEntryFormat;
            set
            {
                if (_dateEntryFormat == value)
                    return;

                _dateEntryFormat = value;
                OnPropertyChanged(nameof(DateEntryFormat));
            }
        }

        private string _dateDisplayFormat;

        public string DateDisplayFormat
        {
            get => _dateDisplayFormat;
            set
            {
                if (_dateDisplayFormat == value)
                    return;

                _dateDisplayFormat = value;
                OnPropertyChanged(nameof(DateDisplayFormat));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _initializing;

        public OptionsViewModel()
        {
            _initializing = true;
            
            RegistrySettings.LoadFromRegistryFile();
            var registrySettings = new RegistrySettings();

            NumberCultureType = registrySettings.NumberCultureType;
            NumberCultureId = registrySettings.NumberCultureId;
            if (NumberCultureType == CultureTypes.Custom)
                CustomCultureId = NumberCultureId;

            DateEntryFormat = registrySettings.DateEntryFormat;
            DateDisplayFormat = registrySettings.DateDisplayFormat;

            _initializing = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
