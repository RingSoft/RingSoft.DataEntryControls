// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 07-11-2024
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="TwoTierProcessingWindow.cs" company="RingSoft">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class TwoTierProcessingWindow.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.BaseWindow" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.BaseWindow" />
    public class TwoTierProcessingWindow : BaseWindow
    {
        /// <summary>
        /// Gets the top tier text.
        /// </summary>
        /// <value>The top tier text.</value>
        public StringReadOnlyBox TopTierText { get; private set; }
        /// <summary>
        /// Gets the top tier progress bar.
        /// </summary>
        /// <value>The top tier progress bar.</value>
        public ProgressBar TopTierProgressBar { get; private set; }
        /// <summary>
        /// Gets the bottom tier text.
        /// </summary>
        /// <value>The bottom tier text.</value>
        public StringReadOnlyBox BottomTierText { get; private set; }
        /// <summary>
        /// Gets the bottom tier progress bar.
        /// </summary>
        /// <value>The bottom tier progress bar.</value>
        public ProgressBar BottomTierProgressBar { get; private set; }
        /// <summary>
        /// Gets the procedure.
        /// </summary>
        /// <value>The procedure.</value>
        public TwoTierProcessingProcedure Procedure { get; }
        /// <summary>
        /// Initializes static members of the <see cref="TwoTierProcessingWindow" /> class.
        /// </summary>
        static TwoTierProcessingWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TwoTierProcessingWindow), new FrameworkPropertyMetadata(typeof(TwoTierProcessingWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoTierProcessingWindow" /> class.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        internal TwoTierProcessingWindow(TwoTierProcessingProcedure procedure)
        {
            Procedure = procedure;
            CloseOnEscape = false;
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            TopTierText = GetTemplateChild(nameof(TopTierText)) as StringReadOnlyBox;
            TopTierProgressBar = GetTemplateChild(nameof(TopTierProgressBar)) as ProgressBar;
            BottomTierText = GetTemplateChild(nameof(BottomTierText)) as StringReadOnlyBox;
            BottomTierProgressBar = GetTemplateChild(nameof(BottomTierProgressBar)) as ProgressBar;
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
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

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="topMax">The top maximum.</param>
        /// <param name="topValue">The top value.</param>
        /// <param name="topText">The top text.</param>
        /// <param name="bottomMax">The bottom maximum.</param>
        /// <param name="bottomValue">The bottom value.</param>
        /// <param name="bottomText">The bottom text.</param>
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
