﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

#nullable disable

namespace RingSoft.DataEntryControls.NorthwindApp.Library.Migrations
{
    [DbContext(typeof(NorthwindDbContext))]
    [Migration("20230726213431_first")]
    partial class first
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Categories", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("CategoryID");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Description")
                        .HasColumnType("text(1073741823)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Customers", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasMaxLength(5)
                        .HasColumnType("char(5)")
                        .HasColumnName("CustomerID");

                    b.Property<string>("Address")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("City")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("ContactName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ContactTitle")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Country")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Fax")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("Phone")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Region")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Employees", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("EmployeeID");

                    b.Property<string>("Address")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime");

                    b.Property<string>("City")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Country")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Extension")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("FullName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("HireDate")
                        .HasColumnType("datetime");

                    b.Property<string>("HomePhone")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Notes")
                        .HasColumnType("text(1073741823)");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Region")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int?>("ReportsTo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("TitleOfCourtesy")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("ReportsTo");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.NonInventoryCodes", b =>
                {
                    b.Property<int>("NonInventoryCodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("NonInventoryCodeID");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("NonInventoryCodeId");

                    b.ToTable("NonInventoryCodes");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.OrderDetails", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("OrderID");

                    b.Property<int>("OrderDetailId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("OrderDetailID");

                    b.Property<string>("Comment")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("CommentCrLf")
                        .HasColumnType("bit");

                    b.Property<float?>("Discount")
                        .HasColumnType("REAL");

                    b.Property<byte>("LineType")
                        .HasColumnType("smallint");

                    b.Property<int?>("NonInventoryCodeId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("NonInventoryCodeID");

                    b.Property<string>("ParentRowId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ParentRowID");

                    b.Property<int?>("ProductId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ProductID");

                    b.Property<double?>("Quantity")
                        .HasColumnType("numeric");

                    b.Property<string>("RowId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("RowID");

                    b.Property<string>("SpecialOrderText")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double?>("UnitPrice")
                        .HasColumnType("numeric");

                    b.HasKey("OrderId", "OrderDetailId");

                    b.HasIndex("NonInventoryCodeId");

                    b.HasIndex("ProductId");

                    b.ToTable("Order Details", (string)null);
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Orders", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("OrderID");

                    b.Property<string>("CustomerId")
                        .HasMaxLength(5)
                        .HasColumnType("char(5)")
                        .HasColumnName("CustomerID");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("EmployeeID");

                    b.Property<double?>("Freight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Notes")
                        .HasColumnType("text(1073741823)");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("RequiredDate")
                        .HasColumnType("datetime");

                    b.Property<string>("ShipAddress")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("ShipCity")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("ShipCountry")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("ShipName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("ShipPostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ShipRegion")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int?>("ShipVia")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ShippedDate")
                        .HasColumnType("datetime");

                    b.HasKey("OrderId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ShipVia");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Products", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ProductID");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("CategoryID");

                    b.Property<bool>("Discontinued")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("0");

                    b.Property<int?>("NonInventoryCodeId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("NonInventoryCodeID");

                    b.Property<string>("Notes")
                        .HasColumnType("text(1073741823)");

                    b.Property<string>("OrderComment")
                        .HasColumnType("text(1073741823)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PurchaseComment")
                        .HasColumnType("text(1073741823)");

                    b.Property<string>("QuantityPerUnit")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<double?>("ReorderLevel")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValueSql("0");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("SupplierID");

                    b.Property<byte>("UnitDecimals")
                        .HasColumnType("smallint");

                    b.Property<double?>("UnitPrice")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValueSql("0");

                    b.Property<double?>("UnitsInStock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValueSql("0");

                    b.Property<double?>("UnitsOnOrder")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValueSql("0");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("NonInventoryCodeId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.PurchaseDetails", b =>
                {
                    b.Property<int>("PurchaseOrderId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("PurchaseOrderID");

                    b.Property<int>("PurchaseDetailId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("PurchaseDetailID");

                    b.Property<string>("Comment")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool?>("CommentCrLf")
                        .HasColumnType("bit");

                    b.Property<int?>("DelayDays")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DirectExpenseText")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte>("LineType")
                        .HasColumnType("smallint");

                    b.Property<string>("ParentRowId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ParentRowID");

                    b.Property<DateTime?>("PickDate")
                        .HasColumnType("datetime");

                    b.Property<double?>("Price")
                        .HasColumnType("numeric");

                    b.Property<int?>("ProductId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ProductID");

                    b.Property<double?>("Quantity")
                        .HasColumnType("numeric");

                    b.Property<bool>("Received")
                        .HasColumnType("bit");

                    b.Property<string>("RowId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("RowID");

                    b.HasKey("PurchaseOrderId", "PurchaseDetailId");

                    b.HasIndex("ProductId");

                    b.ToTable("Purchase Details", (string)null);
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Purchases", b =>
                {
                    b.Property<int>("PurchaseOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("PurchaseOrderID");

                    b.Property<string>("Address")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("City")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Country")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<double>("Freight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Notes")
                        .HasColumnType("text(1073741823)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime");

                    b.Property<string>("PoNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("PONumber");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Region")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime?>("RequiredDate")
                        .HasColumnType("datetime");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("SupplierID");

                    b.HasKey("PurchaseOrderId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Shippers", b =>
                {
                    b.Property<int>("ShipperId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ShipperID");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Phone")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("ShipperId");

                    b.ToTable("Shippers");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Suppliers", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("SupplierID");

                    b.Property<string>("Address")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("City")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("ContactName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ContactTitle")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Country")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Fax")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("HomePage")
                        .HasColumnType("text(1073741823)");

                    b.Property<string>("Phone")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Region")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("SupplierId");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFind", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<bool?>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("FromFormula")
                        .HasColumnType("ntext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("RedAlert")
                        .HasColumnType("integer");

                    b.Property<byte?>("RefreshCondition")
                        .HasColumnType("smallint");

                    b.Property<byte?>("RefreshRate")
                        .HasColumnType("smallint");

                    b.Property<int?>("RefreshValue")
                        .HasColumnType("integer");

                    b.Property<string>("Table")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("YellowAlert")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AdvancedFinds");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn", b =>
                {
                    b.Property<int>("AdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<int>("ColumnId")
                        .HasColumnType("integer");

                    b.Property<string>("Caption")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar");

                    b.Property<byte>("DecimalFormatType")
                        .HasColumnType("smallint");

                    b.Property<byte>("FieldDataType")
                        .HasColumnType("smallint");

                    b.Property<string>("FieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Formula")
                        .HasColumnType("ntext");

                    b.Property<string>("Path")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar");

                    b.Property<double>("PercentWidth")
                        .HasColumnType("numeric");

                    b.Property<string>("PrimaryFieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryTableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("TableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("AdvancedFindId", "ColumnId");

                    b.ToTable("AdvancedFindColumns");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter", b =>
                {
                    b.Property<int>("AdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<int>("FilterId")
                        .HasColumnType("integer");

                    b.Property<bool>("CustomDate")
                        .HasColumnType("bit");

                    b.Property<byte>("DateFilterType")
                        .HasColumnType("smallint");

                    b.Property<byte>("EndLogic")
                        .HasColumnType("smallint");

                    b.Property<string>("FieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Formula")
                        .HasColumnType("ntext");

                    b.Property<byte>("FormulaDataType")
                        .HasColumnType("smallint");

                    b.Property<string>("FormulaDisplayValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<byte>("LeftParentheses")
                        .HasColumnType("smallint");

                    b.Property<byte>("Operand")
                        .HasColumnType("smallint");

                    b.Property<string>("Path")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryFieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryTableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<byte>("RightParentheses")
                        .HasColumnType("smallint");

                    b.Property<int?>("SearchForAdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<string>("SearchForValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("TableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("AdvancedFindId", "FilterId");

                    b.HasIndex("SearchForAdvancedFindId");

                    b.ToTable("AdvancedFindFilters");
                });

            modelBuilder.Entity("RingSoft.DbLookup.RecordLocking.RecordLock", b =>
                {
                    b.Property<string>("Table")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryKey")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime>("LockDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("User")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("Table", "PrimaryKey");

                    b.ToTable("RecordLocks");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Employees", b =>
                {
                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Employees", "Supervisor")
                        .WithMany("Underlings")
                        .HasForeignKey("ReportsTo");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.OrderDetails", b =>
                {
                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.NonInventoryCodes", "NonInventoryCode")
                        .WithMany("OrderDetails")
                        .HasForeignKey("NonInventoryCodeId");

                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Orders", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .IsRequired();

                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Products", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId");

                    b.Navigation("NonInventoryCode");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Orders", b =>
                {
                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Customers", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId");

                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Employees", "Employee")
                        .WithMany("Orders")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Shippers", "Shipper")
                        .WithMany("Orders")
                        .HasForeignKey("ShipVia");

                    b.Navigation("Customer");

                    b.Navigation("Employee");

                    b.Navigation("Shipper");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Products", b =>
                {
                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Categories", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.NonInventoryCodes", "NonInventoryCode")
                        .WithMany("Products")
                        .HasForeignKey("NonInventoryCodeId");

                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Suppliers", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId");

                    b.Navigation("Category");

                    b.Navigation("NonInventoryCode");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.PurchaseDetails", b =>
                {
                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Products", "Product")
                        .WithMany("PurchaseDetails")
                        .HasForeignKey("ProductId");

                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Purchases", "PurchaseOrder")
                        .WithMany("PurchaseDetails")
                        .HasForeignKey("PurchaseOrderId")
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("PurchaseOrder");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Purchases", b =>
                {
                    b.HasOne("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Suppliers", "Supplier")
                        .WithMany("Purchases")
                        .HasForeignKey("SupplierId")
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn", b =>
                {
                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "AdvancedFind")
                        .WithMany("Columns")
                        .HasForeignKey("AdvancedFindId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AdvancedFind");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter", b =>
                {
                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "AdvancedFind")
                        .WithMany("Filters")
                        .HasForeignKey("AdvancedFindId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "SearchForAdvancedFind")
                        .WithMany("SearchForAdvancedFindFilters")
                        .HasForeignKey("SearchForAdvancedFindId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("AdvancedFind");

                    b.Navigation("SearchForAdvancedFind");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Categories", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Customers", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Employees", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Underlings");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.NonInventoryCodes", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Orders", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Products", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("PurchaseDetails");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Purchases", b =>
                {
                    b.Navigation("PurchaseDetails");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Shippers", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("RingSoft.DataEntryControls.NorthwindApp.Library.Model.Suppliers", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFind", b =>
                {
                    b.Navigation("Columns");

                    b.Navigation("Filters");

                    b.Navigation("SearchForAdvancedFindFilters");
                });
#pragma warning restore 612, 618
        }
    }
}