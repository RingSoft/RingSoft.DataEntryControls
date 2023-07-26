namespace RingSoft.DataEntryControls.Maui.App
{
    public partial class App : Application
    {
        public App(IServiceProvider provider)
        {
            InitializeComponent();

            var mainPage = new MainPage();
            MainPage = new NavigationPage(mainPage);

            MauiControlsGlobals.Initialize(provider, mainPage);
            var doThread = false;

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                doThread = true;
            }
            var mauiAppStart = new MauiAppStart(mainPage);
            mauiAppStart.StartApp(new List<string>().ToArray(), doThread);

        }
    }
}