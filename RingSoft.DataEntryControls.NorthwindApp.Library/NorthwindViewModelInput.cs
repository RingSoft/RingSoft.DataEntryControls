using System.Collections.Generic;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
using RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class ProductInput
    {
        public AutoFillValue LockSupplier { get; }

        public ProductInput(AutoFillValue lockSupplier)
        {
            LockSupplier = lockSupplier;
        }
    }

    public class NorthwindViewModelInput
    {
        public List<SalesEntryViewModel> SalesEntryViewModels { get; } = new List<SalesEntryViewModel>();

        public List<ProductViewModel> ProductViewModels { get; } = new List<ProductViewModel>();

        public ProductInput ProductInput { get; set; }
    }
}
