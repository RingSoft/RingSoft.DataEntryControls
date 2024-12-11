using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for SalesEntryUserControl.xaml
    /// </summary>
    public partial class SalesEntryUserControl : ISalesEntryMaintenanceView
    {
        public SalesEntryUserControl()
        {
            InitializeComponent();
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return SalesEntryViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return ButtonsControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Sale";
        }

        protected override void ShowRecordTitle()
        {
            if (SalesEntryViewModel.Customer != null)
            {
                Host.ChangeTitle($"{Title} - {SalesEntryViewModel.Customer.Text}");
                return;
            }
            base.ShowRecordTitle();
        }

        public override void SetInitialFocus()
        {
            SalesEntryViewModel.CustomerUiCommand.SetFocus();
            base.SetInitialFocus();
        }

        public InvalidProductResult CorrectInvalidProduct(AutoFillValue invalidProductValue)
        {
            var invalidProductWindow = new InvalidProductWindow(invalidProductValue);
            invalidProductWindow.Owner = OwnerWindow;
            return invalidProductWindow.ShowDialog();
        }

        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var memoEditor = new DataEntryGridMemoEditor(comment);
            memoEditor.Owner = OwnerWindow;
            memoEditor.Title = "Edit Comment";
            return memoEditor.ShowDialog();
        }
    }
}
