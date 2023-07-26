using CommunityToolkit.Maui.Views;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Maui
{
    public class ControlsUserInterface : IControlsUserInterface
    {
        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            throw new NotImplementedException();
        }

        public async void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            //await DisplayMessageBox(text, caption);
        }

        public async Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            //await MauiControlsGlobals.MainPage.DisplayAlert(text, caption, "OK");
            var messageBox = new MessageBox();
            await MauiControlsGlobals.MainPage.ShowPopupAsync(messageBox);
            return MessageBoxButtonsResult.Yes;
        }

        public MessageBoxButtonsResult ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false)
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
