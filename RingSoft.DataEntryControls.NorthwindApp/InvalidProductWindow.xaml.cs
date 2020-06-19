using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for InvalidProductCorrectionWindow.xaml
    /// </summary>
    public partial class InvalidProductWindow : IInvalidProductView
    {
        public InvalidProductViewModel ViewModel { get; }
        public InvalidProductWindow(AutoFillValue invalidProductValue)
        {
            ViewModel = new InvalidProductViewModel();

            InitializeComponent();

            Loaded += (sender, args) => ViewModel.OnViewLoaded(this, invalidProductValue);
        }

        public new InvalidProductResult ShowDialog()
        {
            base.ShowDialog();
            return ViewModel.Result;
        }

        public bool ShowCommentEditor(GridMemoValue comment)
        {
            throw new System.NotImplementedException();
        }
    }
}
