using System.Windows;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var wpfAppStart = new WpfAppStart(this);
            wpfAppStart.StartApp(e.Args);

            base.OnStartup(e);
        }
    }
}
