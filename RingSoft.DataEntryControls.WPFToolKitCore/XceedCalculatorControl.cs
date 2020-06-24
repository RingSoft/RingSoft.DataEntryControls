using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using Xceed.Wpf.Toolkit;

namespace RingSoft.DataEntryControls.WPFToolKitCore
{
    public class XceedCalculatorControl : Calculator, IDropDownCalculator
    {
        public Control Control => this;
    }
}
