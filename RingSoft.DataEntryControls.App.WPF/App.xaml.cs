using System.Globalization;
using System.Windows;

namespace RingSoft.DataEntryControls.App.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var culture = new CultureInfo("pt-BR"); //Format = R$ 1.234,56
            //var culture = new CultureInfo("ja-JP");  //0 decimals.  Format = ¥1,234
            //var culture = new CultureInfo("sv-SE"); //Format = 1 234,56 kr
            //var culture = CultureInfo.CurrentCulture;

            Globals.CultureId = culture.Name;
            Globals.Precision = culture.NumberFormat.CurrencyDecimalDigits;

            MainWindow = new MainWindow();
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
