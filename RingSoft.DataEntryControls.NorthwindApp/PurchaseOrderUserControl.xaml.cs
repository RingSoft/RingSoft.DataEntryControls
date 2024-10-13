using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrderUserControl.xaml
    /// </summary>
    public partial class PurchaseOrderUserControl : IPurchaseOrderView
    {
        public PurchaseOrderUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(PoNumberControl);

            PurchaseOrderViewModel.CheckDirtyMessageShown += (_, args) =>
            {
                if (args.Result == MessageButtons.Cancel)
                    DetailsGrid.Refocus();
            };

            var tableDefinition = PurchaseOrderViewModel.TableDefinition;
            AddressEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.Address).MaxLength;
            CityEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.City).MaxLength;
            RegionEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.Region).MaxLength;
            PostalCodeEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.PostalCode).MaxLength;
            CountryEdit.MaxLength = tableDefinition.GetFieldDefinition(p => p.Country).MaxLength;

            PoNumberControl.LostFocus += PoNumberControl_LostFocus;
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return PurchaseOrderViewModel;
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
            return "Purchase Order";
        }

        private void PoNumberControl_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Tab) && !PurchaseOrderViewModel.SupplierUiCommand.IsEnabled)
            {
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                {
                    OrderDateControl.Focus();
                }
            }
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
