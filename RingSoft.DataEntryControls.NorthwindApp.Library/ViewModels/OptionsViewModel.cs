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

        private string _otherCultureId;

        public string OtherCultureId
        {
            get => _otherCultureId;
            set
            {
                if (_otherCultureId == value)
                    return;

                _otherCultureId = value;
                OnPropertyChanged(nameof(OtherCultureId));
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
                OtherCultureId = NumberCultureId;
            NumericValue = (decimal)1234.56;

            DateValue = DateTime.Now;
            CustomDateEntryFormat = DateEntryFormat = registrySettings.DateEntryFormat;
            CustomDateDisplayFormat = DateDisplayFormat = registrySettings.DateDisplayFormat;
        }

        public void OnApplyNumberFormat()
        {
            var cultureId = RegistrySettings.GetNumericCultureId(NumberCultureType, OtherCultureId);
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
