﻿using System;
using System.Threading;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public interface IAppSplashWindow
    {
        bool IsDisposed { get; }

        bool Disposing { get; }

        void SetProgress(string progressText);

        void CloseSplash();
    }

    public abstract class AppStart
    {
        public abstract IAppSplashWindow AppSplashWindow { get; }

        protected Thread SplashThread { get; private set; }

        private object _lockCloseWindow = new object();

        public async virtual void StartApp(string[] args, bool doThread = true)
        {
            AppGlobals.Initialize();

            InitializeSplash();

            if (doThread)
            {
                SplashThread = new Thread(ShowSplash);
                SplashThread.SetApartmentState(ApartmentState.STA);
                SplashThread.IsBackground = true;
                SplashThread.Start();
            }

            while (AppSplashWindow == null)
            {
                Thread.Sleep(100);
            }
            AppGlobals.AppStartProgress += (sender, progressArgs) =>
            {
                AppSplashWindow.SetProgress(progressArgs.ProgressText);
            };

            AppGlobals.UpdateGlobalsProgressStatus(StartupProgress.InitStructure);
            AppGlobals.LookupContext = new NorthwindLookupContext();
            SystemGlobals.LookupContext = AppGlobals.LookupContext;

            AppGlobals.DbContextProcessor = new DbContextProcessor();

            try
            {
                AppGlobals.UpdateGlobalsProgressStatus(StartupProgress.ConnectingToDb);
                AppGlobals.DbContextProcessor.GetProduct(1);
            }
            catch (Exception)
            {
                throw new Exception($"Error connecting to the SQLite file: {AppGlobals.LookupContext.NorthwindDataProcessor.FilePath}{AppGlobals.LookupContext.NorthwindDataProcessor.FileName}");
            }

            FinishStartup();
        }

        protected void OnMainWindowShown()
        {
            if (AppSplashWindow != null && !AppSplashWindow.Disposing && !AppSplashWindow.IsDisposed)
            {
                Monitor.Enter(_lockCloseWindow);
                try
                {
                    AppSplashWindow.CloseSplash();
                }
                finally
                {
                    Monitor.Exit(_lockCloseWindow);
                }

                if (SplashThread != null)
                {
                    while (SplashThread.IsAlive)
                        Thread.Sleep(500);
                }

                SplashThread = null;	// we don't need it any more.
            }

        }

        protected abstract void InitializeSplash();

        protected abstract void ShowSplash();

        protected abstract void FinishStartup();
    }
}
