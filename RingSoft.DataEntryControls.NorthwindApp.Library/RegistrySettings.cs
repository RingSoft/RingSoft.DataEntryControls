using System;
using System.Globalization;
using System.IO;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public enum NumericCultureTypes
    {
        Current = 0,
        Usa = 1,
        Brazil = 2,
        Sweden = 3,
        Other = 4
    }

    public enum DateCultureTypes
    {
        Current = 0,
        Usa = 1,
        Spain = 2,
        China = 3,
        Other = 4
    }

    public class RegistrySettings
    {
        public const string RegistryRoot = "Registry";

        public const string NumberCultureIdKey = "NumberCultureId";
        public const string NumberCultureTypeKey = "NumberCultureType";
        public const string DateCultureIdKey = "DateCultureId";
        public const string DateCultureTypeKey = "DateCultureType";
        public const string DateEntryFormatKey = "DateEntryFormat";
        public const string DateDisplayFormatKey = "DateDisplayFormat";

        public NumericCultureTypes NumberCultureType { get; set; }
        public string NumberCultureId { get; set; }
        public DateCultureTypes DateCultureType { get; set; }
        public string DateCultureId { get; set; }
        public string DateEntryFormat { get; set; }
        public string DateDisplayFormat { get; set; }

        private static XmlProcessor _registryXml = new XmlProcessor(RegistryRoot);

        public RegistrySettings()
        {
            LoadFromRegistry();
        }

        internal static void LoadFromRegistryFile()
        {
            if (File.Exists(AppGlobals.RegistryFileName))
            {
                var xml = AppGlobals.OpenTextFile(AppGlobals.RegistryFileName);
                _registryXml.LoadFromXml(xml);
            }
        }

        public void LoadFromRegistry()
        {
            var defaultNumericCultureType = (int) NumericCultureTypes.Current;
            var numericCultureType = (NumericCultureTypes) _registryXml
                .GetElementValue(NumberCultureTypeKey, defaultNumericCultureType.ToString()).ToInt();

            NumberCultureId = GetNumericCultureId(numericCultureType,
                _registryXml.GetElementValue(NumberCultureIdKey, CultureInfo.CurrentCulture.Name));

            var defaultDateCultureType = (int)DateCultureTypes.Current;
            var dateCultureType = (DateCultureTypes)_registryXml
                .GetElementValue(DateCultureTypeKey, defaultDateCultureType.ToString()).ToInt();

            DateCultureId = GetDateCultureId(dateCultureType,
                _registryXml.GetElementValue(DateCultureIdKey, CultureInfo.CurrentCulture.Name));

            DateEntryFormat = _registryXml.GetElementValue(DateEntryFormatKey, "d");
            DateDisplayFormat = _registryXml.GetElementValue(DateDisplayFormatKey, "d");
        }

        public static string GetNumericCultureId(NumericCultureTypes cultureType, string otherCultureId)
        {
            switch (cultureType)
            {
                case NumericCultureTypes.Current:
                    return CultureInfo.CurrentCulture.Name;
                case NumericCultureTypes.Usa:
                    return "en-US";
                case NumericCultureTypes.Brazil:
                    return "pt-BR";
                case NumericCultureTypes.Sweden:
                    return "sv-SE";
                case NumericCultureTypes.Other:
                    return otherCultureId;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetDateCultureId(DateCultureTypes cultureType, string otherCultureId)
        {
            switch (cultureType)
            {
                case DateCultureTypes.Current:
                    return CultureInfo.CurrentCulture.Name;
                case DateCultureTypes.Usa:
                    return "en-US";
                case DateCultureTypes.Spain:
                    return "es-ES";
                case DateCultureTypes.China:
                    return "zh-CH";
                case DateCultureTypes.Other:
                    return otherCultureId;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cultureType), cultureType, null);
            }
        }

        public void SaveToRegistry()
        {
            _registryXml.SetElementValue(NumberCultureIdKey, NumberCultureId);
            _registryXml.SetElementValue(DateEntryFormatKey, DateEntryFormat);
            _registryXml.SetElementValue(DateDisplayFormatKey, DateDisplayFormat);

            var xml = _registryXml.OutputXml();
            AppGlobals.WriteTextFile(AppGlobals.RegistryFileName, xml);
        }
    }
}
