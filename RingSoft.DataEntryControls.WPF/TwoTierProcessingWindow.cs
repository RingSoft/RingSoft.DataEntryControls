using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

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
    ///     <MyNamespace:TwoTierProgressWindow/>
    ///
    /// </summary>
    public class TwoTierProcessingWindow : BaseWindow
    {
        public StringReadOnlyBox TopTierText { get; private set; }
        public ProgressBar TopTierProgressBar { get; private set; }
        public StringReadOnlyBox BottomTierText { get; private set; }
        public ProgressBar BottomTierProgressBar { get; private set; }
        public TwoTierProcessingProcedure Procedure { get; }
        static TwoTierProcessingWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TwoTierProcessingWindow), new FrameworkPropertyMetadata(typeof(TwoTierProcessingWindow)));
        }

        internal TwoTierProcessingWindow(TwoTierProcessingProcedure procedure)
        {
            Procedure = procedure;
            CloseOnEscape = false;
        }

        public override void OnApplyTemplate()
        {
            TopTierText = GetTemplateChild(nameof(TopTierText)) as StringReadOnlyBox;
            TopTierProgressBar = GetTemplateChild(nameof(TopTierProgressBar)) as ProgressBar;
            BottomTierText = GetTemplateChild(nameof(BottomTierText)) as StringReadOnlyBox;
            BottomTierProgressBar = GetTemplateChild(nameof(BottomTierProgressBar)) as ProgressBar;
            base.OnApplyTemplate();
        }

        internal void Process()
        {
            Owner = Procedure.OwnerWindow;
            ShowInTaskbar = false;
            Loaded += async (sender, args) =>
            {
                await Task.Run(() =>
                {
                    if (Procedure.DoProcedure())
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Close();
                        });
                    }
                });

            };
            ShowDialog();
        }

        internal void SetProgress(int topMax, int topValue, string topText, int bottomMax, int bottomValue, string bottomText)
        {
            Dispatcher.Invoke(() =>
            {
                TopTierText.Text = topText;
                TopTierProgressBar.Minimum = 1;
                TopTierProgressBar.Maximum = topMax;
                TopTierProgressBar.Value = topValue;
                BottomTierText.Text = bottomText;
                BottomTierProgressBar.Minimum = 1;
                BottomTierProgressBar.Maximum = bottomMax;
                BottomTierProgressBar.Value = bottomValue;
            });
        }
    }
}
