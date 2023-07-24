using System;
using System.Collections.Generic;
// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.DataEntryControls.NorthwindApp.Library.Model
{
    public class Purchases
    {
        public Purchases()
        {
            PurchaseDetails = new HashSet<PurchaseDetails>();
        }

        public int PurchaseOrderId { get; set; }
        public string PoNumber { get; set; }
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double Freight { get; set; }
        public string Notes { get; set; }

        public virtual Suppliers Supplier { get; set; }
        public virtual ICollection<PurchaseDetails> PurchaseDetails { get; set; }
    }
}
