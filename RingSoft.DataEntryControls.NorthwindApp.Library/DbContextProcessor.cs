﻿using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup.EfCore;
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
                .FirstOrDefault(f => f.OrderId == orderId);
        }

        public bool SaveOrder(Orders order)
        {
            var context = new NorthwindDbContext();
            return context.SaveEntity(context.Orders, order, "Saving Order");
        }

        public bool DeleteOrder(int orderId)
        {
            var context = new NorthwindDbContext();
            var order = context.Orders.FirstOrDefault(p => p.OrderId == orderId);
            return context.DeleteEntity(context.Orders, order, "Deleting Order");
        }
        public Products GetProduct(int productId)
        {
            var context = new NorthwindDbContext();
            return context.Products.FirstOrDefault(f => f.ProductId == productId);
        }
    }
}