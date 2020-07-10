using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    public interface IDecimalEditControl : INumericControl
    {
        Control EditControl { get; }

        DecimalEditControlSetup NumericSetup { get; set; }

        decimal? Value { get; set; }
    }
}
