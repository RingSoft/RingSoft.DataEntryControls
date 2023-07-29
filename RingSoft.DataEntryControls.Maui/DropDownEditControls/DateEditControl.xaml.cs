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
        if (newValue is DateTime dateValue)
        {
            dateEditControl.DatePicker.Date = dateValue;
            dateEditControl.SetupTimeControl(dateValue);
        }
    }

    public DateTime Date
    {
        get => (DateTime)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
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
        dateEditControl.SetupTimeControl(dateEditControl.Date);
    }

    public DatePicker DatePicker { get; private set; }

    public TimePicker TimePicker { get; private set; }

    public DateEditControl()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate()
    {
        DatePicker = GetTemplateChild(nameof(DatePicker)) as DatePicker;
        TimePicker = GetTemplateChild(nameof(TimePicker)) as TimePicker;

        SetupTimeControl(Date);
        DatePicker.DateSelected += (sender, args) =>
        {
            Date = args.NewDate;
        };
        base.OnApplyTemplate();
    }

    private void SetupTimeControl(DateTime newDate)
    {
        if (TimePicker != null)
        {
            TimePicker.IsVisible = FormatType != DateFormatTypes.DateOnly;
            if (TimePicker.IsVisible)
            {
                TimePicker.Time = newDate.TimeOfDay;
            }
        }
    }
}