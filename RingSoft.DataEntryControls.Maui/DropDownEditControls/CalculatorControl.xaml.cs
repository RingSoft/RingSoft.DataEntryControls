using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Maui;

public partial class CalculatorControl : ICalculatorControl
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

    public Button Button1 { get; private set; }
    public Button Button2 { get; private set; }
    public Button Button3 { get; private set; }
    public Button AdditionButton { get; private set; }

    public Button PlusMinusButton { get; private set; }
    public Button Button0 { get; private set; }
    public Button DecimalButton { get; private set; }
    public Button EqualsButton { get; private set; }

    public string EquationText
    {
        get => EquationLabel.Text;
        set => EquationLabel.Text = value;
    }

    public string EntryText
    {
        get => EntryLabel.Text;
        set => EntryLabel.Text = value;
    }

    public bool MemoryRecallEnabled
    {
        get => MrButton.IsEnabled;
        set => MrButton.IsEnabled = value;
    }

    public bool MemoryClearEnabled
    {
        get => McButton.IsEnabled;
        set => McButton.IsEnabled = value;
    }

    public bool MemoryStoreEnabled
    {
        get => MsButton.IsEnabled;
        set => MsButton.IsEnabled = value;
    }

    public bool MemoryPlusEnabled
    {
        get => MAddButton.IsEnabled;
        set => MAddButton.IsEnabled = value;
    }

    public bool MemoryMinusEnabled
    {
        get => MSubtractButton.IsEnabled;
        set => MSubtractButton.IsEnabled = value;
    }

    public bool MemoryStatusVisible
    {
        get => MemoryStatusLabel.IsVisible;
        set => MemoryStatusLabel.IsVisible = value;
    }

    public double? Value
    {
        get => Processor.ComittedValue;
        set => Processor.ReinitializeValue(value);
    }

    protected CalculatorProcessor Processor { get; }


    public CalculatorControl()
	{
        Processor = new CalculatorProcessor(this);
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

        Button1 = GetTemplateChild(nameof(Button1)) as Button;
        Button2 = GetTemplateChild(nameof(Button2)) as Button;
        Button3 = GetTemplateChild(nameof(Button3)) as Button;
        AdditionButton = GetTemplateChild(nameof(AdditionButton)) as Button;

        PlusMinusButton = GetTemplateChild(nameof(PlusMinusButton)) as Button;
        Button0 = GetTemplateChild(nameof(Button0)) as Button;
        DecimalButton = GetTemplateChild(nameof(DecimalButton)) as Button;
        EqualsButton = GetTemplateChild(nameof(EqualsButton)) as Button;

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

        controlValidator.Controls.Add(new ControlValidatorControl(Button1, nameof(Button1), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(Button2, nameof(Button2), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(Button3, nameof(Button3), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(AdditionButton, nameof(AdditionButton), typeof(Button)));

        controlValidator.Controls.Add(new ControlValidatorControl(PlusMinusButton, nameof(PlusMinusButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(Button0, nameof(Button0), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(DecimalButton, nameof(DecimalButton), typeof(Button)));
        controlValidator.Controls.Add(new ControlValidatorControl(EqualsButton, nameof(EqualsButton), typeof(Button)));
        
        controlValidator.Validate();

        Processor.Initialize();

        MrButton.Clicked += (sender, args) => Processor.ProcessMemoryRecall();
        McButton.Clicked += (sender, args) => Processor.ProcessMemoryClear();
        MsButton.Clicked += (sender, args) => Processor.ProcessMemoryStore();
        MAddButton.Clicked += (sender, args) => Processor.ProcessMemoryAdd();
        MSubtractButton.Clicked += (sender, args) => Processor.ProcessMemorySubtract();

        CeButton.Clicked += (sender, args) =>  Processor.ProcessCeButton();
        ClrButton.Clicked += (sender, args) => Processor.ProcessCButton();
        BackButton.Clicked += (sender, args) => Processor.ProcessBackspace();
        DivideButton.Clicked += (sender, args) => Processor.ProcessChar('/');

        Button7.Clicked += (sender, args) => Processor.ProcessChar('7');
        Button8.Clicked += (sender, args) => Processor.ProcessChar('8');
        Button9.Clicked += (sender, args) => Processor.ProcessChar('9');
        MultiplyButton.Clicked += (sender, args) => Processor.ProcessChar('*');

        Button4.Clicked += (sender, args) => Processor.ProcessChar('4');
        Button5.Clicked += (sender, args) => Processor.ProcessChar('5');
        Button6.Clicked += (sender, args) => Processor.ProcessChar('6');
        AdditionButton.Clicked += (sender, args) => Processor.ProcessChar('+');

        Button1.Clicked += (sender, args) => Processor.ProcessChar('1');
        Button2.Clicked += (sender, args) => Processor.ProcessChar('2');
        Button3.Clicked += (sender, args) => Processor.ProcessChar('3');
        SubtractButton.Clicked += (sender, args) => Processor.ProcessChar('-');

        PlusMinusButton.Clicked += (sender, args) => Processor.ProcessPlusMinusButton();
        Button0.Clicked += (sender, args) => Processor.ProcessChar('0');
        DecimalButton.Clicked += (sender, args) => Processor.ProcessDecimal();
        EqualsButton.Clicked += (sender, args) => Processor.ProcessChar('=');

        base.OnApplyTemplate();
    }

    public void OnValueChanged(double? oldValue, double? newValue)
    {
    }
}