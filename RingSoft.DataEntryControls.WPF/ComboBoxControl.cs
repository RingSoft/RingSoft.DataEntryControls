using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF;assembly=RingSoft.DataEntryControls.WPF"
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
    ///     <MyNamespace:ComboBoxControl/>
    ///
    /// </summary>
    public class ComboBoxControl : ComboBox
    {
        //public new static readonly DependencyProperty ItemsSourceProperty =
        //    DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ComboBoxControl),
        //        new FrameworkPropertyMetadata(ItemsSourceChangedCallback));

        //public new IEnumerable ItemsSource
        //{
        //    get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        //    set { SetValue(ItemsSourceProperty, value); }
        //}

        private static object CoerceItemsSourceProperty(DependencyObject obj, object baseValue)
        {
            var comboBoxControl = (ComboBoxControl)obj;
            if (!comboBoxControl.IsDesignMode())
                return baseValue;

            return comboBoxControl.GetItemsSource();
        }

        private static object CoerceItemTemplateProperty(DependencyObject obj, object baseValue)
        {
            var comboBoxControl = (ComboBoxControl)obj;
            if (!comboBoxControl.IsDesignMode())
                return baseValue;

            return null;
        }


        //private static void ItemsSourcePropertyChangedCallback(DependencyObject obj,
        //    DependencyPropertyChangedEventArgs args)
        //{
        //}

        //public new static readonly DependencyProperty SelectedItemProperty =
        //    DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ComboBoxControl),
        //        new FrameworkPropertyMetadata(SelectedItemPropertyChangedCallback));

        //public new object SelectedItem
        //{
        //    get { return GetValue(SelectedItemProperty); }
        //    set { SetValue(SelectedItemProperty, value); }
        //}

        //private static void SelectedItemPropertyChangedCallback(DependencyObject obj,
        //    DependencyPropertyChangedEventArgs args)
        //{
        //    var comboBoxControl = (ComboBoxControl)obj;
        //    comboBoxControl.SetSelectedItem();
        //}


        private string _designText;

        public string DesignText
        {
            get => _designText;
            set
            {
                _designText = value;
                SetDesignText();
            }
        }

        private ObservableCollection<string> _designerList = new ObservableCollection<string>();

        static ComboBoxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxControl),
                new FrameworkPropertyMetadata(typeof(ComboBoxControl)));

            ItemsSourceProperty.OverrideMetadata(typeof(ComboBoxControl), 
                new FrameworkPropertyMetadata(null, CoerceItemsSourceProperty));

            ItemTemplateProperty.OverrideMetadata(typeof(ComboBoxControl),
                new FrameworkPropertyMetadata(null, CoerceItemTemplateProperty));
        }

        //public ComboBoxControl()
        //{
        //    SetResourceReference(StyleProperty, typeof(ComboBox));
        //}

        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                if (IsEditable)
                {
                    Text = DesignText;
                }
                else
                {
                    //MessageBox.Show("SetDesignText");
                    
                    _designerList.Clear();
                    _designerList.Add(DesignText);
                    SelectedItem = DesignText;
                }
            }
        }

        private bool IsDesignMode() => DesignerProperties.GetIsInDesignMode(this);

        private IEnumerable GetItemsSource()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return _designerList;
            }

            return ItemsSource;
        }
    }
}
