namespace RingSoft.DataEntryControls.Maui;

public partial class LabelControl : ContentView
{
    public static readonly BindableProperty CaptionProperty
        = BindableProperty.Create(nameof(Caption)
        , typeof(string)
        , typeof(LabelControl));

    public string Caption
    {
        get => (string)GetValue(CaptionProperty);
        set => SetValue(CaptionProperty, value);
    }
    public LabelControl()
	{
		InitializeComponent();
	}
}