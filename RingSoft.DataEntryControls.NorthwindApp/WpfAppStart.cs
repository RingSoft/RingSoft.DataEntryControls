﻿using System;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using System.Windows;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.App.WPFCore;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;

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

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <AdvancedFindUserControl>(AppGlobals.LookupContext.AdvancedFinds);

            LookupControlsGlobals.WindowRegistry.RegisterWindow<ProductWindow, Products>();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<NonInventoryCodeWindow, NonInventoryCodes>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl<PurchaseOrderUserControl, Purchases>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl<PurchaseOrderUserControl, PurchaseDetails>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl<SalesEntryUserControl, Orders>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl<SalesEntryUserControl, OrderDetails>();

            var appDbMaintenanceProcessorFactory = new AppDbMaintenanceProcessorFactory();
            var appDbMaintenanceButtonsFactory = new AppDbMaintenanceButtonsFactory();
            var appLookupContentTemplateFactory = new AppLookupContentTemplateFactory(_application);

            _application.MainWindow = _mainWindow;
            _mainWindow.Show();
        }
    }
}
