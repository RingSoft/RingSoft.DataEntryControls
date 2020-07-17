using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;

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

        private string _customDateEntryFormat;

        public string CustomDateEntryFormat
        {
            get => _customDateEntryFormat;
            set
            {
                if (_customDateEntryFormat == value)
                    return;

                _customDateEntryFormat = value;
                OnPropertyChanged(nameof(CustomDateEntryFormat));
            }
        }

        private string _customDateDisplayFormat;

        public string CustomDateDisplayFormat
        {
            get => _customDateDisplayFormat;
            set
            {
                if (_customDateDisplayFormat == value)
                    return;

                _customDateDisplayFormat = value;
                OnPropertyChanged(nameof(CustomDateDisplayFormat));
            }
        }

        private decimal _numericValue;


        public string CurrentCultureName => CultureInfo.CurrentCulture.Name;

        public event PropertyChangedEventHandler PropertyChanged;


        public OptionsViewModel()
        {
            RegistrySettings.LoadFromRegistryFile();
            var registrySettings = new RegistrySettings();

            NumberCultureType = registrySettings.NumberCultureType;
            NumberCultureId = registrySettings.NumberCultureId;
            if (NumberCultureType == CultureTypes.Custom)
                CustomCultureId = NumberCultureId;

            CustomDateEntryFormat = DateEntryFormat = registrySettings.DateEntryFormat;
            CustomDateEntryFormat = DateDisplayFormat = registrySettings.DateDisplayFormat;
        }

        public void OnApplyNumberFormat()
        {
            var cultureId = RegistrySettings.GetCultureId(NumberCultureType, CustomCultureId);
            try
            {
                var culture = new CultureInfo(cultureId);
            }
            catch (Exception e)
            {
                DbDataProcessor.UserInterface.ShowMessageBox(e.Message, "Invalid Culture ID",
                    RsMessageBoxIcons.Exclamation);
                return;
            }

            NumberCultureId = cultureId;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
