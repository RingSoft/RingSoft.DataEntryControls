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

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                var mauiAppStart = new MauiAppStart(mainPage);
                mauiAppStart.StartApp(new List<string>().ToArray());
            }
        }
    }
}