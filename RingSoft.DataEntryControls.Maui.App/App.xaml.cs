﻿using RingSoft.DataEntryControls.NorthwindApp.Library;

namespace RingSoft.DataEntryControls.Maui.App
{
    public partial class App : Application
    {
        public App(IServiceProvider provider)
        {
            InitializeComponent();

            var mainPage = new MainPage();
            MainPage = new NavigationPage(mainPage);

            MauiControlsGlobals.Initialize(mainPage);
            var doThread = false;
            var mauiAppStart = new MauiAppStart(mainPage);

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                mauiAppStart.StartApp(new List<string>().ToArray());
                var tables = AppGlobals.LookupContext.TableDefinitions;
            }
            else
            {
#if DEBUG
                mauiAppStart.StartAppEmulator();
#else         
                mauiAppStart.StartAppMobile();
#endif
            }
        }
    }
}