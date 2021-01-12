using RingSoft.DataEntryControls.WPF;
using System.Collections.ObjectModel;
using System.Windows;

namespace TestDummyApp
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TestDummyApp"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TestDummyApp;assembly=TestDummyApp"
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
    ///     <MyNamespace:LineTypeComboBox/>
    ///
    /// </summary>
    //public class LineTypeComboBox : ContentComboBoxControl
    //{
    //    public AppGridLineTypes LineType
    //    {
    //        get => (AppGridLineTypes)SelectedItemId;
    //        set => SelectItem((int)value);
    //    }

    //    static LineTypeComboBox()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(LineTypeComboBox), new FrameworkPropertyMetadata(typeof(LineTypeComboBox)));
    //    }

    //    protected override ObservableCollection<CustomContentItem> GetCustomContent()
    //    {
    //        return Globals.GetLineTypeContents();
    //    }
    //}
}
