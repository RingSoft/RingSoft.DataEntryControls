// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 08-18-2023
// ***********************************************************************
// <copyright file="Calculator.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF.DropDownEditControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class Calculator.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="IDropDownCalculator" />
    /// Implements the <see cref="ICalculatorControl" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="IDropDownCalculator" />
    /// <seealso cref="ICalculatorControl" />
    /// <font color="red">Badly formed XML comment.</font>

    [TemplatePart(Name = "MemoryStatusTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "EquationTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "EntryTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "McButton", Type = typeof(Button))]
    [TemplatePart(Name = "MrButton", Type = typeof(Button))]
    [TemplatePart(Name = "MAddButton", Type = typeof(Button))]
    [TemplatePart(Name = "MSubtractButton", Type = typeof(Button))]
    [TemplatePart(Name = "MsButton", Type = typeof(Button))]
    [TemplatePart(Name = "CeButton", Type = typeof(Button))]
    [TemplatePart(Name = "ClrButton", Type = typeof(Button))]
    [TemplatePart(Name = "BackButton", Type = typeof(Button))]
    [TemplatePart(Name = "DivideButton", Type = typeof(Button))]
    [TemplatePart(Name = "Button7", Type = typeof(Button))]
    [TemplatePart(Name = "Button8", Type = typeof(Button))]
    [TemplatePart(Name = "Button9", Type = typeof(Button))]
    [TemplatePart(Name = "MultiplyButton", Type = typeof(Button))]
    [TemplatePart(Name = "Button4", Type = typeof(Button))]
    [TemplatePart(Name = "Button5", Type = typeof(Button))]
    [TemplatePart(Name = "Button6", Type = typeof(Button))]
    [TemplatePart(Name = "SubtractButton", Type = typeof(Button))]
    [TemplatePart(Name = "Button1", Type = typeof(Button))]
    [TemplatePart(Name = "Button2", Type = typeof(Button))]
    [TemplatePart(Name = "Button3", Type = typeof(Button))]
    [TemplatePart(Name = "AdditionButton", Type = typeof(Button))]
    [TemplatePart(Name = "PlusMinusButton", Type = typeof(Button))]
    [TemplatePart(Name = "Button0", Type = typeof(Button))]
    [TemplatePart(Name = "DecimalButton", Type = typeof(Button))]
    [TemplatePart(Name = "EqualsButton", Type = typeof(Button))]
    public class Calculator : Control, IDropDownCalculator, ICalculatorControl
    {
        #region Controls

        /// <summary>
        /// Gets or sets the memory status text block.
        /// </summary>
        /// <value>The memory status text block.</value>
        public TextBlock MemoryStatusTextBlock { get; set; }
        /// <summary>
        /// Gets or sets the equation text block.
        /// </summary>
        /// <value>The equation text block.</value>
        public TextBlock EquationTextBlock { get; set; }
        /// <summary>
        /// Gets or sets the entry text block.
        /// </summary>
        /// <value>The entry text block.</value>
        public TextBlock EntryTextBlock { get; set; }

        /// <summary>
        /// The mc button
        /// </summary>
        private Button _mcButton;

        /// <summary>
        /// Gets or sets the mc button.
        /// </summary>
        /// <value>The mc button.</value>
        public Button McButton
        {
            get => _mcButton;
            set
            {
                if (_mcButton != null)
                    _mcButton.Click -= _mcButton_Click;

                _mcButton = value;

                if (_mcButton != null)
                    _mcButton.Click += _mcButton_Click;
            }
        }

        /// <summary>
        /// The mr button
        /// </summary>
        private Button _mrButton;

        /// <summary>
        /// Gets or sets the mr button.
        /// </summary>
        /// <value>The mr button.</value>
        public Button MrButton
        {
            get => _mrButton;
            set
            {
                if (_mrButton != null)
                    _mrButton.Click -= _mrButton_Click;

                _mrButton = value;

                if (_mrButton != null)
                    _mrButton.Click += _mrButton_Click;
            }
        }

        /// <summary>
        /// The m add button
        /// </summary>
        private Button _mAddButton;

        /// <summary>
        /// Gets or sets the m add button.
        /// </summary>
        /// <value>The m add button.</value>
        public Button MAddButton
        {
            get => _mAddButton;
            set
            {
                if (_mAddButton != null)
                    _mAddButton.Click -= _mAddButton_Click;

                _mAddButton = value;

                if (_mAddButton != null)
                    _mAddButton.Click += _mAddButton_Click;
            }
        }

        /// <summary>
        /// The m subtract button
        /// </summary>
        private Button _mSubtractButton;

        /// <summary>
        /// Gets or sets the m subtract button.
        /// </summary>
        /// <value>The m subtract button.</value>
        public Button MSubtractButton
        {
            get => _mSubtractButton;
            set
            {
                if (_mSubtractButton != null)
                    _mSubtractButton.Click -= _mSubtractButton_Click;

                _mSubtractButton = value;

                if (_mSubtractButton != null)
                    _mSubtractButton.Click += _mSubtractButton_Click;
            }
        }

        /// <summary>
        /// The ms button
        /// </summary>
        private Button _msButton;

        /// <summary>
        /// Gets or sets the ms button.
        /// </summary>
        /// <value>The ms button.</value>
        public Button MsButton
        {
            get => _msButton;
            set
            {
                if (_msButton != null)
                    _msButton.Click -= _msButton_Click;

                _msButton = value;
                
                if (_msButton != null)
                    _msButton.Click += _msButton_Click;
            }
        }

        /// <summary>
        /// The plus minus button
        /// </summary>
        private Button _plusMinusButton;

        /// <summary>
        /// Gets or sets the plus minus button.
        /// </summary>
        /// <value>The plus minus button.</value>
        public Button PlusMinusButton
        {
            get => _plusMinusButton;
            set
            {
                if (_plusMinusButton != null)
                    _plusMinusButton.Click -= _plusMinusButton_Click;

                _plusMinusButton = value;

                if (_plusMinusButton != null)
                    _plusMinusButton.Click += _plusMinusButton_Click;
            }
        }

        /// <summary>
        /// The equals button
        /// </summary>
        private Button _equalsButton;

        /// <summary>
        /// Gets or sets the equals button.
        /// </summary>
        /// <value>The equals button.</value>
        public Button EqualsButton
        {
            get => _equalsButton;
            set
            {
                if (_equalsButton != null)
                    _equalsButton.Click -= _equalsButton_Click;

                _equalsButton = value;

                if (_equalsButton != null)
                    _equalsButton.Click += _equalsButton_Click;
            }
        }

        /// <summary>
        /// The ce button
        /// </summary>
        private Button _ceButton;

        /// <summary>
        /// Gets or sets the ce button.
        /// </summary>
        /// <value>The ce button.</value>
        public Button CeButton
        {
            get => _ceButton;
            set
            {
                if (_ceButton != null)
                    _ceButton.Click -= _ceButton_Click;

                _ceButton = value;
                
                if (_ceButton != null)
                    _ceButton.Click += _ceButton_Click;
            }
        }

        /// <summary>
        /// The color button
        /// </summary>
        private Button _clrButton;

        /// <summary>
        /// Gets or sets the color button.
        /// </summary>
        /// <value>The color button.</value>
        public Button ClrButton
        {
            get => _clrButton;
            set
            {
                if (_clrButton != null)
                    _clrButton.Click -= _clrButton_Click;

                _clrButton = value;

                if (_clrButton != null)
                    _clrButton.Click += _clrButton_Click;
            }
        }

        /// <summary>
        /// The back button
        /// </summary>
        private Button _backButton;

        /// <summary>
        /// Gets or sets the back button.
        /// </summary>
        /// <value>The back button.</value>
        public Button BackButton
        {
            get => _backButton;
            set
            {
                if (_backButton != null)
                    _backButton.Click -= _backButton_Click;

                _backButton = value;

                if (_backButton != null)
                    _backButton.Click += _backButton_Click;
            }
        }

        /// <summary>
        /// The divide button
        /// </summary>
        private Button _divideButton;

        /// <summary>
        /// Gets or sets the divide button.
        /// </summary>
        /// <value>The divide button.</value>
        public Button DivideButton
        {
            get => _divideButton;
            set
            {
                if (_divideButton != null)
                    _divideButton.Click -= _divideButton_Click;

                _divideButton = value;

                if (_divideButton != null)
                    _divideButton.Click += _divideButton_Click;
            }
        }

        /// <summary>
        /// The button7
        /// </summary>
        private Button _button7;

        /// <summary>
        /// Gets or sets the button7.
        /// </summary>
        /// <value>The button7.</value>
        public Button Button7
        {
            get => _button7;
            set
            {
                if (_button7 != null)
                    _button7.Click -= _button7_Click;

                _button7 = value;
                
                if (_button7 != null)
                    _button7.Click += _button7_Click;
            }
        }

        /// <summary>
        /// The button8
        /// </summary>
        private Button _button8;
        /// <summary>
        /// Gets or sets the button8.
        /// </summary>
        /// <value>The button8.</value>
        public Button Button8
        {
            get => _button8;
            set
            {
                if (_button8 != null)
                    _button8.Click -= _button8_Click;

                _button8 = value;

                if (_button8 != null)
                    _button8.Click += _button8_Click;
            }
        }

        /// <summary>
        /// The button9
        /// </summary>
        private Button _button9;
        /// <summary>
        /// Gets or sets the button9.
        /// </summary>
        /// <value>The button9.</value>
        public Button Button9
        {
            get => _button9;
            set
            {
                if (_button9 != null)
                    _button9.Click -= _button9_Click;

                _button9 = value;

                if (_button9 != null)
                    _button9.Click += _button9_Click;
            }
        }

        /// <summary>
        /// The multiply button
        /// </summary>
        private Button _multiplyButton;

        /// <summary>
        /// Gets or sets the multiply button.
        /// </summary>
        /// <value>The multiply button.</value>
        public Button MultiplyButton
        {
            get => _multiplyButton;
            set
            {
                if (_multiplyButton != null)
                    _multiplyButton.Click -= _multiplyButton_Click;

                _multiplyButton = value;

                if (_multiplyButton != null)
                    _multiplyButton.Click += _multiplyButton_Click;
            }
        }

        /// <summary>
        /// The button4
        /// </summary>
        private Button _button4;
        /// <summary>
        /// Gets or sets the button4.
        /// </summary>
        /// <value>The button4.</value>
        public Button Button4
        {
            get => _button4;
            set
            {
                if (_button4 != null)
                    _button4.Click -= _button4_Click;

                _button4 = value;

                if (_button4 != null)
                    _button4.Click += _button4_Click;
            }
        }

        /// <summary>
        /// The button5
        /// </summary>
        private Button _button5;
        /// <summary>
        /// Gets or sets the button5.
        /// </summary>
        /// <value>The button5.</value>
        public Button Button5
        {
            get => _button5;
            set
            {
                if (_button5 != null)
                    _button5.Click -= _button5_Click;

                _button5 = value;

                if (_button5 != null)
                    _button5.Click += _button5_Click;
            }
        }

        /// <summary>
        /// The button6
        /// </summary>
        private Button _button6;
        /// <summary>
        /// Gets or sets the button6.
        /// </summary>
        /// <value>The button6.</value>
        public Button Button6
        {
            get => _button6;
            set
            {
                if (_button6 != null)
                    _button6.Click -= _button6_Click;

                _button6 = value;

                if (_button6 != null)
                    _button6.Click += _button6_Click;
            }
        }

        /// <summary>
        /// The subtract button
        /// </summary>
        private Button _subtractButton;

        /// <summary>
        /// Gets or sets the subtract button.
        /// </summary>
        /// <value>The subtract button.</value>
        public Button SubtractButton
        {
            get => _subtractButton;
            set
            {
                if (_subtractButton != null)
                    _subtractButton.Click -= _subtractButton_Click;

                _subtractButton = value;
                
                if (_subtractButton != null)
                    _subtractButton.Click += _subtractButton_Click;
            }
        }

        /// <summary>
        /// The button1
        /// </summary>
        private Button _button1;
        /// <summary>
        /// Gets or sets the button1.
        /// </summary>
        /// <value>The button1.</value>
        public Button Button1
        {
            get => _button1;
            set
            {
                if (_button1 != null)
                    _button1.Click -= _button1_Click;

                _button1 = value;

                if (_button1 != null)
                    _button1.Click += _button1_Click;
            }
        }

        /// <summary>
        /// The button2
        /// </summary>
        private Button _button2;
        /// <summary>
        /// Gets or sets the button2.
        /// </summary>
        /// <value>The button2.</value>
        public Button Button2
        {
            get => _button2;
            set
            {
                if (_button2 != null)
                    _button2.Click -= _button2_Click;

                _button2 = value;

                if (_button2 != null)
                    _button2.Click += _button2_Click;
            }
        }

        /// <summary>
        /// The button3
        /// </summary>
        private Button _button3;
        /// <summary>
        /// Gets or sets the button3.
        /// </summary>
        /// <value>The button3.</value>
        public Button Button3
        {
            get => _button3;
            set
            {
                if (_button3 != null)
                    _button3.Click -= _button3_Click;

                _button3 = value;

                if (_button3 != null)
                    _button3.Click += _button3_Click;
            }
        }

        /// <summary>
        /// The addition button
        /// </summary>
        private Button _additionButton;

        /// <summary>
        /// Gets or sets the addition button.
        /// </summary>
        /// <value>The addition button.</value>
        public Button AdditionButton
        {
            get => _additionButton;
            set
            {
                if (_additionButton != null)
                    _additionButton.Click -= _additionButton_Click;

                _additionButton = value;

                if (_additionButton !=null)
                    _additionButton.Click += _additionButton_Click;
            }
        }

        /// <summary>
        /// The button0
        /// </summary>
        private Button _button0;

        /// <summary>
        /// Gets or sets the button0.
        /// </summary>
        /// <value>The button0.</value>
        public Button Button0
        {
            get => _button0;
            set
            {
                if (_button0 != null)
                    _button0.Click -= _button0_Click;

                _button0 = value;

                if (_button0 != null)
                    _button0.Click += _button0_Click;
            }
        }

        /// <summary>
        /// The decimal button
        /// </summary>
        private Button _decimalButton;

        /// <summary>
        /// Gets or sets the decimal button.
        /// </summary>
        /// <value>The decimal button.</value>
        public Button DecimalButton
        {
            get => _decimalButton;
            set
            {
                if (_decimalButton != null)
                    _decimalButton.Click -= _decimalButton_Click;

                _decimalButton = value;

                if (_decimalButton != null)
                    _decimalButton.Click += _decimalButton_Click;
            }
        }

        #endregion

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public Control Control => this;

        #region ICalculatorControl Interface

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double? Value
        {
            get => Processor.ComittedValue;
            set => Processor.ReinitializeValue(value);
        }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        /// <value>The precision.</value>
        public int Precision
        {
            get => Processor.Precision;
            set => Processor.Precision = value;
        }

        /// <summary>
        /// Gets or sets the equation text.
        /// </summary>
        /// <value>The equation text.</value>
        public string EquationText
        {
            get
            {
                if (EquationTextBlock == null)
                    return string.Empty;

                return EquationTextBlock.Text;
            }
            set
            {
                if (EquationTextBlock != null)
                    EquationTextBlock.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the entry text.
        /// </summary>
        /// <value>The entry text.</value>
        public string EntryText
        {
            get
            {
                if (EntryTextBlock == null)
                    return string.Empty;

                return EntryTextBlock.Text;
            }
            set
            {
                if (EntryTextBlock != null)
                    EntryTextBlock.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [memory recall enabled].
        /// </summary>
        /// <value><c>true</c> if [memory recall enabled]; otherwise, <c>false</c>.</value>
        public bool MemoryRecallEnabled
        {
            get
            {
                if (MrButton != null)
                    return MrButton.IsEnabled;

                return false;
            }
            set
            {
                if (MrButton != null)
                    MrButton.IsEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [memory clear enabled].
        /// </summary>
        /// <value><c>true</c> if [memory clear enabled]; otherwise, <c>false</c>.</value>
        public bool MemoryClearEnabled
        {
            get
            {
                if (McButton != null)
                    return McButton.IsEnabled;

                return false;
            }
            set
            {
                if (McButton != null)
                    McButton.IsEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [memory store enabled].
        /// </summary>
        /// <value><c>true</c> if [memory store enabled]; otherwise, <c>false</c>.</value>
        public bool MemoryStoreEnabled
        {
            get
            {
                if (MsButton != null)
                    return MsButton.IsEnabled;

                return false;
            }
            set
            {
                if (MsButton != null)
                    MsButton.IsEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [memory plus enabled].
        /// </summary>
        /// <value><c>true</c> if [memory plus enabled]; otherwise, <c>false</c>.</value>
        public bool MemoryPlusEnabled
        {
            get
            {
                if (MAddButton != null)
                    return MAddButton.IsEnabled;

                return false;
            }
            set
            {
                if (MAddButton != null)
                    MAddButton.IsEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [memory minus enabled].
        /// </summary>
        /// <value><c>true</c> if [memory minus enabled]; otherwise, <c>false</c>.</value>
        public bool MemoryMinusEnabled
        {
            get
            {
                if (MSubtractButton != null)
                    return MSubtractButton.IsEnabled;

                return false;
            }
            set
            {
                if (MSubtractButton != null)
                    MSubtractButton.IsEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [memory status visible].
        /// </summary>
        /// <value><c>true</c> if [memory status visible]; otherwise, <c>false</c>.</value>
        public bool MemoryStatusVisible
        {
            get
            {
                if (MemoryStatusTextBlock != null)
                    return MemoryStatusTextBlock.Visibility == Visibility.Visible;

                return false;
            }
            set
            {
                if (MemoryStatusTextBlock != null)
                {
                    if (value)
                        MemoryStatusTextBlock.Visibility = Visibility.Visible;
                    else
                        MemoryStatusTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <value>The processor.</value>
        protected CalculatorProcessor Processor { get; }

        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event RoutedPropertyChangedEventHandler<object> ValueChanged;

        /// <summary>
        /// Initializes static members of the <see cref="Calculator"/> class.
        /// </summary>
        static Calculator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calculator), new FrameworkPropertyMetadata(typeof(Calculator)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Calculator"/> class.
        /// </summary>
        public Calculator()
        {
            Processor = new CalculatorProcessor(this);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            MemoryStatusTextBlock = GetTemplateChild(nameof(MemoryStatusTextBlock)) as TextBlock;
            EquationTextBlock = GetTemplateChild(nameof(EquationTextBlock)) as TextBlock;
            EntryTextBlock = GetTemplateChild(nameof(EntryTextBlock)) as TextBlock;

            McButton = GetTemplateChild(nameof(McButton)) as Button;
            MrButton = GetTemplateChild(nameof(MrButton)) as Button;
            MAddButton = GetTemplateChild(nameof(MAddButton)) as Button;
            MSubtractButton = GetTemplateChild(nameof(MSubtractButton)) as Button;
            MsButton = GetTemplateChild(nameof(MsButton)) as Button;

            CeButton = GetTemplateChild(nameof(CeButton)) as Button;
            ClrButton = GetTemplateChild(nameof(ClrButton)) as Button;
            BackButton = GetTemplateChild(nameof(BackButton)) as Button;
            DivideButton = GetTemplateChild(nameof(DivideButton)) as Button;

            Button7 = GetTemplateChild(nameof(Button7)) as Button;
            Button8 = GetTemplateChild(nameof(Button8)) as Button;
            Button9 = GetTemplateChild(nameof(Button9)) as Button;
            MultiplyButton = GetTemplateChild(nameof(MultiplyButton)) as Button;

            Button4 = GetTemplateChild(nameof(Button4)) as Button;
            Button5 = GetTemplateChild(nameof(Button5)) as Button;
            Button6 = GetTemplateChild(nameof(Button6)) as Button;
            SubtractButton = GetTemplateChild(nameof(SubtractButton)) as Button;

            Button1 = GetTemplateChild(nameof(Button1)) as Button;
            Button2 = GetTemplateChild(nameof(Button2)) as Button;
            Button3 = GetTemplateChild(nameof(Button3)) as Button;
            AdditionButton = GetTemplateChild(nameof(AdditionButton)) as Button;
            
            PlusMinusButton = GetTemplateChild(nameof(PlusMinusButton)) as Button;
            Button0 = GetTemplateChild(nameof(Button0)) as Button;
            DecimalButton = GetTemplateChild(nameof(DecimalButton)) as Button;
            EqualsButton = GetTemplateChild(nameof(EqualsButton)) as Button;

            Processor.Initialize();

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var keyChar = e.Key.GetCharFromKey();
            if (!Processor.ProcessChar(keyChar))
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        Processor.ProcessChar('=');
                        e.Handled = true;
                        break;
                    case Key.Delete:
                        Processor.ProcessCeButton();
                        break;
                    case Key.Back:
                        Processor.ProcessBackspace();
                        break;
                }
            }
            base.OnPreviewKeyDown(e);
        }

        #region Button Click Events

        /// <summary>
        /// Handles the Click event of the _mcButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _mcButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemoryClear();
        }

        /// <summary>
        /// Handles the Click event of the _mrButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _mrButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemoryRecall();
        }

        /// <summary>
        /// Handles the Click event of the _mAddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _mAddButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemoryAdd();
        }

        /// <summary>
        /// Handles the Click event of the _mSubtractButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _mSubtractButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemorySubtract();
        }

        /// <summary>
        /// Handles the Click event of the _msButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _msButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessMemoryStore();
        }

        /// <summary>
        /// Handles the Click event of the _ceButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _ceButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessCeButton();
        }

        /// <summary>
        /// Handles the Click event of the _clrButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _clrButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessCButton();
        }

        /// <summary>
        /// Handles the Click event of the _backButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _backButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessBackspace();
        }

        /// <summary>
        /// Handles the Click event of the _divideButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _divideButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('/');
        }

        /// <summary>
        /// Handles the Click event of the _button7 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button7_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('7');
        }

        /// <summary>
        /// Handles the Click event of the _button8 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button8_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('8');
        }

        /// <summary>
        /// Handles the Click event of the _button9 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button9_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('9');
        }

        /// <summary>
        /// Handles the Click event of the _multiplyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _multiplyButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('*');
        }

        /// <summary>
        /// Handles the Click event of the _button4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button4_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('4');
        }

        /// <summary>
        /// Handles the Click event of the _button5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button5_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('5');
        }

        /// <summary>
        /// Handles the Click event of the _button6 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button6_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('6');
        }

        /// <summary>
        /// Handles the Click event of the _subtractButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _subtractButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('-');
        }

        /// <summary>
        /// Handles the Click event of the _button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button1_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('1');
        }

        /// <summary>
        /// Handles the Click event of the _button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button2_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('2');
        }

        /// <summary>
        /// Handles the Click event of the _button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button3_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('3');
        }

        /// <summary>
        /// Handles the Click event of the _additionButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _additionButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('+');
        }

        /// <summary>
        /// Handles the Click event of the _plusMinusButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _plusMinusButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessPlusMinusButton();
        }

        /// <summary>
        /// Handles the Click event of the _button0 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _button0_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('0');
        }

        /// <summary>
        /// Handles the Click event of the _decimalButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _decimalButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessDecimal();
        }

        /// <summary>
        /// Handles the Click event of the _equalsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void _equalsButton_Click(object sender, RoutedEventArgs e)
        {
            Processor.ProcessChar('=');
        }

        #endregion

        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public void OnValueChanged(double? oldValue, double? newValue)
        {
            ValueChanged?.Invoke(this, new RoutedPropertyChangedEventArgs<object>(oldValue, newValue));
        }
    }
}
