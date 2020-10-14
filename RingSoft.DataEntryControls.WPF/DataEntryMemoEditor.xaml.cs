using System;
using System.Windows;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Interaction logic for DataEntryMemoEditor.xaml
    /// </summary>
    public partial class DataEntryMemoEditor
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(DataEntryMemoEditor),
            new FrameworkPropertyMetadata(TextChangedCallback));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void TextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var memoEditor = (DataEntryMemoEditor)obj;
            if (memoEditor._controlLoaded)
                memoEditor.SetText();
        }

        private bool _controlLoaded;

        public DataEntryMemoEditor()
        {
            InitializeComponent();

            Loaded += (sender, args) => OnLoad();
            TextBox.TextChanged += (sender, args) => Text = TextBox.Text;
            DateStampButton.Click += DateStampButton_Click;
        }

        private void DateStampButton_Click(object sender, RoutedEventArgs e)
        {
            var stamp = $"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")} - ";
            TextBox.Text = $"{stamp}\r\n{TextBox.Text}";
            TextBox.SelectionStart = TextBox.Text.IndexOf(stamp, StringComparison.Ordinal) + stamp.Length;
            TextBox.SelectionLength = 0;
        }

        private void SetText()
        {
            TextBox.Text = Text;
        }

        private void OnLoad()
        {
            if (!_controlLoaded)
            {
                SetText();
                TextBox.SelectAll();
                _controlLoaded = true;
            }
        }
    }
}
