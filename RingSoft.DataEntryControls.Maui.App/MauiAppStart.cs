using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.NorthwindApp.Library;

namespace RingSoft.DataEntryControls.Maui.App
{
    public class MauiAppStart : AppStart
    {
        public static string ProgramDataFolder
        {
            get
            {
#if DEBUG
                return AppDomain.CurrentDomain.BaseDirectory;
#else
                return
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\DataEntryNorthwindDemoApp\\";
#endif
            }
        }

        public override IAppSplashWindow AppSplashWindow => _mainPage;

        public MauiAppStart(MainPage mainPage)
        {
            _mainPage = mainPage;
            ControlsGlobals.UserInterface = new ControlsUserInterface();
        }

        private MainPage _mainPage;

        protected override void InitializeSplash()
        {
            
        }

        protected override void ShowSplash()
        {
            
        }

        protected override void FinishStartup()
        {
            
        }
    }
}
