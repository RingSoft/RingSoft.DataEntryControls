using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    public interface IDecimalEditControl : INumericControl
    {
        Control EditControl { get; }

        DecimalEditControlSetup NumericSetup { get; set; }

        double? Value { get; set; }
    }
}
