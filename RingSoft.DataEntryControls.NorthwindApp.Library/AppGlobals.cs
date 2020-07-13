using System;
using System.IO;
using System.Reflection;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class AppStartProgressArgs
    {
        public string ProgressText { get; set; }
    }

    public static class AppGlobals
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string AppDataDirectory
        {
            get
            {
#if DEBUG
                return AssemblyDirectory;
#else
                return $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\DataEntryNorthwindDemoApp";
#endif
            }
        }

        public static NorthwindLookupContext LookupContext { get; set; }

        public static DbContextProcessor DbContextProcessor { get; set; }

        public static string RegistryFileName { get; private set; }

        public static string NumberCultureId { get; private set; }

        public static string DateEntryFormat { get; private set; }

        public static string DateDisplayFormat { get; private set; }


        public static bool FirstTime { get; set; }

        public static event EventHandler<AppStartProgressArgs> AppStartProgress;

        public static void Initialize()
        {
            var registryElementName = "RegistryFileName";
            var appSettingsFile = $"{AppDataDirectory}\\AppSettings.xml";
            var xmlProcessor = new XmlProcessor("AppSettings");
            if (File.Exists(appSettingsFile))
            {
                var xml = OpenTextFile(appSettingsFile);
                xmlProcessor.LoadFromXml(xml);
            }
            else
            {
                FirstTime = true;
                xmlProcessor.SetElementValue(registryElementName, $"{AppDataDirectory}\\Registry.xml");
                var xml = xmlProcessor.OutputXml();
                WriteTextFile(appSettingsFile, xml);
            }

            RegistryFileName = xmlProcessor.GetElementValue(registryElementName, string.Empty);
            RegistrySettings.LoadFromRegistryFile();

            var registry = new RegistrySettings();
            registry.LoadFromRegistry();

            NumberCultureId = registry.NumberCultureId;
            DateEntryFormat = registry.DateEntryFormat;
            DateDisplayFormat = registry.DateDisplayFormat;
        }

        public static string OpenTextFile(string fileName)
        {
            var result = string.Empty;
            try
            {
                var openFile = new StreamReader(fileName);
                result = openFile.ReadToEnd();
            }
            catch (Exception e)
            {
                DbDataProcessor.UserInterface.ShowMessageBox(e.Message, "Error Opening Text File", RsMessageBoxIcons.Error);
            }

            return result;
        }

        public static void WriteTextFile(string fileName, string text)
        {
            try
            {
                var directory = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(directory))
                    if (directory != null)
                        Directory.CreateDirectory(directory);

                File.WriteAllText(fileName, text);
            }
            catch (Exception e)
            {
                DbDataProcessor.UserInterface.ShowMessageBox(e.Message, "Error Writing Text File", RsMessageBoxIcons.Error);
            }
        }

        public static void UpdateGlobalsProgressStatus()
        {
            var appStartProgress = new AppStartProgressArgs
            {
                ProgressText = "Initializing Northwind Entity Framework Structure."
            };

            AppStartProgress?.Invoke(null, appStartProgress);
        }
    }
}
