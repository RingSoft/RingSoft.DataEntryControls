using System.Collections.Generic;
using RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry;
using RingSoft.DataEntryControls.NorthwindApp.Library.ViewModels;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class NorthwindViewModelInput
    {
        public List<SalesEntryViewModel> SalesEntryViewModels { get; } = new List<SalesEntryViewModel>();

        public List<ProductViewModel> ProductViewModels { get; } = new List<ProductViewModel>();

        public ProductInput ProductInput { get; set; }
    }
}
