using CommunityToolkit.Maui.Views;

namespace RingSoft.DataEntryControls.Maui;

public partial class DecimalEditControl : ContentView
{
	public Entry Entry { get; private set; }

	public ImageButton Button { get; private set; }
	public DecimalEditControl()
	{
		InitializeComponent();
	}

    protected override void OnApplyTemplate()
    {
		Entry = GetTemplateChild(nameof(Entry)) as Entry;
		Button = GetTemplateChild(nameof(Button)) as ImageButton;

        var controlValidator = new ControlValidator(typeof (DecimalEditControl));
        controlValidator.Controls.Add(new ControlValidatorControl(Entry, nameof(Entry), typeof(Entry)));
        controlValidator.Controls.Add(new ControlValidatorControl(Button, nameof(Button), typeof(ImageButton)));
        controlValidator.Validate();

        Button.Clicked += Button_Clicked;

        base.OnApplyTemplate();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var calculator = new CalculatorPopUp();
        await MauiControlsGlobals.MainPage.ShowPopupAsync(calculator);
        await MauiControlsGlobals.MainPage.DisplayAlert("Test", "Test", "OK");
    }
}