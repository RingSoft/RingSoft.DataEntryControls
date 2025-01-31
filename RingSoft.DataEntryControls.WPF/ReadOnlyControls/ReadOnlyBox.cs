// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="ReadOnlyBox.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A base control that displays values to the user.  User is not allowed to edit.
    /// Implements the <see cref="Control" />
    /// </summary>
    /// <seealso cref="Control" />
    [TemplatePart(Name = "TextBlock", Type = typeof(TextBlock))]
    public class ReadOnlyBox : Control
    {
        /// <summary>
        /// The design text property
        /// </summary>
        public static readonly DependencyProperty DesignTextProperty =
            DependencyProperty.Register(nameof(DesignText), typeof(string), typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(DesignTextChangedCallback));

        /// <summary>
        /// Gets or sets the design text.  This is a bind-able property.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get { return (string)GetValue(DesignTextProperty); }
            set { SetValue(DesignTextProperty, value); }
        }

        /// <summary>
        /// Designs the text changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void DesignTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (ReadOnlyBox)obj;
            readOnlyBox.SetDesignText();
        }

        /// <summary>
        /// The text alignment property
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(TextAlignmentChangedCallback));

        /// <summary>
        /// Gets or sets the text alignment.  This is a bind-able property.
        /// </summary>
        /// <value>The text alignment.</value>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Texts the alignment changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void TextAlignmentChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (ReadOnlyBox)obj;
            readOnlyBox.SetTextAlignment();
        }

        /// <summary>
        /// Backgrounds the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void BackgroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (ReadOnlyBox)obj;
            if (readOnlyBox.TextBlock != null)
            {
                readOnlyBox.TextBlock.Background = readOnlyBox.Background;
            }
        }

        /// <summary>
        /// Foregrounds the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ForegroundChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (ReadOnlyBox)obj;
            if (readOnlyBox.TextBlock != null)
            {
                readOnlyBox.TextBlock.Foreground = readOnlyBox.Foreground;
            }
        }

        /// <summary>
        /// The UI command property
        /// </summary>
        public static readonly DependencyProperty UiCommandProperty =
            DependencyProperty.Register(nameof(UiCommand), typeof(UiCommand), typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(UiCommandChangedCallback));

        /// <summary>
        /// Gets or sets the UI command.  This is a bind-able property.
        /// </summary>
        /// <value>The UI command.</value>
        public UiCommand UiCommand
        {
            get { return (UiCommand)GetValue(UiCommandProperty); }
            set { SetValue(UiCommandProperty, value); }
        }

        /// <summary>
        /// UIs the command changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void UiCommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var readOnlyBox = (ReadOnlyBox)obj;
            if (readOnlyBox._vmUiControl == null)
            {
                readOnlyBox._vmUiControl = WPFControlsGlobals.VmUiFactory.CreateUiControl(readOnlyBox, readOnlyBox.UiCommand);
                if (readOnlyBox.UiLabel != null)
                {
                    readOnlyBox._vmUiControl.SetLabel(readOnlyBox.UiLabel);
                }
            }
        }

        /// <summary>
        /// The UI label property
        /// </summary>
        public static readonly DependencyProperty UiLabelProperty =
            DependencyProperty.Register(nameof(UiLabel), typeof(Label), typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(UiLabelChangedCallback));

        /// <summary>
        /// Gets or sets the UI label.  This is a bind-able property.
        /// </summary>
        /// <value>The UI label.</value>
        public Label UiLabel
        {
            get { return (Label)GetValue(UiLabelProperty); }
            set { SetValue(UiLabelProperty, value); }
        }

        /// <summary>
        /// UIs the label changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void UiLabelChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dropDownEditControl = (ReadOnlyBox)obj;
            if (dropDownEditControl._vmUiControl != null)
                dropDownEditControl._vmUiControl.SetLabel(dropDownEditControl.UiLabel);
        }

        /// <summary>
        /// Gets or sets the text block.
        /// </summary>
        /// <value>The text block.</value>
        public TextBlock TextBlock { get; set; }

        /// <summary>
        /// The text
        /// </summary>
        private string _text;
        /// <summary>
        /// The vm UI control
        /// </summary>
        private VmUiControl _vmUiControl;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        protected string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                    return;

                _text = value;
                SetText();
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="ReadOnlyBox" /> class.
        /// </summary>
        static ReadOnlyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReadOnlyBox), new FrameworkPropertyMetadata(typeof(ReadOnlyBox)));

            BackgroundProperty.OverrideMetadata(typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(BackgroundChangedCallback));

            ForegroundProperty.OverrideMetadata(typeof(ReadOnlyBox),
                new FrameworkPropertyMetadata(ForegroundChangedCallback));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBox" /> class.
        /// </summary>
        public ReadOnlyBox()
        {
            IsTabStop = false;
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            TextBlock = GetTemplateChild(nameof(TextBlock)) as TextBlock;

            base.OnApplyTemplate();

            SetText();
            SetDesignText();
            SetTextAlignment();
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        private void SetText()
        {
            if (TextBlock != null)
            {
                if (DesignerProperties.GetIsInDesignMode(this) && !DesignText.IsNullOrEmpty())
                    return;

                TextBlock.Text = Text;
            }
        }

        /// <summary>
        /// Sets the design text.
        /// </summary>
        private void SetDesignText()
        {
            if (DesignerProperties.GetIsInDesignMode(this) && TextBlock != null && !DesignText.IsNullOrEmpty())
                TextBlock.Text = DesignText;
        }

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        private void SetTextAlignment()
        {
            if (TextBlock != null)
                TextBlock.TextAlignment = TextAlignment;
        }
    }
}
