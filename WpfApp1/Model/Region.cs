using System;
using System.Collections.Generic;

namespace WpfApp1.Model
{
    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territories>();
        }

        public long RegionId { get; set; }
        public string RegionDescription { get; set; }

        public virtual ICollection<Territories> Territories { get; set; }
    }
}
