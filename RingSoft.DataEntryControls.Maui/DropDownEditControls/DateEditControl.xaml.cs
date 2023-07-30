using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.Maui;

public partial class DateEditControl : ContentView
{
    public static readonly BindableProperty ValueProperty
        = BindableProperty.Create(nameof(Value)
            , typeof(DateTime?)
            , typeof(DateEditControl)
            , propertyChanged: OnValueChanged);


    public DateTime? Value
    {
        get => (DateTime?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var dateEditControl = bindable as DateEditControl;
        if (newValue is DateTime dateValue)
        {
            dateEditControl._settingValue = true;
            dateEditControl.DatePicker.Date = dateValue;
            dateEditControl.SetupTimeControl(dateValue);
            dateEditControl._settingValue = false;
        }
    }

    public static readonly BindableProperty FormatTypeProperty
        = BindableProperty.Create(nameof(FormatType)
            , typeof(DateFormatTypes)
            , typeof(DateEditControl)
            , propertyChanged: OnFormatTypeChanged);

    public DateFormatTypes FormatType
    {
        get => (DateFormatTypes)GetValue(FormatTypeProperty);
        set => SetValue(FormatTypeProperty, value);
    }

    static void OnFormatTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var dateEditControl = bindable as DateEditControl;
        dateEditControl.SetupTimeControl(dateEditControl.Value);
    }


    public DatePicker DatePicker { get; private set; }

    public TimePicker TimePicker { get; private set; }

    private bool _settingValue;
    private bool _settingTimeValue;
    public DateEditControl()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate()
    {
        DatePicker = GetTemplateChild(nameof(DatePicker)) as DatePicker;
        TimePicker = GetTemplateChild(nameof(TimePicker)) as TimePicker;

        var controlValidator = new ControlValidator(typeof(DateEditControl));
        controlValidator.Controls.Add(new ControlValidatorControl(DatePicker, nameof(DatePicker), typeof(DatePicker)));
        controlValidator.Controls.Add(new ControlValidatorControl(TimePicker, nameof(TimePicker), typeof(TimePicker)));
        controlValidator.Validate();

        SetupTimeControl(Value);
        DatePicker.DateSelected += (sender, args) =>
        {
            if (!_settingValue)
            {
                Value = args.NewDate;
            }
        };

        TimePicker.PropertyChanged += (sender, args) =>
        {
            if (!_settingValue && !_settingTimeValue)
            {
                var dateString = DatePicker.Date.ToShortDateString();
                dateString += $" {TimePicker.Time.ToString()}";
                var date = DateTime.Parse(dateString);
                Value = date;
            }
        };

        base.OnApplyTemplate();
    }

    private void SetupTimeControl(DateTime? newDate)
    {
        if (TimePicker != null)
        {
            TimePicker.IsVisible = FormatType != DateFormatTypes.DateOnly;
            if (TimePicker.IsVisible && newDate.HasValue)
            {
                _settingTimeValue = true;
                TimePicker.Time = newDate.Value.TimeOfDay;
                _settingTimeValue = false;
            }
        }
    }
}