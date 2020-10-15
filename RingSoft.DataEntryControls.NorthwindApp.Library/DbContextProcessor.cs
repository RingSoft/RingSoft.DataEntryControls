﻿using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup.EfCore;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class DbContextProcessor
    {
        public Customers GetCustomer(string customerId)
        {
            var context = new NorthwindDbContext();
            return context.Customers.FirstOrDefault(f => f.CustomerId == customerId);
        }

        public bool SaveCustomer(Customers customer)
        {
            using (var context = new NorthwindDbContext())
            {
                if (context.Customers.FirstOrDefault(f => f.CustomerId == customer.CustomerId) == null)
                    return context.AddNewEntity(context.Customers, customer, "Saving Customer");
            }

            using (var context = new NorthwindDbContext())
            {
                return context.SaveEntity(context.Customers, customer, "Saving Customer");
            }
        }

        public bool DeleteCustomer(string customerId)
        {
            var context = new NorthwindDbContext();
            var customer = context.Customers.FirstOrDefault(p => p.CustomerId == customerId);
            return context.DeleteEntity(context.Customers, customer, "Deleting Customer");
        }

        public Employees GetEmployee(int employeeId)
        {
            var context = new NorthwindDbContext();
            return context.Employees.Include(i => i.Supervisor).FirstOrDefault(f => f.EmployeeId == employeeId);
        }

        public bool SaveEmployee(Employees employee)
        {
            var context = new NorthwindDbContext();
            return context.SaveEntity(context.Employees, employee, "Saving Employee");
        }

        public bool DeleteEmployee(int employeeId)
        {
            var context = new NorthwindDbContext();
            var employee = context.Employees.FirstOrDefault(p => p.EmployeeId == employeeId);
            return context.DeleteEntity(context.Employees, employee, "Deleting Employee");
        }


        public Orders GetOrder(int orderId)
        {
            var context = new NorthwindDbContext();
            return context.Orders.Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.Shipper)
                .Include(i => i.OrderDetails)
                .ThenInclude(t => t.NonInventoryCode)
                .Include(i => i.OrderDetails)
                .ThenInclude(t => t.Product)
                .ThenInclude(t => t.NonInventoryCode)
                .FirstOrDefault(f => f.OrderId == orderId);
        }

        public bool SaveOrder(Orders order, List<OrderDetails> orderDetails)
        {
            var context = new NorthwindDbContext();
            var result = context.SaveEntity(context.Orders, order, "Saving Order");

            if (result)
            {
                context.OrderDetails.RemoveRange(context.OrderDetails.Where(w => w.OrderId == order.OrderId));

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderId = order.OrderId;
                    context.OrderDetails.Add(orderDetail);
                }
                result = context.SaveEfChanges("Saving Order Details");
            }
            return result;
        }

        public bool DeleteOrder(int orderId)
        {
            var context = new NorthwindDbContext();
            var order = context.Orders.FirstOrDefault(p => p.OrderId == orderId);

            context.OrderDetails.RemoveRange(context.OrderDetails.Where(w => w.OrderId == order.OrderId));
            return context.DeleteEntity(context.Orders, order, "Deleting Order");
        }
        public Products GetProduct(int productId)
        {
            var context = new NorthwindDbContext();
            var result = context.Products.Include(i => i.Supplier)
                .Include(i => i.Category)
                .Include(i => i.NonInventoryCode)
                .FirstOrDefault(f => f.ProductId == productId);
            return result;
        }

        public bool SaveProduct(Products product)
        {
            var context = new NorthwindDbContext();
            return context.SaveEntity(context.Products, product, "Saving Product");
        }

        public bool DeleteProduct(int productId)
        {
            var context = new NorthwindDbContext();
            var product = context.Products.FirstOrDefault(p => p.ProductId == productId);
            return context.DeleteEntity(context.Products, product, "Deleting Product");
        }
    }
}
