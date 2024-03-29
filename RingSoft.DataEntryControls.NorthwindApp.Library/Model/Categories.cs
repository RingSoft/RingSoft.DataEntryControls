﻿using System.Collections.Generic;
// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.DataEntryControls.NorthwindApp.Library.Model
{
    public class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
