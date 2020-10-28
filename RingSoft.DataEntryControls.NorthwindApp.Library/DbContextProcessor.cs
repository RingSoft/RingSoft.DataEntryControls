using Microsoft.EntityFrameworkCore;
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

        public Purchases GetPurchase(int purchaseOrderId)
        {
            var context = new NorthwindDbContext();
            return context.Purchases.Include(p => p.Supplier)
                .Include(p => p.PurchaseDetails)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(f => f.PurchaseOrderId == purchaseOrderId);
        }

        public bool SavePurchase(Purchases purchase, List<PurchaseDetails> purchaseDetails)
        {
            var context = new NorthwindDbContext();
            var result = context.SaveEntity(context.Purchases, purchase, "Saving Purchase");

            if (result)
            {
                context.PurchaseDetails.RemoveRange(
                    context.PurchaseDetails.Where(w => w.PurchaseOrderId == purchase.PurchaseOrderId));

                if (purchaseDetails != null)
                {
                    foreach (var purchaseOrderDetail in purchaseDetails)
                    {
                        purchaseOrderDetail.PurchaseOrderId = purchase.PurchaseOrderId;
                        context.PurchaseDetails.Add(purchaseOrderDetail);
                    }
                }
                result = context.SaveEfChanges("Saving Purchase Details");
            }
            return result;
        }

        public bool DeletePurchase(int purchaseOrderId)
        {
            var context = new NorthwindDbContext();
            var purchase = context.Purchases.FirstOrDefault(p => p.PurchaseOrderId == purchaseOrderId);

            context.PurchaseDetails.RemoveRange(
                context.PurchaseDetails.Where(w => w.PurchaseOrderId == purchase.PurchaseOrderId));
            return context.DeleteEntity(context.Purchases, purchase, "Deleting Purchase");
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

        public Products GetProduct(string productName)
        {
            var context = new NorthwindDbContext();
            var result = context.Products.Include(i => i.Supplier)
                .Include(i => i.Category)
                .Include(i => i.NonInventoryCode)
                .FirstOrDefault(f => f.ProductName == productName);
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

        public NonInventoryCodes GetNonInventoryCode(int nonInventoryCodeId)
        {
            var context = new NorthwindDbContext();
            return context.NonInventoryCodes.FirstOrDefault(f => f.NonInventoryCodeId == nonInventoryCodeId);
        }

        public NonInventoryCodes GetNonInventoryCode(string nonInventoryCodeDesc)
        {
            var context = new NorthwindDbContext();
            return context.NonInventoryCodes.FirstOrDefault(f => f.Description == nonInventoryCodeDesc);
        }

        public bool SaveNonInventoryCode(NonInventoryCodes nonInventoryCode)
        {
            var context = new NorthwindDbContext();
            return context.SaveEntity(context.NonInventoryCodes, nonInventoryCode, "Saving Non Inventory Code");
        }

        public bool DeleteNonInventoryCode(int nonInventoryCodeId)
        {
            var context = new NorthwindDbContext();
            var nonInventoryCode =
                context.NonInventoryCodes.FirstOrDefault(f => f.NonInventoryCodeId == nonInventoryCodeId);
            return context.DeleteEntity(context.NonInventoryCodes, nonInventoryCode, "Deleting Non InventoryCode");
        }

        public Suppliers GetSupplier(int supplierId)
        {
            var context = new NorthwindDbContext();
            return context.Suppliers.FirstOrDefault(f => f.SupplierId == supplierId);
        }
    }
}
