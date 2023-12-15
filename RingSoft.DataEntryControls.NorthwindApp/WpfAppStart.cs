using System;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using System.Windows;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.App.WPFCore;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class WpfAppStart : AppStart
    {
        public override IAppSplashWindow AppSplashWindow => _splashWindow;

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


        private Application _application;
        private MainWindow _mainWindow;
        private AppSplashWindow _splashWindow;

        public WpfAppStart(Application application)
        {
            _application = application;
        }

        public override void StartAppMobile(string dataDir)
        {
            SystemGlobals.ProgramDataFolder = ProgramDataFolder;

            base.StartAppMobile(dataDir);
        }

        public override void StartApp(string[] args)
        {
            SystemGlobals.ProgramDataFolder = ProgramDataFolder;

            _mainWindow = new MainWindow();

            try
            {
                base.StartApp(args);
            }
            catch (Exception e)
            {
                _splashWindow.ShowError(e.Message, "Database Connection Error!");
                OnMainWindowShown();
                _application.Shutdown();
            }
        }

        protected override void InitializeSplash()
        {
            _mainWindow.Done += (_, _) =>
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
            AppGlobals.LookupContext.Initialize();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<ProductWindow>(
                AppGlobals.LookupContext.Products);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<NonInventoryCodeWindow>(
                AppGlobals.LookupContext.NonInventoryCodes);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<SalesEntryWindow>(
                AppGlobals.LookupContext.Orders);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<SalesEntryWindow>(
                AppGlobals.LookupContext.OrderDetails);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<PurchaseOrderWindow>(
                AppGlobals.LookupContext.Purchases);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<PurchaseOrderWindow>(
                AppGlobals.LookupContext.PurchaseDetails);

            var appDbMaintenanceProcessorFactory = new AppDbMaintenanceProcessorFactory();
            var appDbMaintenanceButtonsFactory = new AppDbMaintenanceButtonsFactory();
            var appLookupContentTemplateFactory = new AppLookupContentTemplateFactory(_application);

            _application.MainWindow = _mainWindow;
            _mainWindow.Show();
        }
    }
}
