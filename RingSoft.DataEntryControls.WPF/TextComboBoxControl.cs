// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 09-19-2023
// ***********************************************************************
// <copyright file="TextComboBoxControl.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// A combo box which displays text values.
    /// Implements the <see cref="ComboBox" />
    /// </summary>
    /// <seealso cref="ComboBox" />
    public class TextComboBoxControl : ComboBox
    {
        /// <summary>
        /// The rs selected item property
        /// </summary>
        public new static readonly DependencyProperty RSSelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(TextComboBoxItem), typeof(TextComboBoxControl),
                new FrameworkPropertyMetadata(SelectedItemPropertyChangedCallback));

        /// <summary>
        /// Gets or sets the selected item.  This is a bind-able property.
        /// </summary>
        /// <value>The selected item.</value>
        public new TextComboBoxItem SelectedItem
        {
            get { return (TextComboBoxItem)GetValue(RSSelectedItemProperty); }
            set { SetValue(RSSelectedItemProperty, value); }
        }

        /// <summary>
        /// Selectes the item property changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SelectedItemPropertyChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var comboBoxControl = (TextComboBoxControl)obj;
            comboBoxControl.SetSelectedItem();
        }

        /// <summary>
        /// The setup property.
        /// </summary>
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register(nameof(Setup), typeof(TextComboBoxControlSetup), typeof(TextComboBoxControl),
                new FrameworkPropertyMetadata(SetupPropertyChangedCallback));

        /// <summary>
        /// Gets or sets the setup.  This is a bind-able property.
        /// </summary>
        /// <value>The setup.</value>
        public TextComboBoxControlSetup Setup
        {
            get { return (TextComboBoxControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        /// <summary>
        /// Setups the property changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SetupPropertyChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var comboBoxControl = (TextComboBoxControl)obj;
            comboBoxControl.DoSetup();
        }

        /// <summary>
        /// Coerces the items source property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns>System.Object.</returns>
        private static object CoerceItemsSourceProperty(DependencyObject obj, object baseValue)
        {
            var comboBoxControl = (TextComboBoxControl)obj;
            if (!comboBoxControl.IsDesignMode())
                return baseValue;

            return comboBoxControl.GetItemsSource();
        }

        /// <summary>
        /// Coerces the item template property.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns>System.Object.</returns>
        private static object CoerceItemTemplateProperty(DependencyObject obj, object baseValue)
        {
            var comboBoxControl = (TextComboBoxControl)obj;
            if (!comboBoxControl.IsDesignMode())
                return baseValue;

            return null;
        }

        /// <summary>
        /// The UI command property
        /// </summary>
        public static readonly DependencyProperty UiCommandProperty =
            DependencyProperty.Register(nameof(UiCommand), typeof(UiCommand), typeof(TextComboBoxControl),
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
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void UiCommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var textComboBoxControl = (TextComboBoxControl)obj;
            if (textComboBoxControl._vmUiControl == null)
            {
                textComboBoxControl._vmUiControl = WPFControlsGlobals.VmUiFactory.CreateUiControl(
                    textComboBoxControl, textComboBoxControl.UiCommand);
                if (textComboBoxControl.UiLabel != null)
                {
                    textComboBoxControl._vmUiControl.SetLabel(textComboBoxControl.UiLabel);
                }
            }
        }

        /// <summary>
        /// The UI label property
        /// </summary>
        public static readonly DependencyProperty UiLabelProperty =
            DependencyProperty.Register(nameof(UiLabel), typeof(Label), typeof(TextComboBoxControl),
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
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void UiLabelChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var customContentControl = (TextComboBoxControl)obj;
            if (customContentControl._vmUiControl != null)
                customContentControl._vmUiControl.SetLabel(customContentControl.UiLabel);
        }


        /// <summary>
        /// The design text
        /// </summary>
        private string _designText;

        /// <summary>
        /// Gets or sets the design text.
        /// </summary>
        /// <value>The design text.</value>
        public string DesignText
        {
            get => _designText;
            set
            {
                _designText = value;
                SetDesignText();
            }
        }

        /// <summary>
        /// Gets or sets the text box.
        /// </summary>
        /// <value>The text box.</value>
        public TextBox TextBox { get; set; }

        /// <summary>
        /// The designer list
        /// </summary>
        private ObservableCollection<string> _designerList = new ObservableCollection<string>();
        /// <summary>
        /// The selected ComboBox item
        /// </summary>
        private TextComboBoxItem _selectedComboBoxItem;
        /// <summary>
        /// The height
        /// </summary>
        private double _height;
        /// <summary>
        /// The set focus
        /// </summary>
        private bool _setFocus;
        /// <summary>
        /// The vm UI control
        /// </summary>
        private VmUiControl _vmUiControl;

        /// <summary>
        /// Initializes static members of the <see cref="TextComboBoxControl"/> class.
        /// </summary>
        static TextComboBoxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextComboBoxControl),
                new FrameworkPropertyMetadata(typeof(TextComboBoxControl)));

            ItemsSourceProperty.OverrideMetadata(typeof(TextComboBoxControl), 
                new FrameworkPropertyMetadata(null, CoerceItemsSourceProperty));

            ItemTemplateProperty.OverrideMetadata(typeof(TextComboBoxControl),
                new FrameworkPropertyMetadata(null, CoerceItemTemplateProperty));
        }

        //public ComboBoxControl()
        //{
        //    SetResourceReference(StyleProperty, typeof(ComboBox));
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="TextComboBoxControl"/> class.
        /// </summary>
        public TextComboBoxControl()
        {
            Loaded += (sender, args) =>
            {
                UpdateLayout(); 
                _height  = ActualHeight;
                if (IsFocused)
                {
                    TextComboBoxControl_GotFocus(this, new RoutedEventArgs());
                }
                GotFocus += TextComboBoxControl_GotFocus;
                LostFocus += (o, eventArgs) =>
                {
                    var border = this.GetVisualChild<Border>();
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = new SolidColorBrush(Colors.Transparent);
                };
                DropDownOpened += (o, eventArgs) =>
                {
                    if (SelectedItem == null)
                    {
                        if (!IsEditable)
                        {
                            SelectedItem = Setup?.Items.FirstOrDefault();
                        }
                    }
                };
            };
        }

        /// <summary>
        /// Handles the GotFocus event of the TextComboBoxControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextComboBoxControl_GotFocus(object sender, RoutedEventArgs e)
        {
            //var border = this.GetVisualChild<Border>();
            //border.BorderThickness = new Thickness(2);
            //border.BorderBrush = new SolidColorBrush(Colors.Blue);
            //Height = _height + 5;
            //UpdateLayout();
            if (TextBox == null)
            {
                _setFocus = true;
            }
            else
            {
                TextBox.Focus();
            }

        }

        /// <summary>
        /// Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> is called.
        /// </summary>
        public override void OnApplyTemplate()
        {
            TextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
            var selBox = GetTemplateChild("SelectionBoxItem");

            if (_setFocus)
            {
                _setFocus = false;
                TextBox.Focus();
            }
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Sets the design text.
        /// </summary>
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
                    base.SelectedItem = DesignText;
                    ItemsSource = _designerList;
                }
            }
        }

        /// <summary>
        /// Does the setup.
        /// </summary>
        private void DoSetup()
        {
            if (Setup != null)
            {
                ItemsSource = Setup.Items;
            }

            if (_selectedComboBoxItem != null)
                SelectedItem = _selectedComboBoxItem;

            _selectedComboBoxItem = null;
        }

        /// <summary>
        /// Gets the items source.
        /// </summary>
        /// <returns>IEnumerable.</returns>
        private IEnumerable GetItemsSource()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return _designerList;
            }

            return ItemsSource;
        }

        /// <summary>
        /// Sets the selected item.
        /// </summary>
        private void SetSelectedItem()
        {
            if (Setup == null)
            {
                _selectedComboBoxItem = SelectedItem;
            }
            else
            {
                base.SelectedItem = SelectedItem;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:SelectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (base.SelectedItem is TextComboBoxItem comboBoxItem)
                SelectedItem = comboBoxItem;

            base.OnSelectionChanged(e);
        }
    }
}
