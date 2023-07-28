using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Maui;

public partial class DateEditControl : ContentView
{

    private bool _settingDefaultText;
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
                    SetDefaultText("mm/dd/yyyy");
                }
            };

            TextBox.Focused += (sender, args) =>
            {
                if (_value.IsNullOrEmpty())
                {
                    SetDefaultText("mm/dd/yyyy");
                    TextBox.CursorPosition = 1;
                    TextBox.CursorPosition = 0;
                    TextBox.SelectionLength = TextBox.Text.Length;
                }
            };

            TextBox.Unfocused += (sender, args) =>
            {
                if (_value.IsNullOrEmpty())
                {
                    SetDefaultText(string.Empty);
                }
            };

            TextBox.TextChanged += (sender, args) =>
            {
                if (!_settingDefaultText)
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
                    SetDefaultText(string.Empty);
                }
            };

        }
    }

    private void SetDefaultText(string value)
    {
        _settingDefaultText = true;
        TextBox.Text = value;
        _settingDefaultText = false;
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