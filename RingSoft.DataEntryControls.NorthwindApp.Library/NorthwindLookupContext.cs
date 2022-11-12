using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.NorthwindApp.Library.LookupModel;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class NorthwindLookupContext : LookupContext, IAdvancedFindLookupContext
    {
        public override DbDataProcessor DataProcessor => NorthwindDataProcessor;
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

        public LookupContextBase Context => this;
        public TableDefinition<RecordLock> RecordLocks { get; set; }
        public TableDefinition<AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        public LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }
        public LookupDefinition<RecordLockingLookup, RecordLock> RecordLockingLookup { get; set; }

        public LookupDefinition<OrderLookup, Orders> OrdersLookup { get; private set; }

        public LookupDefinition<ProductLookup, Products> ProductsLookup { get; private set; }

        public LookupDefinition<CustomerLookup, Customers> CustomerIdLookup { get; private set; }

        public LookupDefinition<EmployeeLookup, Employees> EmployeesLookup { get; private set; }

        public LookupDefinition<ShipperLookup, Shippers> ShippersLookup { get; private set; }

        public LookupDefinition<SupplierLookup, Suppliers> SuppliersLookup { get; private set; }

        public LookupDefinition<CategoryLookup, Categories> CategoriesLookup { get; private set; }

        public LookupDefinition<NonInventoryCodeLookup, NonInventoryCodes> NonInventoryCodesLookup { get; private set; }

        public LookupDefinition<PurchaseOrderLookup, Purchases> PurchaseOrderLookup { get; private set; }

        public SqliteDataProcessor NorthwindDataProcessor { get; }

        public NorthwindLookupContext()
        {
            NorthwindDataProcessor = new SqliteDataProcessor()
            {
                FilePath = AppGlobals.AppDataDirectory,
                FileName = "RSDEC_Northwind.sqlite"
            };
            DbContext = new NorthwindDbContext(this);
            SetAdvancedFind();
            Initialize();
        }
        protected override void InitializeLookupDefinitions()
        {
            //var orderEmployeeNameFormula = "[{Alias}].[FirstName] || ' ' || [{Alias}].[LastName]";
            var employeeNameFormula = "[{Alias}].[FirstName] || ' ' || [{Alias}].[LastName]";
            //var employeeSupervisorFormula = "[Employees_Employees_ReportsTo].[FirstName] || ' ' || [Employees_Employees_ReportsTo].[LastName]";

            OrdersLookup = new LookupDefinition<OrderLookup, Orders>(Orders);
            //OrdersLookup.AddVisibleColumnDefinition(p => p.OrderId, "Order ID", p => p.OrderId, 15);
            OrdersLookup.AddVisibleColumnDefinition(p => p.OrderDate, "Date", p => p.OrderDate, 20);
            OrdersLookup.Include(p => p.Customer)
                .AddVisibleColumnDefinition(p => p.Customer, "Customer", p => p.CompanyName, 50);
            OrdersLookup.Include(p => p.Employee)
                .AddVisibleColumnDefinition(p => p.Employee, "Employee", employeeNameFormula, 30, FieldDataTypes.String);

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
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Name, "Name", employeeNameFormula, 40, "");
            EmployeesLookup.AddVisibleColumnDefinition(p => p.Title, "Title", p => p.Title, 20);
            EmployeesLookup.Include(p => p.Supervisor)
                .AddVisibleColumnDefinition(p => p.Supervisor, "Supervisor", employeeNameFormula, 40, FieldDataTypes.String);

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

            NonInventoryCodesLookup = new LookupDefinition<NonInventoryCodeLookup, NonInventoryCodes>(NonInventoryCodes);
            NonInventoryCodesLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description,
                60);
            NonInventoryCodesLookup.AddVisibleColumnDefinition(p => p.Price, "Price", p => p.Price, 40);
            NonInventoryCodes.HasLookupDefinition(NonInventoryCodesLookup);

            PurchaseOrderLookup = new LookupDefinition<PurchaseOrderLookup, Purchases>(Purchases);
            PurchaseOrderLookup.AddVisibleColumnDefinition(p => p.PoNumber, "PO Number", p => p.PoNumber, 25);
            PurchaseOrderLookup.AddVisibleColumnDefinition(p => p.OrderDate, "Date", p => p.OrderDate, 25);
            PurchaseOrderLookup.Include(p => p.Supplier)
                .AddVisibleColumnDefinition(p => p.Supplier, "Supplier", p => p.CompanyName, 50);
            Purchases.HasLookupDefinition(PurchaseOrderLookup);
        }

        protected override void SetupModel()
        {
            Products.GetFieldDefinition(p => p.UnitPrice).HasDecimalFieldType(DecimalFieldTypes.Currency);
            Products.GetFieldDefinition(p => p.Notes).IsMemo();
            NonInventoryCodes.GetFieldDefinition(p => p.Price).HasDecimalFieldType(DecimalFieldTypes.Currency);
            Employees.GetFieldDefinition(p => p.ReportsTo).HasDescription("Supervisor")
                .DoesAllowRecursion(false);
            Orders.GetFieldDefinition(p => p.Notes).IsMemo();
            Purchases.GetFieldDefinition(p => p.Notes).IsMemo();
        }


        public void SetAdvancedFind()
        {
            SystemGlobals.AdvancedFindLookupContext = this;
            var configuration = new AdvancedFindLookupConfiguration(SystemGlobals.AdvancedFindLookupContext);
            configuration.InitializeModel();
            configuration.ConfigureLookups();

        }

    }
}
