using System;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using System.Windows;
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

        public override void StartApp(string[] args)
        {
            SystemGlobals.ProgramDataFolder = ProgramDataFolder;

            LookupControlsGlobals.InitUi();

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
            AppGlobals.LookupContext.LookupAddView += LookupContext_LookupAddView;

            LookupControlsGlobals.DbMaintenanceProcessorFactory = new AppDbMaintenanceProcessorFactory();
            LookupControlsGlobals.DbMaintenanceButtonsFactory = new AppDbMaintenanceButtonsFactory();
            LookupControlsGlobals.LookupControlContentTemplateFactory =
                new AppLookupContentTemplateFactory(_application);

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
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.Orders)
            {
                ShowAddOnTheFlyWindow(new SalesEntryWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.OrderDetails)
            {
                ShowAddOnTheFlyWindow(new SalesEntryWindow(), e);
            }
        }

        private void ShowAddOnTheFlyWindow(DbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
        {
            if (e.OwnerWindow is Window ownerWindow)
                maintenanceWindow.Owner = ownerWindow;

            maintenanceWindow.ShowInTaskbar = false;
            maintenanceWindow.Processor.InitializeFromLookupData(e);
            maintenanceWindow.ShowDialog();
        }
    }
}
