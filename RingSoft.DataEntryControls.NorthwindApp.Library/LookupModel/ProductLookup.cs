namespace RingSoft.DataEntryControls.NorthwindApp.Library.LookupModel
{
    public class ProductLookup
    {
        public string ProductName { get; set; }

        public double UnitPrice { get; set; }

        public short UnitsInStock { get; set; }

        public virtual string Category { get; set; }
    }
}
