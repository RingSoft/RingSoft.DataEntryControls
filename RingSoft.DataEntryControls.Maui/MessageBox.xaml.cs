namespace RingSoft.DataEntryControls.Maui;

public partial class MessageBox
{
	public MessageBox()
	{
		InitializeComponent();
	}

    private void Button_OnClicked(object sender, EventArgs e)
    {
        Close();
    }
}