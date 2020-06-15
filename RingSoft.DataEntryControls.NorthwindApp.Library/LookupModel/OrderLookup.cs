using System;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.LookupModel
{
    public class OrderLookup
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Customer { get; set; }

        public string Employee { get; set; }
    }
}
