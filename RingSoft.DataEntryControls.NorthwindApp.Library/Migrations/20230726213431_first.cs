using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DataEntryControls.NorthwindApp.Library.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvancedFinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    Table = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    FromFormula = table.Column<string>(type: "ntext", nullable: true),
                    RefreshRate = table.Column<byte>(type: "smallint", nullable: true),
                    RefreshValue = table.Column<int>(type: "integer", nullable: true),
                    RefreshCondition = table.Column<byte>(type: "smallint", nullable: true),
                    YellowAlert = table.Column<int>(type: "integer", nullable: true),
                    RedAlert = table.Column<int>(type: "integer", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "text(1073741823)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "char(5)", maxLength: 5, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TitleOfCourtesy = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    HomePhone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Notes = table.Column<string>(type: "text(1073741823)", nullable: true),
                    ReportsTo = table.Column<int>(type: "INTEGER", nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ReportsTo",
                        column: x => x.ReportsTo,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "NonInventoryCodes",
                columns: table => new
                {
                    NonInventoryCodeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonInventoryCodes", x => x.NonInventoryCodeID);
                });

            migrationBuilder.CreateTable(
                name: "RecordLocks",
                columns: table => new
                {
                    Table = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    LockDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    User = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordLocks", x => new { x.Table, x.PrimaryKey });
                });

            migrationBuilder.CreateTable(
                name: "Shippers",
                columns: table => new
                {
                    ShipperID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippers", x => x.ShipperID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    HomePage = table.Column<string>(type: "text(1073741823)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindColumns",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    PrimaryTableName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Path = table.Column<string>(type: "nvarchar", maxLength: 1000, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar", maxLength: 250, nullable: true),
                    PercentWidth = table.Column<double>(type: "numeric", nullable: false),
                    Formula = table.Column<string>(type: "ntext", nullable: true),
                    FieldDataType = table.Column<byte>(type: "smallint", nullable: false),
                    DecimalFormatType = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindColumns", x => new { x.AdvancedFindId, x.ColumnId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindFilters",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    FilterId = table.Column<int>(type: "integer", nullable: false),
                    LeftParentheses = table.Column<byte>(type: "smallint", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    PrimaryTableName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Path = table.Column<string>(type: "nvarchar", maxLength: 1000, nullable: true),
                    Operand = table.Column<byte>(type: "smallint", nullable: false),
                    SearchForValue = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Formula = table.Column<string>(type: "ntext", nullable: true),
                    FormulaDataType = table.Column<byte>(type: "smallint", nullable: false),
                    FormulaDisplayValue = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    SearchForAdvancedFindId = table.Column<int>(type: "integer", nullable: true),
                    CustomDate = table.Column<bool>(type: "bit", nullable: false),
                    RightParentheses = table.Column<byte>(type: "smallint", nullable: false),
                    EndLogic = table.Column<byte>(type: "smallint", nullable: false),
                    DateFilterType = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindFilters", x => new { x.AdvancedFindId, x.FilterId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                        column: x => x.SearchForAdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerID = table.Column<string>(type: "char(5)", maxLength: 5, nullable: true),
                    EmployeeID = table.Column<int>(type: "INTEGER", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequiredDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShipVia = table.Column<int>(type: "INTEGER", nullable: true),
                    Freight = table.Column<double>(type: "numeric", nullable: true, defaultValueSql: "0"),
                    ShipName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ShipAddress = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ShipCity = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ShipRegion = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ShipPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ShipCountry = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Notes = table.Column<string>(type: "text(1073741823)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Orders_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Orders_Shippers_ShipVia",
                        column: x => x.ShipVia,
                        principalTable: "Shippers",
                        principalColumn: "ShipperID");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: true),
                    QuantityPerUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UnitPrice = table.Column<double>(type: "numeric", nullable: true, defaultValueSql: "0"),
                    UnitsInStock = table.Column<double>(type: "numeric", nullable: true, defaultValueSql: "0"),
                    UnitsOnOrder = table.Column<double>(type: "numeric", nullable: true, defaultValueSql: "0"),
                    ReorderLevel = table.Column<double>(type: "numeric", nullable: true, defaultValueSql: "0"),
                    Discontinued = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    OrderComment = table.Column<string>(type: "text(1073741823)", nullable: true),
                    PurchaseComment = table.Column<string>(type: "text(1073741823)", nullable: true),
                    NonInventoryCodeID = table.Column<int>(type: "INTEGER", nullable: true),
                    UnitDecimals = table.Column<byte>(type: "smallint", nullable: false),
                    Notes = table.Column<string>(type: "text(1073741823)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_Products_NonInventoryCodes_NonInventoryCodeID",
                        column: x => x.NonInventoryCodeID,
                        principalTable: "NonInventoryCodes",
                        principalColumn: "NonInventoryCodeID");
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID");
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseOrderID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Freight = table.Column<double>(type: "numeric", nullable: false, defaultValueSql: "0"),
                    Notes = table.Column<string>(type: "text(1073741823)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseOrderID);
                    table.ForeignKey(
                        name: "FK_Purchases_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID");
                });

            migrationBuilder.CreateTable(
                name: "Order Details",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderDetailID = table.Column<int>(type: "INTEGER", nullable: false),
                    LineType = table.Column<byte>(type: "smallint", nullable: false),
                    RowID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentRowID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductID = table.Column<int>(type: "INTEGER", nullable: true),
                    NonInventoryCodeID = table.Column<int>(type: "INTEGER", nullable: true),
                    SpecialOrderText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommentCrLf = table.Column<bool>(type: "bit", nullable: false),
                    UnitPrice = table.Column<double>(type: "numeric", nullable: true),
                    Quantity = table.Column<double>(type: "numeric", nullable: true),
                    Discount = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order Details", x => new { x.OrderID, x.OrderDetailID });
                    table.ForeignKey(
                        name: "FK_Order Details_NonInventoryCodes_NonInventoryCodeID",
                        column: x => x.NonInventoryCodeID,
                        principalTable: "NonInventoryCodes",
                        principalColumn: "NonInventoryCodeID");
                    table.ForeignKey(
                        name: "FK_Order Details_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK_Order Details_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "Purchase Details",
                columns: table => new
                {
                    PurchaseOrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseDetailID = table.Column<int>(type: "INTEGER", nullable: false),
                    LineType = table.Column<byte>(type: "smallint", nullable: false),
                    RowID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentRowID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Received = table.Column<bool>(type: "bit", nullable: false),
                    ProductID = table.Column<int>(type: "INTEGER", nullable: true),
                    DirectExpenseText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommentCrLf = table.Column<bool>(type: "bit", nullable: true),
                    Quantity = table.Column<double>(type: "numeric", nullable: true),
                    Price = table.Column<double>(type: "numeric", nullable: true),
                    PickDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DelayDays = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase Details", x => new { x.PurchaseOrderID, x.PurchaseDetailID });
                    table.ForeignKey(
                        name: "FK_Purchase Details_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                    table.ForeignKey(
                        name: "FK_Purchase Details_Purchases_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalTable: "Purchases",
                        principalColumn: "PurchaseOrderID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedFindFilters_SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                column: "SearchForAdvancedFindId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReportsTo",
                table: "Employees",
                column: "ReportsTo");

            migrationBuilder.CreateIndex(
                name: "IX_Order Details_NonInventoryCodeID",
                table: "Order Details",
                column: "NonInventoryCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_Order Details_ProductID",
                table: "Order Details",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeID",
                table: "Orders",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipVia",
                table: "Orders",
                column: "ShipVia");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_NonInventoryCodeID",
                table: "Products",
                column: "NonInventoryCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierID",
                table: "Products",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase Details_ProductID",
                table: "Purchase Details",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SupplierID",
                table: "Purchases",
                column: "SupplierID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancedFindColumns");

            migrationBuilder.DropTable(
                name: "AdvancedFindFilters");

            migrationBuilder.DropTable(
                name: "Order Details");

            migrationBuilder.DropTable(
                name: "Purchase Details");

            migrationBuilder.DropTable(
                name: "RecordLocks");

            migrationBuilder.DropTable(
                name: "AdvancedFinds");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Shippers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "NonInventoryCodes");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
