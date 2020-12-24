using Microsoft.EntityFrameworkCore;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.Model
{
    public class NorthwindDbContext : DbContext
    {
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<NonInventoryCodes> NonInventoryCodes { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<PurchaseDetails> PurchaseDetails { get; set; }
        public virtual DbSet<Purchases> Purchases { get; set; }
        public virtual DbSet<Shippers> Shippers { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }

        private static NorthwindLookupContext _lookupContext;

        public NorthwindDbContext()
        {
        }

        internal NorthwindDbContext(NorthwindLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
        }

        public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_lookupContext.DataProcessor.ConnectionString);

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.Description).HasColumnType("text(1073741823)");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("char(5)")
                    .HasMaxLength(5);

                entity.Property(e => e.Address).HasColumnType("nvarchar(60)").HasMaxLength(60);

                entity.Property(e => e.City).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("nvarchar(40)")
                    .HasMaxLength(40);

                entity.Property(e => e.ContactName).HasColumnType("nvarchar(30)").HasMaxLength(30);

                entity.Property(e => e.ContactTitle).HasColumnType("nvarchar(30)").HasMaxLength(30);

                entity.Property(e => e.Country).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.Fax).HasColumnType("nvarchar(24)").HasMaxLength(24);

                entity.Property(e => e.Phone).HasColumnType("nvarchar(24)").HasMaxLength(24);

                entity.Property(e => e.PostalCode).HasColumnType("nvarchar(10)").HasMaxLength(10);

                entity.Property(e => e.Region).HasColumnType("nvarchar(15)").HasMaxLength(15);
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.Address).HasColumnType("nvarchar(60)").HasMaxLength(60);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.City).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.Country).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.Extension).HasColumnType("nvarchar(4)").HasMaxLength(4);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("nvarchar(10)")
                    .HasMaxLength(10);

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.HomePhone).HasColumnType("nvarchar(24)").HasMaxLength(24);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("nvarchar(20)")
                    .HasMaxLength(20);

                entity.Property(e => e.Notes).HasColumnType("text(1073741823)");

                entity.Property(e => e.PhotoPath).HasColumnType("nvarchar(255)");

                entity.Property(e => e.PostalCode).HasColumnType("nvarchar(10)").HasMaxLength(10);

                entity.Property(e => e.Region).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.Title).HasColumnType("nvarchar(30)").HasMaxLength(30);

                entity.Property(e => e.TitleOfCourtesy).HasColumnType("nvarchar(25)").HasMaxLength(25);

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.Underlings)
                    .HasForeignKey(d => d.ReportsTo);
            });

            modelBuilder.Entity<NonInventoryCodes>(entity =>
            {
                entity.HasKey(e => e.NonInventoryCodeId);

                entity.Property(e => e.NonInventoryCodeId)
                    .HasColumnName("NonInventoryCodeID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.OrderDetailId });

                entity.ToTable("Order Details");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.Comment).HasColumnType("nvarchar(50)").HasMaxLength(50);

                entity.Property(e => e.CommentCrLf)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(e => e.LineType).HasColumnType("smallint");

                entity.Property(e => e.NonInventoryCodeId).HasColumnName("NonInventoryCodeID");

                entity.Property(e => e.ParentRowId)
                    .HasColumnName("ParentRowID")
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasColumnType("numeric");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.SpecialOrderText).HasColumnType("nvarchar(50)").HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("numeric");

                entity.HasOne(d => d.NonInventoryCode)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.NonInventoryCodeId);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("char(5)")
                    .HasMaxLength(5);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Freight)
                    .HasColumnType("numeric")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Notes).HasColumnType("text(1073741823)");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.ShipAddress).HasColumnType("nvarchar(60)").HasMaxLength(60);

                entity.Property(e => e.ShipCity).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.ShipCountry).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.ShipName).HasColumnType("nvarchar(40)").HasMaxLength(40);

                entity.Property(e => e.ShipPostalCode).HasColumnType("nvarchar(10)").HasMaxLength(10);

                entity.Property(e => e.ShipRegion).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId);

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipVia);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Discontinued)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NonInventoryCodeId).HasColumnName("NonInventoryCodeID");

                entity.Property(e => e.Notes).HasColumnType("text(1073741823)");

                entity.Property(e => e.OrderComment).HasColumnType("text(1073741823)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("nvarchar(40)")
                    .HasMaxLength(40);

                entity.Property(e => e.PurchaseComment).HasColumnType("text(1073741823)");

                entity.Property(e => e.QuantityPerUnit).HasColumnType("nvarchar(20)").HasMaxLength(20);

                entity.Property(e => e.ReorderLevel)
                    .HasColumnType("numeric")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.UnitDecimals).HasColumnType("smallint");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("numeric")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UnitsInStock)
                    .HasColumnType("numeric")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UnitsOnOrder)
                    .HasColumnType("numeric")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.NonInventoryCode)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.NonInventoryCodeId);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId);
            });

            modelBuilder.Entity<PurchaseDetails>(entity =>
            {
                entity.HasKey(e => new { e.PurchaseOrderId, e.PurchaseDetailId });

                entity.ToTable("Purchase Details");

                entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");

                entity.Property(e => e.PurchaseDetailId).HasColumnName("PurchaseDetailID");

                entity.Property(e => e.Comment).HasColumnType("nvarchar(50)").HasMaxLength(50);

                entity.Property(e => e.CommentCrLf).HasColumnType("bit");

                entity.Property(e => e.DirectExpenseText).HasColumnType("nvarchar(50)").HasMaxLength(50);

                entity.Property(e => e.LineType).HasColumnType("smallint");

                entity.Property(e => e.ParentRowId)
                    .HasColumnName("ParentRowID")
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.PickDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("numeric");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasColumnType("numeric");

                entity.Property(e => e.Received)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(e => e.RowId)
                    .HasColumnName("RowID")
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.ProductId);

                entity.HasOne(d => d.PurchaseOrder)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Purchases>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrderId);

                entity.Property(e => e.PurchaseOrderId)
                    .HasColumnName("PurchaseOrderID");

                entity.Property(e => e.Address).HasColumnType("nvarchar(60)").HasMaxLength(60);

                entity.Property(e => e.City).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.Country).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.Freight)
                    .HasColumnType("numeric")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Notes).HasColumnType("text(1073741823)");

                entity.Property(e => e.OrderDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.PoNumber)
                    .IsRequired()
                    .HasColumnName("PONumber")
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasColumnType("nvarchar(10)").HasMaxLength(10);

                entity.Property(e => e.Region).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Shippers>(entity =>
            {
                entity.HasKey(e => e.ShipperId);

                entity.Property(e => e.ShipperId)
                    .HasColumnName("ShipperID");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("nvarchar(40)")
                    .HasMaxLength(40);

                entity.Property(e => e.Phone).HasColumnType("nvarchar(24)").HasMaxLength(24);
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.HasKey(e => e.SupplierId);

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID");

                entity.Property(e => e.Address).HasColumnType("nvarchar(60)").HasMaxLength(60);

                entity.Property(e => e.City).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("nvarchar(40)")
                    .HasMaxLength(40);

                entity.Property(e => e.ContactName).HasColumnType("nvarchar(30)").HasMaxLength(30);

                entity.Property(e => e.ContactTitle).HasColumnType("nvarchar(30)").HasMaxLength(30);

                entity.Property(e => e.Country).HasColumnType("nvarchar(15)").HasMaxLength(15);

                entity.Property(e => e.Fax).HasColumnType("nvarchar(24)").HasMaxLength(24);

                entity.Property(e => e.HomePage).HasColumnType("text(1073741823)");

                entity.Property(e => e.Phone).HasColumnType("nvarchar(24)").HasMaxLength(24);

                entity.Property(e => e.PostalCode).HasColumnType("nvarchar(10)").HasMaxLength(10);

                entity.Property(e => e.Region).HasColumnType("nvarchar(15)").HasMaxLength(15);
            });

        }
    }
}
