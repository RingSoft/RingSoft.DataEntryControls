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
        private NumericCultureTypes _numberCultureType;

        public NumericCultureTypes NumberCultureType
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

        private string _otherNumberCultureId;

        public string OtherNumberCultureId
        {
            get => _otherNumberCultureId;
            set
            {
                if (_otherNumberCultureId == value)
                    return;

                _otherNumberCultureId = value;
                OnPropertyChanged(nameof(OtherNumberCultureId));
            }
        }

        private DateCultureTypes _dateCultureType;

        public DateCultureTypes DateCultureType
        {
            get => _dateCultureType;
            set
            {
                if (_dateCultureType == value)
                    return;

                _dateCultureType = value;
                OnPropertyChanged(nameof(DateCultureType));
            }
        }

        private string _dateCultureId;

        public string DateCultureId
        {
            get => _dateCultureId;
            set
            {
                if (_dateCultureId == value)
                    return;

                _dateCultureId = value;
                OnPropertyChanged(nameof(DateCultureId));
            }
        }

        private string _otherDateCultureId;

        public string OtherDateCultureId
        {
            get => _otherDateCultureId;
            set
            {
                if (_otherDateCultureId == value)
                    return;

                _otherDateCultureId = value;
                OnPropertyChanged(nameof(OtherDateCultureId));
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

        private decimal _numericValue;

        public decimal NumericValue
        {
            get => _numericValue;
            set
            {
                if (_numericValue == value)
                    return;

                _numericValue = value;
                OnPropertyChanged(nameof(NumericValue));
            }
        }

        private DateTime? _dateValue;

        public DateTime? DateValue
        {
            get => _dateValue;
            set
            {
                if (_dateValue == value)
                    return;

                _dateValue = value;
                OnPropertyChanged(nameof(DateValue));
            }
        }

        public string CurrentCultureName => CultureInfo.CurrentCulture.Name;

        public event PropertyChangedEventHandler PropertyChanged;

        public OptionsViewModel()
        {
            RegistrySettings.LoadFromRegistryFile();
            var registrySettings = new RegistrySettings();

            NumberCultureType = registrySettings.NumberCultureType;
            NumberCultureId = registrySettings.NumberCultureId;
            if (NumberCultureType == NumericCultureTypes.Other)
                OtherNumberCultureId = NumberCultureId;
            NumericValue = (decimal)1234.56;

            DateCultureType = registrySettings.DateCultureType;
            DateCultureId = registrySettings.DateCultureId;
            if (DateCultureType == DateCultureTypes.Other)
                OtherDateCultureId = DateCultureId;

            DateValue = DateTime.Now;
            DateEntryFormat = registrySettings.DateEntryFormat;
            DateDisplayFormat = registrySettings.DateDisplayFormat;
        }

        public void OnApplyNumberFormat()
        {
            var cultureId = RegistrySettings.GetNumericCultureId(NumberCultureType, OtherNumberCultureId);
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

        public void OnApplyDateFormat()
        {
            try
            {

            }
            catch (Exception e)
            {
                DbDataProcessor.UserInterface.ShowMessageBox(e.Message, "Invalid Date Format",
                    RsMessageBoxIcons.Exclamation);
                return;
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
