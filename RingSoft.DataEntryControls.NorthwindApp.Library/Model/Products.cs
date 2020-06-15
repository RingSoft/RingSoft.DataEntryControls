using System.Collections.Generic;
// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.DataEntryControls.NorthwindApp.Library.Model
{
    public class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
            PurchaseDetails = new HashSet<PurchaseDetails>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? UnitsInStock { get; set; }
        public decimal? UnitsOnOrder { get; set; }
        public decimal? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public string OrderComment { get; set; }
        public string PurchaseComment { get; set; }
        public int? NonInventoryCodeId { get; set; }
        public byte UnitDecimals { get; set; }

        public virtual Categories Category { get; set; }
        public virtual NonInventoryCodes NonInventoryCode { get; set; }
        public virtual Suppliers Supplier { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<PurchaseDetails> PurchaseDetails { get; set; }
    }
}
