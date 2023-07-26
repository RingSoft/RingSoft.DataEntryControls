namespace RingSoft.DataEntryControls.Maui;

public partial class MessagePage : ContentPage
{
	public MessagePage()
	{
		InitializeComponent();
	}

    private void Button_OnClicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}