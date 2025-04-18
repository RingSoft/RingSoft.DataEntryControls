﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.DataEntryControls.NorthwindApp.Library.Model
{
    public class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public double? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public string Notes { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Employees Employee { get; set; }
        public virtual Shippers Shipper { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
