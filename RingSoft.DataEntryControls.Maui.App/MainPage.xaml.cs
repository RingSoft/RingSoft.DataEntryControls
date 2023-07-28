using CommunityToolkit.Maui.Views;
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