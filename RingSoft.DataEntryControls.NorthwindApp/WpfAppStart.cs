using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.Controls.WPF;
using System.Windows;

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
            //RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupAddView += NorthwindLookupContext_LookupView;
            //RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupAddView += MegaDbLookupContextOnLookupView;

            _application.MainWindow = _mainWindow;
            _mainWindow.Show();
        }


        //private void MegaDbLookupContextOnLookupView(object sender, LookupAddViewArgs e)
        //{
        //    if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Items)
        //    {
        //        ShowAddOnTheFlyWindow(new ItemsWindow(), e);
        //    }
        //    else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Locations)
        //    {
        //        ShowAddOnTheFlyWindow(new LocationWindow(), e);
        //    }
        //    else if (e.LookupData.LookupDefinition.TableDefinition == RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Manufacturers)
        //    {
        //        ShowAddOnTheFlyWindow(new ManufacturerWindow(), e);
        //    }
        //    else if (e.LookupData.LookupDefinition.TableDefinition ==
        //             RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Stocks)
        //    {
        //        ShowAddOnTheFlyWindow(new StockMasterWindow(), e);
        //    }
        //    else if (e.LookupData.LookupDefinition.TableDefinition ==
        //             RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.StockCostQuantities)
        //    {
        //        ShowAddOnTheFlyWindow(new StockCostQuantityWindow(), e);
        //    }
        //}


        //private void ShowAddOnTheFlyWindow(DbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
        //{
        //    if (e.OwnerWindow is Window ownerWindow)
        //        maintenanceWindow.Owner = ownerWindow;

        //    maintenanceWindow.ShowInTaskbar = false;
        //    maintenanceWindow.InitializeFromLookupData(e);
        //    maintenanceWindow.ShowDialog();
        //}
    }
}
