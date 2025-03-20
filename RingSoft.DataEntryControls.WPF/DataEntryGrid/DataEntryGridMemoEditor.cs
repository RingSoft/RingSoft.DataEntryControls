// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="DataEntryGridMemoEditor.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// A grid memo editor.  Breaks down text to lines to display on a grid.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.BaseWindow" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.BaseWindow" />
    [TemplatePart(Name = "MemoEditor", Type = typeof(DataEntryMemoEditor))]
    [TemplatePart(Name = "OkButton", Type = typeof(Button))]
    [TemplatePart(Name = "CancelButton", Type = typeof(Button))]
    public class DataEntryGridMemoEditor : BaseWindow
    {
        /// <summary>
        /// The memo editor
        /// </summary>
        private DataEntryMemoEditor _memoEditor;
        /// <summary>
        /// Gets or sets the memo editor.
        /// </summary>
        /// <value>The memo editor.</value>
        public DataEntryMemoEditor MemoEditor
        {
            get => _memoEditor;
            set
            {
                _memoEditor = value;
            }
        }

        /// <summary>
        /// The ok button
        /// </summary>
        private Button _okButton;

        /// <summary>
        /// Gets or sets the ok button.
        /// </summary>
        /// <value>The ok button.</value>
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

        /// <summary>
        /// The cancel button
        /// </summary>
        private Button _cancelButton;

        /// <summary>
        /// Gets or sets the cancel button.
        /// </summary>
        /// <value>The cancel button.</value>
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

        /// <summary>
        /// Gets the grid memo value.
        /// </summary>
        /// <value>The grid memo value.</value>
        public DataEntryGridMemoValue GridMemoValue { get; }



        /// <summary>
        /// The dialog result
        /// </summary>
        private bool _dialogResult;

        /// <summary>
        /// Initializes static members of the <see cref="DataEntryGridMemoEditor" /> class.
        /// </summary>
        static DataEntryGridMemoEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEntryGridMemoEditor), new FrameworkPropertyMetadata(typeof(DataEntryGridMemoEditor)));
            ShowInTaskbarProperty.OverrideMetadata(typeof(DataEntryGridMemoEditor), new FrameworkPropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridMemoEditor" /> class.
        /// </summary>
        /// <param name="gridMemoValue">The grid memo value.</param>
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

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            MemoEditor = GetTemplateChild(nameof(MemoEditor)) as DataEntryMemoEditor;
            OkButton = GetTemplateChild(nameof(OkButton)) as Button;
            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;


            base.OnApplyTemplate();
        }

        /// <summary>
        /// Handles the Click event of the OkButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                GridMemoValue.Text = MemoEditor.Text;
                _dialogResult = true;
                Close();
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool Validate()
        {
            return true;
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public new bool ShowDialog()
        {
            base.ShowDialog();
            return _dialogResult;
        }

    }
}
