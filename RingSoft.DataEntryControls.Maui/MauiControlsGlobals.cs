using CommunityToolkit.Maui.Views;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Maui
{
    public class ControlsUserInterface : IControlsUserInterface
    {
        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            
        }

        public async Task ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            await MauiControlsGlobals.MainPage.DisplayAlert(caption, text, "OK");
        }

        public async Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            var result = await MauiControlsGlobals.MainPage.DisplayActionSheet(text
                , null
                , null
                , FlowDirection.LeftToRight
                , "Yes"
                , "No");

            if (result == "Yes")
            {
                return MessageBoxButtonsResult.Yes;
            }

            return MessageBoxButtonsResult.No;
        }

        public Task<MessageBoxButtonsResult> ShowYesNoCancelMessageBox(string text, string caption,
            bool playSound = false)
        {
            throw new NotImplementedException();
        }
    }
    public static class MauiControlsGlobals
    {
        public static ContentPage MainPage { get; internal set; }

        public static void Initialize(ContentPage mainPage)
        {
            MainPage = mainPage;
        }
    }
}
