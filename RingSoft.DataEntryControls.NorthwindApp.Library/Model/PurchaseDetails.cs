namespace RingSoft.DataEntryControls.NorthwindApp.Library.Model
{
    public class PurchaseDetails
    {
        public int PurchaseOrderId { get; set; }
        public int PurchaseDetailId { get; set; }
        public byte LineType { get; set; }
        public string RowId { get; set; }
        public string ParentRowId { get; set; }
        public bool Received { get; set; }
        public int? ProductId { get; set; }
        public string DirectExpenseText { get; set; }
        public string Comment { get; set; }
        public bool? CommentCrLf { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Products Product { get; set; }
        public virtual Purchases PurchaseOrder { get; set; }
    }
}
