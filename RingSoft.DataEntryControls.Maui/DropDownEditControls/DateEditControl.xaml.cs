using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Maui;

public partial class DateEditControl : ContentView
{
    public static readonly BindableProperty DateProperty
        = BindableProperty.Create(nameof(Date)
            , typeof(DateTime)
            , typeof(DateEditControl)
            , propertyChanged: OnDateChanged);

    static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var dateEditControl = bindable as DateEditControl;
        var dateString = string.Empty;
        if (newValue is DateTime dateValue)
        {
            var formatter = new DateEditControlSetup();
            formatter.DateFormatType = DateFormatTypes.DateOnly;
            dateEditControl._value = formatter.FormatValueForDisplay(dateValue);
            dateEditControl.SetText(dateEditControl._value);
        }
        else
        {
            dateEditControl.SetText(string.Empty);
        }
    }

    public DateTime Date
    {
        get => (DateTime)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }

    public Entry TextBox { get; set; }

    private bool _settingText;
    private string _value;

    public DateEditControl()
    {
        InitializeComponent();
        WidthRequest = 150;

        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            TextBox.Completed += (sender, args) =>
            {
                var validDate = ValidDate();

                if (!validDate)
                {
                    SetText("mm/dd/yyyy");
                }
            };

            TextBox.Focused += (sender, args) =>
            {
                if (_value.IsNullOrEmpty())
                {
                    SetText("mm/dd/yyyy");
                    TextBox.CursorPosition = 1;
                    TextBox.CursorPosition = 0;
                    TextBox.SelectionLength = TextBox.Text.Length;
                }
            };

            TextBox.Unfocused += (sender, args) =>
            {
                if (_value.IsNullOrEmpty())
                {
                    SetText(string.Empty);
                }
            };

            TextBox.TextChanged += (sender, args) =>
            {
                if (!_settingText)
                {
                    var text = args.NewTextValue[TextBox.CursorPosition - 1];
                    _value = args.NewTextValue;
                    //TextBox.Text = "10/26/2023";
                    //TextBox.CursorPosition = TextBox.Text.Length;
                }
            };
        }
        else
        {
            TextBox.Placeholder = "mm/dd/yyyy";

            TextBox.Completed += (sender, args) =>
            {
                var validDate = ValidDate();

                if (!validDate)
                {
                    SetText(string.Empty);
                }
            };

        }
    }

    protected override void OnApplyTemplate()
    {
        TextBox = GetTemplateChild(nameof(TextBox)) as Entry;
        base.OnApplyTemplate();
    }

    private void SetText(string value)
    {
        _settingText = true;
        TextBox.Text = value;
        _settingText = false;
    }
    private bool ValidDate()
    {
        if (TextBox.Text.IsNullOrEmpty())
        {
            return true;
        }
        var validDate = true;
        if (!DateTime.TryParse(TextBox.Text, out var dateValue))
        {
            validDate = false;
            ControlsGlobals.UserInterface.ShowMessageBox("Invalid Date Value", "Invalid Date Value",
                RsMessageBoxIcons.Exclamation);
        }

        return validDate;
    }
}