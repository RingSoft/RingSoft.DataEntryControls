using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.Controls.WPF;
using System.Windows;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class WpfAppStart : AppStart
    {
        public override IAppSplashWindow AppSplashWindow => _splashWindow;

        private Application _application;
        private MainWindow _mainWindow;
        private AppSplashWindow _splashWindow;

        public WpfAppStart(Application application)
        {
            _application = application;
        }

        public override void StartApp(string[] args)
        {
            ControlsGlobals.InitUi();

            _mainWindow = new MainWindow();
            base.StartApp(args);
        }

        protected override void InitializeSplash()
        {
            _mainWindow.Done += (sender, args) =>
            {
                OnMainWindowShown();
            };
        }

        protected override void ShowSplash()
        {
            _splashWindow = new AppSplashWindow();
            _splashWindow.ShowDialog();
        }

        protected override void FinishStartup()
        {
            AppGlobals.LookupContext.LookupAddView += LookupContext_LookupAddView;

            _application.MainWindow = _mainWindow;
            _mainWindow.Show();
        }

        private void LookupContext_LookupAddView(object sender, LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.Products)
            {
                ShowAddOnTheFlyWindow(new ProductWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.NonInventoryCodes)
            {
                ShowAddOnTheFlyWindow(new NonInventoryCodeWindow(), e);
            }
        }

        private void ShowAddOnTheFlyWindow(DbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
        {
            if (e.OwnerWindow is Window ownerWindow)
                maintenanceWindow.Owner = ownerWindow;

            maintenanceWindow.ShowInTaskbar = false;
            maintenanceWindow.InitializeFromLookupData(e);
            maintenanceWindow.ShowDialog();
        }
    }
}
