namespace RingSoft.DataEntryControls.NorthwindApp.Library.Model
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public byte LineType { get; set; }
        public string RowId { get; set; }
        public string ParentRowId { get; set; }
        public int? ProductId { get; set; }
        public int? NonInventoryCodeId { get; set; }
        public string SpecialOrderText { get; set; }
        public string Comment { get; set; }
        public bool CommentCrLf { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Quantity { get; set; }
        public float? Discount { get; set; }

        public virtual NonInventoryCodes NonInventoryCode { get; set; }
        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
    }
}
