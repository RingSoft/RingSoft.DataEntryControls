using System;
using System.IO;
using System.Reflection;

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

        public static event EventHandler<AppStartProgressArgs> AppStartProgress;

        public static void Initialize()
        {

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
