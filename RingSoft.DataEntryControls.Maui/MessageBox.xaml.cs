namespace RingSoft.DataEntryControls.Maui;

public partial class MessageBox
{
    public bool Result { get; private set; }
    public MessageBox()
	{
		InitializeComponent();
	}

    private void Button_OnClicked(object sender, EventArgs e)
    {
        Result = true;
        Close(true);
    }
}