using System.Globalization;
using System.IO;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class RegistrySettings
    {
        public const string RegistryRoot = "Registry";

        public const string NumberCultureIdKey = "NumberCultureId";
        public const string DateEntryFormatKey = "DateEntryFormat";
        public const string DateDisplayFormatKey = "DateDisplayFormat";

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
            NumberCultureId = _registryXml.GetElementValue(NumberCultureIdKey, CultureInfo.CurrentCulture.Name);
            DateEntryFormat = _registryXml.GetElementValue(DateEntryFormatKey, "MM/dd/yyyy");
            DateDisplayFormat = _registryXml.GetElementValue(DateDisplayFormatKey, "MM/dd/yyyy");
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
