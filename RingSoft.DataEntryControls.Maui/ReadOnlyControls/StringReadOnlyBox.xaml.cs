namespace RingSoft.DataEntryControls.Maui;

public partial class StringReadOnlyBox : ContentView
{
	public Label Label { get; private set; }
	public StringReadOnlyBox()
	{
		InitializeComponent();
	}

    protected override void OnApplyTemplate()
    {
		Label = GetTemplateChild(nameof(Label)) as Label;
        base.OnApplyTemplate();
    }
}