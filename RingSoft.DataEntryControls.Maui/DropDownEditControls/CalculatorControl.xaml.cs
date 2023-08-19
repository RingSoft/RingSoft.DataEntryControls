namespace RingSoft.DataEntryControls.Maui;

public partial class CalculatorControl : ContentView
{
    public Label MemoryStatusLabel { get; private set; }
    public Label EquationLabel { get; private set; }

    public Label EntryLabel { get; private set; }
    public Button McButton { get; private set; }
    public Button MrButton { get; private set; }
    public Button MAddButton { get; private set; }
    public Button MSubtractButton { get; private set; }
    public Button MsButton { get; private set; }

    public Button CeButton { get; private set; }
    public Button ClrButton { get; private set; }
    public Button BackButton { get; private set; }
    public Button DivideButton { get; private set; }

    public Button Button7 { get; private set; }
    public Button Button8 { get; private set; }
    public Button Button9 { get; private set; }
    public Button MultiplyButton { get; private set; }

    public Button Button4 { get; private set; }
    public Button Button5 { get; private set; }
    public Button Button6 { get; private set; }
    public Button SubtractButton { get; private set; }

    public CalculatorControl()
	{
		InitializeComponent();
	}

    protected override void OnApplyTemplate()
    {
        MemoryStatusLabel = GetTemplateChild(nameof(MemoryStatusLabel)) as Label;
        EquationLabel = GetTemplateChild(nameof(EquationLabel)) as Label;
        EntryLabel = GetTemplateChild(nameof(EntryLabel)) as Label;

        McButton = GetTemplateChild(nameof(McButton)) as Button;
        MrButton = GetTemplateChild(nameof(MrButton)) as Button;
        MAddButton = GetTemplateChild(nameof(MAddButton)) as Button;
        MSubtractButton = GetTemplateChild(nameof(MSubtractButton)) as Button;
        MsButton = GetTemplateChild(nameof(MsButton)) as Button;

        CeButton = GetTemplateChild(nameof(CeButton)) as Button;
        ClrButton = GetTemplateChild(nameof(ClrButton)) as Button;
        BackButton = GetTemplateChild(nameof(BackButton)) as Button;
        DivideButton = GetTemplateChild(nameof(DivideButton)) as Button;

        Button7 = GetTemplateChild(nameof(Button7)) as Button;
        Button8 = GetTemplateChild(nameof(Button8)) as Button;
        Button9 = GetTemplateChild(nameof(Button9)) as Button;
        MultiplyButton = GetTemplateChild(nameof(MultiplyButton)) as Button;

        Button4 = GetTemplateChild(nameof(Button4)) as Button;
        Button5 = GetTemplateChild(nameof(Button5)) as Button;
        Button6 = GetTemplateChild(nameof(Button6)) as Button;
        SubtractButton = GetTemplateChild(nameof(SubtractButton)) as Button;

        var controlValidator = new ControlValidator(typeof(CalculatorControl));
        controlValidator.Controls.Add(new ControlValidatorControl(MemoryStatusLabel, nameof(MemoryStatusLabel), typeof(Label)));
        controlValidator.Controls.Add(new ControlValidatorControl(EquationLabel, nameof(EquationLabel), typeof(Label)));
        controlValidator.Controls.Add(new ControlValidatorControl(EntryLabel, nameof(EntryLabel), typeof(Label)));

        controlValidator.Controls.Add(new ControlValidatorControl(McButton, nameof(McButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(MrButton, nameof(MrButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(MAddButton, nameof(MAddButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(MSubtractButton, nameof(MSubtractButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(MsButton, nameof(MsButton), typeof(Button)));

        controlValidator.Controls.Add(new ControlValidatorControl(CeButton, nameof(CeButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(ClrButton, nameof(ClrButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(BackButton, nameof(BackButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(DivideButton, nameof(DivideButton), typeof(Button)));

        controlValidator.Controls.Add(new ControlValidatorControl(Button7, nameof(Button7), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(Button8, nameof(Button8), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(Button9, nameof(Button9), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(MultiplyButton, nameof(MultiplyButton), typeof(Button)));

        controlValidator.Controls.Add(new ControlValidatorControl(Button4, nameof(Button4), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(Button5, nameof(Button5), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(Button6, nameof(Button6), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(SubtractButton, nameof(SubtractButton), typeof(Button)));
        controlValidator.Validate();

        base.OnApplyTemplate();
    }
}