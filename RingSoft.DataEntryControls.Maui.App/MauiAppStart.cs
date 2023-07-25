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

        public override IAppSplashWindow AppSplashWindow => _splashPage;

        public MauiAppStart(MainPage splashPage)
        {
            _splashPage = splashPage;
        }

        private MainPage _splashPage;

        protected override void InitializeSplash()
        {
            
        }

        protected override async void ShowSplash()
        {
        }

        protected override async void FinishStartup()
        {
        }
    }
}
