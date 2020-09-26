using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.NorthwindApp.Library.LookupModel;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class NorthwindLookupContext : LookupContext
    {
        public override DbDataProcessor DataProcessor => _dataProcessor;
        protected override DbContext DbContext { get; }

        public virtual TableDefinition<Categories> Categories { get; set; }
        public virtual TableDefinition<Customers> Customers { get; set; }
        public virtual TableDefinition<Employees> Employees { get; set; }
        public virtual TableDefinition<NonInventoryCodes> NonInventoryCodes { get; set; }
        public virtual TableDefinition<OrderDetails> OrderDetails { get; set; }
        public virtual TableDefinition<Orders> Orders { get; set; }
        public virtual TableDefinition<Products> Products { get; set; }
        public virtual TableDefinition<PurchaseDetails> PurchaseDetails { get; set; }
        public virtual TableDefinition<Purchases> Purchases { get; set; }
        public virtual TableDefinition<Shippers> Shippers { get; set; }
        public virtual TableDefinition<Suppliers> Suppliers { get; set; }

        public LookupDefinition<OrderLookup, Orders> OrdersLookup { get; private set; }

        public LookupDefinition<ProductLookup, Products> ProductsLookup { get; private set; }

        public LookupDefinition<CustomerLookup, Customers> CustomerIdLookup { get; private set; }

        public LookupDefinition<EmployeeLookup, Employees> EmployeesLookup { get; private set; }

        public LookupDefinition<ShipperLookup, Shippers> ShippersLookup { get; private set; }

        public LookupDefinition<SupplierLookup, Suppliers> SuppliersLookup { get; private set; }

        public LookupDefinition<CategoryLookup, Categories> CategoriesLookup { get; private set; }


        private readonly SqliteDataProcessor _dataProcessor;

        public NorthwindLookupContext()
        {
            _dataProcessor = new SqliteDataProcessor()
            {
                FilePath = AppGlobals.AppDataDirectory,
                FileName = "RSDEC_Northwind.sqlite"
            };
            DbContext = new NorthwindDbContext(this);

            Initialize();
        }
        protected override void InitializeLookupDefinitions()
        {
            var orderEmployeeNameFormula = "[Orders_Employees_EmployeeID].[FirstName] || ' ' || [Orders_Employees_EmployeeID].[LastName]";
            var employeeNameFormula = "[Employees].[FirstName] || ' ' || [Employees].[LastName]";
            var employeeSupervisorFormula = "[Employees_Employees_ReportsTo].[FirstName] || ' ' || [Employees_Employees_ReportsTo].[LastName]";

            OrdersLookup = new LookupDefinition<OrderLookup, Orders>(Orders);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderId, "Order ID", p => p.OrderId, 15);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderDate, "Date", p => p.OrderDate, 20);
            OrdersLookup.Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer, "Customer", p => p.CompanyName, 40);
            OrdersLookup.Include(p => p.Employee);
            OrdersLookup.AddVisibleColumnDefinition(p => p.Employee, "Employee", orderEmployeeNameFormula, 25);

            Orders.HasLookupDefinition(OrdersLookup);

            ProductsLookup = new LookupDefinition<ProductLookup, Products>(Products);
            ProductsLookup.AddVisibleColumnDefinition(p => p.ProductName, "Name", p => p.ProductName, 40);
            ProductsLookup.Include(p => p.Category)
                .AddVisibleColumnDefinition(p => p.Category, "Category", p => p.CategoryName, 20);
            ProductsLookup.AddVisibleColumnDefinition(p => p.UnitsInStock, "Quantity On Hand", p => p.UnitsInStock, 20);
            ProductsLookup.AddVisibleColumnDefinition(p => p.UnitPrice, "Price", p => p.UnitPrice, 20);

            Products.HasLookupDefinition(ProductsLookup);

            CustomerIdLookup = new LookupDefinition<CustomerLookup, Customers>(Customers);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.CustomerId, "Customer Id", p => p.CustomerId, 20);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 40);
            CustomerIdLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);

            Customers.HasLookupDefinition(CustomerIdLookup);

            EmployeesLookup = new LookupDefinition<EmployeeLookup, Employees>(Employees);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Name, "Name", employeeNameFormula, 40);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Title, "Title", p => p.Title, 20);
            EmployeesLookup.Include(p => p.Supervisor);
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Supervisor, "Supervisor", employeeSupervisorFormula, 40);

            Employees.HasLookupDefinition(EmployeesLookup);

            ShippersLookup = new LookupDefinition<ShipperLookup, Shippers>(Shippers);
            ShippersLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 75);
            ShippersLookup.AddVisibleColumnDefinition(p => p.Phone, "Phone", p => p.Phone, 25);

            Shippers.HasLookupDefinition(ShippersLookup);

            SuppliersLookup = new LookupDefinition<SupplierLookup, Suppliers>(Suppliers);
            SuppliersLookup.AddVisibleColumnDefinition(p => p.CompanyName, "Company Name", p => p.CompanyName, 60);
            SuppliersLookup.AddVisibleColumnDefinition(p => p.ContactName, "Contact", p => p.ContactName, 40);
            Suppliers.HasLookupDefinition(SuppliersLookup);

            CategoriesLookup = new LookupDefinition<CategoryLookup, Categories>(Categories);
            CategoriesLookup.AddVisibleColumnDefinition(p => p.CategoryName, "Category Name", p => p.CategoryName, 40);
            CategoriesLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description, 60);
            Categories.HasLookupDefinition(CategoriesLookup);
        }

        protected override void SetupModel()
        {
            Products.GetFieldDefinition(p => p.UnitPrice).HasDecimalFieldType(DecimalFieldTypes.Currency);
        }

    }
}
