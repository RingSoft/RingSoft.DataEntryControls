using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WpfApp1.Model
{
    public partial class NorthwindContext : DbContext
    {
        public NorthwindContext()
        {
        }

        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
        public virtual DbSet<CustomerDemographics> CustomerDemographics { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<EmployeeTerritories> EmployeeTerritories { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Shippers> Shippers { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Territories> Territories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("DataSource=C:\\Temp\\Northwind.sqlite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.HasIndex(e => e.CategoryName)
                    .HasName("CategoryName");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnType("nvarchar(15)");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Picture).HasColumnType("image");
            });

            modelBuilder.Entity<CustomerCustomerDemo>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.CustomerTypeId });

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("nchar(5)");

                entity.Property(e => e.CustomerTypeId)
                    .HasColumnName("CustomerTypeID")
                    .HasColumnType("nchar(10)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerCustomerDemo)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.CustomerCustomerDemo)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CustomerDemographics>(entity =>
            {
                entity.HasKey(e => e.CustomerTypeId);

                entity.Property(e => e.CustomerTypeId)
                    .HasColumnName("CustomerTypeID")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.CustomerDesc).HasColumnType("ntext");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.HasIndex(e => e.City)
                    .HasName("City");

                entity.HasIndex(e => e.CompanyName)
                    .HasName("CompanyName1");

                entity.HasIndex(e => e.PostalCode)
                    .HasName("PostalCode2");

                entity.HasIndex(e => e.Region)
                    .HasName("Region1");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("nchar(5)");

                entity.Property(e => e.Address).HasColumnType("nvarchar(60)");

                entity.Property(e => e.City).HasColumnType("nvarchar(15)");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("nvarchar(40)");

                entity.Property(e => e.ContactName).HasColumnType("nvarchar(30)");

                entity.Property(e => e.ContactTitle).HasColumnType("nvarchar(30)");

                entity.Property(e => e.Country).HasColumnType("nvarchar(15)");

                entity.Property(e => e.Fax).HasColumnType("nvarchar(24)");

                entity.Property(e => e.Phone).HasColumnType("nvarchar(24)");

                entity.Property(e => e.PostalCode).HasColumnType("nvarchar(10)");

                entity.Property(e => e.Region).HasColumnType("nvarchar(15)");
            });

            modelBuilder.Entity<EmployeeTerritories>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TerritoryId });

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasColumnType("int");

                entity.Property(e => e.TerritoryId)
                    .HasColumnName("TerritoryID")
                    .HasColumnType("nvarchar(20)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Territory)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.TerritoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.HasIndex(e => e.LastName)
                    .HasName("LastName");

                entity.HasIndex(e => e.PostalCode)
                    .HasName("PostalCode1");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnType("nvarchar(60)");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.City).HasColumnType("nvarchar(15)");

                entity.Property(e => e.Country).HasColumnType("nvarchar(15)");

                entity.Property(e => e.Extension).HasColumnType("nvarchar(4)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("nvarchar(10)");

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.HomePhone).HasColumnType("nvarchar(24)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("nvarchar(20)");

                entity.Property(e => e.Notes).HasColumnType("ntext");

                entity.Property(e => e.Photo).HasColumnType("image");

                entity.Property(e => e.PhotoPath).HasColumnType("nvarchar(255)");

                entity.Property(e => e.PostalCode).HasColumnType("nvarchar(10)");

                entity.Property(e => e.Region).HasColumnType("nvarchar(15)");

                entity.Property(e => e.ReportsTo).HasColumnType("int");

                entity.Property(e => e.Title).HasColumnType("nvarchar(30)");

                entity.Property(e => e.TitleOfCourtesy).HasColumnType("nvarchar(25)");

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo);
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.ToTable("Order Details");

                entity.HasIndex(e => e.OrderId)
                    .HasName("OrderID");

                entity.HasIndex(e => e.ProductId)
                    .HasName("ProductID");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("int");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasColumnType("int");

                entity.Property(e => e.Quantity)
                    .HasColumnType("smallint")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.UnitPrice)
                    .IsRequired()
                    .HasColumnType("money")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.HasIndex(e => e.CustomerId)
                    .HasName("CustomerID");

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("EmployeeID");

                entity.HasIndex(e => e.OrderDate)
                    .HasName("OrderDate");

                entity.HasIndex(e => e.ShipPostalCode)
                    .HasName("ShipPostalCode");

                entity.HasIndex(e => e.ShipVia)
                    .HasName("ShippersOrders");

                entity.HasIndex(e => e.ShippedDate)
                    .HasName("ShippedDate");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("nchar(5)");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasColumnType("int");

                entity.Property(e => e.Freight)
                    .HasColumnType("money")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.ShipAddress).HasColumnType("nvarchar(60)");

                entity.Property(e => e.ShipCity).HasColumnType("nvarchar(15)");

                entity.Property(e => e.ShipCountry).HasColumnType("nvarchar(15)");

                entity.Property(e => e.ShipName).HasColumnType("nvarchar(40)");

                entity.Property(e => e.ShipPostalCode).HasColumnType("nvarchar(10)");

                entity.Property(e => e.ShipRegion).HasColumnType("nvarchar(15)");

                entity.Property(e => e.ShipVia).HasColumnType("int");

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId);

                entity.HasOne(d => d.ShipViaNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipVia);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.HasIndex(e => e.CategoryId)
                    .HasName("CategoriesProducts");

                entity.HasIndex(e => e.ProductName)
                    .HasName("ProductName");

                entity.HasIndex(e => e.SupplierId)
                    .HasName("SupplierID");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int");

                entity.Property(e => e.Discontinued)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("nvarchar(40)");

                entity.Property(e => e.QuantityPerUnit).HasColumnType("nvarchar(20)");

                entity.Property(e => e.ReorderLevel)
                    .HasColumnType("smallint")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID")
                    .HasColumnType("int");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UnitsInStock)
                    .HasColumnType("smallint")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UnitsOnOrder)
                    .HasColumnType("smallint")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.RegionId)
                    .HasColumnName("RegionID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.RegionDescription)
                    .IsRequired()
                    .HasColumnType("nchar(50)");
            });

            modelBuilder.Entity<Shippers>(entity =>
            {
                entity.HasKey(e => e.ShipperId);

                entity.Property(e => e.ShipperId)
                    .HasColumnName("ShipperID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("nvarchar(40)");

                entity.Property(e => e.Phone).HasColumnType("nvarchar(24)");
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.HasKey(e => e.SupplierId);

                entity.HasIndex(e => e.CompanyName)
                    .HasName("CompanyName");

                entity.HasIndex(e => e.PostalCode)
                    .HasName("PostalCode");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnType("nvarchar(60)");

                entity.Property(e => e.City).HasColumnType("nvarchar(15)");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("nvarchar(40)");

                entity.Property(e => e.ContactName).HasColumnType("nvarchar(30)");

                entity.Property(e => e.ContactTitle).HasColumnType("nvarchar(30)");

                entity.Property(e => e.Country).HasColumnType("nvarchar(15)");

                entity.Property(e => e.Fax).HasColumnType("nvarchar(24)");

                entity.Property(e => e.HomePage).HasColumnType("ntext");

                entity.Property(e => e.Phone).HasColumnType("nvarchar(24)");

                entity.Property(e => e.PostalCode).HasColumnType("nvarchar(10)");

                entity.Property(e => e.Region).HasColumnType("nvarchar(15)");
            });

            modelBuilder.Entity<Territories>(entity =>
            {
                entity.HasKey(e => e.TerritoryId);

                entity.Property(e => e.TerritoryId)
                    .HasColumnName("TerritoryID")
                    .HasColumnType("nvarchar(20)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("RegionID")
                    .HasColumnType("int");

                entity.Property(e => e.TerritoryDescription)
                    .IsRequired()
                    .HasColumnType("nchar(50)");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Territories)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
