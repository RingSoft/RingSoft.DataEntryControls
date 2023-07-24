using System.Collections.Generic;
// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.DataEntryControls.NorthwindApp.Library.Model
{
    public class NonInventoryCodes
    {
        public NonInventoryCodes()
        {
            OrderDetails = new HashSet<OrderDetails>();
            Products = new HashSet<Products>();
        }

        public int NonInventoryCodeId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
