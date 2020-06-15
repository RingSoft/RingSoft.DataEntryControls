using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    /// <summary>
    /// Interaction logic for DataEntryGridMemoEditor.xaml
    /// </summary>
    public partial class DataEntryGridMemoEditor
    {
        private bool _dialogResult;

        public GridMemoValue GridMemoValue { get; }

        public DataEntryGridMemoEditor(GridMemoValue gridMemoValue)
        {
            GridMemoValue = gridMemoValue;

            InitializeComponent();

            MemoEditor.Text = gridMemoValue.Text;
            OkButton.Click += (sender, args) => OnOkButton();
            CancelButton.Click += (sender, args) => Close();
        }

        public new bool ShowDialog()
        {
            base.ShowDialog();
            return _dialogResult;
        }

        private void OnOkButton()
        {
            GridMemoValue.Text = MemoEditor.Text;
            _dialogResult = true;
            Close();
        }
    }
}
