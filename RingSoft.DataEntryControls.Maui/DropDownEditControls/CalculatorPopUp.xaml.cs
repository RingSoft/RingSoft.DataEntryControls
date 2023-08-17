namespace RingSoft.DataEntryControls.Maui;

public partial class CalculatorPopUp
{
	public CalculatorPopUp()
	{
		InitializeComponent();

        OkButton.Clicked += (sender, args) =>
        {
            Close();
        };
    }
}