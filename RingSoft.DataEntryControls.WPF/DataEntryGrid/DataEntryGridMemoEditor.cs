using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DataEntryGrid"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DataEntryControls.WPF.DataEntryGrid;assembly=RingSoft.DataEntryControls.WPF.DataEntryGrid"
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
    ///     <MyNamespace:NewDataEntryGridMemoEditor/>
    ///
    /// </summary>
    [TemplatePart(Name = "MemoEditor", Type = typeof(DataEntryMemoEditor))]
    [TemplatePart(Name = "OkButton", Type = typeof(Button))]
    [TemplatePart(Name = "CancelButton", Type = typeof(Button))]
    public class DataEntryGridMemoEditor : BaseWindow
    {
        private DataEntryMemoEditor _memoEditor;
        public DataEntryMemoEditor MemoEditor
        {
            get => _memoEditor;
            set
            {
                _memoEditor = value;
            }
        }

        private Button _okButton;

        public Button OkButton
        {
            get => _okButton;
            set
            {
                if (OkButton != null)
                {
                    OkButton.Click -= OkButton_Click;
                }

                _okButton = value;

                if (OkButton != null)
                {
                    OkButton.Click += OkButton_Click;
                }
            }
        }

        private Button _cancelButton;

        public Button CancelButton
        {
            get => _cancelButton;
            set
            {
                if (CancelButton != null)
                {
                    CancelButton.Click -= CancelButton_Click;
                }

                _cancelButton = value;

                if (CancelButton != null)
                {
                    CancelButton.Click += CancelButton_Click;
                }
            }
        }

        public DataEntryGridMemoValue GridMemoValue { get; }

        

        private bool _dialogResult;

        static DataEntryGridMemoEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGridMemoEditor), new FrameworkPropertyMetadata(typeof(DataEntryGridMemoEditor)));
            ShowInTaskbarProperty.OverrideMetadata(typeof(DataEntryGridMemoEditor), new FrameworkPropertyMetadata(false));
        }

        public DataEntryGridMemoEditor(DataEntryGridMemoValue gridMemoValue)
        {
            if (SnugWidth == 0)
            {
                SnugWidth = 300;
            }
            if (SnugHeight == 0)
            {
                SnugHeight = 300;
            }

            GridMemoValue = gridMemoValue;

            Loaded += (sender, args) =>
            {
                if (MemoEditor != null)
                {
                    MemoEditor.Text = GridMemoValue.Text;
                }
            };
        }

        public override void OnApplyTemplate()
        {
            MemoEditor = GetTemplateChild(nameof(MemoEditor)) as DataEntryMemoEditor;
            OkButton = GetTemplateChild(nameof(OkButton)) as Button;
            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;


            base.OnApplyTemplate();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                GridMemoValue.Text = MemoEditor.Text;
                _dialogResult = true;
                Close();
            }
        }

        protected virtual bool Validate()
        {
            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public new bool ShowDialog()
        {
            base.ShowDialog();
            return _dialogResult;
        }

    }
}
