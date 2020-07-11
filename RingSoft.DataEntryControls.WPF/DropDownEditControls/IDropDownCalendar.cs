using System;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DropDownEditControls
{
    public interface IDropDownCalendar
    {
        Control Control { get; }

        DateTime? SelectedDate { get; set; }

        event EventHandler SelectedDateChanged;

        event EventHandler DatePicked;
    }
}
