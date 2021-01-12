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

            Loaded += (_, _) => ViewModel.OnViewLoaded(this, invalidProductValue);
            AddProductButton.Click += (_, _) =>
            {
                if (ViewModel.AddNewProduct(this))
                    Close();
            };
            AddNonInventoryButton.Click += (_, _) =>
            {
                if (ViewModel.AddNewNonInventoryCode(this))
                    Close();
            };
            AddSpecialOrderButton.Click += (_, _) =>
            {
                ViewModel.AddNewSpecialOrder();
                Close();
            };
            AddCommentButton.Click += (_, _) =>
            {
                if (ViewModel.AddComment())
                    Close();
            };
            CancelButton.Click += (_, _) => Close();
        }

        public new InvalidProductResult ShowDialog()
        {
            base.ShowDialog();
            return ViewModel.Result;
        }

        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var gridMemoEditor = new DataEntryGridMemoEditor(comment);
            gridMemoEditor.Title = "Edit Comment";
            gridMemoEditor.Owner = this;
            gridMemoEditor.ShowInTaskbar = false;
            return gridMemoEditor.ShowDialog();
        }
    }
}
