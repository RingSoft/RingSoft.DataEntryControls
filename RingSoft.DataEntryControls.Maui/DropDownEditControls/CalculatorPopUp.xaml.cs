namespace RingSoft.DataEntryControls.Maui;

public partial class CalculatorPopUp
{
    private double? _value;

    public double? Value
    {
        get => _value;
        set
        {
            _value = value;
            CalculatorControl.Value = value;
        }
    }

    public bool PopupResult { get; private set; }
	public CalculatorPopUp()
	{
		InitializeComponent();
        CalculatorControl.Value = Value;

        OkButton.Clicked += (sender, args) =>
        {
            Value = CalculatorControl.Value;
            PopupResult = true;
            Close();
        };
        CancelButton.Clicked += (sender, args) =>
        {
            Close();
        };
    }
}