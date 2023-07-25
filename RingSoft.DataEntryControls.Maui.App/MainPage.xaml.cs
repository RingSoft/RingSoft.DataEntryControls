﻿using RingSoft.DataEntryControls.NorthwindApp.Library;

namespace RingSoft.DataEntryControls.Maui.App
{
    public partial class MainPage : ContentPage, IAppSplashWindow
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        public bool IsDisposed => true;
        public bool Disposing => false;
        public void SetProgress(string progressText)
        {
            
        }

        public void CloseSplash()
        {
            
        }
    }
}