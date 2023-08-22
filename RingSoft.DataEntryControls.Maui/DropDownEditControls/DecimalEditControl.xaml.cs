using CommunityToolkit.Maui.Views;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Maui;

public partial class DecimalEditControl : ContentView
{
	public Entry Entry { get; private set; }

	public ImageButton Button { get; private set; }

    public static readonly BindableProperty ValueProperty
        = BindableProperty.Create(nameof(Value)
            , typeof(double?)
            , typeof(DecimalEditControl)
            , propertyChanged: OnValueChanged);


    public double? Value
    {
        get => (double?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var decimalEditControl = bindable as DecimalEditControl;
        if (newValue is double doubleValue)
        {
            var formatter = new DecimalEditControlSetup();
            decimalEditControl._settingValue = true;
            decimalEditControl.Entry.Text = formatter.FormatValue(decimalEditControl.Value);
            decimalEditControl._settingValue = false;
        }
    }

    private bool _settingValue;

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
        Value = Entry.Text.ToDecimal();

        base.OnApplyTemplate();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var calculator = new CalculatorPopUp();
        var value = Entry.Text.ToDecimal();
        double.TryParse(Entry.Text, out value);
        calculator.Value = value;

        await MauiControlsGlobals.MainPage.ShowPopupAsync(calculator);

        if (calculator.PopupResult)
        {
            Value = calculator.Value;
        }
    }
}