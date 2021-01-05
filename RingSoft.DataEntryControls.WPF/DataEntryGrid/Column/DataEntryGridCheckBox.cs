using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DataEntryGrid.Column"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DataEntryGrid.Column;assembly=RingSoft.DataEntryControls.WPF.DataEntryGrid.Column"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DataEntryGridCheckBox/>
    ///
    /// </summary>
    public class DataEntryGridCheckBox : CheckBox, IDataEntryGridColumnControl
    {
        static DataEntryGridCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGridCheckBox), new FrameworkPropertyMetadata(typeof(DataEntryGridCheckBox)));
        }

        public void SetControlStyle(DataEntryGridCellStyle cellStyle, DataEntryGridDisplayStyle displayStyle)
        {
            if (cellStyle is DataEntryGridControlCellStyle controlCellStyle)
            {
                IsEnabled = controlCellStyle.IsEnabled;
                Visibility = controlCellStyle.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
