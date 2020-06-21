using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for InvalidProductCorrectionWindow.xaml
    /// </summary>
    public partial class InvalidProductWindow : IInvalidProductView
    {
        public InvalidProductWindow(AutoFillValue invalidProductValue)
        {
            InitializeComponent();

            Loaded += (sender, args) => ViewModel.OnViewLoaded(this, invalidProductValue);
            AddProductButton.Click += (sender, args) =>
            {
                if (ViewModel.AddNewProduct())
                    Close();
            };
            AddNonInventoryButton.Click += (sender, args) =>
            {
                if (ViewModel.AddNewNonInventoryCode())
                    Close();
            };
            AddSpecialOrderButton.Click += (sender, args) =>
            {
                ViewModel.AddNewSpecialOrder();
                Close();
            };
            AddCommentButton.Click += (sender, args) =>
            {
                if (ViewModel.AddComment())
                    Close();
            };
            CancelButton.Click += (sender, args) => Close();
        }

        public new InvalidProductResult ShowDialog()
        {
            base.ShowDialog();
            return ViewModel.Result;
        }

        public bool ShowCommentEditor(GridMemoValue comment)
        {
            var gridMemoEditor = new DataEntryGridMemoEditor(comment);
            return gridMemoEditor.ShowDialog();
        }
    }
}
