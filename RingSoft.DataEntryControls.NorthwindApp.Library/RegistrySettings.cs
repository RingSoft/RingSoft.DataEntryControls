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
        Other = 4,
        Custom = 5
    }

    public class RegistrySettings
    {
        public const string RegistryRoot = "Registry";

        public const string NumberCultureIdKey = "NumberCultureId";
        public const string NumberCultureTypeKey = "NumberCultureType";
        public const string DateEntryFormatKey = "DateEntryFormat";
        public const string DateDisplayFormatKey = "DateDisplayFormat";

        public NumericCultureTypes NumberCultureType { get; set; }
        public string NumberCultureId { get; set; }

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
            var defaultCultureType = (int) NumericCultureTypes.Current;
            var cultureType = (NumericCultureTypes) _registryXml
                .GetElementValue(NumberCultureTypeKey, defaultCultureType.ToString()).ToInt();

            NumberCultureId = GetCultureId(cultureType,
                _registryXml.GetElementValue(NumberCultureIdKey, CultureInfo.CurrentCulture.Name));
            
            DateEntryFormat = _registryXml.GetElementValue(DateEntryFormatKey, "MM/dd/yyyy");
            DateDisplayFormat = _registryXml.GetElementValue(DateDisplayFormatKey, "MM/dd/yyyy");
        }

        public static string GetCultureId(NumericCultureTypes cultureType, string otherCultureId)
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
