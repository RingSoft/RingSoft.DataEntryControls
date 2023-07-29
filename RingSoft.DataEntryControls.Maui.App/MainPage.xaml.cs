using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Platform;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.NorthwindApp.Library;

namespace RingSoft.DataEntryControls.Maui.App
{
    public partial class MainPage : IAppSplashWindow
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            var labelControl = new LabelControl();
            labelControl.Caption = "Testing";
            labelControl.Content = new DateEditControl() { Date = DateTime.Today.AddDays(7) };
            FlexLayout.Children.Add(labelControl);
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