using System;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF.DropDownEditControls
{
    public interface IDropDownCalendar
    {
        Control Control { get; }

        DateTime? Value { get; set; }

        event EventHandler ValueChanged;

    }
}
