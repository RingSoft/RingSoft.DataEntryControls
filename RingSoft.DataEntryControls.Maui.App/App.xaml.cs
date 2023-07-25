namespace RingSoft.DataEntryControls.Maui.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var mainPage = new MainPage();
            MainPage = new NavigationPage(mainPage);

            var mauiAppStart = new MauiAppStart(mainPage);
            mauiAppStart.StartApp(new List<string>().ToArray());
        }
    }
}