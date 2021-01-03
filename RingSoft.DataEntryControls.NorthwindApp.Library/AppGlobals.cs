using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using System;
using System.IO;
using System.Reflection;
using RsMessageBoxIcons = RingSoft.DataEntryControls.Engine.RsMessageBoxIcons;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public enum StartupProgress
    {
        InitStructure = 0,
        ConnectingToDb = 1
    }
    public class AppStartProgressArgs
    {
        public string ProgressText { get; set; }
    }

    public static class AppGlobals
    {
        public const int CommentDisplayStyleId = 100;
        public const int NonInventoryDisplayStyleId = 101;

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

        public static string DateCultureId { get; private set; }

        public static string DateEntryFormat { get; private set; }

        public static string DateDisplayFormat { get; private set; }

        public static bool FirstTime { get; set; }

        public static bool SalesEntryScannerMode { get; set; }

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
            DateCultureId = registry.DateCultureId;
            DateEntryFormat = registry.DateEntryFormat;
            DateDisplayFormat = registry.DateDisplayFormat;
            SalesEntryScannerMode = registry.ScannerMode;

            LookupDefaults.SetDefaultNumberCultureId(NumberCultureId);
            LookupDefaults.SetDefaultDateFormatId(DateCultureId);
        }

        public static DecimalEditControlSetup CreateNewDecimalEditControlSetup()
        {
            return new DecimalEditControlSetup()
            {
                CultureId = NumberCultureId
            };
        }

        public static DateEditControlSetup CreateNewDateEditControlSetup()
        {
            return new DateEditControlSetup{CultureId = DateCultureId};
        }

        public static IntegerEditControlSetup CreateNewIntegerEditControlSetup()
        {
            return new IntegerEditControlSetup {CultureId = NumberCultureId};
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
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error Opening Text File", RsMessageBoxIcons.Error);
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
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error Writing Text File", RsMessageBoxIcons.Error);
            }
        }

        public static void UpdateGlobalsProgressStatus(StartupProgress progress)
        {
            string progressStep;
            switch (progress)
            {
                case StartupProgress.InitStructure:
                    progressStep = "Initializing Northwind Entity Framework Structure.";
                    break;
                case StartupProgress.ConnectingToDb:
                    progressStep = "Initializing Database Connection";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(progress), progress, null);
            }
            var appStartProgress = new AppStartProgressArgs
            {
                ProgressText = progressStep
            };

            AppStartProgress?.Invoke(null, appStartProgress);
        }
    }
}
