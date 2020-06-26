using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace RingSoft.DataEntryControls.WPFToolKitCore
{
    public class XceedCalculatorControl : Calculator, IDropDownCalculator
    {
        public Control Control => this;
    }
}
